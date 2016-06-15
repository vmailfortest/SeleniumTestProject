using AutomatedTester.BrowserMob;
using AutomatedTester.BrowserMob.HAR;
using Fiddler;
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

namespace SeleniumTestProject.Lesson5
{
    [TestFixture]
    class StatusCodesFiddlerTests
    {
        IWebDriver driver;

        [SetUp]
        public void TestInit()
        {
            // Clean log file
            File.WriteAllText(@"C:\TEMP\WriteText.txt", "");

            // Starting Fiddler proxy
            FiddlerCoreStartupFlags flags = FiddlerCoreStartupFlags.Default;
            FiddlerApplication.Startup(0, flags);
            int proxyPort = FiddlerApplication.oProxy.ListenPort;

            // Setup Selenium proxy
            OpenQA.Selenium.Proxy proxy = new OpenQA.Selenium.Proxy();
            proxy.HttpProxy = string.Format("127.0.0.1:{0}", proxyPort);

            // Setup WebDriver
            DesiredCapabilities caps = DesiredCapabilities.Chrome();
            caps.SetCapability(CapabilityType.Proxy, proxy);
            driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4444/wd/hub"), caps);
        }

        [TearDown]
        public void TestCleanUp()
        {
            driver.Quit();
            FiddlerApplication.Shutdown();
        }

        [Test]
        public void FiddlerTest()
        {
            bool result = true;
            // Hook up the event for monitoring proxied traffic.
            FiddlerApplication.AfterSessionComplete += delegate (Session targetSession)
            {
                if (targetSession.responseCode.ToString().StartsWith("4") || targetSession.responseCode.ToString().StartsWith("5"))
                {
                    result = false;

                    string str = string.Empty;
                    str += $"Request: {targetSession.RequestMethod} {targetSession.fullUrl}\r\n";
                    str += $"Response: {targetSession.responseCode}\r\n";

                    using (StreamWriter file = new StreamWriter(@"C:\TEMP\WriteText.txt", true))
                    {
                        file.WriteLine(str);
                    }
                }
            };

            // Selenium steps
            driver.Navigate().GoToUrl("http://ya.ru/NoSuchPage.html");

            result.Should().BeTrue("Some of resources returned an error while loading");
        }
    }
}
