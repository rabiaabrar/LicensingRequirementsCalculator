using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LicensingRequirementsCalculator.Code;

namespace LicensingRequirementsCalculator.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void CalculateLicensingRequirements_1Laptop1Desktop_1LicenseRequired()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "App_Data/Test1.csv");
            var licensingRequirements = BusinessLogic.CalculateLicensingRequirements(filePath);

            Assert.AreEqual(licensingRequirements[0].NumberOfLicenses, 1);
        }

        [TestMethod]
        public void CalculateLicensingRequirements_1Laptop3Desktop_3LicenseRequired()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "App_Data/Test2.csv");
            var licensingRequirements = BusinessLogic.CalculateLicensingRequirements(filePath);

            Assert.AreEqual(licensingRequirements[0].NumberOfLicenses, 3);
        }

        [TestMethod]
        public void CalculateLicensingRequirements_1Laptop1DesktopDuplicates_1LicenseRequired()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "App_Data/Test3.csv");
            var licensingRequirements = BusinessLogic.CalculateLicensingRequirements(filePath);

            Assert.AreEqual(licensingRequirements[0].NumberOfLicenses, 1);
        }
    }
}
