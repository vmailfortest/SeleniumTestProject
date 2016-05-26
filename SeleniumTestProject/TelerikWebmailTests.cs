using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using SeleniumTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumTestProject
{
    [TestFixture]
    class TelerikWebmailTests
    {
        static string url = "http://demos.telerik.com/aspnet-ajax/webmail/default.aspx";
        IWebDriver driver;

        [SetUp]
        public void TestInit()
        {
            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(url);
        }

        [TearDown]
        public void TestCleanUp()
        {
            driver.Quit();
        }

        [Test]
        public void WebmailFoldersAreOpenedTest()
        {
            // Act
            TelerikWebmailPage homePage = new TelerikWebmailPage(driver);
            homePage.ClickOnSpecificFolder("Kendo UI");

            homePage.ExpandSpecificFolder("Kendo UI");

            homePage.VerifySubfoldersOfSpecificFolder("Kendo UI").Should().BeTrue();
        }
    }
}
