using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class TranslucentUIStartup
{
    static TranslucentUIStartup()
    {
        ////////////creating layers ///////////////////
        var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        var layerProp = tagManager.FindProperty("layers");
        for (var i = 8; i <= 29; i++)
        {
            var sp0 = layerProp.GetArrayElementAtIndex(i);

            if (sp0.stringValue.Equals("TranslucentUI")) break;
            if (sp0 != null)
                if (sp0.stringValue.Trim().Length == 0)
                {
                    sp0.stringValue = "TranslucentUI";
                    Debug.Log("TranslucentUI layer added");
                    break;
                }
        }

        tagManager.ApplyModifiedProperties();
        /////////////////////////////////////////////
    }
}