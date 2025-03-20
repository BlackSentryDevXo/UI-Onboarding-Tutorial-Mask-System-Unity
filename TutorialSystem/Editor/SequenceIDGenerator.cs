using System.IO;
using UnityEditor;
using UnityEngine;

public class SequenceIDGenerator : EditorWindow
{
    private SequenceIDScriptableObject sequenceIDList; // Reference to the ScriptableObject

    [MenuItem("Tools/SequenceID Enum Generator")]
    public static void ShowWindow()
    {
        GetWindow<SequenceIDGenerator>("SequenceID Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate SequenceID Enum", EditorStyles.boldLabel);

        sequenceIDList = (SequenceIDScriptableObject)EditorGUILayout.ObjectField("Sequence ID List", sequenceIDList, typeof(SequenceIDScriptableObject), false);

        if (sequenceIDList == null)
        {
            EditorGUILayout.HelpBox("Assign a SequenceIDList ScriptableObject.", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("File Path:", EditorStyles.boldLabel);
        sequenceIDList.saveFilePath = EditorGUILayout.TextField(sequenceIDList.saveFilePath);

        if (GUILayout.Button("Generate Enum"))
        {
            GenerateButtonEnum();
        }
    }

    private void GenerateButtonEnum()
    {
        if (sequenceIDList == null)
        {
            Debug.LogError("No SequenceIDList assigned!");
            return;
        }

        string filePath = sequenceIDList.saveFilePath;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Debug.LogError("File path is empty! Please specify a valid path.");
            return;
        }

        string enumContent = "public enum SequenceID {\n    None,\n";

        foreach (string name in sequenceIDList.sequenceNames)
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

        Debug.Log("SequenceID enum generated successfully at: " + filePath);
    }
}