using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using TechTalk.SpecFlow;

namespace JotterAPI.Selenium.Features.Login1
{
    [Binding]
    public class UserLoginSteps
    {
        private IWebDriver _driver;
        private ScenarioContext _scenarioContext;

        public UserLoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"User presented in system and entered correct login and password\.")]
        public void GivenUserPresentedInSystemAndEnteredCorrectLoginAndPassword_()
        {
            var service = FirefoxDriverService.CreateDefaultService(@"D:\University\6\Machno\JotterAPI\JotterAPI.Selenium\Drivers", "geckodriver.exe");
            _driver = new FirefoxDriver(service);
            _driver.Navigate().GoToUrl("http://localhost:3000");

            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(1);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
        }

        [When(@"User entered correct login password ""(.*)"", ""(.*)""")]
        public void WhenUserEnteredCorrectLoginPassword(string login, string password)
        {
            var email = _driver.FindElement(By.Id("email"));
            email.SendKeys(login);

            var passwordInput = _driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(password);
            _driver.FindElement(By.Id("login_button")).Click();
        }

        [Then(@"He successfully logged in")]
        public void ThenHeSuccessfullyLoggedIn()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            var data = (string)js.ExecuteScript("return localStorage.getItem('token');");

            if (data is null)
            {
                throw new Exception("Error");
            }
        }
        
        [Then(@"He can get a list of his categories")]
        public void ThenHeCanGetAListOfHisCategories()
        {
            _driver.Close();
        }
    }
}
