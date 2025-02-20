using Unity.Mathematics.Geometry;
using UnityEditor;
using UnityEngine;

/*[CustomEditor(typeof(TextureImporter),  true)]
public class TextureOptimizer : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Optimize Texture"))
        {
            SetMaxTextureSizeOptimizer();
        }#1#

    void SetMaxTextureSizeOptimizer()
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
        
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
/*}*/
