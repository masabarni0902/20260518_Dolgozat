using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Net;
using OpenQA.Selenium.Support.UI;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");
            IWebDriver driver = new ChromeDriver(options);

            try
            {
                driver.Navigate().GoToUrl("http://localhost/login.html");

                var nameInput = driver.FindElement(By.Id("name"));
                nameInput.SendKeys("Teszt Elek");

                var emailInput = driver.FindElement(By.Id("email"));
                emailInput.SendKeys("tesztelek@gmail.com");

                var pwdInput = driver.FindElement(By.Id("password"));
                pwdInput.SendKeys("asd123");

                var stayInput = driver.FindElement(By.Id("stay"));
                stayInput.Click();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Hiba Történt: " + ex.Message);
            }
            finally
            {
                //driver.Quit(); 
            }
        }
    }
}
