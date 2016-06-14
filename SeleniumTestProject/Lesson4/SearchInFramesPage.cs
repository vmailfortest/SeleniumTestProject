using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTestProject.Lesson4
{
    public class SearchInFramesPage
    {
        IWebDriver driver;
        private double timeout = 20;

        public SearchInFramesPage(IWebDriver webdriver)
        {
            this.driver = webdriver;
            PageFactory.InitElements(driver, this);
        }

        public IWebElement FindElementInAnyFrame(IWebDriver driver, By locator, List<IWebElement> pathToFrame = null)
        {
            IWebElement result = null;

            // Path to current frame with all frames located before it;
            // If pathToFrame=null, it's first run of method and need switch to DefaultContent
            if (pathToFrame == null)
            {
                driver.SwitchTo().DefaultContent();
                pathToFrame = new List<IWebElement>();
            }

            //Check if element exists in current frame
            try
            {
                result = driver.FindElement(locator);
                return result;
            }
            catch (WebDriverException e)
            {
            }

            // Get list of frames and iframes in current frame
            var listOfFrames = driver.FindElements(By.TagName("frame")).ToList();
            listOfFrames.AddRange(driver.FindElements(By.TagName("iframe")));

            // Return if there is no any frames
            if (listOfFrames.Count == 0) { return result; }

            // Iterate through frames, if they were found
            foreach (var frame in listOfFrames)
            {
                driver.SwitchTo().DefaultContent();

                pathToFrame = pathToFrame.Except(listOfFrames).ToList();
                pathToFrame.Add(frame);

                foreach(var element in pathToFrame)
                {
                    driver.SwitchTo().Frame(element);
                }

                result =  this.FindElementInAnyFrame(driver, locator, pathToFrame);
                if (result != null) {  return result; }
            }

            return result;
        }


    }
}
