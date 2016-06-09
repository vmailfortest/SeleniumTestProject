using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Keys = OpenQA.Selenium.Keys;

namespace SeleniumTestProject.Lesson3
{
    public class CatalogPage
    {
        IWebDriver driver;
        private double timeout = 20;

        public CatalogPage(IWebDriver webdriver)
        {
            this.driver = webdriver;
            PageFactory.InitElements(driver, this);
        }

        //[FindsBySequence]
        //[FindsBy(How = How.Id, Using = "results")]
        //[FindsBy(How = How.ClassName, Using = "title")]
        //private IWebElement listOfFilms;

        [FindsBy(How = How.Id, Using = "q")]
        private IWebElement searchField;

        public List<string> GetListOfFilms()
        {
            List<string> results = new List<string>();

            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until<IWebElement>(d => d.FindElement(By.ClassName("title")));

            var list = driver.FindElement(By.Id("results")).FindElements(By.ClassName("title"));

            foreach (var element in list)
            {
                results.Add(element.Text);
            }

            return results;
        }

        public void SearchFor(string searchText)
        {
            this.searchField.Clear();

            //this.searchField.SendKeys(searchText);
            Thread thread = new Thread(() => Clipboard.SetText(searchText));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            this.searchField.SendKeys(Keys.Control + "v");

            this.searchField.SendKeys(Keys.Enter);

            var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeout));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
        }

    }
}