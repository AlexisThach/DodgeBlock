using System;
using System.Xml;
using System.Xml.Schema;

class XmlValidator
{
    static bool isValid = true;

    public static void Main(string[] args)
    {
        // Chemins des fichiers XML et XSD
        string xmlFilePath = "../xml/jeu.xml"; // Chemin du fichier XML
        string xsdFilePath = "../xsd/jeu.xsd"; // Chemin du fichier XSD

        // Valider le fichier XML
        ValidateXml(xmlFilePath, xsdFilePath);

        // Résultat
        if (isValid)
        {
            Console.WriteLine("Le document XML est valide et conforme au schéma XSD.");
        }
        else
        {
            Console.WriteLine("Le document XML contient des erreurs de validation.");
        }
    }

    static void ValidateXml(string xmlPath, string xsdPath)
    {
        // Charger le schéma XSD
        XmlSchemaSet schemas = new XmlSchemaSet();
        schemas.Add("http://www.univ-grenoble-alpes.fr/l3miage/game", xsdPath);

        // Configurer les paramètres de validation
        XmlReaderSettings settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Schemas = schemas
        };
        settings.ValidationEventHandler += ValidationEventHandler;

        try
        {
            // Lire et valider le document XML
            using (XmlReader reader = XmlReader.Create(xmlPath, settings))
            {
                while (reader.Read()) ; // Parcourir tout le document
            }
        }
        catch (XmlException ex)
        {
            Console.WriteLine($"Erreur XML : {ex.Message}");
            isValid = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur inattendue : {ex.Message}");
            isValid = false;
        }
    }

    static void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
        // Gérer les erreurs de validation
        Console.WriteLine($"Erreur de validation : {e.Message}");
        isValid = false;
    }
}