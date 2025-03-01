using System;
using Unity.Mathematics.Geometry;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
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
    
    private Button modifyTextureButton;

    
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
        modifyTextureButton = root.Q<Button>("modify-texture");

        wrapModeDropDown = root.Q<DropdownField>("wrap-mode");
        filterModeDropDown = root.Q<DropdownField>("filter-mode");
        messageUser = root.Q<Label>("message-user");
        
        
        autoLoadSection = root.Q<VisualElement>("auto-load-section");
        
        /*Assign CallBacks*/
        objectField.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt));
        selectTextureOptions.RegisterValueChangedCallback<string>(TextureSelectionOptions);
        
        modifyTextureButton.clicked += () => ModifyTexture();
        
        /*starting CallBacks*/
        currentTexture = null;
        DisplayMessage();

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
    private void ModifyTexture()
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






