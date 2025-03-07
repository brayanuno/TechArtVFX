using System;
using System.Reflection;
using System.IO;
using Unity.Mathematics.Geometry;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
<<<<<<< HEAD
using Slider = UnityEngine.UIElements.Slider;
using Toggle = UnityEngine.UIElements.Toggle;
=======

>>>>>>> c9578f0514b5d7e18a43a472bd927a3c40df7ca4

/*[CustomEditor(typeof(TextureImporter),  true)]*/
public class TextureOptimizer : EditorWindow
{
    /*DropDowns*/
    private DropdownField selectTextureOptions;
    
    /*SettingsDropDown*/
    private DropdownField textureTypeOptions;
    private DropdownField textureSpriteMode;
    private DropdownField textureFilterMode;
    private DropdownField compressionMode;
    private Toggle rgbCheck;
    private Toggle optimizeChecker;
    private IntegerField pixelsUnit;
    
    /*labels text validator */
    private Label validateTextOutput;
    
    
    /*Objects Fields;*/
    private ObjectField objectField;
    private ObjectField autoTextureField;
    /*Textures 2d*/
    private Texture2D currentTexture;
<<<<<<< HEAD
=======
    private Label validateText;
    
    private TextField outputText;
    
    private DropdownField wrapModeDropDown;
    private DropdownField filterModeDropDown;
    
    private Label messageUser;
    
    private Label news;
    private VisualElement autoLoadSection;
    /*private VisualElement settingOption;*/
    
    private Button optimizeTextureButton;
    private Button loadMultipleTexturesButton;
    
>>>>>>> c9578f0514b5d7e18a43a472bd927a3c40df7ca4
    private Texture2D loadedTexture;
    
    /*Text*/
    private TextField outputText;
    private Label messageUser;


    /*Buttons*/
    private Button optimizeTextureButton;
    private Button loadMultipleTexturesButton;
    private Button validateTextureButton;
    
    /*Visual Elements*/
    private VisualElement autoLoadSection;
    private VisualElement folderOutput;
    private Foldout textureSettings;
    private VisualElement settingsContainer;
    
    /*ValuesElements*/
    private int currentTextureTypeOption;

