using System.Xml.Serialization;

[XmlRoot("Localization")]
public class LocalizationXmlRepack
{
    [XmlArray("Entries")]
    [XmlArrayItem("Entry")]
    public LocalizationXmlEntry[] Entries { get; set; }
}