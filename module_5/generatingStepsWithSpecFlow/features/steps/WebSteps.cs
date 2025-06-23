using generatingStepsWithSpecFlow.features.contexts;
using TechTalk.SpecFlow;

namespace generatingStepsWithSpecFlow.features.steps;

[Binding]
public class WebSteps
{
    private readonly Context _context;

    public WebSteps(Context context)
    {
        _context = context;
    }

    [Given(@"the following pets")]
    public void GivenTheFollowingPets(Table table)
    {
    }

    [Given(@"I am the ""(.*)""")]
    public void GivenIAmThe(string p0)
    {
    }

    [When(@"I set the ""(.*)"" to ""(.*)""")]
    public void WhenISetTheTo(string category0, string dog1)
    {
    }

    [When(@"I click the ""(.*)"" button")]
    public void WhenIClickTheButton(string search0)
    {
    }

    [Then(@"I should see ""(.*)"" in the results")]
    public void ThenIShouldSeeInTheResults(string fido0)
    {
    }

    [Then(@"I should not see ""(.*)"" in the results")]
    public void ThenIShouldNotSeeInTheResults(string kitty0)
    {
    }
}