    private LibrarySettings librarySettings;
    
    
    [MenuItem("Tools/TextureOptimizer")]
    public static void OpenEditorWindow()
    {
        TextureOptimizer window = (TextureOptimizer)EditorWindow.GetWindow(typeof(TextureOptimizer));
        window.titleContent = new GUIContent("TextureOptimizer");
        window.maxSize = new Vector2(350, 450);
        window.minSize = window.maxSize;
        
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorScripting/TextureOptimizer/Resources/UI.Document/TextureCompression.uxml");
        VisualElement tree = visualTree.Instantiate();
        root.Add(tree);
        
        /*Assign Elements*/
        selectTextureOptions = root.Q<DropdownField>("texture-selection");
        objectField = root.Q<ObjectField>("texture-field");
        autoTextureField = root.Q<ObjectField>("auto-texture-field");
        validateTextOutput = root.Q<Label>("validate-text");
        textureTypeOptions = root.Q<DropdownField>("texture-type");
        folderOutput = root.Q<VisualElement>("folder-output");
        textureSpriteMode = root.Q<DropdownField>("texture-sprite-mode");
        textureFilterMode = root.Q<DropdownField>("texture-filter-mode");
        autoLoadSection = root.Q<VisualElement>("auto-load-section");
        textureSettings = root.Q<Foldout>("texture-settings");
        settingsContainer = root.Q("settings-container");
        messageUser = root.Q<Label>("message-user");
        compressionMode = root.Q<DropdownField>("compression-mode");
        rgbCheck = root.Q<Toggle>("rgb-check");
        pixelsUnit = root.Q<IntegerField>("pixels-unit");
        optimizeChecker = root.Q<Toggle>("optimize-checker");
        validateTextureButton = root.Q<Button>("validate-texture");
        
        /*Buttons*/
        optimizeTextureButton = root.Q<Button>("modify-texture");
        loadMultipleTexturesButton = root.Q<Button>("load-multiple-textures");
        
        
        /*Assign CallBacks*/
        objectField.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt));
        selectTextureOptions.RegisterValueChangedCallback<string>(TextureSelectionOptions);
        textureSettings.RegisterValueChangedCallback(FoldoutChanged);
        optimizeTextureButton.clicked += () => OptimizeTexture();
        loadMultipleTexturesButton.clicked += () => LoadMultipleTextures();
        validateTextureButton.clicked += () => ValidateTexture();
        
        /*starting CallBacks*/
        currentTexture = null;
        librarySettings = new LibrarySettings();
        Initializer();
        
    }
    
    private void FoldoutChanged(ChangeEvent<bool> evt)
    {
        if (evt.newValue != true)
        {
            settingsContainer.style.display = DisplayStyle.None;
        }
        else
        {
            settingsContainer.style.display = DisplayStyle.Flex;
        }
    }

    private void Initializer()
    {
       optimizeTextureButton.SetEnabled(false);
       loadMultipleTexturesButton.style.display = DisplayStyle.None;
       folderOutput.style.display = DisplayStyle.None;
       textureSettings.value = false;
       settingsContainer.style.display = DisplayStyle.None;
       DisplayMessage();
       
       textureTypeOptions.index = currentTextureTypeOption;
       textureTypeOptions.index = 2;
       
    }
    
    private void LoadMultipleTextures()
    {  
        var path = EditorUtility.OpenFilePanel(                             
            "Save Edit Texture",                                            
            Application.dataPath + "/EditorScripting",                      
            "png"                                                           
        );                                                                  
        
        /*only look if file is selected*/
        if (path.Length != 0)
        {
            LoadTexture(path);
        }
    }
    private void OnGUI()
    {
        
    }

    private void LoadTexture(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path);
        
        string assetPath = Path.Combine(GetSelectedFolderPath(),fileName);
        //Writing png to bytes
        File.WriteAllBytes(assetPath, fileData);
        
        /*AssetDatabase.ImportAsset(assetPath);*/
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        
        Selection.activeObject = (Texture2D)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));  
        Debug.Log($"File imported Successfull to: {assetPath}");
        
        return;
        
        loadedTexture = new Texture2D(256, 256);
        
        loadedTexture.LoadImage(fileData);
        
        //Writing png to bytes
        
        byte[] bytes = loadedTexture.EncodeToPNG();
        /*string fileName = Path.GetFileName(path);*/
        /*string assetPath = Application.dataPath + "/test" + fileName;*/
        /*string assetPath = Path.Combine("Assets", fileName); */           

        File.WriteAllBytes(assetPath, bytes);
        
        AssetDatabase.ImportAsset(path);         
        AssetDatabase.Refresh();             
        EditorUtility.FocusProjectWindow();  


        Selection.activeObject = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
        
        Debug.Log("Texture loaded: " + path);
    }

    private void TextureValidator()
    {
        
    }
    private string GetSelectedFolderPath()
    {
        if (Selection.activeObject != null)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            
            if (AssetDatabase.IsValidFolder(path))
            {
                Debug.Log($"Selected Folder Path: {path}");
                return path;
                EditorUtility.DisplayDialog("Selected Folder Path", $"Selected Folder Path:\n{path}", "OK");
                return path;    
            }
            else
            {
                Debug.LogWarning("Please select a folder in the Project window.");
                EditorUtility.DisplayDialog("Warning", "Please select a folder in the Project window.", "OK");
            }
        }
        else
        {
            Debug.LogWarning("No folder selected in the Project window.");
            EditorUtility.DisplayDialog("Warning", "No folder selected in the Project window.", "OK");
        } 
        return null;
    }
    private void AutoLoadTexture()
    {
        Object selectedAsset = Selection.activeObject;
        autoTextureField.value = selectedAsset;
        currentTexture = selectedAsset as Texture2D;
        
        string assetPath = AssetDatabase.GetAssetPath(currentTexture);
        
    }
    private void DisplayMessage()
    {
        messageUser.style.display =  DisplayStyle.Flex;
        messageUser.style.color = Color.red; 
        messageUser.text = "Please Selected A texture";
    }
    
    private void TextureSelectionOptions(ChangeEvent<string> evt)
    {
        if (selectTextureOptions.value == selectTextureOptions.choices[0] )
        {

            objectField.style.display = DisplayStyle.Flex;
            messageUser.style.display = DisplayStyle.Flex;
            
            loadMultipleTexturesButton.style.display = DisplayStyle.None;
            folderOutput.style.display = DisplayStyle.None;
            
        }
        else
        {
            objectField.style.display = DisplayStyle.None;
            messageUser.style.display = DisplayStyle.None;
            
            loadMultipleTexturesButton.style.display = DisplayStyle.Flex;
            folderOutput.style.display = DisplayStyle.Flex;
        }
    }
    /*Button-OptimizeTexture-Action => Optimize The texture ON Click*/
    private void OptimizeTexture()
    {
        if (currentTexture == null)
        {
            return;
        }
        
        /* Loading Asset*/
        string assetPath = AssetDatabase.GetAssetPath(currentTexture);
        
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
        
        /* CustomSettings */
        importer = librarySettings.SetTextureType(importer, textureTypeOptions.index);
        importer = librarySettings.SetSpriteMode(importer, textureSpriteMode.index);
        importer = librarySettings.SetFilterMode(importer, textureFilterMode.index);
        importer = librarySettings.SetCompression(importer, compressionMode.index);
        importer = librarySettings.SetRGB(importer, rgbCheck.value);
        importer = librarySettings.SetPixelUnit(importer,pixelsUnit.value);

        if (optimizeChecker.value)
        {
            importer = librarySettings.SetMaxTextureSizeOptimizer(importer); 
        }
        
        /* Save and Reimport */
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport(); 

    }
    
    private void TextureSelected(ChangeEvent<Object> evt)
    {
        if (evt.newValue !=null)
        {
            optimizeTextureButton.SetEnabled(true);
            currentTexture = evt.newValue as Texture2D;
            messageUser.style.display =  DisplayStyle.None;
            validateTextureButton.SetEnabled(true);
            return;
        }
        else
        {
            optimizeTextureButton.SetEnabled(false);
            validateTextureButton.SetEnabled(false);
            DisplayMessage();
        }
    }
    
    public bool ValidateTexture()
    {
        if (currentTexture!= null)
        {
            bool returnTexture = false;
            string message = "";
            string assetPath = AssetDatabase.GetAssetPath(currentTexture);
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
            
            if (textureTypeOptions.value == importer.textureType.ToString())
            {
                returnTexture = true;
                WriteTextValidator(Color.green, "All good Test Passed");
                
            }
            else
            {
                returnTexture = false;
                
            }
            if (textureSpriteMode.value == importer.spriteImportMode.ToString())
            {
                returnTexture = true;
            }
            else if (textureSpriteMode.value == importer.spriteImportMode.ToString())
            {
                returnTexture = true;
            }
            else if (textureFilterMode.value == importer.filterMode.ToString())
            {

            }
            /*else if (textureSpriteMode.value == importer.spriteImportMode.ToString())
            {
                returnTexture = true;
                WriteTextValidator(Color.green, "Test Passed");
            }*/
            /*
            if (textureSpriteMode.value == importer.spriteImportMode.ToString())
            {
                WriteTextValidator(Color.green, "All good Test Passed");
            }
            
            if (textureFilterMode.value == importer.filterMode.ToString())
            {
                WriteTextValidator(Color.green, "All good Test Passed");
            }
            
            if (textureTypeOptions.value == importer.textureType.ToString())
            {
                WriteTextValidator(Color.green, "All good Test Passed");
            }
            */
            if (returnTexture)
            {
                WriteTextValidator(Color.green, "Test Passed");
            }
            else
            {
                WriteTextValidator(Color.red, "Test Not Passed");
            }
        }
        
    }
    
    private void WriteTextValidator(Color _color , string _message)
    {
        validateTextOutput.style.color = _color;
        validateTextOutput.text = _message;
    }
}






