using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;
using UnityEditor.ProjectWindowCallback;

public sealed class ScriptTemplateDataCreator : ScriptableObject
{
    const string Title = "Data template generator";

    const string DataTemplate = "ExampleDataTemplate.cs.txt";

    [MenuItem("Assets/Create/Data/ExampleData", false, 50)]
    static void CreateDataTemplate()
    {
        CreateAndRenameAsset($"{GetAssetPath()}/ExampleData.cs", GetIcon(),
            (name) => CreateTemplateInternal(GetTemplateContent(DataTemplate), name));
    }

    public static string CreateTemplate(string proto, string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return "Invalid filename";
        }
        proto = proto.Replace("#NAME#", SanitizeClassName(Path.GetFileNameWithoutExtension(fileName)));
        try
        {
            File.WriteAllText(AssetDatabase.GenerateUniqueAssetPath(fileName), proto);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        AssetDatabase.Refresh();
        return null;
    }

    static string SanitizeClassName(string className)
    {
        var sb = new StringBuilder();
        var needUp = true;
        foreach (var c in className)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(needUp ? char.ToUpperInvariant(c) : c);
                needUp = false;
            }
            else
            {
                needUp = true;
            }
        }
        return sb.ToString();
    }

    static string CreateTemplateInternal(string proto, string fileName)
    {
        var res = CreateTemplate(proto, fileName);
        if (res != null)
        {
            EditorUtility.DisplayDialog(Title, res, "Close");
        }
        return res;
    }

    static string GetTemplateContent(string proto)
    {
        var pathHelper = CreateInstance<ScriptTemplateDataCreator>();

        var path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(pathHelper)));
        DestroyImmediate(pathHelper);
        try
        {
            return File.ReadAllText(Path.Combine(path ?? "", proto));
        }
        catch
        {
            return null;
        }
    }

    static string GetAssetPath()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(path) && AssetDatabase.Contains(Selection.activeObject))
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                path = Path.GetDirectoryName(path);
            }
        }
        else
        {
            path = "Assets";
        }
        return path;
    }

    static Texture2D GetIcon()
    {
        return EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
    }

    static void CreateAndRenameAsset(string fileName, Texture2D icon, Action<string> onSuccess)
    {
        var action = CreateInstance<CustomEndNameAction>();
        action.Callback = onSuccess;
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, action, fileName, icon, null);
    }

    sealed class CustomEndNameAction : EndNameEditAction
    {
        [NonSerialized] public Action<string> Callback;

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            Callback?.Invoke(pathName);
        }
    }
}