using System;

namespace XmlValidatorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            XmlValidator validator = new XmlValidator();
            ValidatorRunner runner = new ValidatorRunner(validator);

            Console.WriteLine("Bienvenue dans le validateur XML/XSD !");
            runner.Run();
        }
    }
}