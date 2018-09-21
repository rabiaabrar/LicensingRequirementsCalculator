using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using LicensingRequirementsCalculator.Models;

namespace LicensingRequirementsCalculator.Code
{
    public abstract class BusinessLogic
    {
        public static List<LicensingRequirements> CalculateLicensingRequirements(string filePath)
        {
            var keys = new HashSet<Tuple<int, int>>();
            var applicationsDictionary = new SortedList<int, LicensingRequirements>();

            using (var reader = new StreamReader(File.OpenRead(filePath)))
            {
                //Assuming that columns are always in sequence, ignore header
                reader.ReadLine();

                //Read all remaining lines from file
                while (!reader.EndOfStream)
                {
                    var fileLine = reader.ReadLine();
                    var dataRow = fileLine.Split(',');
                    var computerId = int.Parse(dataRow[0]);
                    var applicationId = int.Parse(dataRow[2]);
                    //Assuming that the same Application can't be installed on the same Computer by different Users, we take ComputerId+ApplicationId to be primary key
                    var key = Tuple.Create(computerId, applicationId);

                    //Check if primary key already exists, then don't add again
                    if (!keys.Contains(key))
                    {
                        keys.Add(key);
                        var computerType = (ComputerTypes)Enum.Parse(typeof(ComputerTypes), dataRow[3], true);

                        //A fast, memory conserving way to count all Laptops and Desktops for each ApplicationId is to do it along the way in a dictionary
                        if (!applicationsDictionary.ContainsKey(applicationId))
                            applicationsDictionary.Add(applicationId, new LicensingRequirements
                            {
                                ApplicationId = applicationId,
                                Desktops = 0,
                                Laptops = 0,
                                NumberOfLicenses = 0
                            });

                        if (computerType == ComputerTypes.Desktop)
                            applicationsDictionary[applicationId].Desktops++;
                        else if (computerType == ComputerTypes.Laptop)
                            applicationsDictionary[applicationId].Laptops++;
                    }
                }
            }

            //For each application id, use the provided business logic to calculate number of licenses required
            foreach (var licensingRequirement in applicationsDictionary)
            {
                int desktops = licensingRequirement.Value.Desktops;
                int laptops = licensingRequirement.Value.Laptops;

                //You need at least as many licenses as there are Desktops
                licensingRequirement.Value.NumberOfLicenses = desktops;

                //If there are more Laptops than Desktops then you need 1 license per additional 2 Laptops
                if (laptops > desktops)
                    licensingRequirement.Value.NumberOfLicenses += (int)Math.Ceiling(((decimal)laptops - desktops) / 2);
            }

            return applicationsDictionary.Values.ToList();
        }

        public static void SaveLicensingRequirementsToFile(List<LicensingRequirements> licensingRequirements, string outputFilePath)
        {
            //Create output directory if doesn't exist already
            var outputFileDirectory = Path.GetDirectoryName(outputFilePath);
            Directory.CreateDirectory(outputFileDirectory);

            //Write all LicensingRequirements to output file in csv format
            File.WriteAllLines(outputFilePath, ConvertToCSVArray(licensingRequirements));
        }

        private static string[] ConvertToCSVArray(List<LicensingRequirements> licensingRequirements)
        {
            string headerLine = "ApplicationId,Desktops,Laptops,NumberOfLicenses";

            var csvArray =
                new[] { headerLine }
                .Concat(licensingRequirements
                .Select(r => string.Format("{0},{1},{2},{3}", r.ApplicationId, r.Desktops, r.Laptops, r.NumberOfLicenses)))
                .ToArray();

            return csvArray;
        }
    }
}