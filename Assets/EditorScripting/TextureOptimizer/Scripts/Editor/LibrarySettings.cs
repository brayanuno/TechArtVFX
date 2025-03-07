using NUnit.Framework;
using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
    public class LibrarySettings
    {
        
        public void StoreData(TextureImporter importer )
        {
            
        }
        
        public TextureImporter SetTextureType(TextureImporter importer , int value )
        {
            switch (value)
            {
                case 0:
                    importer.textureType = TextureImporterType.Sprite;
                    break;
                case 1:
                    importer.textureType = TextureImporterType.Default;
                    break;
                case 2:
                    importer.textureType = TextureImporterType.NormalMap;
                    break;
                case 3:
                    importer.textureType = TextureImporterType.GUI;
                    break;
            }
            return importer;
            
        }

        public TextureImporter SetSpriteMode(TextureImporter importer, int value)
        {
            switch (value)
            {
                case 0:
                    importer.spriteImportMode = SpriteImportMode.Single;
                    break;
                case 1:
                    importer.spriteImportMode = SpriteImportMode.Multiple;
                    break;
                case 2:
                    importer.spriteImportMode = SpriteImportMode.Polygon;
                    break;

            }
            return importer;
        }

        public TextureImporter SetFilterMode(TextureImporter importer, int value)
        {
            switch (value)
            {
                case 0:
                    importer.filterMode = FilterMode.Point;
                    break;
                case 1:
                    importer.filterMode = FilterMode.Bilinear;
                    break;
                case 2:
                    importer.filterMode = FilterMode.Trilinear;
                    break;

            }
            return importer;
        }
        public TextureImporter SetCompression(TextureImporter importer, int value)
        {
            switch (value)
            {
                case 0:

                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    break;
                case 1:
                    importer.textureCompression = TextureImporterCompression.CompressedLQ;
                    break;
                case 2:
                    importer.textureCompression = TextureImporterCompression.Compressed;
                    break;
                case 3:
                    importer.textureCompression = TextureImporterCompression.CompressedHQ;
                    break;
                
            }
            return importer;
        }
        public TextureImporter SetRGB(TextureImporter importer, bool value)
        {
            importer.sRGBTexture = value;
            return importer;
        }
        
        public TextureImporter SetPixelUnit(TextureImporter importer, int value)
        {
            importer.spritePixelsPerUnit = value;
            return importer;
        }
        
        public TextureImporter SetMaxTextureSizeOptimizer(TextureImporter importer)
        {
            int height;
            int width;
             
            importer.GetSourceTextureWidthAndHeight(out height, out width);
            int maxDimension = Mathf.Max(width, height);
            importer.maxTextureSize = CalculateMaxTextureSizeOptimizer(maxDimension);
            //save importer
             
            /*//read Textures from importer
            importer.ReadTextureSettings(textureImporterSettings);*/
            return importer;
        }

        private int CalculateMaxTextureSizeOptimizer(int maxDimension)
        {
            return (int)Mathf.Pow(2, (int)Mathf.Log(maxDimension -1,2) + 1);
        
        }
        
        
    }
