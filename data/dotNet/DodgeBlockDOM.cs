using System;
using System.Collections.Generic;
using System.Xml;

namespace DodgeBlock.data.dotNet;
public class DodgeBlockDOM
{
    private XmlDocument doc;
    private XmlNode root;
    private XmlNamespaceManager nsmgr;

    public DodgeBlockDOM(string filename)
    {
        // Charger le document XML
        doc = new XmlDocument();
        doc.Load(filename);

        // Récupérer la racine et initialiser le gestionnaire de namespace
        root = doc.DocumentElement;
        nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("db", "http://www.univ-grenoble-alpes.fr/l3miage/dodgeblock");
    }
    
    // Compter le nombre total de joueurs
    public uint CountJoueur()
    {
        XmlNodeList joueurNodes = root.SelectNodes("//db:player", nsmgr);
        return (uint)joueurNodes.Count;
    }

    // Compter le nombre total de parties
    public uint CountPartie()
    {
        XmlNodeList partieNodes = root.SelectNodes("//db:partie", nsmgr);
        return (uint)partieNodes.Count;
    }

    // Récuperer les meilleurs scores pour chaque joueur
    public Dictionary<string, int> GetBestScores()
    {
        Dictionary<string, int> bestScores = new Dictionary<string, int>();

        // Sélectionner chaque joueur
        XmlNodeList players = root.SelectNodes("//db:player", nsmgr);
        foreach (XmlNode player in players)
        {
            string playerName = player.SelectSingleNode("db:nom", nsmgr)?.InnerText;
            int bestScore = 0;

            // Parcourir les scores des parties
            XmlNodeList scores = player.SelectNodes("db:parties/db:partie/db:score", nsmgr);
            foreach (XmlNode scoreNode in scores)
            {
                int score = int.Parse(scoreNode.InnerText);
                if (score > bestScore)
                    bestScore = score;
            }

            // Ajouter le meilleur score au dictionnaire
            if (!string.IsNullOrEmpty(playerName))
                bestScores[playerName] = bestScore;
        }

        return bestScores;
    }
}