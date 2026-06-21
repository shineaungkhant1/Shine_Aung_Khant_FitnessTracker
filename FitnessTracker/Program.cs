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

            IActivityDefinitionFactory activityDefinitionFactory = new ActivityDefinitionFactory();
            Application.Run(new LoginForm(activityDefinitionFactory));
        }
    }
}