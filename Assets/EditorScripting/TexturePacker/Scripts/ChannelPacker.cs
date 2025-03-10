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

public class ChannelPacker : EditorWindow
{
    private ObjectField textureRChannel ;
    private Button packChannelButton;
    
    [MenuItem("Tools/ChannelPacker")]
    public static void OpenEditorWindow()
    {
        ChannelPacker window = (ChannelPacker)EditorWindow.GetWindow(typeof(ChannelPacker));
        window.titleContent = new GUIContent("ChannelPacker");
        window.maxSize = new Vector2(650, 600);
        window.minSize = window.maxSize;
        
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var visualTree =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/EditorScripting/TexturePacker/Scripts/Resources/ChannelPacker.uxml");
        VisualElement tree = visualTree.Instantiate();
        root.Add(tree);
        
        /*Assign Elements*/
  
        textureRChannel = root.Q<ObjectField>("texture-field");
        packChannelButton = root.Q<Button>("button-packing");
        
        textureRChannel.RegisterValueChangedCallback<Object>(evt => TextureRSelected(evt));
        packChannelButton.clicked += () => PackImages();
        Initialize();
    }

    private void TextureRSelected(ChangeEvent<Object> evt)
    {
        
    }

    private void Initialize()
    {
        Texture2D redChannelTexture = new Texture2D(selectedTexture.width, selectedTexture.height, TextureFormat.RGB24, false);
    }
    
    private void PackImages()
    {
        if (textureRChannel.value != null)
        {
            Debug.LogError("No texture selected!");
            
            // Create a new texture to store the red channel
            Texture2D redChannelTexture = new Texture2D(selectedTexture.width, selectedTexture.height, TextureFormat.RGB24, false);
            return;
        }
    }
    
}
