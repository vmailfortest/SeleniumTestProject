//using AutomatedTester.BrowserMob;
//using AutomatedTester.BrowserMob.HAR;
//using Fiddler;
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
//    class FiddlerReplacesHeaders
//    {
//        IWebDriver driver;

//        [SetUp]
//        public void TestInit()
//        {
//            // Clean log file
//            File.WriteAllText(@"C:\TEMP\WriteText.txt", "");

//            // Starting Fiddler proxy
//            FiddlerCoreStartupFlags flags = FiddlerCoreStartupFlags.Default;
//            FiddlerApplication.Startup(0, flags);
//            int proxyPort = FiddlerApplication.oProxy.ListenPort;

//            // Setup Selenium proxy
//            OpenQA.Selenium.Proxy proxy = new OpenQA.Selenium.Proxy();
//            proxy.HttpProxy = string.Format("127.0.0.1:{0}", proxyPort);

//            // Setup WebDriver
//            DesiredCapabilities caps = DesiredCapabilities.Chrome();
//            caps.SetCapability(CapabilityType.Proxy, proxy);
//            driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4444/wd/hub"), caps);
//        }

//        [TearDown]
//        public void TestCleanUp()
//        {
//            driver.Quit();
//            FiddlerApplication.Shutdown();
//        }

//        [Test]
//        public void FiddlerTest()
//        {
//            // Hook up the event for monitoring proxied traffic.
//            FiddlerApplication.AfterSessionComplete += delegate (Session targetSession)
//            {
//                string str = string.Empty;
//                str += $"Request: {targetSession.RequestMethod} {targetSession.fullUrl}\r\n";
//                str += $"Response: {targetSession.responseCode}\r\n";

//                using (StreamWriter file = new StreamWriter(@"C:\TEMP\WriteText.txt", true))
//                {
//                    file.WriteLine(str);
//                }
//            };

//            // Replace User-Agent to load mobile version of website
//            FiddlerApplication.RequestHeadersAvailable += delegate (Session targetSession)
//            {
//                targetSession.RequestHeaders.Remove("User-Agent");
//                targetSession.RequestHeaders.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 8_1 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Version/8.0 Mobile/12B411 Safari/600.1.4");
//            };

//            // Selenium steps
//            driver.Navigate().GoToUrl("http://citrus.ua");
//            Thread.Sleep(3000);
//        }
//    }
//}
