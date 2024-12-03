using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Drawing;
using UnityEditor.UIElements;

[InitializeOnLoad]
public static class CustomHierarchyOptions 
{
    static CustomHierarchyOptions()
    {
        EditorApplication.hierarchyWindowItemOnGUI += hierarchyWindowItemOnGui;
    }

    static void hierarchyWindowItemOnGui(int id , Rect rect)
    {
        //DrawActiveToggleButton(id, rect);

        //AddInfoScriptToHierarchyGameObjects(id);

        //buttons
        //DrawInfoButton(id, rect, "");
        DrawZoomButton(id, rect, "Zoom And Frame This Object");
        //DrawPrefabButton(id, rect, "Save as Prefab");
        //DrawDeleteButton(id, rect, "DeleteObject");
        
    }

    #region ToggleVisibility
    static Rect DrawRect(float x , float y , float size )
    {
        return new Rect(x, y, size , size);
    }

    static void DrawButtonWithToggle(int id, float x ,float y , float size)
    {
        GameObject gameObj = EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObj)
        {
            Rect r = DrawRect(x, y, size);
            gameObj.SetActive(GUI.Toggle(r,gameObj.activeSelf,string.Empty));

        }
    }

    static void DrawActiveToggleButton(int id ,Rect rect)
    {
        DrawButtonWithToggle(id, rect.x - 20, rect.y + 3, 10);
    }
    #endregion


    #region CustomInfoToggleButon
    // DrawInfoIcon
    static void DrawButtonWithCustomTexture (float x, float y, float size , string name, Action action,GameObject gameObj, string tooltip)
    {
        if(gameObj)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fixedHeight = 0;
            guiStyle.fixedWidth = 0;
            guiStyle.stretchHeight = true;
            guiStyle.stretchWidth = true;


            Rect r = DrawRect(x, y, size);
            Texture t = Resources.Load(name) as Texture;

            GUIContent guiContent = new GUIContent();
            guiContent.image = t;
            guiContent.text = "";
            guiContent.tooltip = tooltip;

            bool isClicked = GUI.Button(r, guiContent, guiStyle);

            if (isClicked)
            {
                action.Invoke();
            }
        }
    }

    static void DrawInfoButton(int id, Rect rec, string toolTip)
    {
        GameObject gameObj = EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObj)
        {
            bool hasInfoScriptComp = gameObj.GetComponent<Info>();
            if (hasInfoScriptComp)
            {
                Info infoScript = gameObj.GetComponent<Info>();
                if (infoScript)
                {
                    toolTip = infoScript.infoText;
                }
            }
        }

        DrawButtonWithCustomTexture(rec.x + 150, rec.y + 2, 14, "Icon", () => { } , gameObj, toolTip);

    }

    //adding the info component to all game objects on hierarchy
    static void AddInfoScriptToHierarchyGameObjects(int id)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        if (gameObject)
        {
                bool hasInfoComponent = gameObject.GetComponent<Info>();
                if (!hasInfoComponent)
                {
                    gameObject.AddComponent<Info>();
                }
            
        }

    }
    #endregion

    static void DrawZoomButton(int id , Rect rect, string tooltip)
    {
        GameObject gameObj = EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObj)
        {
            DrawButtonWithCustomTexture(rect.x + 175, rect.y + 2, 14, "zoom_in",
                                    () =>
        {
            Selection.activeGameObject = gameObj;
            SceneView.FrameLastActiveSceneView();
        }, gameObj, tooltip);

        }
    }

    static void DrawPrefabButton(int id , Rect rect, string toolTip)
    {
        GameObject gameObj = EditorUtility.InstanceIDToObject(id) as GameObject;

        if (gameObj)
        {
            DrawButtonWithCustomTexture(rect.x + 198, rect.y, 18, "Prefab", () =>
        {
            const string pathToPrefabsFolder = "Assets/EditorScripting/HierarchyInfo/Prefabs";
            bool doesPrefabsFolderExist = AssetDatabase.IsValidFolder(pathToPrefabsFolder);
            if (!doesPrefabsFolderExist)
            {
                AssetDatabase.CreateFolder("Assets/EditorScripting/HierarchyInfo", "Prefabs");
            }
            string prefabName = gameObj.name + ".prefab";
            string prefabPath = pathToPrefabsFolder + "/" + prefabName;
            
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObj, prefabPath);
            EditorGUIUtility.PingObject(prefab);

        }, gameObj, toolTip);               
        }
    }

    static void DrawDeleteButton(int id, Rect rect, string toolTip)
    {
        GameObject gameObj = EditorUtility.InstanceIDToObject(id) as GameObject;
        if(gameObj)
        {
            DrawButtonWithCustomTexture(rect.x + 260, rect.y + 2, 14, "delete", () =>
            {
                UnityEngine.Object.DestroyImmediate(gameObj);
            }, gameObj, toolTip);
        }


    }
}
