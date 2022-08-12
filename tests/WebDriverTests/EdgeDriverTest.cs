using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace WebDriverTests
{
    [TestClass]
    public class EdgeDriverTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from http://go.microsoft.com/fwlink/?LinkId=619687
        // to install Microsoft WebDriver.

        private EdgeDriver _driver;

        public EdgeDriverTest()
        {
            //Process process = new Process()
            //{
            //    StartInfo = new ProcessStartInfo(@"C:\Users\sereg\source\repos\certificate-MVC\bin\Release\Web.exe")
            //    {
            //        WindowStyle = ProcessWindowStyle.Normal,
            //        WorkingDirectory = Path.GetDirectoryName(path)
            //    }
            //};
            //var length = Process.GetProcessesByName("Web").Length;

            //if (length == 0)
            //{
            //    Process.Start(@"C:\Users\sereg\source\repos\certificate-MVC\bin\Debug\Web.exe");
            //}

            //var process = Process.Start(@"C:\Users\sereg\source\repos\certificate-MVC\bin\Debug\Web.exe");
            //int id = process.Id;
            //Process tempProc = Process.GetProcessById(id);
            //tempProc.WaitForExit();
        }

        [TestInitialize]
        public void EdgeDriverInitialize()
        {
            // Initialize edge driver 
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            _driver = new EdgeDriver(options);
        }

        [TestMethod]
        public void CRUD()
        {
            var random = new Random();
            var title = "Test " + random.Next(0, 10_000);

            // Login
            _driver.Url = "https://localhost:5001/Identity/Account/Login";

            _driver.FindElementById("Input_UserNameOrEmail").SendKeys("admin");
            _driver.FindElementById("Input_Password").SendKeys("Password12");
            _driver.FindElementByClassName("btn").Click();

            Assert.AreEqual("https://localhost:5001/", _driver.Url);

            // Create
            _driver.Navigate().GoToUrl("https://localhost:5001/Certificate/Create");

            _driver.FindElementById("Title").SendKeys(title);
            _driver.FindElementById("File").SendKeys(@"C:\Users\sereg\Source\Repos\certificate-MVC\src\Web\wwwroot\img\example_image.jpg");
            _driver.FindElementById("Description").SendKeys(@"test test test");

            var select = new SelectElement(_driver.FindElementById("Stage"));
            select.SelectByIndex(2);

            _driver.FindElementById("Date").SendKeys(@"29.01.2001");

            _driver.FindElementByClassName("btn-success").Click();

            var elem = _driver.FindElementsByClassName("card-title").Where(i => i.Text == title);

            Assert.AreEqual(1, elem.Count());

            // Change
            elem.First().Click();
            _driver.FindElementByCssSelector("a[href^=\"/Certificate/Edit/\"]").Click();

            var desc = _driver.FindElementById("Description");
            desc.Clear();
            desc.SendKeys(@"test2 test2 test2");

            select = new SelectElement(_driver.FindElementById("Stage"));
            select.SelectByIndex(1);

            _driver.FindElementsByClassName("btn-primary").Last().Click();

            Assert.IsTrue(_driver.FindElementsByTagName("p").Any(i => i.Text == "test2 test2 test2"));

            // Create Link
            _driver.FindElementByCssSelector("a[href^=\"/Link/Index/\"]").Click();
            _driver.FindElementByCssSelector("a[href^=\"/Link/Create/\"]").Click();
            _driver.FindElementById("Url").SendKeys("https://example.com/");
            _driver.FindElementByClassName("btn-success").Click();

            Assert.IsTrue(_driver.FindElementsByTagName("a").Any(i => i.Text == "https://example.com/"));

            // Delete Link
            _driver.FindElementByCssSelector("a[href^=\"/Link/Delete/\"]").Click();
            Thread.Sleep(1000);
            _driver.FindElementsByTagName("input").FirstOrDefault(i => i.GetAttribute("value") == "Да").Click();

            Assert.IsFalse(_driver.FindElementsByTagName("a").Any(i => i.Text == "https://example.com/"));

            // Delete
            _driver.FindElementByCssSelector("a[href^=\"/Certificate/Details\"]").Click();

            _driver.FindElementByClassName("delete").Click();
            Thread.Sleep(1000);
            _driver.FindElementsByTagName("input").FirstOrDefault(i => i.GetAttribute("value") == "Да").Click();

            Assert.IsFalse(_driver.FindElementsByClassName("card-title").Any(i => i.Text == title));
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }
    }
}
