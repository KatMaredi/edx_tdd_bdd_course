namespace generatingStepsWithSpecFlow.features.contexts;
using OpenQA.Selenium;

public class Context
{
    public string BaseUrl { get; set; }
    public int WaitSeconds { get; set; }
    public IWebDriver WebDriver { get; set; }
}