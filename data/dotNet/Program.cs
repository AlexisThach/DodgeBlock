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
        
        DodgeBlockDOM dodgeBlockDOM = new DodgeBlockDOM("../xml/joueur.xml");
        Console.WriteLine("-------------------- XMLReader -------------------------------");
        DodgeBlockXMLReader read = new DodgeBlockXMLReader();
        //read.AnalyseGlobal("../xml/joueur.xml"); 
        Console.WriteLine("--------------------------------------------------------------");
        read.GetTexteFromElements("../xml/joueur.xml" , "nom", "player");

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
    }
}
