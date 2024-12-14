using System;
using System.Xml;
using System.Xml.Schema;

namespace DodgeBlock.data.dotNet;

public class XmlValidator
{
    public void ValidateXml(string xmlFilePath, string xsdFilePath)
    {
        try
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, xsdFilePath);

            XmlReaderSettings settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemaSet
            };
            settings.ValidationEventHandler += ValidationEventHandler;

            using (XmlReader reader = XmlReader.Create(xmlFilePath, settings))
            {
                while (reader.Read()) { } // Lecture pour validation
            }

            Console.WriteLine("Validation réussie : Le fichier est conforme au schéma.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la validation : {ex.Message}");
        }
    }

    private void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
        Console.WriteLine($"Erreur : {e.Message}");
    }
}

