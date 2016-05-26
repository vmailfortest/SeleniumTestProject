using System;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using FluentAssertions;

namespace SeleniumTestProject
{
    [TestFixture]
    public class RemoteTestsToSauceLabs
    {
        static string url = "http://www.bbc.com/";

        static string USERNAME = "vmailfortest";
        static string ACCESS_KEY = "84ca6b21-b566-4ff7-aa04-9ba4985345b4";
        //static string SeleniumUrl = "http://" + USERNAME + ":" + ACCESS_KEY + "@ondemand.saucelabs.com:80/wd/hub";
        static string SeleniumUrl = "http://ondemand.saucelabs.com:80/wd/hub";

        IWebDriver driver;

        [SetUp]
        public void TestInit()
        {
            DesiredCapabilities capability = DesiredCapabilities.Chrome();
            capability.SetCapability("platform", "Windows 10");
            capability.SetCapability("version", "latest");
            capability.SetCapability("name", "My Selenium Test with Saucelabs");
            capability.SetCapability("tags", "SmokeTest");
            capability.SetCapability("name", TestContext.CurrentContext.Test.Name);
            capability.SetCapability("username", USERNAME);
            capability.SetCapability("accessKey", ACCESS_KEY);

            driver = new RemoteWebDriver(new Uri(SeleniumUrl), capability);
        }

        [TearDown]
        public void TestCleanUp()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            try
            {
                // Logs the result to Sauce Labs
                ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                // Terminates the remote webdriver session
                driver.Quit();
            }
        }


        [Test]
        public void SauceLabsTest()
        {
            // Act
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Travel")).Click();

            // Assert
            driver.FindElement(By.Id("brand")).Text.Should().Be("Travel");
        }
    }
}
