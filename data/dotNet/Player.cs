using System.Collections.Generic;
using System.Xml.Serialization;

namespace DodgeBlock.data.dotNet;

public class Player
{
    [XmlElement("nom")]
    public string Nom { get; set; }

    [XmlElement("posX")]
    public float PosX { get; set; }

    [XmlElement("posY")]
    public float PosY { get; set; }

    [XmlElement("speedX")]
    public float SpeedX { get; set; }

    [XmlElement("speedY")]
    public float SpeedY { get; set; }

    [XmlArray("parties")]
    [XmlArrayItem("partie")]
    public List<Partie> ListePartie { get; set; }

    public Player()
    {
        ListePartie = new List<Partie>();
    }
}