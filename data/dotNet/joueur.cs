using System;
using System.Xml;
using System.Xml.Schema;

public class XmlValidator
{
    public void Validate(string xmlFilePath, string xsdFilePath)
    {
        XmlReaderSettings settings = GetXmlReaderSettings(xsdFilePath);
        using (XmlReader reader = XmlReader.Create(xmlFilePath, settings))
        {
            try
            {
                while (reader.Read()) { }
                Console.WriteLine("Validation réussie.");
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"Erreur XML : {ex.Message}");
            }
        }
    }

    private XmlReaderSettings GetXmlReaderSettings(string xsdFilePath)
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas.Add("http://www.univ-grenoble-alpes.fr/l3miage/player", xsdFilePath);
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
        return settings;
    }

    private void ValidationCallback(object sender, ValidationEventArgs e)
    {
        switch (e.Severity)
        {
            case XmlSeverityType.Error:
                Console.WriteLine($"Erreur de validation : {e.Message}");
                break;
            case XmlSeverityType.Warning:
                Console.WriteLine($"Avertissement de validation : {e.Message}");
                break;
        }
    }
}
