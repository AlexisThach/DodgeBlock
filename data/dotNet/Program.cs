using System;

namespace DodgeBlock.data.dotNet;
public class Program
{
    public static void Main(string[] args)
    {
        XmlValidator validator = new XmlValidator();
        ValidatorRunner runner = new ValidatorRunner(validator);

        Console.WriteLine("Bienvenue dans le validateur XML/XSD !");
        runner.Run();
        
        DodgeBlockDOM dodgeBlockDOM = new DodgeBlockDOM("../xml/joueurs.xml");
        Console.WriteLine("-------------------------- XMLReader -------------------------------");
        DodgeBlockXMLReader read = new DodgeBlockXMLReader();
        read.AnalyseGlobal("../xml/joueurs.xml"); 
        Console.WriteLine("----------------- FIN D'ANALYSE GLOBALE DU FICHIER XML ------------------");
        
        read.GetTexteFromElements("../xml/joueurs.xml" , "nom", "player");

        Console.WriteLine("------------------------- DOM --------------------------------");
        Console.WriteLine("Nombre de joueurs : " + dodgeBlockDOM.CountJoueur());
        Console.WriteLine("Nombre total de parties : " + dodgeBlockDOM.CountPartie());
        
        // Récupération des meilleurs scores
        var bestScores = dodgeBlockDOM.GetBestScores();
        Console.WriteLine("Meilleurs scores par joueur :");
        foreach (var score in bestScores)
        {
            Console.WriteLine($"Joueur : {score.Key}, Meilleur Score : {score.Value}");
        }
        
        // lecture du fichier XML et conversion en objets
        Console.WriteLine("\n---------------- Désérialisation ----------------");
        Players players = XmlSerializer.Deserialization<Players>("../xml/joueurs.xml");
        Console.WriteLine($"Nombre total de joueurs : {players.ListePlayer.Count}");

        foreach (var player in players.ListePlayer)
        {
            Console.WriteLine($"\nJoueur : {player.Nom}");
            Console.WriteLine($"Position : ({player.PosX}, {player.PosY})");
            Console.WriteLine($"Vitesse : ({player.SpeedX}, {player.SpeedY})");

            Console.WriteLine("Scores des parties :");
            foreach (var partie in player.ListePartie)
            {
                Console.WriteLine($"  Date : {partie.Date}, Score : {partie.Score}");
            }
        }

        // Écriture des objets vers un nouveau fichier XML
        Console.WriteLine("\n---------------- Sérialisation ----------------");
        Console.WriteLine("Sérialisation des données dans : " + "../xml/joueurs_output.xml");
        XmlSerializer.Serialization(players, "../xml/joueurs_output.xml");
    }
}
