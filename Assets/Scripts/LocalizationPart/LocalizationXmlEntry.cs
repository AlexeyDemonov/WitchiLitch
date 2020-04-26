using System.Xml.Serialization;

public class LocalizationXmlEntry
{
    [XmlElement]
    public string Key { get; set; }

    [XmlElement]
    public string Value { get; set; }
}