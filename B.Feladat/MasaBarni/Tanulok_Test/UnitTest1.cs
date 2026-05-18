using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace Tanulok_Test
{
    public class Tanulok_Test
    {

        private readonly WindowsDriver<WindowsElement> _driver;

        public Tanulok_Test()
        {
            var options = new AppiumOptions();
            options.AddAdditionalCapability(
                "app",
                @"C:\Users\Asus2026\Desktop\MasaBarni_jav\MasaBarni\Tanulok_WPF\bin\Debug\net10.0-windows\Tanulok_WPF.exe");

            options.AddAdditionalCapability(
               "deviceName",
               "WindowsPC");

            _driver = new WindowsDriver<WindowsElement>(
                new Uri("http://127.0.0.1:4723/"),
                options);

            _driver.Manage().Timeouts().ImplicitWait =
                TimeSpan.FromSeconds(10);
        }
        [Fact]
        public void Test1()
        {
            _driver.Manage().Window.Maximize();

            _driver.SwitchTo().Window(_driver.WindowHandles.First());

            _driver.FindElementByName("Új tanuló hozzáadása").Click();

            _driver.SwitchTo().Window(_driver.WindowHandles.First());

            _driver.FindElementByAccessibilityId("txtName").SendKeys("Tesyt Elek");

            _driver.FindElementByAccessibilityId("txtClass").SendKeys("11A");

            _driver.FindElementByAccessibilityId("txtMath").SendKeys("5");

            _driver.FindElementByAccessibilityId("txtPhysics").SendKeys("2");

            _driver.FindElementByName("Hozzáadás").Click();

        }
    }
}
