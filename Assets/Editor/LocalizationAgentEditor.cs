using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LocalizationAgent))]
public class LocalizationAgentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if/*user presses*/(GUILayout.Button("Dump texts as XML"))
        {
            var instance = (LocalizationAgent)target;
            DumpAllTexts(instance.Texts);
        }
    }

    void DumpAllTexts(Text[] texts)
    {
        var length = texts.Length;
        LocalizationXmlEntry[] entries = new LocalizationXmlEntry[length];

        for (int i = 0; i < length; i++)
        {
            var content = texts[i].text;
            entries[i] = new LocalizationXmlEntry() { Key = content, Value=content };
        }

        LocalizationXmlRepack container = new LocalizationXmlRepack() { Entries = entries };

        var serializer = new XmlSerializer(typeof(LocalizationXmlRepack));
        var sceneName = SceneManager.GetActiveScene().name;
        var fileName = $"{Application.dataPath}\\{sceneName}Localization.xml";

        using (var stream = File.Create(fileName))
        {
            serializer.Serialize(stream, container);
        }
    }
}