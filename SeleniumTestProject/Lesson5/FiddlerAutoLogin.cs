//using Fiddler;
//using NUnit.Framework;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Remote;
//using System;
//using System.IO;
//using System.Threading;

//namespace SeleniumTestProject.Lesson5
//{
//    [TestFixture]
//    class FiddlerAutoLogin
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
//            DesiredCapabilities caps = DesiredCapabilities.Firefox();
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
//        public void FiddlerAutoLoginTest()
//        {
//            // Replace User-Agent to load mobile version of website
//            FiddlerApplication.RequestHeadersAvailable += delegate (Session targetSession)
//            {
//                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("test:test");
//                string creds = Convert.ToBase64String(plainTextBytes);
//                targetSession.RequestHeaders.Add("Authorization", "Basic " + creds);
//            };

//            // Selenium steps
//            driver.Navigate().GoToUrl("http://vs-lenovo:808/HtmlPage.html");

//            driver.SwitchTo().Frame(1);
//            driver.FindElement(By.XPath("//*[contains(text(), 'Frame')]"));

//            Thread.Sleep(3000);
//        }
//    }
//}
