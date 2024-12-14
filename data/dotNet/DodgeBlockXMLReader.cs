using System.Xml;
using System;

namespace DodgeBlock.data.dotNet;


public class DodgeBlockXMLReader {
    
    public void AnalyseGlobal(string filepath) {
        var settings = new XmlReaderSettings();
        using(var reader = XmlReader.Create(filepath,settings)){
            while(reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.XmlDeclaration :
                        Console.Write("Found XML declaration ( <? xml version = ’1.0 ’? >) ");
                        break;
                    case XmlNodeType.Document:
                        Console.WriteLine("Début du document");
                        break;
                    case XmlNodeType.Comment:
                        Console.Write("Comment = <!--{0}--> \n", reader.Value);
                        break;
                    case XmlNodeType.Element:
                        Console.WriteLine("Element: {0}", reader.Name);
                        // Lire et afficher les attributs de l'élément
                        if (reader.HasAttributes) {
                            Console.WriteLine("Attributs de {0}", reader.Name);
                            while (reader.MoveToNextAttribute()) {
                                Console.WriteLine(" - {0} = {1}", reader.Name, reader.Value);
                            }
                            // Revenir à l'élément après avoir lu les attributs
                            reader.MoveToElement();
                        }
                        break;
                    case XmlNodeType.Text:
                        Console.WriteLine("Texte: {0}",reader.Value);
                        break;
                    case XmlNodeType.EndElement:
                        Console.WriteLine("Fin de l'élément {0}", reader.Name);
                        break;
                }
            }
        }
    }

    // Méthode qui prend en paramètre le chemin d’un fichier XML, le nom d’un élément et l'élément parent et qui retourne
    // une liste de chaînes de caractères contenant le texte des éléments de ce nom.
    // Par exemple retourner tous les noms des joueurs. 
    public void GetTexteFromElements(string filepath, string element, string parent) {
        var settings = new XmlReaderSettings();
        using var reader = XmlReader.Create(filepath, settings);
        reader.MoveToContent();
        
        bool estParent = false; 
        while (reader.Read()) {
            switch (reader.NodeType) {
                case XmlNodeType.Element:
                    if (reader.Name == parent) {
                       estParent = true; 
                    }
                    if (estParent && reader.Name == element) {
                        Console.Write("Element {0} - ", reader.Name);
                        if (reader.Read() && reader.NodeType == XmlNodeType.Text) {
                            Console.WriteLine("Texte: {0}", reader.Value);
                        }
                    }
                    break;
                
                case XmlNodeType.EndElement:
                    if (reader.Name == parent) {
                        estParent = false; 
                    }
                    break;
            }
        }
    } 
}