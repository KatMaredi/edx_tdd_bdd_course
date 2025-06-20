using OpenQA.Selenium;

namespace loadingTestDataWithSpecFlow.features.contexts;

public class Context
{
    public string BaseUrl { get; set; }
    public int WaitSeconds { get; set; }
    public IWebDriver WebDriver { get; set; }
}