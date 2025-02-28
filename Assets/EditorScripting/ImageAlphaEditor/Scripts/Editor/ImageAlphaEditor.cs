using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Object = UnityEngine.Object;
using Slider = UnityEngine.UIElements.Slider;

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
            window.maxSize = new Vector2(320, 500);
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
            customTexValues = root.Q<VisualElement>("custom-text-values");
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
            alphaGradientField.RegisterValueChangedCallback<Gradient>(AlphaGradienChanged);
            tint.RegisterValueChangedCallback<Color>(TintChanged);
            exportButton.clicked += () => ExportImage(outputTexture);
            createTexButton.clicked += () => CreateTexture();
        
            exportButton.SetEnabled(false);
            imagePreview.style.backgroundImage = null;
            
            TextureOptionSelected(null);
            AlphaOptionSelected(null);


            alphaSliderInt.value = 255;

        }

        private void AlphaGradienChanged(ChangeEvent<Gradient> evt)
        {
            ApplyAlphaGradient();
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
            
            SetPreviewDimensions(textWidth, textHeight);

            ApplyAlphaGradient();
        }

        private void SetPreviewDimensions(int textWidth, int textHeight)
        {
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
        }

        private void ExportImage(Texture2D texture2D)
        {
            //Opening Panel
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
            //Writing png to bytes
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
            ApplyAlphaGradient();
        }

        private void AlphaInputChanged(ChangeEvent<int> evt)
        {
            
            alphaSliderInt.SetValueWithoutNotify(evt.newValue);
            ApplyAlphaGradient();
        }

        private void AlphaSliderChanged(ChangeEvent<int> evt)
        {
        
            alphaInput.SetValueWithoutNotify(evt.newValue);
            ApplyAlphaGradient();
        }

        private void TextureOptionSelected(ChangeEvent<string> evt)
        {
            //is the other one arr[1]
            if (textureOption.value != textureOption.choices[0])
            {
                textureField.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                customTexValues.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
            else
            {
                textureField.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                customTexValues.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }

            selectedTexture = null;
            textureField.value = null;
            imagePreview.style.backgroundImage = null;
            
            ApplyAlphaGradient();
        }

        private void TextureSelected(ChangeEvent<Object> evt)
        {
            if (evt.newValue == null)
            {
                selectedTexture = null;
                //Setting the texture on The Background
                imagePreview.style.backgroundImage = null;
                return;
            }
            
            outputName = evt.newValue.name + "Modified";
            selectedTexture = evt.newValue as Texture2D;
            SetPreviewDimensions(selectedTexture.width, selectedTexture.height);
            ApplyAlphaGradient();
        }
        private void AlphaOptionSelected(ChangeEvent<string> evt)
        {
            if (alphaDropdown.value != alphaDropdown.choices[0])
            {
                alphaSliderInt.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                alphaGradientField.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
            else
            {
                alphaSliderInt.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                alphaGradientField.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
            ApplyAlphaGradient();

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

