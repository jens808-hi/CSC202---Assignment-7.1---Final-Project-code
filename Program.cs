namespace WinFormsAppFINAL
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            {
                Application.Run(new frmMainMenu()); 

               // Shows the Profile Selection form as dialog
                using (ProfileSelect profileForm = new ProfileSelect())
                {
                    if (profileForm.ShowDialog() == DialogResult.OK)
                    {
                        // Player has selected a profile, now run the Main Menu
                        Application.Run(new frmMainMenu());
                    }
                    else
                    {
                        // If the player closes profile selection without choosing, exit application 
                        return;
                    }
                }
            }
           
        }
    }
}