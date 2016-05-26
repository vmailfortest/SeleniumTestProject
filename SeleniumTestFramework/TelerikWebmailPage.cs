using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SeleniumTestFramework
{
    public class TelerikWebmailPage
    {
        IWebDriver driver;
        private double timeout = 20;


        public TelerikWebmailPage(IWebDriver webdriver)
        {
            this.driver = webdriver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "ctl00_FolderContent_FolderNavigationControl_rtvFolders")]
        private IWebElement FoldersPanel;

        [FindsBySequence]
        [FindsBy(How = How.Id, Using = "ctl00_MainContent_messages_ctl00")]
        [FindsBy(How = How.XPath, Using = ".//span[contains(text(), 'Date:')]")]
        private IWebElement emailsOnPanel;
        public string emailsOnPanelXpath = ".//span[contains(text(), 'Date:')]";

        public void ClickOnSpecificFolder(string folderName)
        {
            this.FoldersPanel.FindElement(By.XPath("//span[contains(text(), '" + folderName + "')]")).Click();
        }

        public void ExpandSpecificFolder(string folderName)
        {
            this.FoldersPanel.FindElement(By.XPath("//span[contains(text(), '" + folderName + "')]/preceding::span[1]")).Click();
        }

        public bool VerifySubfoldersOfSpecificFolder(string folderName)
        {
            var listOfSubfolders = this.FoldersPanel.FindElements(By.XPath("//span[contains(text(), '" + folderName + "')]/../../ul/li"));
            bool result = true;

            if(listOfSubfolders.Count > 0)
            {
                foreach(var element in listOfSubfolders)
                {
                    element.Click();
                    try { 
                        new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.ElementIsVisible(By.XPath(this.emailsOnPanelXpath)));
                    }
                    catch(ElementNotVisibleException e)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}
