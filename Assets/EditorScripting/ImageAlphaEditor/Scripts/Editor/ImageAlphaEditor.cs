using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Object = UnityEngine.Object;
namespace NS
{
    public class ImageAlphaEditor : EditorWindow
    {
        private ObjectField textureField;
        private DropdownField alphaDropdown;
        private GradientField alphaGradientField;
        private VisualElement imagePreview;
        private SliderInt alphaSliderInt;
        private Button exportButton;
        private Texture2D selectedTexture;
        private Texture2D outputTexture;
        private VisualElement customTexValues;
        private DropdownField textureOption;
        private IntegerField widthField;
        private IntegerField heightField;
        private Button createTexButton;
        private ColorField tint;
        private ComputeShader shader;
        private IntegerField alphaInput;

        private string outputName = "TestTexturePro";
        
        [MenuItem("Tools/ImageAlphaEditor")]
        public static void OpenEditorWindow()
        {
            ImageAlphaEditor window = (ImageAlphaEditor)EditorWindow.GetWindow(typeof(ImageAlphaEditor));
            window.titleContent = new GUIContent("ImageAlphaEditor");
            window.maxSize = new Vector2(320, 550);
            window.minSize = window.maxSize;
            
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorScripting/ImageAlphaEditor/Resources/UI.Document/ImageAlphaEditor.uxml");
            VisualElement tree = visualTree.Instantiate();
            root.Add(tree);
            
            /*Assign Elements*/
            textureField = root.Q<ObjectField>("texture-field");
            alphaDropdown = root.Q<DropdownField>("alpha-dropdown");
            textureOption = root.Q<DropdownField>("texture-option");
            alphaGradientField= root.Q<GradientField>();
            imagePreview = root.Q<VisualElement>("image-preview");
            customTexValues = root.Q<VisualElement>("custom-tex-values");
            alphaSliderInt = root.Q<SliderInt>();
            exportButton = root.Q<Button>("export-button");
            
            createTexButton = root.Q<Button>("create-text-button");
            widthField = root.Q<IntegerField>("width-field");
            heightField = root.Q<IntegerField>("height-field");
            tint = root.Q<ColorField>("tint");
            alphaInput = root.Q<IntegerField>("alpha-input");
            
            /*Assign CallBacks*/
            textureField.RegisterValueChangedCallback<Object>(TextureSelected);
            alphaDropdown.RegisterValueChangedCallback<string>(AlphaOptionSelected);
            textureOption.RegisterValueChangedCallback<string>(TextureOptionSelected);
            alphaSliderInt.RegisterValueChangedCallback<int>(AlphaSliderChanged);
            alphaInput.RegisterValueChangedCallback<int>(AlphaInputChanged);
            tint.RegisterValueChangedCallback<Color>(TintChanged);
            exportButton.clicked += () => ExportImage(outputTexture);
            createTexButton.clicked += () => CreateTexture();
        
            exportButton.SetEnabled(false);
            imagePreview.style.backgroundImage = null;
            
            /*
            TextureOptionSelected(null);
            AlphaOptionSelected(null);*/
            
        }
        
        private void CreateTexture()
        {
            int textWidth = widthField.value;
            int textHeight = heightField.value;
            selectedTexture = new Texture2D(textWidth, textHeight, TextureFormat.ARGB32, false);
            for (int y = 0; y < textHeight; y++)
            {
                for (int x = 0; x < textWidth; x++)
                {
                    selectedTexture.SetPixel(x, y, Color.white);
                }
            }

            selectedTexture.Apply();
            
            bool greaterWidth = (textWidth > textHeight);
            float xRatio = 1;
            float yRatio = 1;
            if (greaterWidth)
            {
                yRatio = (float)textHeight / (float)textWidth; 
            }
            else
            {
                xRatio = (float)textWidth / (float)textHeight;
            }
            
            imagePreview.style.width = 300 * xRatio;
            imagePreview.style.height = 300 * yRatio;

            ApplyAlphaGradient();
        }
        private void ExportImage(Texture2D texture2D)
        {
            
            var path  = EditorUtility.SaveFilePanel(
                "Save Edit Texture",
                Application.dataPath + "/EditorScripting",
                outputName + ".png",
                "png");
            
            //converted bytes to texturePNG
            byte[] bytes = texture2D.EncodeToPNG();
            
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            
            File.WriteAllBytes(path, bytes);
            
            string pathString = path;
            int assetIndex = pathString.IndexOf("Assets" , StringComparison.Ordinal);
            //completedPath
            string filePath = pathString.Substring(assetIndex, path.Length - assetIndex);
            AssetDatabase.ImportAsset(filePath);
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = (Texture2D)AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D));
            Debug.Log("FilePath is + " + filePath);
            
            
        }

        private void ApplyAlphaGradient()
        {
            if (selectedTexture == null)
            {
                exportButton.SetEnabled(false);
                return;
            }
            exportButton.SetEnabled(true);
            outputTexture = selectedTexture;
            imagePreview.style.backgroundImage = outputTexture;
        }
        
        
        
        private void TintChanged(ChangeEvent<Color> evt)
        {
            throw new System.NotImplementedException();
        }

        private void AlphaInputChanged(ChangeEvent<int> evt)
        {
            throw new System.NotImplementedException();
        }

        private void AlphaSliderChanged(ChangeEvent<int> evt)
        {
            throw new System.NotImplementedException();
        }

        private void TextureOptionSelected(ChangeEvent<string> evt)
        {
            throw new System.NotImplementedException();
        }

        private void TextureSelected(ChangeEvent<Object> evt)
        {
            Debug.Log("Texture selected changed");
        }
        private void AlphaOptionSelected(ChangeEvent<string> evt)
        {
            throw new System.NotImplementedException();
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
}

