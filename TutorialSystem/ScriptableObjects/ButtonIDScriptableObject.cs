using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonsDatabase", menuName = "UI/Buttons DB")]
public class ButtonIDScriptableObject : ScriptableObject
{
    [Tooltip("Specify where to save the generated ButtonID enum.")]
    public string saveFilePath = "Assets/Scripts/Systems/UISystems/Tutorials/Enum/ButtonID.cs";

    [Tooltip("Enter button names to include in the enum.")]
    public List<string> buttonNames = new List<string>();

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

        string enumContent = "public enum ButtonID {\n    None,\n";

        foreach (string name in buttonNames)
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