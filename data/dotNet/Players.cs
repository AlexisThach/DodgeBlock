using System.Xml.Serialization;
using System.Collections.Generic;

namespace DodgeBlock.data.dotNet;

[XmlRoot("players", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/dodgeblock")]
public class Players
{
    [XmlElement("player")] public List<Player> ListePlayer { get; set; }

    public Players()
    {
        ListePlayer = new List<Player>();
    }
}
