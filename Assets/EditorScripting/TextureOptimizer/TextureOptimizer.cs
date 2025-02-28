using Unity.Mathematics.Geometry;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Object = UnityEngine.Object;
using Slider = UnityEngine.UIElements.Slider;

/*[CustomEditor(typeof(TextureImporter),  true)]*/
public class TextureOptimizer : EditorWindow
{
    private DropdownField dropDownField;
    private ObjectField objectField;
    private Texture2D currentTexture;
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
        dropDownField = root.Q<DropdownField>("alpha-dropdown");
        objectField = root.Q<ObjectField>("texture-field");
        modifyTextureButton = root.Q<Button>("modify-texture");
        
        /*Assign CallBacks*/
        objectField.RegisterValueChangedCallback<Object>(TextureSelected);
        modifyTextureButton.clicked += () => ModifyTexture();
        
        modifyTextureButton.SetEnabled(false);
    }

    private void ModifyTexture()
    {
        if (objectField.value == null)
        {
            return;
        }
        
        /*if (currentTexture = null)
        {
            return;
        }*/
        
        modifyTextureButton.SetEnabled(true);
        string assetPath = AssetDatabase.GetAssetPath(currentTexture);
        
        /*Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));*/
        
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.filterMode = FilterMode.Point; 
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport(); 
        
    }
    private void TextureSelected(ChangeEvent<Object> evt)
    {
        currentTexture = evt.newValue as Texture2D;
        modifyTextureButton.SetEnabled(true);

        /*TextureImporter importer = assetImporter as TextureImporter;*/
        
    }
}

    /*public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Optimize Texture"))
        {
            SetMaxTextureSizeOptimizer();
        }
    }*/

    /*void SetMaxTextureSizeOptimizer()
    {
        //Getting the path of selectedObject
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Debug.Log("Optimized Texture");
        Debug.Log(path);
        //Getting the actually importer
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
            
        int height;
        int width;
            
        //storing maxDimension on height and width
        importer.GetSourceTextureWidthAndHeight(out height, out width);
        //get max Dimension
        int maxDimension = Mathf.Max(width, height);
            
        //creating TextureImporterSettings instance
        TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
        
        //read Textures from importer
        importer.ReadTextureSettings(textureImporterSettings);
        //adjusting importSetting to new Dimension
        textureImporterSettings.maxTextureSize = CalculateMaxTextureSizeOptimizer(maxDimension);
         
        //save importer
        importer.SetTextureSettings(textureImporterSettings);
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();    

        };
    }

    private int  CalculateMaxTextureSizeOptimizer(int maxDimension)
    {
        return (int)Mathf.Pow(2, (int)Mathf.Log(maxDimension -1,2) + 1);
        
    }*/



