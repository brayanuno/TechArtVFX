using System;
using System.Reflection;
using System.IO;
using SFB;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using Application = UnityEngine.Application;
using Button = UnityEngine.UIElements.Button;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

using Slider = UnityEngine.UIElements.Slider;
using Toggle = UnityEngine.UIElements.Toggle;

/*[CustomEditor(typeof(TextureImporter),  true)]*/
public class TextureOptimizer : EditorWindow
{
    /*DropDowns*/
    private DropdownField selectTextureOptions;
    private DropdownField wrapModeDropDown;
    private DropdownField filterModeDropDown;
    
    /*SettingsDropDown*/
    private DropdownField textureTypeOptions;
    private DropdownField textureSpriteMode;
    private DropdownField textureFilterMode;
    private DropdownField compressionMode;
    private Toggle multipleSelection;
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
    private Texture2D loadedTexture;
    
    /*Text*/
    private TextField outputText;
    private Label messageUser;
    private Label validateText;
    

    /*Buttons*/
    private Button optimizeTextureButton;
    private Button loadMultipleTexturesButton;
    private Button validateTextureButton;
    private Button outputFolderbutton;
    
    /*Visual Elements*/
    private VisualElement autoLoadSection;
    private VisualElement folderOutput;
    private Foldout textureSettings;
    private VisualElement settingsContainer;
    private bool selectMultipleTextures = false;
    private VisualElement textOuputContainer;
    
    
    /*ValuesElements*/
    private int currentTextureTypeOption;
    private LibrarySettings librarySettings;

    private string currentOutputPath;
    
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
        outputText = root.Q<TextField>("output-text");
        outputFolderbutton = root.Q<Button>("output-folder-button");
        /*Buttons*/
        optimizeTextureButton = root.Q<Button>("modify-texture");
        loadMultipleTexturesButton = root.Q<Button>("load-multiple-textures");
        multipleSelection = root.Q<Toggle>("multiple-selection");
        textOuputContainer = root.Q<VisualElement>("container-text-output");
        
        /*Assign CallBacks*/
        objectField.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt));
        selectTextureOptions.RegisterValueChangedCallback<string>(TextureSelectionOptions);
        textureSettings.RegisterValueChangedCallback(FoldoutChanged);
        optimizeTextureButton.clicked += () => OptimizeTexture();
        loadMultipleTexturesButton.clicked += () => LoadMultipleTextures();
        validateTextureButton.clicked += () => ValidateTexture();
        outputFolderbutton.clicked += () => AssignCurrentPath();
        multipleSelection.RegisterValueChangedCallback(MultipleSelectionToogle);
        
        /*starting CallBacks*/
        currentTexture = null;
        librarySettings = new LibrarySettings();
        Initializer();
        
    }

    private void MultipleSelectionToogle(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            selectMultipleTextures = true;
            optimizeTextureButton.SetEnabled(value: true);
            validateTextureButton.SetEnabled(value: true);
            objectField.style.display = DisplayStyle.None;
        }
        else
        {
            selectMultipleTextures = false;
            optimizeTextureButton.SetEnabled(value: false);
            validateTextureButton.SetEnabled(value: false);
            objectField.style.display = DisplayStyle.Flex;
        }
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
       validateTextureButton.SetEnabled(false);
       loadMultipleTexturesButton.style.display = DisplayStyle.None;
       folderOutput.style.display = DisplayStyle.None;
       textureSettings.value = false;
       settingsContainer.style.display = DisplayStyle.None;
       DisplayMessage();
       
       textureTypeOptions.index = currentTextureTypeOption;
       textureTypeOptions.index = 2;
       outputText.value = "Assets/";
       loadMultipleTexturesButton.SetEnabled(false);
       
    }

    private void createNewMessageOutput(string message)
    {
        Label newFoundIt = new Label(message);
        /*newFoundIt.style.color = Color.green;*/
        textOuputContainer.Add( newFoundIt);
    }
    private void LoadMultipleTextures()
    {  
        /*var path = EditorUtility.OpenFilePanel(                             
            "Save Edit Texture",                                            
            Application.dataPath + "/EditorScripting",                      
            ""                                                           
        );     */                                                             
        // Open file with filter
        var extensions = new [] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            /*new ExtensionFilter("Sound Files", "mp3", "wav" ),*/
            /*new ExtensionFilter("All Files", "*" ),*/
        };
        // Open file async
        String[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        Debug.Log(paths.Length);
        /*only look if file is selected*/
        if (paths.Length != 0)
        {
            foreach (var path in paths)
            {
                Debug.Log(path);
                LoadTexture(path);
            }
        }
        return;
    }
    private void AssignCurrentPath()
    {
        currentOutputPath = GetSelectedFolderPath();
        outputText.value = currentOutputPath;

        if (currentOutputPath != null)
        {
            loadMultipleTexturesButton.SetEnabled(true);
        }
    }
    
    private void LoadTexture(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path);
        
        string assetPath = Path.Combine(currentOutputPath,fileName);
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
        if (currentTexture == null)
        {
            messageUser.style.display =  DisplayStyle.Flex;
            messageUser.style.color = Color.red; 
            messageUser.text = "Please Selected A texture";
        }
        else
        {
            messageUser.text = "";
        }
        
    }
    
    private void TextureSelectionOptions(ChangeEvent<string> evt)
    {
        if (selectTextureOptions.value == selectTextureOptions.choices[0] )
        {
            
            objectField.style.display = DisplayStyle.Flex;
            messageUser.style.display = DisplayStyle.Flex;
            optimizeTextureButton.style.display = DisplayStyle.Flex;
            validateTextureButton.style.display = DisplayStyle.Flex;
            loadMultipleTexturesButton.style.display = DisplayStyle.None;
            folderOutput.style.display = DisplayStyle.None;
        }
        else
        {
            optimizeTextureButton.style.display = DisplayStyle.None;
            validateTextureButton.style.display = DisplayStyle.None;
            objectField.style.display = DisplayStyle.None;
            messageUser.style.display = DisplayStyle.None;
            
            loadMultipleTexturesButton.style.display = DisplayStyle.Flex;
            folderOutput.style.display = DisplayStyle.Flex;
        }
    }
    /*Button-OptimizeTexture-Action => Optimize The texture ON Click*/
    private void OptimizeTexture()
    {
        /*if (currentTexture == null)
        {
            return;
        }*/
        
        Object[] selectedObjects = Selection.objects;
        foreach (var obj in selectedObjects)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);

            
            Debug.Log(assetPath);
            
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
            /* validate and send messages */
            
            createNewMessageOutput( obj.name + " imported");
        
            /* Save and Reimport */
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport(); 
            
        }
        
        /* Loading Asset*/
        /*string assetPath = AssetDatabase.GetAssetPath(currentTexture);*/
    }
    
    private void TextureSelected(ChangeEvent<Object> evt)
    {
        if (evt.newValue !=null)
        {
            optimizeTextureButton.SetEnabled(true);
            currentTexture = evt.newValue as Texture2D;
            messageUser.style.display =  DisplayStyle.None;
            validateTextureButton.SetEnabled(true);
            
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

        return true;
    }
    
    private void WriteTextValidator(Color _color , string _message)
    {
        validateTextOutput.style.color = _color;
        validateTextOutput.text = _message;
    }
}






