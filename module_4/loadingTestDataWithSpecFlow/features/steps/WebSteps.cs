using loadingTestDataWithSpecFlow.features.contexts;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace loadingTestDataWithSpecFlow.features.steps;

[Binding]
public class WebSteps
{
    private readonly Context _context;
    private const string ID_PREFIX = "pet_";

    public WebSteps(Context context)
    {
        _context = context;
    }

    [Given("I am on the {string}")]
    public void GivenIAmOnTheHomePage(string page)
    {
        _context.WebDriver.Navigate().GoToUrl(_context.BaseUrl);
    }

    [When("I set the {string} to {string}")]
    public void WhenISetTheElementTo(string elementName, string text)
    {
        string elementId = ID_PREFIX + elementName.ToLower().Replace(" ", "_");
        var element = _context.WebDriver.FindElement(By.Id(elementId));
        element.Clear();
        element.SendKeys(text);
    }

    [When("I click the {string} button")]
    public void WhenIClickTheButton(string button)
    {
        string buttonId = button.ToLower() + "-btn";
        _context.WebDriver.FindElement(By.Id(buttonId)).Click();
    }

    [Then("I should see the message {string}")]
    public void ThenIShouldSeeTheMessage(string message)
    {
        var wait = new WebDriverWait(_context.WebDriver, TimeSpan.FromSeconds(_context.WaitSeconds));
        string flashText = wait.Until(driver => driver.FindElement(By.Id("flash_message")).Text);
        Assert.That(flashText, Does.Contain(message), $"Expected message '{message}' not found.");
    }

    [Then("I should see {string} in the results")]
    public void ThenIShouldSeeTheNameInTheResults(string name)
    {
        var wait = new WebDriverWait(_context.WebDriver, TimeSpan.FromSeconds(_context.WaitSeconds));
        string resultsText = wait.Until(driver => driver.FindElement(By.Id("search_results")).Text);
        Assert.That(resultsText, Does.Contain(name), $"Expected result '{name}' not found.");
    }

    [Then("I should not see {string} in the results")]
    public void ThenIShouldNotSeeTheNameInTheResults(string name)
    {
        var element = _context.WebDriver.FindElement(By.Id("search_results"));
        Assert.That(element.Text, Does.Not.Contain(name),
            $"'{name}' should not appear in the results, but was found");
    }
}