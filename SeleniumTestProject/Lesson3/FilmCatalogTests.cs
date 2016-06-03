using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumTestProject.Lesson3
{
    [TestFixture]
    class FilmCatalogTests
    {
        static string url = "http://barancev.w.pw/php4dvd/";
        IWebDriver driver;

        public IWebDriver FirefoxDriverWithCustomProfile(string profileName)
        {
            // Pre-test
            string pathToCurrentUserProfiles = Environment.ExpandEnvironmentVariables("%APPDATA%") + @"\Mozilla\Firefox\Profiles\";
            string pathsToProfiles = Directory.GetDirectories(pathToCurrentUserProfiles, $"*.{profileName}", SearchOption.TopDirectoryOnly)[0];
            FirefoxProfile profile = new FirefoxProfile(pathsToProfiles);

            var driver = new FirefoxDriver(profile);

            return driver;
        }

        [SetUp]
        public void TestInit()
        {

            driver = new FirefoxDriver();

            //driver = new ChromeDriver("c:\\temp\\");

            //driver = new InternetExplorerDriver("c:\\temp\\");

            //driver = FirefoxDriverWithCustomProfile("SeleniumFirefoxProfile");


            driver.Navigate().GoToUrl(url);
        }

        [TearDown]
        public void TestCleanUp()
        {
            driver.Quit();
        }

        [Test]
        public void FilmCatalogSearchTest()
        {
            // Test
            LoginPage loginPage = new LoginPage(driver);
            CatalogPage catalogPage = loginPage.Login("admin", "admin");

            var listOfFilms = catalogPage.GetListOfFilms();

            string searchText = listOfFilms[new Random().Next(0, listOfFilms.Count-1)];
            catalogPage.SearchFor(searchText);

            var listAfterSearch = catalogPage.GetListOfFilms();

            // Assert
            foreach (var element in listAfterSearch)
            {
                element.Should().Contain(searchText);
                Console.WriteLine(element);
                if (!element.Contains(searchText))
                {
                    new Exception("In search results presented not valid result!");
                }
            }
        }

        [Test]
        public void FilmCatalogSessionIsSavedTest()
        {
            // Pre-test
            driver = FirefoxDriverWithCustomProfile("SeleniumFirefoxProfile");
            driver.Navigate().GoToUrl(url);

            // Test
            LoginPage loginPage = new LoginPage(driver);
            CatalogPage catalogPage = loginPage.Login("admin", "admin");
            Thread.Sleep(2000);
            //catalogPage.SearchFor(""); // Clear field;
            var listOfFilms = catalogPage.GetListOfFilms();

            string searchText = listOfFilms[new Random().Next(0, listOfFilms.Count - 1)];
            catalogPage.SearchFor(searchText);
            driver.Close();

            driver = FirefoxDriverWithCustomProfile("SeleniumFirefoxProfile");
            loginPage = new LoginPage(driver);
            driver.Navigate().GoToUrl(url);

            catalogPage = loginPage.Login("admin", "admin");

            var listAfterSearch = catalogPage.GetListOfFilms();

            // Assert
            foreach (var element in listAfterSearch)
            {
                element.Should().Contain(searchText);
                Console.WriteLine(element);
                if (!element.Contains(searchText))
                {
                    new Exception("In search results presented not valid result!");
                }
            }
            Thread.Sleep(3000);
        }
    }
}
