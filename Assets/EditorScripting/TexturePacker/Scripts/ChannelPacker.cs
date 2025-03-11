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
    private DropdownField RChannelSelected;
    private DropdownField GChannelSelected;
    private DropdownField BChannelSelected;
    private DropdownField AChannelSelected;
    
    private ObjectField textureRChannel ;
    private ObjectField textureGChannel;
    private ObjectField textureBChannel;
    private ObjectField textureAChannel;

    private DropdownField maxTextureSize;
    
    private Button packChannelButton;
    private Button saveButton;

    private VisualElement imagePreview;
    
    private Texture2D RchannelTexture;
    private Texture2D GchannelTexture;
    private Texture2D BchannelTexture;
    private Texture2D AchannelTexture;
    
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
  
        textureRChannel = root.Q<ObjectField>("r-channel-texture");
        textureGChannel = root.Q<ObjectField>("g-channel-texture");
        textureBChannel = root.Q<ObjectField>("b-channel-texture");
        textureAChannel = root.Q<ObjectField>("a-channel-texture");
        
        
        packChannelButton = root.Q<Button>("button-packing");
        maxTextureSize = root.Q<DropdownField>("size-texture");
        imagePreview = root.Q<VisualElement>("image-preview");
        
        RChannelSelected = root.Q<DropdownField>("r-channel-selection");
        GChannelSelected = root.Q<DropdownField>("g-channel-selection");
        BChannelSelected = root.Q<DropdownField>("b-channel-selection");
        AChannelSelected = root.Q<DropdownField>("a-channel-selection");
        
        textureRChannel.RegisterValueChangedCallback<Object>(evt => TextureRSelected(evt,RChannelSelected.value));
        textureGChannel.RegisterValueChangedCallback<Object>(evt => TextureRSelected(evt,GChannelSelected.value));
        textureBChannel.RegisterValueChangedCallback<Object>(evt => TextureRSelected(evt,BChannelSelected.value));
        textureAChannel.RegisterValueChangedCallback<Object>(evt => TextureRSelected(evt,AChannelSelected.value));
        
        
        /*packChannelButton.clicked += () => PackImages();*/
        Initialize();
    }

    private void TextureRSelected(ChangeEvent<Object> evt, string channelSelected)
    {
        Texture2D textureCopied =  evt.newValue as Texture2D;
        
        if (textureCopied != null)
        {
            switch (channelSelected)
            {
                case "R":
                    RchannelTexture = extractTexture(channelSelected, textureCopied);
                    break;
                case "G":
                    GchannelTexture = extractTexture(channelSelected, textureCopied);
                    break;
                case "B":
                    BchannelTexture = extractTexture(channelSelected, textureCopied);
                    break;
                case "A":
                    AchannelTexture = extractTexture(channelSelected, textureCopied);
                    break;
                default:
                    break;
            }
        }
        
        updatePreview();
    }
    
    private void Initialize()
    {
        /*int textureSize = Int32.Parse(maxTextureSize.value);*/
        
    }

    private Texture2D extractTexture(string channel, Texture2D texture)
    { 
        int widthSize = Int32.Parse(texture.width.ToString());
        int heightSize = Int32.Parse(texture.height.ToString());
        
        Texture2D newTextureCopied = new Texture2D(widthSize, heightSize, TextureFormat.RGBA32, false);
        
        for (int y = 0; y < heightSize ; y++)
        { 
            for (int x = 0; x < widthSize; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);
                    
                    /*getting the pixel Color from the texture*/
                    if (channel == "R")
                    {
                        Color rChannelColor = new Color(pixelColor.r, 0, 0, 1);
                        newTextureCopied.SetPixel(x, y, rChannelColor);
                    }
                    else if (channel == "G")
                    {
                        Color gChannelColor = new Color(0, pixelColor.g, 0, 1);
                        newTextureCopied.SetPixel(x, y, gChannelColor);
                    }
                    else if (channel == "B")
                    {
                        Color bChannelColor = new Color(0, 0, pixelColor.b, 1);
                        newTextureCopied.SetPixel(x, y, bChannelColor);
                    }
                    else if (channel == "A")
                    {
                        Color aChannelColor = new Color(0, 0, 0, pixelColor.a);
                        newTextureCopied.SetPixel(x, y, aChannelColor);
                    }
                    else
                    {
                        Color defaultColor = new Color(0, 0, 0, 1);
                        newTextureCopied.SetPixel(x, y,defaultColor);
                    }
                }
            }    
        newTextureCopied.Apply();
        Debug.Log("texture Created ");
        return newTextureCopied;
        // Save the new texture as an asset
        byte[] bytes = newTextureCopied.EncodeToPNG();
        string path = "Assets/RedChannelTexture.png";
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();

        Debug.Log("Red channel texture saved at: " + path);
    }
    
    

    private void updatePreview()
    {
        Debug.Log(maxTextureSize.value);
        int textureSize = Int32.Parse(maxTextureSize.value);
        Texture2D finalBackgroundTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                Color redpixelColor= RchannelTexture.GetPixel(x, y);
                /*Color greenPixelColor = GchannelTexture.GetPixel(x, y);
                Color bluePixelColor = BchannelTexture.GetPixel(x, y);
                Color alphaPixelColor= AchannelTexture.GetPixel(x, y);*/
                
                float redValue = redpixelColor.r;
                /*float greenValue = greenPixelColor.g;
                float blueValue = bluePixelColor.b;
                float alphaValue = alphaPixelColor.a;*/
                
                finalBackgroundTexture.SetPixel(x,y,new Color(redValue, 0, 0, 1));
            }
        }
        
        imagePreview.style.backgroundImage = finalBackgroundTexture as Texture2D;
        
    }

    private void PackImages()            
    {
        if (textureRChannel.value != null)
        {
            Debug.LogError("No texture selected!");
            
            // Create a new texture to store the red channel
            
            return;
        }
    }
    
}
