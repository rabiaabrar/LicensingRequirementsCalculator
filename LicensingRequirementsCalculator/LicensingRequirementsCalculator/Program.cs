using System;
using System.IO;
using LicensingRequirementsCalculator.Code;

namespace LicensingRequirementsCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();

            if (args.Length == 0)
            {
                Console.WriteLine("****************************************************");
                Console.WriteLine("*** Welcome to Licensing Requirements Calculator ***");
                Console.WriteLine("****************************************************");
                Console.WriteLine("Provide input file path in double quotes as the first and only unnamed parameter after \"LicensingRequirementsCalculator.exe\"");
                Console.WriteLine("Or just type \"LicensingRequirementsCalculator.exe sample\" to execute sample input file");
            }
            else
            {
                var inputFilePath = args[0] == "sample" ? Path.Combine(Environment.CurrentDirectory, "App_Data", "sample-small.csv") : args[0];
                var licensingRequirements = BusinessLogic.CalculateLicensingRequirements(inputFilePath);

                var inputtFileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFilePath);
                var outputFileName = string.Format("{0}-licensing-requirements.csv", inputtFileNameWithoutExtension);
                var outputFilePath = Path.Combine(Environment.CurrentDirectory, "Output", outputFileName);

                BusinessLogic.SaveLicensingRequirementsToFile(licensingRequirements, outputFilePath);

                Console.WriteLine("Output saved to file {0}", outputFilePath);
            }
        }
    }
}
