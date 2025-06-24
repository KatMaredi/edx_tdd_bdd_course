using implementingSteps.features.contexts;

namespace implementingSteps.features.hooks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

[Binding]
public class Hooks
{
    private readonly Context _context;

    public Hooks(Context context)
    {
        _context = context;
    }

    [BeforeTestRun]
    public static void BeforeAllTests()
    {
        
    }
    
    [BeforeScenario]
    public void BeforeScenario()
    {
        _context.BaseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost:8080";
        _context.WaitSeconds = Convert.ToInt32(Environment.GetEnvironmentVariable("WAIT_SECONDS") ?? "60");

        var options = new ChromeOptions();
        options.AddArgument("--headless");
        _context.WebDriver = new ChromeDriver(options);
        _context.WebDriver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(_context.WaitSeconds);
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _context.WebDriver.Quit();
    }
}