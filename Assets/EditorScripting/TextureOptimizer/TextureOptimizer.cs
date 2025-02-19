using Unity.Mathematics.Geometry;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureImporter),  true)]
public class TextureOptimizer : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Optimize Texture"))
        {
            string path = AssetDatabase.GetAssetPath(Selection);
            //Getting the actually importer
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
            int height;
            int width;
            importer.GetSourceTextureWidthAndHeight(out height, out width);
            int maxDimension = Mathf.Max(height, width);

            TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(textureImporterSettings);
            textureImporterSettings.maxTextureSize = (int)Mathf.Pow(2, (int)Mathf.Log(maxDimension -1,2) + 1);
            importer.SetTextureSettings(textureImporterSettings);
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        };
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
