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
    private string path;
    
    private int widthTextureSize;
    private int heightTextureSize;

    private Button sortRGBA;
    private Button sortR;
    private Button sortG;
    private Button sortB;
    private Button sortA;
    private Button sortRGB;
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
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
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
        saveButton = root.Q<Button>("save-button");
        
        sortRGBA = root.Q<Button>("sort-rgba");
        sortRGB = root.Q<Button>("sort-rgb");
        sortR = root.Q<Button>("sort-r");
        sortG = root.Q<Button>("sort-g");
        sortB = root.Q<Button>("sort-b");
        sortA = root.Q<Button>("sort-a");
        
        
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
        
        /*Buttons*/
        saveButton.clicked += () => SaveButton();
        buttonPacking.clicked += () => PackTextures();

        sortRGBA.clicked += () => SortTexture(1);
        sortRGB.clicked += () => SortTexture(2);
        sortR.clicked += () => SortTexture(3);
        sortG.clicked += () => SortTexture(4);
        sortB.clicked += () => SortTexture(5);
        sortA.clicked += () => SortTexture(6);
        
        useRGBA.RegisterValueChangedCallback<string>(SetOutputs);
        
        Initialize();
    }

    private void SortTexture(int id )
    {
        
        /*RGBA Channel Sort*/
        if (id == 1)
        {
            imagePreview.style.backgroundImage = combinedTexture;
        }
        
        /*RGB Channel Sort*/
        if (id == 2)
        {
            imagePreview.style.backgroundImage = combinedTexture;
        }
        
        /*G Channel Sort*/
        if (id == 4)
        {
            Texture2D newTextureCopied = new Texture2D(widthTextureSize, heightTextureSize, TextureFormat.RGBA32, false);
        
            if (combinedTexture != null)
            {
                for (int y = 0; y < heightTextureSize; y++)
                {
                    for (int x = 0; x < widthTextureSize; x++)
                    {
                        float pixelColor = combinedTexture.GetPixel(x, y).g;
                        newTextureCopied.SetPixel(x, y, new Color(0,  pixelColor , 0, 1));
                        
                    }
                }
            } 
            newTextureCopied.Apply();
            imagePreview.style.backgroundImage = newTextureCopied;
        }
    }
    
        
        
    
    private void SaveButton()
    {
        Texture2D resizedTexture;
        
        int widthSize = Int32.Parse(maxTextureSize.value.ToString());
        int heightSize = Int32.Parse(maxTextureSize.value.ToString());
        
        Texture2D blankTexture = new Texture2D(widthSize,heightSize, TextureFormat.RGBA32, false);
        
        if (combinedTexture != null)
        {
            blankTexture = ResizeTexture2D(combinedTexture,widthSize , heightSize);
            
        }
        
        byte[] bytes =  blankTexture.EncodeToPNG();
        
        System.IO.File.WriteAllBytes(path, bytes);
  
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));  
   
    }
    
    Texture2D ResizeTexture2D(Texture2D texture, int width, int height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        RenderTexture.active = rt;

        Graphics.Blit(texture, rt);

        Texture2D result = new Texture2D(width, height, texture.format, false);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return result;
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
            
            case "B":
                if (currentTexture != null)
                {
                    BchannelTexture = ExtractTextureByChannel(currentTexture, evt.newValue);
                    previewBChannel.style.backgroundImage = BchannelTexture;
                }
                break;
            
            case "A":
                if (currentTexture != null)
                {
                    AchannelTexture = ExtractTextureByChannel(currentTexture, evt.newValue);
                    previewAChannel.style.backgroundImage = AchannelTexture;
                }
                break;
        }
        
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

    void GetMaxTextureSize(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null)
        {
            widthTextureSize = importer.maxTextureSize;
            heightTextureSize = importer.maxTextureSize;
        }
        else
        {
            Debug.LogError("Failed to get TextureImporter.");
        }
    }
    
    private void TextureSelected(ChangeEvent<Object> evt, string channelToLook, string rowChannelSelected)
    {
        
        
        /*Texture2D textureCopied =  evt.newValue as Texture2D;*/
        
        if (evt.newValue != null)
        {
            Texture2D TextS = evt.newValue as Texture2D;
            widthTextureSize = TextS.width; 
            heightTextureSize = TextS.height;
            /*GetMaxTextureSize(evt.newValue as Texture2D);*/
            
            nameTexture.value = evt.newValue.name;
            /*Getting path from texture*/
            string fullPath = AssetDatabase.GetAssetPath(evt.newValue);
            
            int lastSlashIndex = fullPath .LastIndexOf('/');
            path = fullPath.Substring(0, lastSlashIndex);
            path += "/" + nameTexture.value + ".png";
            
           
            switch (rowChannelSelected)
            {
                case "R":
                    /*store the texture R */
                    RchannelTexture = ExtractTextureByChannel(evt.newValue as Texture2D, channelToLook);
                    Debug.Log(RchannelTexture.width + "x" + RchannelTexture.height);
                    previewRChannel.style.backgroundImage = RchannelTexture;
                    break;
                case "G":
                    /*store the texture G */
                    GchannelTexture = ExtractTextureByChannel(evt.newValue as Texture2D, channelToLook);
                    previewGChannel.style.backgroundImage = GchannelTexture;
                    break;
                case "B":
                    BchannelTexture = ExtractTextureByChannel(evt.newValue as Texture2D, channelToLook);
                    previewBChannel.style.backgroundImage = BchannelTexture;
                    break;
                case "A":
                    AchannelTexture = ExtractTextureByChannel(evt.newValue as Texture2D, channelToLook);
                    previewAChannel.style.backgroundImage = AchannelTexture;
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
                    previewRChannel.style.backgroundImage= RchannelTexture;
                    break;
                case "G":
                    GchannelTexture = null;
                    previewGChannel.style.backgroundImage = GchannelTexture;
                    break;
                case "B":
                    BchannelTexture = null;
                    previewBChannel.style.backgroundImage = BchannelTexture;
                    break;
                case "A":
                    AchannelTexture = null;
                    previewAChannel.style.backgroundImage = AchannelTexture;
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

        Texture2D blankTexture = new Texture2D(widthTextureSize, heightTextureSize, TextureFormat.RGBA32, false);

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
       

        /*combinedTexture = new Texture2D(widthTextureSize, heightTextureSize, TextureFormat.RGBA32, false);*/
    }

    private Texture2D extractTexture(string channel, Texture2D texture)
    {
        /*int widthSize = Int32.Parse(texture.width.ToString());
        int heightSize = Int32.Parse(texture.height.ToString());
        */
        
        Texture2D newTextureCopied = new Texture2D(widthTextureSize, heightTextureSize, TextureFormat.RGBA32, false);
        
        if (texture != null)
        {
            for (int y = 0; y < heightTextureSize; y++)
            {
                for (int x = 0; x < widthTextureSize; x++)
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
            Debug.Log("Texture No Found");
        }

        newTextureCopied.Apply();
        return newTextureCopied;

        // Save the new texture as an asset

    }

    private void PackTextures()
    {
        
        combinedTexture  = new Texture2D(widthTextureSize, heightTextureSize, TextureFormat.RGBA32, false);
        

        // R Channel Only One Texture Preview
        if (currentType == useTypes.useR)
        {
            /*R ChannelCombineTexture*/
            if (RchannelTexture != null)
            {
                combinedTexture = RchannelTexture;
            }
        }
        
        if (currentType == useTypes.useRG)
        {
            /* RG ChannelCombineTexture */
            combinedTexture = ExtractCombinedTexture();
            
        }

        if (currentType == useTypes.useRGB)
        {
            /* RG ChannelCombineTexture */
            combinedTexture = ExtractCombinedTexture();
        }
        
        if (currentType == useTypes.useRGBA)
        {
            /* RG ChannelCombineTexture */
            combinedTexture = ExtractCombinedTexture();
        }

        if (combinedTexture != null)
        {
            imagePreview.style.backgroundImage = combinedTexture;
        }
        
        saveButton.SetEnabled(true);
    

    }

    private Texture2D ExtractCombinedTexture()
    {
        Texture2D MergedTexture = new Texture2D(widthTextureSize, heightTextureSize, TextureFormat.RGBA32, false);

        if (RchannelTexture != null)
        {
            /*Running R Channel */
            for (int y = 0; y < heightTextureSize; y++)
            {
                for (int x = 0; x < widthTextureSize; x++)
                {
                    Color pixelColor = RchannelTexture.GetPixel(x, y);

                    /*CalculateR Channel Only */
                    if (RChannelSelected.value == "R")
                    {
                        MergedTexture.SetPixel(x, y, new Color(pixelColor.r, 0, 0, 1));
                    }
                    else if (RChannelSelected.value == "G")
                    {
                        MergedTexture.SetPixel(x, y, new Color(pixelColor.g, 0, 0, 1));
                    }
                    else if (RChannelSelected.value  == "B")
                    {
                        MergedTexture.SetPixel(x, y, new Color(pixelColor.b, 0, 0, 1));
                    }
                    else if (RChannelSelected.value  == "A")
                    {
                        MergedTexture.SetPixel(x, y, new Color(pixelColor.a, 0, 0, 1));
                    }
                }
            }
        }
        else
        {
            Debug.Log("Not Texture Found");
        }

        if (GchannelTexture != null)
        {
            /*Running RG Channel */
            for (int y = 0; y <  heightTextureSize; y++)
            {
                for (int x = 0; x < widthTextureSize; x++)
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
        }

        if (BchannelTexture != null)
        {
            if (currentType == useTypes.useRGB || currentType == useTypes.useRGBA)
            {
                for (int y = 0; y < heightTextureSize; y++)
                {
                    for (int x = 0; x < widthTextureSize; x++)
                    {
                        Color pixelColor = BchannelTexture.GetPixel(x, y);

                        /*CalculateR Channel Only */
                        if (BChannelSelected.value == "R")
                        {
                            MergedTexture.SetPixel(x, y,
                                new Color(MergedTexture.GetPixel(x, y).r, MergedTexture.GetPixel(x, y).g, pixelColor.b,
                                    1));
                        }
                        else if (BChannelSelected.value == "G")
                        {
                            MergedTexture.SetPixel(x, y,
                                new Color(MergedTexture.GetPixel(x, y).r, MergedTexture.GetPixel(x, y).g, pixelColor.b,
                                    1));
                        }
                        else if (BChannelSelected.value == "B")
                        {
                            MergedTexture.SetPixel(x, y,
                                new Color(MergedTexture.GetPixel(x, y).r, MergedTexture.GetPixel(x, y).g, pixelColor.b,
                                    1));
                        }
                        else if (BChannelSelected.value == "A")
                        {
                            MergedTexture.SetPixel(x, y,
                                new Color(MergedTexture.GetPixel(x, y).r, MergedTexture.GetPixel(x, y).g, pixelColor.b,
                                    1));
                        }
                    }
                }
            }
        }
        
        if (BchannelTexture != null)
        {
            if (currentType == useTypes.useRGBA)
            {
                for (int y = 0; y < heightTextureSize; y++)
                {
                    for (int x = 0; x < widthTextureSize; x++)
                    {
                        Color pixelColor = AchannelTexture.GetPixel(x, y);

                        /*CalculateR Channel Only */
                        if (AChannelSelected.value == "R")
                        {
                            MergedTexture.SetPixel(x, y,
                                new Color(MergedTexture.GetPixel(x, y).r, MergedTexture.GetPixel(x, y).g, MergedTexture.GetPixel(x, y).b,
                                    pixelColor.a));
                        }
                        else if (AChannelSelected.value == "G")
                        {
                            MergedTexture.SetPixel(x, y,
                                new Color(MergedTexture.GetPixel(x, y).r, MergedTexture.GetPixel(x, y).g, MergedTexture.GetPixel(x, y).b,
                                    pixelColor.a));
                        }
                        else if (AChannelSelected.value == "B")
                        {
                            MergedTexture.SetPixel(x, y,
                                new Color(MergedTexture.GetPixel(x, y).r, MergedTexture.GetPixel(x, y).g, MergedTexture.GetPixel(x, y).b,
                                    pixelColor.a));
                        }
                        else if (AChannelSelected.value == "A")
                        {
                            MergedTexture.SetPixel(x, y,
                                new Color(MergedTexture.GetPixel(x, y).r, MergedTexture.GetPixel(x, y).g, MergedTexture.GetPixel(x, y).b,
                                    pixelColor.a));
                        }
                    }
                }
            }
        }

        /*Running RGB Channel */

        
        MergedTexture.Apply();
        return MergedTexture;
    }
 


    
}

      
    


