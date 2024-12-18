using System.Xml.Serialization;


namespace DodgeBlock.data.dotNet;

public class Partie
{
    [XmlAttribute("date")]
    public string Date { get; set; }

    [XmlElement("score")]
    public int Score { get; set; }
}