using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LocalizationCore : MonoBehaviour
{
    public bool OverrideLocal;

    [Tooltip("Will be used if OverrideLocal set to true")]
    public SystemLanguage SystemLanguageToUse;

    private static LocalizationCore _singleInstance;
    private Dictionary<string, string> _localisation;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (_singleInstance == null)
        {
            _singleInstance = this;
            DontDestroyOnLoad(this.gameObject);
            _localisation = TryLoadLocalisation();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static string Localize(string entry)
    {
        var localisation = _singleInstance?._localisation;

        if (localisation != null && localisation.TryGetValue(entry, out string localizedEntry) == true)
        {
            return localizedEntry;
        }
        else
        {
            return entry;
        }
    }

    Dictionary<string, string> TryLoadLocalisation()
    {
        SystemLanguage languageToLoad = OverrideLocal ? SystemLanguageToUse : Application.systemLanguage;

        if (languageToLoad == SystemLanguage.Unknown || languageToLoad == SystemLanguage.English)
            return null;

        var localisationFile = Resources.Load<TextAsset>(languageToLoad.ToString());

        if (localisationFile == null)
            return null;

        try
        {
            var fileContent = localisationFile.text;

            var serializer = new XmlSerializer(typeof(LocalizationXmlRepack));
            object container;

            using (TextReader reader = new StringReader(fileContent))
            {
                container = serializer.Deserialize(reader);
            }

            var packedDictionary = (LocalizationXmlRepack)container;
            var unpackedDictionary = new Dictionary<string, string>();

            foreach (var item in packedDictionary.Entries)
            {
                unpackedDictionary[item.Key] = item.Value;
            }

            return unpackedDictionary;
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
}