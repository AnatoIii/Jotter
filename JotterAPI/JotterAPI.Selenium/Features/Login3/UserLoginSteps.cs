using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using TechTalk.SpecFlow;

namespace JotterAPI.Selenium.Features.Login3
{
    [Binding]
    public class UserLoginSteps
    {
        private IWebDriver _driver;

        [Given(@"User is not presented in system and such email is registered\.")]
        public void GivenUserIsNotPresentedInSystemAndSuchEmailIsRegistered_()
        {
            var service = FirefoxDriverService.CreateDefaultService(@"D:\University\6\Machno\JotterAPI\JotterAPI.Selenium\Drivers", "geckodriver.exe");
            _driver = new FirefoxDriver(service);
            _driver.Navigate().GoToUrl("http://localhost:4200/register");
            _driver.FindElement(By.Id("show_form")).Click();
        }
        
        [When(@"User registered ""(.*)"", ""(.*)"", ""(.*)""")]
        public void WhenUserRegistered(string login, string password, string name)
        {
            var email = _driver.FindElement(By.Id("email"));
            email.SendKeys(login);

            var nameInput = _driver.FindElement(By.Id("name"));
            nameInput.SendKeys(name);

            var passwordInput = _driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(password);

            var repeatPasswordInput = _driver.FindElement(By.Id("repeatPassword"));
            repeatPasswordInput.SendKeys(password);
            _driver.FindElement(By.Id("register_button")).Click();
        }
        
        [Then(@"User gets an error mesasge")]
        public void ThenUserGetsAnErrorMesasge()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            var data = (string)js.ExecuteScript("return document.getElementById('toast-container').textContent;");

            _driver.Close();
            if (!data.Contains("such email already registered"))
            {
                throw new Exception("No error message");
            }
        }
    }
}
