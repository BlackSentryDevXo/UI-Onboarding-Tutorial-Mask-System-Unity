using System.IO;
using SentryToolkit;
using UnityEditor;
using UnityEngine;

public class ButtonIDGenerator : EditorWindow
{
    private ButtonIDScriptableObject buttonIDList; // Reference to the ScriptableObject

    [MenuItem("Tools/ButtonID Enum Generator")]
    public static void ShowWindow()
    {
        GetWindow<ButtonIDGenerator>("ButtonID Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate ButtonID Enum", EditorStyles.boldLabel);

        buttonIDList = (ButtonIDScriptableObject)EditorGUILayout.ObjectField("Button ID List", buttonIDList, typeof(ButtonIDScriptableObject), false);

        if (buttonIDList == null)
        {
            EditorGUILayout.HelpBox("Assign a ButtonIDList ScriptableObject.", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("File Path:", EditorStyles.boldLabel);
        buttonIDList.saveFilePath = EditorGUILayout.TextField(buttonIDList.saveFilePath);

        if (GUILayout.Button("Generate Enum"))
        {
            GenerateButtonEnum();
        }
    }

    private void GenerateButtonEnum()
    {
        if (buttonIDList == null)
        {
            Debug.LogError("No ButtonIDList assigned!");
            return;
        }

        string filePath = buttonIDList.saveFilePath;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Debug.LogError("File path is empty! Please specify a valid path.");
            return;
        }

        string enumContent = "public enum ButtonID {\n    None,\n";

        foreach (string name in buttonIDList.buttonNames)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                enumContent += $"    {name},\n";
            }
        }

        enumContent += "}";

        // Write to file
        File.WriteAllText(filePath, enumContent);
        AssetDatabase.Refresh();

        Debug.Log("ButtonID enum generated successfully at: " + filePath);
    }
}