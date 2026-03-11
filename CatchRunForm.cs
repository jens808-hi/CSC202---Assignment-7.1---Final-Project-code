using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsAppFINAL
{
    public partial class frmCatchRun : Form
    {
        /// Random number generator to increase difficulty of events
        Random rand = new Random();

        // Array that stores the different lanes the player can run their play
        string[] lanes = { "Left", "Middle", "Right" };

        // Variable that stores the lane a player chooses during the game
        string chosenLane = "";

        // Variable that stores the lane that the defender randomly covers
        string defenderLane = "";

        // Array that sotres the different defender types 
        string[] defenders = { "Rookie Cornerback", "Veteran Cornerback", "All-Pro Cornerback" };
        // Difficulty values that associates to each defender above 
        int[] defenderDifficulty = { 5, 10, 15 };

        // Message displayed when the player catches the ball 
        string[] catchMessages = { "Amazing catch!", "Nice grab!", "Almost fumbled catch but secured!", "Dropped the ball!" };

        // Variable that stores the difficulty value for the current defender
        int currentDifficulty = 0;

        // Boolean that keeps track of whether the ball was caught
        bool caughtBall = false;

        // Runs when the form is first created 
        public frmCatchRun()
        {
            InitializeComponent();
        }
        // Runs when the form first appears on the screen 
        private void frmCatchRun_Load(object sender, EventArgs e)
        {
            try
            {
                // Set default values for the defender lane and player's lane 
                defenderLane = "";
                chosenLane = "";

                // Results of the play are displayed to the player here, starts off as a clear label
                lblResult.Text = "";
                lblCatch.Text = "";
                lblCalculation.Text = "";
                lblDefender.Text = "";
                lblResult.ForeColor = Color.Transparent;

                // Disable the lane buttons until the player catches the ball
                btnLeft.Enabled = false;
                btnMiddle.Enabled = false;
                btnRight.Enabled = false;

                // Enables the catch button 
                btnCatch.Enabled = true;

                // Set numeric default
                numericRunPower.Value = 5;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting the Catch & Run form: " + ex.Message);
            }
        }

        // Method that runs when the "Catch" button is clicked
        private void btnCatch_Click(object sender, EventArgs e)
        {
            try
            {
                // Enables catch button 
                btnCatch.Enabled = false;
                // Resets the catch messages each round
                lblResult.Text = "";
                lblCalculation.Text = "";

                // Randomly chooses which defender is in play
                int defenderIndex = rand.Next(defenders.Length);

                // Gets the defender's name from the array 
                string defender = defenders[defenderIndex];
                // Gets the matching difficulty value for the defender
                currentDifficulty = defenderDifficulty[defenderIndex];

                // Displays the defender type too the player
                lblDefender.Text = $"Defender: {defender}";

                // Generates a random number from 1 - 100 to determine the player's catch success 
                int catchRoll = rand.Next(1, 101);

                // If the number is 25 or less the plaer drops the ball 
                if (catchRoll <= 25)
                {
                    lblCatch.Text = catchMessages[3]; // Dropped ball
                    lblResult.Text = "Incomplete Pass!";
                    lblResult.ForeColor = Color.Red;
                    // The player was unsuccessful at catching the ball 
                    caughtBall = false;

                    // Re-enable catch to allow for a retry
                    btnCatch.Enabled = true;

                    // Stops the play
                    return;
                }

                // If catch is successful a random catch message is displayed to the player
                int catchMessageIndex = rand.Next(0, 3);
                lblCatch.Text = catchMessages[catchMessageIndex];

                // The ball is successfully caught by the player
                caughtBall = true;

                // Enables the lane buttons so the player can now make their selection 
                btnLeft.Enabled = true;
                btnMiddle.Enabled = true;
                btnRight.Enabled = true;

                // Disables catch button until the next play 
                btnCatch.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during catch attempt: " + ex.Message);
            }
        }

        // Method that runs when the player selects the "Run Left" button lane to run down 
        private void btnLeft_Click(object sender, EventArgs e)
        {
            try
            {
                // Variable that stores the left lane choice 
                chosenLane = "Left";
                // Calls this function to run the play and checks to see if the users choice resulted in a touchdown 
                RunPlay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running left lane: " + ex.Message);
            }
        }

        // Method that runs when the player selects the "Run Middle" button lane to run down 
        private void btnMiddle_Click(object sender, EventArgs e)
        {
            try
            {
                // Variable that stores the middle lane choice 
                chosenLane = "Middle";
                // Calls this function to run the play and checks to see if the users choice resulted in a touchdown 
                RunPlay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running middle lane: " + ex.Message);
            }
        }

        // Method that runs when the player selects the "Run Right" button lane to run down
        private void btnRight_Click(object sender, EventArgs e)
        {
            try
            {
                // Variable that stores the right lane choice 
                chosenLane = "Right";
                // Calls this function to run the play and checks to see if the players choice resulted in a touchdown 
                RunPlay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running right lane: " + ex.Message);
            }
        }

        // Function that's called when the player makes their lane selection 
        private void RunPlay()
        {
            try
            {
                //If the ball wasn't caught the play shouldn't run
                if (!caughtBall)
                    return;

                // Randomly selects which lane the defender covers
                defenderLane = lanes[rand.Next(lanes.Length)];

                // Displays the defenders lane coverage to the player 
                lblDefender.Text = $"Defender is covering the {defenderLane} lane.";

                // Checks if the player and defender chose the same lane.
                if (chosenLane == defenderLane)
                {   // If so the player gets tackled, ending the play
                    lblResult.Text = "Defender tackles you immediately! Play over.";
                    lblResult.ForeColor = Color.Red;
                }
                else
                {
                    // If the player doesn't choose the same lane as the defender, the run is completed
                    int runYards = rand.Next(20, 60); // Randomly decides how many yards the user can run between 20 and 60 yards

                    // Get's the player's run power input from the numeric control
                    int runPower = (int)numericRunPower.Value;

                    // Adds wind interference between -5 to +5 yards
                    int wind = rand.Next(-5, 6);

                    // Calculates the final yards the user gains after power, wind and defender's difficulty
                    int finalYards = runYards + runPower + wind - currentDifficulty;

                    // Displays the calculation to the player; noting how the yards were affected by each variable
                    lblCalculation.Text = $"Run({runYards}) + Power({runPower}) + Wind({wind}) - Defender({currentDifficulty}) = {finalYards} yards";

                    // Accounts for chance of fumble based on runPower; 10% base chance
                    int fumbleChance = 10 - (runPower / 10);
                    bool fumble = rand.Next(0, 100) < fumbleChance;

                    // Determines the outcome of fumble
                    if (fumble)
                    {
                        lblResult.Text = "FUMBLE! Defense recoverd.";
                        lblResult.ForeColor = Color.Red;

                        GameManager.Log("Oh no! The player fumbled the ball!\n");
                        MessageBox.Show($"Fumble! No touchdown.");
                        GameManager.AwardMiniGame(0, false);
                        return;
                    }

                    // Determines the outcome based on how many yards the player ran
                    if (finalYards >= 50) // If user ran 50 or more yards, it's a touchdown
                    {
                        lblResult.Text = "TOUCHDOWN!";
                        lblResult.ForeColor = Color.Aquamarine;

                        decimal reward = 10 + finalYards / 2;
                        GameManager.Log($"TOUCHDOWN! You earned {reward} FB coins!\n");
                        MessageBox.Show($"TOUCHDOWN! You earned {reward} FB coins!");
                        GameManager.AwardMiniGame(reward, true);
                        return;
                    }
                    // If the player ran between 30 and 50 yards, it's a big gain
                    else if (finalYards >= 30)
                    {
                        lblResult.Text = "Big gain!";
                        lblResult.ForeColor = Color.Aquamarine;
                        GameManager.Log($"Player ran {finalYards} yards. \n");
                        GameManager.AwardMiniGame(0, false);
                    }
                    // If the player ran between 10 and 30 yards, it's a short gain
                    else if (finalYards >= 10)
                    {
                        lblResult.Text = "Short gain.";
                        lblResult.ForeColor = Color.Orange;
                        GameManager.Log($"Player ran {finalYards} yards. \n");
                        GameManager.AwardMiniGame(0, false);
                    }
                    else // If the player ran less than 10 yards, the defender immediately stops them 
                    {
                        lblResult.Text = "Defender stopped you immediately!";
                        lblResult.ForeColor = Color.OrangeRed;
                        GameManager.Log($"Player was stopped immediately. \n");
                        GameManager.AwardMiniGame(0, false);
                    }

                    // Disables lane buttons until the next catch 
                    btnLeft.Enabled = false;
                    btnMiddle.Enabled = false;
                    btnRight.Enabled = false;
                    // Ball hasn't been caught
                    caughtBall = false;

                    // Re-enables the catch button 
                    btnCatch.Enabled = true;
                    // Resets back to default value
                    numericRunPower.Value = 5;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during play: " + ex.Message);
            }
        }

       
    }
}
