using FitnessTracker.Forms;
using FitnessTracker.Services;

namespace FitnessTracker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            IAppService appService = new AppService();
            IActivityDefinitionFactory activityDefinitionFactory = new ActivityDefinitionFactory();
            Application.Run(new AuthForm(appService, activityDefinitionFactory));
        }
    }
}