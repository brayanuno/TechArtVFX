using System;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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

    private ObjectField textureRChannel;
    private ObjectField textureGChannel;
    private ObjectField textureBChannel;
    private ObjectField textureAChannel;

    private DropdownField maxTextureSize;
    private DropdownField useRGBA;

    private Button buttonPacking;
    private Button saveButton;

    private VisualElement imagePreview;

    private VisualElement containerAChannel;
    private VisualElement containerBChannel;
    private VisualElement containerGChannel;
    private VisualElement containerRChannel;

    private VisualElement previewRChannel;
    private VisualElement previewGChannel;
    private VisualElement previewBChannel;
    private VisualElement previewAChannel;

    /*current values of textures*/
    private Texture2D RchannelTexture;
    private Texture2D GchannelTexture;
    private Texture2D BchannelTexture;
    private Texture2D AchannelTexture;

    private TextField nameTexture;
    private Texture2D combinedTexture;

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

        previewRChannel = root.Q<VisualElement>("preview-r-channel");
        previewGChannel = root.Q<VisualElement>("preview-g-channel");
        previewBChannel = root.Q<VisualElement>("preview-b-channel");
        previewAChannel = root.Q<VisualElement>("preview-a-channel");

        textureRChannel = root.Q<ObjectField>("r-channel-texture");
        textureGChannel = root.Q<ObjectField>("g-channel-texture");
        textureBChannel = root.Q<ObjectField>("b-channel-texture");
        textureAChannel = root.Q<ObjectField>("a-channel-texture");
        useRGBA = root.Q<DropdownField>("use-rgba");

        buttonPacking = root.Q<Button>("button-packing");
        maxTextureSize = root.Q<DropdownField>("size-texture");
        imagePreview = root.Q<VisualElement>("image-preview");

        RChannelSelected = root.Q<DropdownField>("r-channel-selection");
        GChannelSelected = root.Q<DropdownField>("g-channel-selection");
        BChannelSelected = root.Q<DropdownField>("b-channel-selection");
        AChannelSelected = root.Q<DropdownField>("a-channel-selection");

        nameTexture = root.Q<TextField>("name-texture");
        /*RChannelSelected.RegisterValueChangedCallback<string>(ChannelSelections);*/

        /*Channels Textures Objects*/
        textureRChannel.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt, RChannelSelected.value, "R"));
        textureGChannel.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt, GChannelSelected.value, "G"));
        textureBChannel.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt, BChannelSelected.value, "B"));
        textureAChannel.RegisterValueChangedCallback<Object>(evt => TextureSelected(evt, AChannelSelected.value, "A"));

        /*DropDowns*/
        RChannelSelected.RegisterValueChangedCallback<string>(evt =>
            ChannelChanged(evt, textureRChannel.value as Texture2D, "R"));
        GChannelSelected.RegisterValueChangedCallback<string>(evt =>
            ChannelChanged(evt, textureGChannel.value as Texture2D, "G"));
        BChannelSelected.RegisterValueChangedCallback<string>(evt =>
            ChannelChanged(evt, textureBChannel.value as Texture2D, "B"));
        AChannelSelected.RegisterValueChangedCallback<string>(evt =>
            ChannelChanged(evt, textureAChannel.value as Texture2D, "A"));

        buttonPacking.clicked += () => PackTextures();


        useRGBA.RegisterValueChangedCallback<string>(SetOutputs);

        /*packChannelButton.clicked += () => PackImages();*/
        Initialize();
    }

    private void ChannelChanged(ChangeEvent<string> evt, Texture2D currentTexture, string rowChannelSelected)
    {
        
        switch (rowChannelSelected)
        {
            case "R":
                if (currentTexture != null)
                {
                    RchannelTexture = ExtractTextureByChannel(currentTexture, evt.newValue);
                    previewRChannel.style.backgroundImage = RchannelTexture;

                }

                break;

            case "G":
                if (currentTexture != null)
                {
                    
                    GchannelTexture = ExtractTextureByChannel(currentTexture, evt.newValue);
                    previewGChannel.style.backgroundImage = GchannelTexture;
                }

                break;
        }

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


    private void TextureSelected(ChangeEvent<Object> evt, string channelToLook, string rowChannelSelected)
    {
        /*Texture2D textureCopied =  evt.newValue as Texture2D;*/

        if (evt.newValue != null)
        {
            switch (rowChannelSelected)
            {
                case "R":
                    /*store the texture R */
                    RchannelTexture = ExtractTextureByChannel(evt.newValue as Texture2D, channelToLook);
                    previewRChannel.style.backgroundImage = RchannelTexture;
                    break;
                case "G":
                    /*store the texture G */
                    GchannelTexture = ExtractTextureByChannel(evt.newValue as Texture2D, channelToLook);
                    previewGChannel.style.backgroundImage = GchannelTexture;
                    break;
                case "B":
                    BchannelTexture = ExtractTextureByChannel(evt.newValue as Texture2D, channelToLook);
                    break;
                case "A":
                    AchannelTexture = ExtractTextureByChannel(evt.newValue as Texture2D, channelToLook);
                    break;
            }
        }
        /*Not Texture revert to Null*/
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

        /*imagePreview.style.backgroundImage = RchannelTexture;*/
        /*UpdatePreview();*/
    }

    private void StoreName(string value)
    {
        nameTexture.value = value;
    }

    private Texture2D ExtractTextureByChannel(Texture2D textureCopied, string channelSelected)
    {
        /*int widthSize = Int32.Parse(textureCopied.width.ToString());
        int heightSize = Int32.Parse(textureCopied.height.ToString());*/

        Texture2D blankTexture = new Texture2D(256, 256, TextureFormat.RGBA32, false);

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
        combinedTexture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
    }

    private Texture2D extractTexture(string channel, Texture2D texture)
    {
        /*int widthSize = Int32.Parse(texture.width.ToString());
        int heightSize = Int32.Parse(texture.height.ToString());
        */

        Texture2D newTextureCopied = new Texture2D(256, 256, TextureFormat.RGBA32, false);

        if (texture != null)
        {
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
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
                        float alphaC = texture.GetPixel(x, y).a;
                        newTextureCopied.SetPixel(x, y, new Color(alphaC, alphaC, alphaC, 1));
                    }
                }
            }
        }
        else
        {
            Debug.Log("TextureNOT FOUND");
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

    private void PackTextures()
    {
        int textureSize = Int32.Parse(maxTextureSize.value);

        
        Texture2D CombinedTextureRG = new Texture2D(256, 256, TextureFormat.RGBA32, false);

        float redpixelColor = 0f;
        float greenPixelColor = 0f;
        Color bluePixelColor = new Color(0, 0, 0, 1);
        Color alphaPixelColor = new Color(0, 0, 0, 1);

        // R Channel Only One Texture Preview
        if (currentType == useTypes.useR)
        {
            /*R ChannelCombineTexture*/
            combinedTexture = RchannelTexture;
        }
        
        if (currentType == useTypes.useRG)
        {
            /* RG ChannelCombineTexture */
            CombinedTextureRG = ExtractCombinedTexture(RChannelSelected.value,RchannelTexture );
            
        }

        if (currentType == useTypes.useRGB)
        {
            /* RG ChannelCombineTexture */
            CombinedTextureRG = ExtractCombinedTexture(RChannelSelected.value,RchannelTexture );
        }
        
        if (currentType == useTypes.useRGBA)
        {
            /* RG ChannelCombineTexture */
            CombinedTextureRG = ExtractCombinedTexture(RChannelSelected.value ,RchannelTexture);
        }
        
     
        imagePreview.style.backgroundImage = CombinedTextureRG;

    }

    private Texture2D ExtractCombinedTexture(string channelFirst, Texture2D texture )
    {
        Texture2D MergedTexture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        
        if (texture != null)
        {
            /*Running R Channel */
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    Color pixelColor = RchannelTexture.GetPixel(x, y);
                    
                    /*CalculateR Channel Only */
                    if (channelFirst == "R")
                    {
                        MergedTexture.SetPixel(x,y, new Color( pixelColor.r, 0, 0, 1));
                    }
                    else if (channelFirst == "G")
                    {
                        MergedTexture.SetPixel(x,y, new Color( pixelColor.g, 0, 0, 1));
                    }
                    else if (channelFirst == "B")
                    {
                        MergedTexture.SetPixel(x,y, new Color( pixelColor.b, 0, 0, 1));
                    }
                    else if (channelFirst == "A")
                    {
                        MergedTexture.SetPixel(x,y, new Color(pixelColor.a, 0, 0, 1));
                    }
                }
            }
            
            /*Running RG Channel */
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    Color pixelColor = GchannelTexture.GetPixel(x, y);
                    
                    /*CalculateR Channel Only */
                    if (GChannelSelected.value == "R")
                    {
                        MergedTexture.SetPixel(x,y, new Color( MergedTexture.GetPixel(x, y).r, pixelColor.r, 0, 1));
                    }
                    else if (GChannelSelected.value== "G")
                    {
                        MergedTexture.SetPixel(x,y, new Color( MergedTexture.GetPixel(x, y).r, pixelColor.g, 0, 1));
                    }
                    else if (GChannelSelected.value == "B")
                    {
                        MergedTexture.SetPixel(x,y, new Color( MergedTexture.GetPixel(x, y).r, pixelColor.b, 0, 1));
                    }
                    else if (GChannelSelected.value == "A")
                    {
                        MergedTexture.SetPixel(x,y, new Color(MergedTexture.GetPixel(x, y).r, pixelColor.a, 0, 1));
                    }
                }
            }  
            
            if( currentType == useTypes.useRGB)
            {
                
            }
            
            
            /*Running RGB Channel */
            
        }
        
        MergedTexture.Apply();
        return MergedTexture;
    }
    private void UpdatePreview()
    {

        return;
        /*if (currentType == useTypes.useRG)
        {

            /*Texture2D textureValueRed = extractTexture(RChannelSelected.value, ref RchannelTexture);#1#
            Texture2D textureValueGreen = extractTexture(GChannelSelected.value, GchannelTexture);

            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    /*R#1#
                    if (RchannelTexture != null)
                    {
                        //OutPut R look for corresponding channel
                        switch (RChannelSelected.value)
                        {
                            case "R":
                                /*take the R Channel#1#
                                redpixelColor = RchannelTexture.GetPixel(x, y).r;
                                break;
                            case "G":
                                /*take the G Channel#1#
                                redpixelColor = RchannelTexture.GetPixel(x, y).g;
                                break;
                            case "B":
                                /*take the B Channel#1#
                                redpixelColor = RchannelTexture.GetPixel(x, y).b;
                                break;
                            case "A":
                                /*take the A Channel#1#
                                redpixelColor = RchannelTexture.GetPixel(x, y).a;
                                break;
                        }
                    }

                    else
                    {
                        redpixelColor = 0f;
                    }

                    //Paint the new Texture
                    CombinedTextureRG.SetPixel(x, y, new Color(redpixelColor, 1, 0, 1));

                }
            }*/

        /*if (currentType == useTypes.useRGB)
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
        }*/


    }
}
/*private void PackImages()
{
    if (textureRChannel.value != null)
    {
        Debug.LogError("No texture selected!");

        // Create a new texture to store the red channel

        return;
    }
}*/
      
    


