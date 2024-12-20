using System.IO;
using System.Xml.Serialization;

namespace DodgeBlock.data.Entities
{
    public static class PlayerDataManager
    {
        private static string GetFilePath(string playerName)
        {
            return Path.Combine("PlayerData", $"{playerName}.xml");
        }

        public static void SavePlayerData(PlayerData playerData)
        {
            var serializer = new XmlSerializer(typeof(PlayerData));
            var filePath = GetFilePath(playerData.PlayerName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, playerData);
            }
        }

        public static PlayerData LoadPlayerData(string playerName)
        {
            var serializer = new XmlSerializer(typeof(PlayerData));
            var filePath = GetFilePath(playerName);

            if (!File.Exists(filePath))
            {
                return new PlayerData { PlayerName = playerName, LastScore = 0, HighestScore = 0 };
            }

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                return (PlayerData)serializer.Deserialize(stream);
            }
        }
    }
}