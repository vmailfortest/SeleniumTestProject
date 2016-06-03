using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTestProject.Lesson3
{
    public class LoginPage
    {
        IWebDriver driver;
        private double timeout = 20;

        public LoginPage(IWebDriver webdriver)
        {
            this.driver = webdriver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Name, Using = "username")]
        private IWebElement usernameField = null;

        [FindsBy(How = How.Name, Using = "password")]
        private IWebElement passwordField = null;

        [FindsBy(How = How.Name, Using = "submit")]
        private IWebElement submitButton = null;

        public CatalogPage Login(string username, string password)
        {
            usernameField.Clear();
            usernameField.SendKeys(username);

            passwordField.Clear();
            passwordField.SendKeys(password);

            submitButton.Click();
            return new CatalogPage(this.driver);
        }
    }
}
