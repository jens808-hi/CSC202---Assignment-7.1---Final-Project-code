// Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, picture boxes, and timers.
using System;
using System.Drawing;
using System.Windows.Forms;


namespace WinFormsAppFINAL
{
    // Represents the GUI's Main/starting window of my project
    public partial class frmMainMenu : Form
    {
        // Name of my GUI's full Game title text that will appear one character at a time
        string fullTitleGame = "B E T  B L I T Z";
        // Tracks which character in the tiltle it's currently revealing 
        int revealIndex = 0;
        // Checks if the Game Title has been completely revealed 
        bool revealComplete = false;
        // Font size used for Game Title's glowing/pulsing effect
        float fontSize = 34f;
        // Checks whether the Game Title is growing or shrinking 
        bool growing = true;
        // Random generator 
        Random random = new Random();
        // Displays the current balance number
        decimal displayedBalance = 0;
        // The real balance stored inside GameManager
        decimal targetBalance = 0;
        // Timer used to animate the counter
        System.Windows.Forms.Timer balanceTimer = new System.Windows.Forms.Timer();

        // Runs when the form is first created 
        public frmMainMenu()
        {
            // Initializes all from controls 
            InitializeComponent();

            // Ties to the GameManager so the user interface refreshes whenever the game data changes throughout all forms
            GameManager.OnPlayerDataChanged += Refresh;

            // Configures the timer animation for players balances to simulate a casino timer
            balanceTimer.Interval = 30;
            balanceTimer.Tick += BalanceTimer_Tick;
        }

        private void MainMenuform_Load(object sender, EventArgs e)
        {
            // Loads all saved player profiles from the json file
            ProfileManager.LoadProfiles();

            // Begins with an empty title so the reveal animation control can start
            lblTitleGame.Text = "";

            // Centers the title horizontally on the form 
            lblTitleGame.Left = (this.ClientSize.Width - lblTitleGame.Width) / 2;

            // Starts the animation timer for the Game title reveal 
            titleTimer.Start();

            // Load previous Leaderboard data and player stats
            GameManager.LoadLeaderboard();

            // Refreshes the player's balance, wins & losses labels everytime the user returns to the Main Menu
            RefreshUI();
        }

        // Method that updates the player's balance, wins & losses
        private void RefreshUI()
        {
            // Updates the players balance animation 
            UpdateBalanceLabel();
            // Updates the players balance on the screen 
            lblBalance.Text = "Balance: $" + GameManager.PlayerBalance;
            // Updates the players wins on the screen
            lblWins.Text = $"Wins: {GameManager.Wins}";
            // Updates the players losses on the screen
            lblLosses.Text = $"Losses: {GameManager.Losses}";
        }

        // Starts the player's balance casino like animation 
        public void UpdateBalanceLabel()
        {
            // Sets the player's balace equivalent to the GamerManager's recorded balance
            targetBalance = GameManager.PlayerBalance;

            // Starts the animation timer
            balanceTimer.Start();
        }

        // Method that runs the player's balance animation timer 
        private void BalanceTimer_Tick(object sender, EventArgs e)
        {
            // Calculates the difference between displayed and real balance
            decimal difference = Math.Abs(targetBalance - displayedBalance);

            // Determines how fast the counter moves, large wins animate faster than smaller ones
            decimal step = Math.Max(1, difference / 10);

            // If the displayed balance is lower than the target than increase
            if (displayedBalance < targetBalance)
            {
                displayedBalance += step;

                if (displayedBalance > targetBalance)
                    displayedBalance = targetBalance;

                // Green glow effect for winnings
                lblBalance.ForeColor = Color.LimeGreen;
            }
            // If displayed balance is higher than target than decrease 
            else if (displayedBalance > targetBalance)
            {
                displayedBalance -= step;

                if (displayedBalance < targetBalance)
                    displayedBalance = targetBalance;

                // Updates the balance label textt
                lblBalance.Text = "Balance: $" + displayedBalance.ToString();

                // Stops the animation once target is reached
                if (displayedBalance == targetBalance)
                    balanceTimer.Stop();
            }
        }

