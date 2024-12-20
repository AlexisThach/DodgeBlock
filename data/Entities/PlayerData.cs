using System.Xml.Serialization;

namespace DodgeBlock.data.Entities
{
    public class PlayerData
    {
        public string PlayerName { get; set; }
        public int LastScore { get; set; }
        public int HighestScore { get; set; }
    }
}
