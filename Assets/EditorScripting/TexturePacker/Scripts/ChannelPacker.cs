using System;
using System.Reflection;
using System.IO;
using SFB;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.Networking;
using UnityEngine.Rendering;
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
    private DropdownField useRGBA;
    private Button packChannelButton;
    private Button saveButton;

    private VisualElement imagePreview;
    
    private VisualElement containerAChannel;
    private VisualElement containerBChannel;
    private VisualElement containerGChannel;
    private VisualElement containerRChannel;
    
    
    
    /*current values of textures*/
    private Texture2D RchannelTexture;
    private Texture2D GchannelTexture;
    private Texture2D BchannelTexture;
    private Texture2D AchannelTexture;

    private enum rgba
    {
        useR,
        useG,
        UseB,
        useA
    }
    
    private enum useTypes
    {
       useR,
       useRG,
       useRGB,
       useRGBA
    }

    private useTypes currentType;
    private rgba currentChannel;
    
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
        containerRChannel = root.Q<VisualElement>("container-r-channel");
        containerGChannel = root.Q<VisualElement>("container-g-channel");
        containerBChannel = root.Q<VisualElement>("container-b-channel");
        containerAChannel = root.Q<VisualElement>("container-a-channel");
            
        textureRChannel = root.Q<ObjectField>("r-channel-texture");
        textureGChannel = root.Q<ObjectField>("g-channel-texture");
        textureBChannel = root.Q<ObjectField>("b-channel-texture");
        textureAChannel = root.Q<ObjectField>("a-channel-texture");
        useRGBA = root.Q<DropdownField>("use-rgba");
        
        packChannelButton = root.Q<Button>("button-packing");
        maxTextureSize = root.Q<DropdownField>("size-texture");
        imagePreview = root.Q<VisualElement>("image-preview");
        
        RChannelSelected = root.Q<DropdownField>("r-channel-selection");
        GChannelSelected = root.Q<DropdownField>("g-channel-selection");
        BChannelSelected = root.Q<DropdownField>("b-channel-selection");
        AChannelSelected = root.Q<DropdownField>("a-channel-selection");

        /*RChannelSelected.RegisterValueChangedCallback<string>(ChannelSelections);*/
        
        

        
        textureRChannel.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt,RChannelSelected.value,"R"));
        textureGChannel.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt,GChannelSelected.value,"G"));
        textureBChannel.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt,BChannelSelected.value,"B"));
        textureAChannel.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt,AChannelSelected.value,"A"));
        
        RChannelSelected.RegisterValueChangedCallback<string>(evt => ChannelChanged(evt,textureRChannel.value as Texture2D,ref RchannelTexture));
        /*GChannelSelected.RegisterValueChangedCallback<string>(evt => ChannelChanged(evt,"G"));
        BChannelSelected.RegisterValueChangedCallback<string>(evt => ChannelChanged(evt,"B"));
        
        AChannelSelected.RegisterValueChangedCallback<string>(evt => ChannelChanged(evt,"A"));*/
        
        
        
        useRGBA.RegisterValueChangedCallback<string>(SetOutputs);
        
        /*packChannelButton.clicked += () => PackImages();*/
        Initialize();
    }

    private void ChannelChanged(ChangeEvent<string> evt,Texture2D channelToChange ,ref Texture2D textureChanged)
    {
        
        textureChanged = AssignTexture(channelToChange, evt.newValue);
        
        updatePreview();
        /*TextureSelected(evt,RChannelSelected.value);*/
        /*Debug.Log("Channel "+ channelToChange +" changed to " + evt);*/
    }
    
    private void SetOutputs(ChangeEvent<string> evt)
    {
        switch (evt.newValue)
        {
            case "R":
                containerRChannel.style.opacity = 1f;
                containerBChannel.style.opacity = 0.2f;
                containerGChannel.style.opacity = 0.2f;
                containerAChannel.style.opacity = 0.2f;
                
                containerRChannel.SetEnabled(true);
                containerGChannel.SetEnabled(false);
                containerBChannel.SetEnabled(false);
                containerAChannel.SetEnabled(false);
                currentType = useTypes.useR;
                break;
            
            case "RG":
                containerRChannel.style.opacity = 1f;
                containerBChannel.style.opacity = 0.2f;
                containerGChannel.style.opacity = 1f;
                containerAChannel.style.opacity = 0.2f;
                
                containerRChannel.SetEnabled(true);
                containerGChannel.SetEnabled(true);
                containerBChannel.SetEnabled(false);
                containerAChannel.SetEnabled(false);
                currentType = useTypes.useRG;
                break;
            case "RGB":
                containerRChannel.style.opacity = 1f;
                containerBChannel.style.opacity = 1f;
                containerGChannel.style.opacity = 1f;
                containerAChannel.style.opacity = 0.2f;
                containerRChannel.SetEnabled(true);
                containerGChannel.SetEnabled(true);
                containerBChannel.SetEnabled(true);
                containerAChannel.SetEnabled(false);
                currentType = useTypes.useRGB;
                break;
            case "RGBA":
                containerRChannel.style.opacity = 1f;
                containerBChannel.style.opacity = 1f;
                containerGChannel.style.opacity = 1f;
                containerAChannel.style.opacity = 1f;
                containerRChannel.SetEnabled(true);
                containerGChannel.SetEnabled(true);
                containerBChannel.SetEnabled(true);
                containerAChannel.SetEnabled(true);
                currentType = useTypes.useRGBA;
                break;
        }
    }


    private void TextureSelected(ChangeEvent<Object> evt, string channelToLook,string rowChannelSelected)
    {
        
        Texture2D textureCopied =  evt.newValue as Texture2D;
        
        if (textureCopied != null)
        {
            switch (rowChannelSelected)
            {
                case "R":
                    RchannelTexture = AssignTexture(textureCopied, channelToLook);
                    break;
                case "G":
                    GchannelTexture = AssignTexture(textureCopied, channelToLook);
                    break;
                case "B":
                    
                    BchannelTexture = AssignTexture(textureCopied, channelToLook);
                    break;
                case "A":
                    AchannelTexture = AssignTexture(textureCopied, channelToLook);
                    break;
            }
        }
        /*if texture is null revert the channels*/
        else 
        {
            switch (rowChannelSelected)
            {
                case "R":
                    RchannelTexture = null;
                    break;
                case "G":
                    
                    GchannelTexture = null;
                    break;
                case "B":
                    BchannelTexture = null;
                    break;
                case "A":
                    AchannelTexture = null;
                    break;
            }
        }
        
        /*imagePreview.style.backgroundImage = GchannelTexture;*/
        updatePreview();
    }

    private Texture2D AssignTexture(Texture2D textureCopied, string channelSelected)
    {   
        int widthSize = Int32.Parse(textureCopied.width.ToString());
        int heightSize = Int32.Parse(textureCopied.height.ToString());
        
        Texture2D blankTexture = new Texture2D(widthSize, heightSize, TextureFormat.RGBA32, false);
        
        switch (channelSelected)
        {
            case "R":
                blankTexture = extractTexture(channelSelected, textureCopied);
                break;
            case "G":
                blankTexture = extractTexture(channelSelected, textureCopied);
                break;
            case "B":
                blankTexture = extractTexture(channelSelected, textureCopied);
                break;
            case "A":
                blankTexture = extractTexture(channelSelected, textureCopied);
                break;
        }
        blankTexture.Apply();
        return blankTexture;
    }
    
    private void Initialize()
    {
        
        /*int textureSize = Int32.Parse(maxTextureSize.value);*/
        currentType = useTypes.useR;
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
                    /*else
                    {
                        Color defaultColor = new Color(0, 0, 0, 1);
                        newTextureCopied.SetPixel(x, y,defaultColor);
                    }*/
                }
            }  
        
        newTextureCopied.Apply();
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
        int textureSize = Int32.Parse(maxTextureSize.value);
        
        Texture2D CombinedTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        
        Color redpixelColor = new Color(1, 0, 0, 1);
        Color greenPixelColor = new Color(0, 1, 0, 1);
        Color bluePixelColor = new Color(0, 0, 1, 1);
        Color alphaPixelColor = new Color(0, 0, 0, 1);
        
        // if R only take r channel
        if (currentType == useTypes.useR)
        {
            CombinedTexture = RchannelTexture;

        }
        
        if (currentType == useTypes.useRG)
        {
            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    if (RchannelTexture != null )
                    {
                        redpixelColor = RchannelTexture.GetPixel(x, y);
                        
                        //working on progress
                        
                        
                        
                        
                        
                    }
                    else
                    {
                        redpixelColor = new Color(0, 0, 0, 1);
                    }
                    
                    if (GchannelTexture != null )
                    {
                        greenPixelColor = GchannelTexture.GetPixel(x, y);
                    }
                    else
                    {
                        greenPixelColor = new Color(0, 0, 0, 1);
                    }

                    CombinedTexture.SetPixel(x, y, new Color(redpixelColor.b, greenPixelColor.g, 0, 1));

                }
            }
            
        }

        if (currentType == useTypes.useRGB)
        {
            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    if (RchannelTexture != null)
                    {
                        redpixelColor = RchannelTexture.GetPixel(x, y);
                    }
                    else
                    {
                        redpixelColor = new Color(0, 0, 0, 1);
                    }

                    if (GchannelTexture != null)
                    {
                        greenPixelColor = GchannelTexture.GetPixel(x, y);
                    }
                    else
                    {
                        greenPixelColor = new Color(0, 0, 0, 1);
                    }

                    if (BchannelTexture != null)
                    {
                        bluePixelColor = BchannelTexture.GetPixel(x, y);
                    }
                    else
                    {
                        bluePixelColor = new Color(0, 0, 0, 1);
                    }

                    CombinedTexture.SetPixel(x, y, new Color(redpixelColor.r, greenPixelColor.g, bluePixelColor.b, 1));

                }
            }
        }
        
        if (currentType == useTypes.useRGBA)
        {
            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    if (RchannelTexture != null)
                    {
                        redpixelColor = RchannelTexture.GetPixel(x, y);
                    }
                    else
                    {
                        redpixelColor = new Color(0, 0, 0, 1);
                    }

                    if (GchannelTexture != null)
                    {
                        greenPixelColor = GchannelTexture.GetPixel(x, y);
                    }
                    else
                    {
                        greenPixelColor = new Color(0, 0, 0, 1);
                    }

                    if (BchannelTexture != null)
                    {
                        bluePixelColor = BchannelTexture.GetPixel(x, y);
                    }
                    else
                    {
                        bluePixelColor = new Color(0, 0, 0, 1);
                    }
                    if (AchannelTexture != null)
                    {
                        alphaPixelColor = AchannelTexture.GetPixel(x, y);
                    }
                    else
                    {
                        alphaPixelColor = new Color(0, 0, 0, 1);
                    }
                    

                    CombinedTexture.SetPixel(x, y, new Color(redpixelColor.r, greenPixelColor.g, bluePixelColor.b, alphaPixelColor.a));

                }
            }
        }
        
        CombinedTexture.Apply();
        imagePreview.style.backgroundImage = CombinedTexture;
        
        Debug.Log("Finished");
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
