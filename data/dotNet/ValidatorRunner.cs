using System;
using System.Collections.Generic;

namespace DodgeBlock.data.dotNet;
public class ValidatorRunner
{
    private readonly XmlValidator _validator;

    public ValidatorRunner(XmlValidator validator)
    {
        _validator = validator;
    }

    public void Run()
    {
        // Chemins relatifs des fichiers XML et XSD
        List<string> xmlFiles = new List<string>
        {
            "../xml/jeu.xml",
            "../xml/joueurs.xml",
            "../xml/profil.xml",
            "../xml/sauvegarde.xml"
        };

        List<string> xsdFiles = new List<string>
        {
            "../xsd/jeu.xsd",
            "../xsd/joueurs.xsd",
            "../xsd/profil.xsd",
            "../xsd/sauvegarde.xsd"
        };

        for (int i = 0; i < xmlFiles.Count; i++)
        {
            Console.WriteLine($"Validation du fichier XML : {xmlFiles[i]} avec le XSD : {xsdFiles[i]}");
            _validator.ValidateXml(xmlFiles[i], xsdFiles[i]);
            Console.WriteLine();
        }
    }
}

