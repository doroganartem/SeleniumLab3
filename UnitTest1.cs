namespace SeleniumLab3;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

[TestFixture]
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
            driver?.Dispose();
        }

        [Test]
        public void Test_01_Checkboxes(){
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/checkboxes");
            IWebElement firstCheckbox = driver.FindElement(By.CssSelector("form#checkboxes input:nth-child(1)"));

            if (!firstCheckbox.Selected)
            {
                firstCheckbox.Click();
            }

            Assert.That(firstCheckbox.Selected, Is.True);
        }

        [Test]
            public void Test_02_AddOrRemoveElements()
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/add_remove_elements/");

                IWebElement addButton = driver.FindElement(By.XPath("//button[text()='Add Element']"));
                addButton.Click();

                IWebElement deleteButton = wait.Until(d => d.FindElement(By.ClassName("added-manually")));
                Assert.That(deleteButton.Displayed, Is.True);

                deleteButton.Click();

                var deleteButtons = driver.FindElements(By.ClassName("added-manually"));
                Assert.That(deleteButtons.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_03_Dropdown()
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dropdown");

                IWebElement rawSelectElement = driver.FindElement(By.Id("dropdown"));
                SelectElement dropdown = new SelectElement(rawSelectElement);

                dropdown.SelectByText("Option 1");

                Assert.That(dropdown.SelectedOption.Text, Is.EqualTo("Option 1"));

                dropdown.SelectByValue("2");

                Assert.That(dropdown.SelectedOption.Text, Is.EqualTo("Option 2"));
            }

        [Test]
        public void Test_04_Inputs()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/inputs");

            IWebElement inputField = driver.FindElement(By.TagName("input"));

            inputField.SendKeys("10");

            inputField.SendKeys(Keys.ArrowUp);
            Assert.That(inputField.GetAttribute("value"), Is.EqualTo("11"));

            inputField.SendKeys(Keys.ArrowDown);
            Assert.That(inputField.GetAttribute("value"), Is.EqualTo("10"));
        }

        [Test]
        public void Test_05_StatusCodes()
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/status_codes");

                IWebElement code200Link = driver.FindElement(By.LinkText("200"));
                code200Link.Click();

                IWebElement contentText = driver.FindElement(By.TagName("p"));
                Assert.That(contentText.Text, Does.Contain("This page returned a 200 status code"));

                driver.Navigate().Back();

                IWebElement code404Link = driver.FindElement(By.LinkText("404"));
                code404Link.Click();

                contentText = driver.FindElement(By.TagName("p"));
                Assert.That(contentText.Text, Does.Contain("This page returned a 404 status code"));
            }

        [Test]
        public void Test_06_NestedFrames()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/nested_frames");

            driver.SwitchTo().Frame("frame-top");
            driver.SwitchTo().Frame("frame-middle");

            IWebElement middleContent = driver.FindElement(By.Id("content"));
            Assert.That(middleContent.Text, Is.EqualTo("MIDDLE"));

            driver.SwitchTo().DefaultContent();

            driver.SwitchTo().Frame("frame-bottom");
            IWebElement bottomBody = driver.FindElement(By.TagName("body"));
            Assert.That(bottomBody.Text.Trim(), Is.EqualTo("BOTTOM"));
        }

        [Test]
        public void Test_07_FormAuthentication()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            driver.FindElement(By.Id("username")).SendKeys("tomsmith");
            driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            IWebElement flashMessage = wait.Until(d => d.FindElement(By.Id("flash")));
            Assert.That(flashMessage.Text, Does.Contain("You logged into a secure area!"));
        }

        // 8. Dynamic Content
        [Test]
        public void Test_08_DynamicContent()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_content");

            string initialText = driver.FindElements(By.CssSelector("#content .row .large-10")).First().Text;

            driver.Navigate().Refresh();

            string updatedText = driver.FindElements(By.CssSelector("#content .row .large-10")).First().Text;

            Assert.That(updatedText, Is.Not.EqualTo(initialText));
        }

        [Test]
        public void Test_09_DisappearingElements()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/disappearing_elements");

            var menuItems = driver.FindElements(By.CssSelector("ul li"));
            Assert.That(menuItems.Count, Is.GreaterThanOrEqualTo(4));
        }

        [Test]
        public void Test_10_Hover()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/hovers");

            var firstUserFigure = driver.FindElements(By.ClassName("figure")).First();

            Actions actions = new Actions(driver);
            actions.MoveToElement(firstUserFigure).Perform();

            IWebElement caption = firstUserFigure.FindElement(By.ClassName("figcaption"));
            IWebElement header = caption.FindElement(By.TagName("h5"));

            Assert.That(header.Displayed, Is.True);
            Assert.That(header.Text, Does.Contain("name: user1"));
        }
    }