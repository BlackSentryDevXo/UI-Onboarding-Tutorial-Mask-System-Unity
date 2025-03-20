using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SequenceDatabase", menuName = "UI/SequenceID DB")]
public class SequenceIDScriptableObject : ScriptableObject
{
    [Tooltip("Specify where to save the generated Tutorial Sequence enum.")]
    public string saveFilePath = "Assets/Scripts/Systems/UISystems/Tutorials/Enum/SequenceID.cs";

    [Tooltip("Enter tutorial sequence names to include in the enum.")]
    public List<string> sequenceNames = new List<string>();

    /*
        You can remove this button property and remove the NaughtyAttributes namespace 
        if you don't need it and want to use the Editor
    */
    [Button]
    private void GenerateButtonEnum()
    {
        string filePath = saveFilePath;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Debug.LogError("File path is empty! Please specify a valid path.");
            return;
        }

        string enumContent = "public enum SequenceID {\n    None,\n";

        foreach (string name in sequenceNames)
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