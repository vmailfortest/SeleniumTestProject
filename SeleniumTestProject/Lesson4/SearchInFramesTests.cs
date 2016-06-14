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

namespace SeleniumTestProject.Lesson4
{
    [TestFixture]
    class SearchInFramesTests
    {
        IWebDriver driver;

        [SetUp]
        public void TestInit()
        {

            driver = new FirefoxDriver();

            //driver = new ChromeDriver("c:\\temp\\");

            //driver = new InternetExplorerDriver("c:\\temp\\");
        }

        [TearDown]
        public void TestCleanUp()
        {
            driver.Quit();
        }

        [Test]
        public void FindElementsInFramesLocalhostTest()
        {
            driver.Navigate().GoToUrl("http://localhost:180/HtmlPage.html");

            SearchInFramesPage newPage = new SearchInFramesPage(driver);

            newPage.FindElementInAnyFrame(driver, By.XPath("//*[contains(text(), 'Frame One')]")).Text.Should()
                .Be("Hello Frame One");

            newPage.FindElementInAnyFrame(driver, By.XPath("//*[contains(text(), 'Frame Two')]")).Text.Should()
                .Be("Hello Frame Two");

            newPage.FindElementInAnyFrame(driver, By.XPath("//*[contains(text(), 'No such frame')]")).Should()
                .BeNull();

            newPage.FindElementInAnyFrame(driver, By.XPath("//*[contains(text(), 'Frame Three')]")).Text.Should()
                .Be("Hello Frame Three");

            newPage.FindElementInAnyFrame(driver, By.XPath("//*[contains(text(), 'Frame Four')]")).Text.Should()
                .Be("Hello Frame Four");

            newPage.FindElementInAnyFrame(driver, By.XPath("//*[contains(text(), 'Frame Five')]")).Text.Should()
                .Be("Hello Frame Five");
        }

        [Test]
        public void FindElementsInFramesYourhtmlsourceTest()
        {
            driver.Navigate().GoToUrl("http://www.yourhtmlsource.com/examples/frameset1.html");

            SearchInFramesPage newPage = new SearchInFramesPage(driver);

            newPage.FindElementInAnyFrame(driver, By.XPath("//*[contains(text(), 'make this layout')]")).Text.Should()
                .Be("The type of code that would make this layout looks like this:");
        }
    }
}
