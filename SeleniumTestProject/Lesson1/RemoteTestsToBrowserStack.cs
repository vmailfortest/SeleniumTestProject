using System;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using FluentAssertions;
using System.Net;
using System.IO;
using System.Text;

namespace SeleniumTestProject.Lesson1
{
    [TestFixture]
    public class RemoteTestsToBrowserStack
    {
        static string url = "http://www.bbc.com/";

        static string USERNAME = "vmailfortest";
        static string ACCESS_KEY = "84ca6b21-b566-4ff7-aa04-9ba4985345b4";
        //static string SeleniumUrl = "http://" + USERNAME + ":" + ACCESS_KEY + "@ondemand.saucelabs.com:80/wd/hub";
        static string SeleniumUrl = "http://hub.browserstack.com/wd/hub";

        IWebDriver driver;

        [SetUp]
        public void TestInit()
        {
            DesiredCapabilities capability = DesiredCapabilities.Chrome();
            capability.SetCapability("browser", "Edge");
            capability.SetCapability("browser_version", "13.0");
            capability.SetCapability("os", "Windows");
            capability.SetCapability("os_version", "10");
            capability.SetCapability("resolution", "1024x768");
            capability.SetCapability("browserstack.user", "vmailfortest1");
            capability.SetCapability("browserstack.key", "bhVbiZzxJKWYENauEMqb");
            capability.SetCapability("browserstack.debug", "true");

            driver = new RemoteWebDriver(new Uri(SeleniumUrl), capability);
        }

        [TearDown]
        public void TestCleanUp()
        {
            string session = ((RemoteWebDriver)driver).SessionId.ToString();

            bool testResult = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            try
            {
                if (testResult == true) { new BrowserStackChangeSessionStatus(session, "Completed"); }
                else { new BrowserStackChangeSessionStatus(session, "Error"); };
            }
            finally
            {
                driver.Quit();
            }
        }


        [Test]
        public void CheckTravelSectionIsOpened()
        {
            // Act
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Travel")).Click();

            // Assert
            driver.FindElement(By.Id("brand")).Text.Should().Be("Travel");
        }

        [Test]
        public void CheckEarthSectionIsOpened()
        {
            // Act
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Earth")).Click();

            // Assert
            driver.FindElement(By.Id("brand")).Text.Should().Be("Earth");
        }

        [Test]
        public void CheckCultureSectionIsOpenedFails()
        {
            // Act
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Culture")).Click();

            // Assert
            driver.FindElement(By.Id("Culture")).Text.Should().Be("Culturrre");
        }

        public class BrowserStackChangeSessionStatus
        {
            public BrowserStackChangeSessionStatus(string sessionID, string status)
            {
                //  Completed, Error or Timeout.
                string reqString = "{\"status\":\"" + status + "\", \"reason\":\"\"}";

                byte[] requestData = Encoding.UTF8.GetBytes(reqString);
                Uri myUri = new Uri(string.Format("https://www.browserstack.com/automate/sessions/" + sessionID + ".json"));
                WebRequest myWebRequest = HttpWebRequest.Create(myUri);
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myWebRequest.ContentType = "application/json";
                myWebRequest.Method = "PUT";
                myWebRequest.ContentLength = requestData.Length;
                using (Stream st = myWebRequest.GetRequestStream()) st.Write(requestData, 0, requestData.Length);

                NetworkCredential myNetworkCredential = new NetworkCredential("vmailfortest1", "bhVbiZzxJKWYENauEMqb");
                CredentialCache myCredentialCache = new CredentialCache();
                myCredentialCache.Add(myUri, "Basic", myNetworkCredential);
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Credentials = myCredentialCache;

                myWebRequest.GetResponse().Close();
            }
        }
    }
}
