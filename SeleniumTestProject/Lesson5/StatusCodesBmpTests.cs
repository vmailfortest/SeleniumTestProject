//using AutomatedTester.BrowserMob;
//using AutomatedTester.BrowserMob.HAR;
//using FluentAssertions;
//using NUnit.Framework;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Firefox;
//using OpenQA.Selenium.IE;
//using OpenQA.Selenium.Remote;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace SeleniumTestProject.Lesson5
//{
//    [TestFixture]
//    class StatusCodesBmpTests
//    {
//        IWebDriver driver;
//        Client BmpClient;
//        Server BmpServer;

//        [SetUp]
//        public void TestInit()
//        {
//            // Setup proxy
//            BmpServer = new Server(@"c:\TEMP\browsermob-proxy-2.1.1\bin\browsermob-proxy.bat");
//            BmpServer.Start();

//            BmpClient = BmpServer.CreateProxy();
//            BmpClient.NewHar();

//            var seleniumProxy = new Proxy { HttpProxy = BmpClient.SeleniumProxy };

//            // Setup WebDriver
//            DesiredCapabilities caps = DesiredCapabilities.InternetExplorer();
//            caps.SetCapability(CapabilityType.Proxy, seleniumProxy);
//            driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4444/wd/hub"), caps);
//        }

//        [TearDown]
//        public void TestCleanUp()
//        {
//            driver.Quit();
//            BmpClient.Close();
//            BmpServer.Stop();
//        }

//        [Test]
//        public void CheckReturnedStatusCodesPassedTest()
//        {
//            bool result = true;

//            driver.Navigate().GoToUrl("http://gmail.com");
//            driver.Navigate().GoToUrl("http://software-testing.ru");

//            HarResult harData = BmpClient.GetHar();

//            foreach (var element in harData.Log.Entries)
//            {
//                if (element.Response.Status.ToString().StartsWith("4") || element.Response.Status.ToString().StartsWith("5"))
//                {
//                    result = false;

//                    Console.WriteLine(
//                        $"Request: {element.Request.Method} {element.Request.Url}\r\n" +
//                        $"Response: {element.Response.Status} {element.Response.StatusText}\r\n"
//                    );
//                }
//            }

//            result.Should().BeTrue("Some of resources returned an error while loading");
//        }

//        [Test]
//        public void CheckReturnedStatusCodesFailedTest()
//        {
//            bool result = true;

//            driver.Navigate().GoToUrl("http://google.com");
//            driver.Navigate().GoToUrl("http://ya.ru/NoSuchPage.html");
//            driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/status_codes/500");
//            driver.Navigate().GoToUrl("http://software-testing.ru");

//            HarResult harData = BmpClient.GetHar();

//            foreach (var element in harData.Log.Entries)
//            {
//                if (element.Response.Status.ToString().StartsWith("4") || element.Response.Status.ToString().StartsWith("5"))
//                {
//                    result = false;

//                    Console.WriteLine(
//                        $"Request: {element.Request.Method} {element.Request.Url}\r\n" +
//                        $"Response: {element.Response.Status} {element.Response.StatusText}\r\n"
//                    );
//                }
//            }

//            result.Should().BeTrue("Some of resources returned an error while loading");
//        }
//    }
//}
