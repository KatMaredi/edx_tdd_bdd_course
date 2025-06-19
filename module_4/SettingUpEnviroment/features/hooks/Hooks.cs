using SettingUpEnviroment.features.contexts;
using TechTalk.SpecFlow;

namespace SettingUpEnviroment.hooks;

[Binding]
public class Hooks
{
   private readonly Context _context;

   public Hooks(Context context)
   {
      _context = context;
   }

   [BeforeScenario]
   public void BeforeScenario()
   {
      _context.BaseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost:8080";
      _context.WaitSeconds = Convert.ToInt32(Environment.GetEnvironmentVariable("WAIT_SECONDS") ?? "60");
   }
}