        // Method that controls the letter reveal and glow effect of the GUI Game's Title animation timer
        private void titleTimer_Tick(object sender, EventArgs e)
        {
            // First part of letter reveal
            if (!revealComplete)
            {
                //If there are still letters left to reveal 
                if (revealIndex < fullTitleGame.Length)
                {
                    // Adds the next character
                    lblTitleGame.Text += fullTitleGame[revealIndex];

                    // Moves to the next character
                    revealIndex++;

                    // Re-centers the title as it grows
                    lblTitleGame.Left = (this.ClientSize.Width - lblTitleGame.Width) / 2;
                }
                else
                {
                    // Once all letters are revealed, move to the Glowing / Pulse annimation effect
                    revealComplete = true;
                }
            }
            else // Second part of letter reveal. After the title completes the reveal, animates the font size to create glowing pulse effect
            {
                // If title grows larger
                if (growing)
                    fontSize += 0.3f;

                // If title shrinks
                else
                    fontSize -= 0.3f;

                // If the title reaches the maximum size
                if (fontSize >= 48)
                    growing = false;

                // If the title reaches the minimum size
                if (fontSize <= 34)
                    growing = false;

                // Applies the updated font size
                lblTitleGame.Font = new Font("Wide Latin", fontSize, FontStyle.Bold);

                // Color flicker to mimic casino glow
                lblTitleGame.ForeColor = Color.White;
            }
        }

        // Method that runs the "Mini Games" button when clicked
        private void btnMiniGames_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form
            frmMiniGamesMenu miniGamesMenu = new frmMiniGamesMenu();

            // Hide the main menu while the mini games runs
            this.Hide();

            // Shows the simulation form
            miniGamesMenu.ShowDialog();

            // When the Mini Games form closes, show main menu again
            this.Show();
        }

        // Method that runs the football "Game Simulation" button when clicked
        private void btnGameSimulation_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form
            frmGameSimulation simulationForm = new frmGameSimulation();

            // Hide the main menu while the simulation runs
            this.Hide();

            // Shows the simulation form
            simulationForm.ShowDialog();

            // When the simulation form closes, show main menu again
            this.Show();

            // Refresh the labels in case GameManager changed them
            UpdateBalanceLabel();

        }
        private void btnOpenProfiles_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form
            ProfileSelect profileForm = new ProfileSelect();

            // Hide the main menu while the simulation runs
            this.Hide();

            profileForm.ShowDialog();

            // When the simulation form closes, show main menu again
            this.Show();
        }

        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the Create Player Profile form
            CreatePlayerProfile createForm = new CreatePlayerProfile();

            // Hides the main menu while the profile creation form is open
            this.Hide();

            // Shows the form
            createForm.ShowDialog();

            // Closes the form to show main menu again
            this.Show();
        }

        // Method that run when the "Exit" button is clicked
        private void btnExit_Click(object sender, EventArgs e)
        {
            // Creates a a message box asking the user for confirmation before exiting the application
            DialogResult result = MessageBox.Show(
                " Are you sure you want to exit Bet Blitz?",
                // Title of the message box
                "Exit Game",
                // Displays two buttons on the message box, Yes and No
                MessageBoxButtons.YesNo,
                // Displays a question mark icon
                MessageBoxIcon.Question);

            // Asks the player to make a decision 
            if (result == DialogResult.Yes)
            {
                // Sends message to the GameManager log
                GameManager.Log("Player exited the game.");
                // Closes all forms and stops the application completely
                Application.Exit();
            }
        }

        private void btnBets_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the Create Player Profile form
            frmPlaceBets betsForm = new frmPlaceBets();

            // Hides the main menu while the profile creation form is open
            this.Hide();

            // Shows the form
            betsForm.ShowDialog();

            // Closes the form to show main menu again
            this.Show();
        }
    }
}



