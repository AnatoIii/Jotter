using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using TechTalk.SpecFlow;

namespace JotterAPI.Selenium.Features.Login4
{
    [Binding]
    public class UserLoginSteps
    {
        private IWebDriver _driver;

        [Given(@"User is not presented in system and such email is not registered\.")]
        public void GivenUserIsNotPresentedInSystemAndSuchEmailIsNotRegistered_()
        {
            var service = FirefoxDriverService.CreateDefaultService(@"D:\University\6\Machno\JotterAPI\JotterAPI.Selenium\Drivers", "geckodriver.exe");
            _driver = new FirefoxDriver(service);
            _driver.Navigate().GoToUrl("http://localhost:4200/register");
            _driver.FindElement(By.Id("show_form")).Click();
        }

        [When(@"User registered with credentials ""(.*)"", ""(.*)"", ""(.*)""")]
        public void WhenUserRegisteredWithCredentials(string login, string password, string name)
        {
            var email = _driver.FindElement(By.Id("email"));
            email.SendKeys($"{Guid.NewGuid().ToString("N")}{login}");

            var nameInput = _driver.FindElement(By.Id("name"));
            nameInput.SendKeys(name);

            var passwordInput = _driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(password);

            var repeatPasswordInput = _driver.FindElement(By.Id("repeatPassword"));
            repeatPasswordInput.SendKeys(password);
            _driver.FindElement(By.Id("register_button")).Click();
        }

        [Then(@"He successfully created and logged in")]
        public void ThenHeSuccessfullyCreatedAndLoggedIn()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            var data = (string)js.ExecuteScript("return localStorage.getItem('token');");

            if (data is null)
            {
                throw new Exception("Error");
            }
        }
    }
}
