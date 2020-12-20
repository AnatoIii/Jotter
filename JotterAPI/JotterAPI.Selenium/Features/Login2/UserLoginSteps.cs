using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using TechTalk.SpecFlow;

namespace JotterAPI.Selenium.Features.Login2
{
    [Binding]
    public class UserLoginSteps
    {
        private IWebDriver _driver;

        [Given(@"User is presented in system, but entered incorrect login and password\.")]
        public void GivenUserIsPresentedInSystemButEnteredIncorrectLoginAndPassword_()
        {
            var service = FirefoxDriverService.CreateDefaultService(@"D:\University\6\Machno\JotterAPI\JotterAPI.Selenium\Drivers", "geckodriver.exe");
            _driver = new FirefoxDriver(service);
            _driver.Navigate().GoToUrl("http://localhost:4200");
            _driver.FindElement(By.Id("show_form")).Click();
        }
        
        [When(@"User entered incorrect login, password ""(.*)"", ""(.*)""")]
        public void WhenUserEnteredIncorrectLoginPassword(string login, string password)
        {
            var email = _driver.FindElement(By.Id("email"));
            email.SendKeys(login);

            var passwordInput = _driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(password);
            _driver.FindElement(By.Id("login_button")).Click();
        }
        
        [Then(@"He gets an error message")]
        public void ThenHeGetsAnErrorMessage()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            var data = (string)js.ExecuteScript("return document.getElementById('toast-container').textContent;");

            if (!data.Contains("Incorrect"))
            {
                throw new Exception("No error message");
            }
        }
        
        [Then(@"He is not logged in")]
        public void ThenHeIsNotLoggedIn()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            var data = (string)js.ExecuteScript("return localStorage.getItem('token');");

            _driver.Close();
            if (!(data is null))
            {
                throw new Exception("Error");
            }
        }
    }
}
