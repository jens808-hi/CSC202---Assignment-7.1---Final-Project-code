/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files */
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsAppFINAL
{
    /* This form simulates a football passing play where the player chooses a throwing power using the numeric Updown control. The object of the game is to get the
      is to throw the football a far enough distance that the wide reciever can catch it and run it to the endzone to score a touchdown all while avoiding 
      a combination of obstacles. The game calculates the outcome using random wind interference, random defender difficulty, a "perfect throw zone" that 
      awards bonus points and a final calculation that determines the result of the play */
    public partial class frmThrowFootball : Form
    {
        // Random number generator to increase difficulty of events
        Random rand = new Random();

        // Tracks the defender type and it's difficulty level
        string defenderType = ""; // stores the name of defender currently guarding the receiver
        int defenderDifficulty = 0; // Sets up the defender difficulty for the play; higher values equal harder plays to defeat

        // Perfect throwing range values between 45 and 65 yards
        int perfectMin = 45; // Minimum value for perfect throw power range
        int perfectMax = 65; // Maximum value for perfect throw power range 

        int bonus = 25; // Bonus points added if the player lands inside the perfect throw zone 

        // Runs when the form is first created 
        public frmThrowFootball()
        {
            // Initializes all form controls 
            InitializeComponent();
        }
        // Runs when the form first appears on the screen 
        private void frmThrowFootball_Load(object sender, EventArgs e)
        {
            // User input recieved for throw power
            int userPower = (int)numericPower.Value;

            // Determines the random perfect zone
            perfectMin = rand.Next(35, 55); // Minimum range is between 35 and 55
            perfectMax = perfectMin + 20; // Perfect range always spans 20 poer points

            // Displays the Perfect throw range to the player
            lblPerfectZone.Text = $"Perfect Throw Zone: {perfectMin} - {perfectMax}";

            // Sets the limits for the NumericUpDown controls range so players cannot exceed bounds
            numericPower.Minimum = 0;
            numericPower.Maximum = 100;
        }

        // Method that runs when the "Throw Football" button is clicked 
        private void btnThrow_Click(object sender, EventArgs e)
        {
            try
            {
                // Power value recieved from user input in numeric control
                int userPower = (int)numericPower.Value;

                // Simulates wind interference (randomized from -3 to +4 yards)
                int wind = rand.Next(-3, 4);

                // Randomly determines which type of defender is covering the reciever (randomized from 0 = easy, 1 = medium, 2 = hard)
                int defender = rand.Next(3);

                // Determines the defender type and it's associated level of difficulty
                switch (defender)
                {
                    case 0:
                        defenderType = "Rookie Cornerback";
                        defenderDifficulty = 5;
                        break;
                    case 1:
                        defenderType = "Veteran Cornerback";
                        defenderDifficulty = 10;
                        break;
                    case 2:
                        defenderType = "All-Pro Cornerback";
                        defenderDifficulty = 15;
                        break;
                }

                // Displays the defender type on the screen 
                lblDefender.Text = $"Defender: {defenderType}";
                // Displays the wind interference 
                lblWind1.Text = $"Wind: {wind} yards";
                // Resets bonus to zero after hitting the perfect throw zone
                bonus = 0;
                // Checks if the player landed inside the perfect throw zone 
                if (userPower >= perfectMin && userPower <= perfectMax)
                    bonus = 25; // bonus reward

                // Calculates the final throw result 
                int finalThrow = userPower + bonus + wind - defenderDifficulty;

                // Displays the math behind the throw so the player understands the results
                lblCalculation.Text = $"Power: ({userPower}) + Bonus: ({bonus}) + Wind: ({wind}) - Defender: ({defenderDifficulty}) = {finalThrow}";

                // Determines the outcome of the play based on the final score
                if (finalThrow >= 50) // marks a touchdown
                {
                    decimal reward = 25; // bonus
                    lblResult.Text = $"Congratulations! You beat the {defenderType} and made a TOUCHDOWN! You earned (+{reward} coins)";
                    // Color of the results label changes dependent upon the outcome of the play 
                    lblResult.ForeColor = Color.LightSkyBlue;
                    // Sends message to GameManager log
                    GameManager.Log($"Congratulations! You beat the {defenderType} and made a TOUCHDOWN! You earned (+{reward} coins)");
                    // Awards coins and records the win 
                    GameManager.AwardMiniGame(reward, true);
                }
                else if (finalThrow >= 30) // gives a completed pass
                {
                    decimal reward2 = 15;
                    lblResult.Text = $"Nice throw! The reciever catches it despite the {defenderType}! Completed pass! You earned (+{reward2} coins)";
                    // Color of the results label changes dependent upon the outcome of the play 
                    lblResult.ForeColor = Color.PaleGoldenrod;
                    GameManager.Log($"Nice throw! The reciever catches it despite the {defenderType}! Completed pass!");
                    // Updates stats to GameManager
                    GameManager.AwardMiniGame(0, false);
                }
                else if (finalThrow >= 10) // gives an interception 
                {
                    lblResult.Text = $"The {defenderType} breaks up the pass! Incomplete. No Touchdown!";
                    // Color of the results label changes dependent upon the outcome of the play 
                    lblResult.ForeColor = Color.Salmon;
                    GameManager.Log($"The {defenderType} breaks up the pass! Incomplete. No Touchdown!");
                    // Updates stats to GameManager
                    GameManager.AwardMiniGame(0, false);
                }
                else
                {
                    lblResult.Text = $"INTERCEPTION! The {defenderType} jumps the route!";
                    // Color of the results label changes dependent on the outcome of the play 
                    lblResult.ForeColor = Color.IndianRed;
                    GameManager.Log($"INTERCEPTION! The {defenderType} jumps the route!");
                    // Updates stats to GameManager
                    GameManager.AwardMiniGame(0, false);
                }

            }
            // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            catch (Exception ex) 
            {
                // Logs any exceptions that occur while saving to console and returns false
                Console.WriteLine("An error occured during the throw: " + ex.Message);
                // Displays error message to the player
                MessageBox.Show("An error occured during the throw: " + ex.Message);
            }
        }
        // Method that runs when the "Return to Main Menu" button is clicked 
        private void btnReturn_Click(object sender, EventArgs e)
        {
            //Creates a new instance of the form 
            frmMainMenu mainMenu = new frmMainMenu();

            // Closes the current mini-game form
            this.Close();

            // Shows the main menu 
            mainMenu.Show();
        }

        // Method that runs when the "Mini Games" button is clicked 
        private void btnMini_Click(object sender, EventArgs e)
        {
            //Creates a new instance of the form 
            frmMiniGamesMenu miniGames = new frmMiniGamesMenu();

            // Closes the current game form 
            this.Close();

            // Shows the Mini Games menu
            miniGames.Show();

        }

   

    }
}
