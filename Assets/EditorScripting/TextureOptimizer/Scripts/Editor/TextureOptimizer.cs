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
using Slider = UnityEngine.UIElements.Slider;

/*[CustomEditor(typeof(TextureImporter),  true)]*/
public class TextureOptimizer : EditorWindow
{
    private DropdownField selectTextureOptions;
    private ObjectField objectField;
    private ObjectField autoTextureField;
    private Texture2D currentTexture;
    private DropdownField wrapModeDropDown;
    private DropdownField filterModeDropDown;
    private Label messageUser;
    
    private VisualElement autoLoadSection;
    /*private VisualElement settingOption;*/
    
    private Button optimizeTextureButton;
    private Button loadMultipleTexturesButton;
    
    private Texture2D loadedTexture;
    
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
        optimizeTextureButton = root.Q<Button>("modify-texture");

        wrapModeDropDown = root.Q<DropdownField>("wrap-mode");
        filterModeDropDown = root.Q<DropdownField>("filter-mode");
        messageUser = root.Q<Label>("message-user");
        loadMultipleTexturesButton = root.Q<Button>("load-multiple-textures");
        
        autoLoadSection = root.Q<VisualElement>("auto-load-section");
        
        /*Assign CallBacks*/
        objectField.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt));
        selectTextureOptions.RegisterValueChangedCallback<string>(TextureSelectionOptions);
        
        optimizeTextureButton.clicked += () => OptimizeTexture();
        loadMultipleTexturesButton.clicked += () => LoadMultipleTextures();
        
        /*starting CallBacks*/
        currentTexture = null;
        DisplayMessage();

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
        /*if (selectTextureOptions.value == selectTextureOptions.choices[0] )
        {
            autoLoadSection.style.display = DisplayStyle.None;
            objectField.style.display = DisplayStyle.Flex;
        }
        else
        {
            objectField.style.display = DisplayStyle.None;
            autoLoadSection.style.display = DisplayStyle.Flex;
        }*/
    }
    private void OptimizeTexture()
    {
        if (currentTexture == null)
        {
            return;
        }
        
        string assetPath = AssetDatabase.GetAssetPath(currentTexture);
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
        
        /* settings assets */
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.filterMode = FilterMode.Point; 
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.textureCompression = TextureImporterCompression.CompressedHQ;
        importer = SetMaxTextureSizeOptimizer(importer);
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.textureType = TextureImporterType.Sprite;
        
        /* saving and reimporting assets */
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport(); 
    }

    private TextureImporter SetMaxTextureSizeOptimizer(TextureImporter importer)
    {
        int height;
        int width;
        
        importer.GetSourceTextureWidthAndHeight(out height, out width);
        int maxDimension = Mathf.Max(width, height);
        
        TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
        /*textureImporterSettings.maxTextureSize = CalculateMaxTextureSizeOptimizer(maxDimension);*/
        importer.maxTextureSize = CalculateMaxTextureSizeOptimizer(maxDimension);
        //save importer
        
        /*//read Textures from importer
        importer.ReadTextureSettings(textureImporterSettings);*/
        return importer;
    }

    private int CalculateMaxTextureSizeOptimizer(int maxDimension)
    {
        return (int)Mathf.Pow(2, (int)Mathf.Log(maxDimension -1,2) + 1);
        
    }

    private void TextureSelected(ChangeEvent<Object> evt)
    {

        if (evt.newValue !=null)
        {
            currentTexture = evt.newValue as Texture2D;
            messageUser.style.display =  DisplayStyle.None;
            return;
        }
        else
        {
            DisplayMessage();
        }
    }

    private void SelectTextureFromPanel()
    {
        
    }
}






