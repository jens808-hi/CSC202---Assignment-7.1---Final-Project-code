/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, panels, 
 sound effects, to read/write files, access embedded resources, and use WM player */
using AxWMPLib; // Addeed to use Windows Media Player to work inside my form 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib; // Windows Media Player Library

namespace WinFormsAppFINAL
{
    public partial class frmGameSimulation : Form
    {
        // Keeps track of each teams score
        int homeScore = 0;
        int awayScore = 0;

        // Tracks different types of yards gained or lost during game play simulation 
        int passingYards = 0;
        int rushingYards = 0;
        int penaltyYards = 0;
        int kickingYards = 0;

        // Tracks plays with negative yards
        int sackedYards = 0;
        int holdingYards = 0;

        // Accounts for fumbes and interceptions during game play simulation 
        int turnovers = 0;

        // Tracks the yards gained specificially by the quarterback
        int QbYards = 0;

        // Counters for scoring touchdown and safety events 
        int Touchdown = 0;
        int Safety = 0;

        // Game info
        int quarter = 1; // Keeps track of which quarter the game is currently in, always starts in the 1st quarter
        int gameSeconds = 900; // 15 minutes 

        // Random number generator used to simulate game plays
        Random rand = new Random();

        // Runs first when the form is initially created 
        public frmGameSimulation()
        {
            // Automatically creates all the controls from the designer
            InitializeComponent();
            // Runs the Game Simulation form's load method when the form loads
            this.Load += frmGameSimulation_Load;
        }
        // Method that runs to extract video file from Resources
        private string ExtractVideo()
        {
            // Creates a temporary Path to where my video will be saved 
            string tempPath = Path.Combine(Path.GetTempPath(), "GameSimulation.mp4");

            // Gets the video file stored inside my programs Resources folder
            byte[] systemBytes = WinFormsAppFINAL.Resource1.GameSimulation;

            // Writes the video file to a temporary location 
            File.WriteAllBytes(tempPath, systemBytes);

            // Returns the location so the video player can use it
            return tempPath;
        }

        // Runs when the form first appears on the screen 
        private void frmGameSimulation_Load(object sender, EventArgs e)
        {
            // Enables the windows media player 
            axWindowsMediaPlayer.Enabled = true;
            // Hides the normal windows media player controls 
            axWindowsMediaPlayer.uiMode = "none";
            // Loops the video continuously during the game play simulation 
            axWindowsMediaPlayer.settings.setMode("loop", true);
            // Starts playing the video 
            axWindowsMediaPlayer.Ctlcontrols.play();

            // Loads the embedded video file from resources
            string videoPath = ExtractVideo();
            // Tells the media player which video to play 
            axWindowsMediaPlayer.URL = videoPath;

            // If the video fails, display an error message 
            if (!File.Exists(videoPath))
            {
                // Displays error message to the user
                MessageBox.Show("Video file not found: " + videoPath);
                return;
            }

            // Clears the play-by-play textbox when the form starts 
            rtbPlayByPlay.Clear();
        }
        // Updates my scoreboard during the game play simulation 
        private void UpdateScoreboard()
        {
            // Updates the Home and Away Teams scores
            lblHomeScore.Text = homeScore.ToString();
            lblAwayScore.Text = awayScore.ToString();

            // Updates all the game status labels on the screen during the game play simulation 
            lblPassingYards.Text = "Passing Yards: " + passingYards;
            lblRushingYards.Text = "Rushing Yards: " + rushingYards;
            lblSackedYards.Text = "Yards lost to sacks: " + sackedYards;
            lblPenaltyYards.Text = "Yards lost to penalty: " + penaltyYards;
            lblKickingYards.Text = "Yards from Kicks: " + kickingYards;
            lblHoldingYards.Text = "Holding Yards: " + holdingYards;
            lblTurnovers.Text = "Turnovers: " + turnovers;
            lblQbYards.Text = "Quarterback Yards: " + QbYards;
            lblTouchdown.Text = "Touchdowns: " + Touchdown;
            lblSafety.Text = "Safety: " + Safety;
            // Displays which quarter the game is currently in 
            lblQuarter.Text = "Quarter: " + quarter;
        }

        // Method that updates the game clock 
        private void UpdateGameClock()
        {
            // Converts the total seconds into minutes
            int minutes = gameSeconds / 60;
            // Gets the remaining seconds
            int seconds = gameSeconds % 60;
            // Displays the clock in MM:SS format 
            lblGameClock.Text = $"{minutes:00}:{seconds:00}";
        }

        // Method used to generate random football plays during the game simulation 
        private string GeneratePlay()
        {
            // Randomly selects one of the 16 possible football play outcomes 
            int play = rand.Next(16);
            // Used to determine which play occurs based on the randomly generated number above
            switch (play)
            {
                // Short passing play by the quarterback
                case 0:
                    // Generates a random number of yards gained to the passing yards stat between 1-4 yards
                    int passShort = rand.Next(1, 5);
                    // Adds the gained yards to the total passing yards stat
                    passingYards += passShort; 
                    // Returns a play by play description that will appear in the game log
                    return $"Quarterback drops back and throws a pass for {passShort}. Gain of {passShort} yards.";
                // Rushing play by the running back
                case 1:
                    // Generates a random number of yards gained to the total rushing yards stat between 3 and 14 yards
                    int run = rand.Next(3, 15);
                    // Adds the rushing yards to the total rushing yards stat 
                    rushingYards += run;
                    // Returns a play by play description that will appear in the game log
                    return $"Running back breaks through the line! Gain of {run} yards.";
                // Deep pass play down the field
                case 2:
                    // Generates a random number of yards gaained to the total passing yards stat between 16 and 29 yards
                    int passDeep = rand.Next(16, 30);
                    // Adds the passing yards to the total passing yards stat 
                    passingYards += passDeep;
                    // Returns a play by play description that will appear in the game log
                    return $"Deep pass down the sideline.....CAUGHT! {passDeep} yard gain!";
                // Quarterback sacked by the defense play
                case 3:
                    // Generates a random number of yards gained to the total yards lost due to being sacked stat between 5 and 14 yards
                    int sacked = rand.Next(5, 15);
                    // Increases total yards lost
                    sackedYards += sacked; 
                    // Returns a play by play description that will appear in the game log
                    return $"Defense blitzes! Quarterback is sacked for a loss of {sacked} yards.";
                // Short rush play up the field
                case 4:
                    // Generates a random number of yards gained to the total rushing yards stat between 2 and 8 yards 
                    int runShort = rand.Next(2, 9);
                    // Adds the rushing yards to the total rushing yards stat 
                    rushingYards += runShort;
                    // Returns a play by play description that will appear in the game log
                    return $"Running play up the middle. Gain of {runShort} yards.";
                // Long touchdown pass play
                case 5:
                    // Generates a random number of yards gained to the total quarterback yards stat between 20 and 29 yards 
                    int runLong = rand.Next(20, 30);
                    // Adds the qb yards to the total qb yards stat 
                    QbYards += runLong;
                    // Adds 7 points to the home teams score for touchdown 
                    homeScore += 7;
                    // Increments touchdown counter by 1
                    Touchdown++; 
                    // Returns a play by play description that will appear in the game log
                    return $"Quarterback launches a deep ball to the reciever...{runLong} yards... TOUCHDOWN!!";
                // Interception play
                case 6:
                    // Increments turnovers counter by 1
                    turnovers++; 
                    // Returns a play by play description that will appear in the game log
                    return "Intercepted! Defense takes the ball!";
                // Fumble play
                case 7:
                    // Increments turnovers counter by 1
                    turnovers++;
                    // Returns a play by play description that will appear in the game log
                    return "Fumble!, Defense recovers the ball!";
                // Home team touchdown play 
                case 8:
                    // Adds 7 points to the home teams score for touchdown 
                    homeScore += 7;
                    // Increments touchdown counter by 1
                    Touchdown++; 
                    // Returns a play by play description that will appear in the game log
                    return "TOUCHDOWN home team!!";
                // Away team touchdown play
                case 9:
                    // Adds 7 points to the away teams score for touchdown 
                    awayScore += 7;
                    // Increments touchdown counter by 1
                    Touchdown++; 
                    // Returns a play by play description that will appear in the game log
                    return "TOUCHDOWN away team!!";
                // Safety 
                case 10:
                    // Adds 2 points to the away teams score for safety
                    awayScore += 2;
                    // Increments safety counter by 1
                    Safety++; 
                    // Returns a play by play description that will appear in the game log
                    return $"Defense tackles Offense in their own endzone for a safety. Defense {awayScore} points!";
                // Extra PAT (point after touchdown) kick play
                case 11:
                    // Generates a random number of yards gained to the total kicking yards stat between 30 and 39 yards 
                    int kick = rand.Next(30, 40);
                    // Adds the kicking yards to the total kicking yards stat
                    kickingYards += 7;
                    // Adds 1 points to the away teams score
                    awayScore += 1;
                    // Returns a play by play description that will appear in the game log
                    return $"And there's the kick, Offense kicked an amazing {kick} yard goal. {awayScore} point after touchdown to the Offense!";
                // Pass interference penalty by Defense
                case 12:
                    // Generates a random number of yards to the total penalty yards stat between 5 and 9 yards
                    int pass = rand.Next(5, 10);
                    // Adds the penalty yards to the total penalty yards stat
                    penaltyYards += pass;
                    // Returns a play by play description that will appear in the game log
                    return $"Pass interference. Offense gains {pass} yards";
                // Holding penatly
                case 13:
                    // Generates a random number of yards to the holding yards stat between 5 and 9 yards
                    int hold = rand.Next(5, 10);
                    // Adds the holding yards to the total holding yards stat
                    holdingYards -= hold;
                    // Returns a play by play description that will appear in the game log
                    return $"Holding penalty! {hold} yards deducted from Offense!";
                // Safety 
                case 14:
                    // Adds 2 points to the home teams score for safety
                    homeScore += 2;
                    // Increments safety counter by 1
                    Safety++; 
                    // Returns a play by play description that will appear in the game log
                    return $"Defense tackles Offense in their own endzone for a safety. Defense {homeScore} points!";
                // Medium pass play
                case 15:
                    // Generates a random number of yards to the medium passing yards stat between 6 and 14 yards
                    int passMedium = rand.Next(6, 15);
                    // Adds the passing yards to the total medium passing yards stat
                    passingYards += passMedium;
                    // Returns a play by play description that will appear in the game log
                    return $"Quarterback drops back and throws a pass for {passMedium}. Gain of {passMedium} yards.";
            }
            // Returns a play by play description that will appear in the game log
            return "The play results in no gain.";
        }

        // Method to add play by play log 
        private void AddPlay(string text)
        {
            // Adds the different plays description to the Rich Text box
            rtbPlayByPlay.AppendText(text + Environment.NewLine);
            // Automatically scrolls to the newest play
            rtbPlayByPlay.SelectionStart = rtbPlayByPlay.Text.Length;
            rtbPlayByPlay.ScrollToCaret();
        }
        // Method that runs the Main Game Play Simulation Loop for the football game
        private async void RunSimulation()
        {
            // Displays a kickoff message in the play by play log when the game simulation begins
            AddPlay("Kickoff! The game has started.");
            // Using a short delay to mimic a live game where the user can see the kickoff before the plays begin, in this case it's a kickoff message displayed
            await Task.Delay(500);

            // Game runs for the normal 4 quarters of a football game
            while (quarter <= 4)
            {
                // Continues running plays while there is still time left in the current quarter
                while (gameSeconds > 0)
                {
                    // Generates a random play result (run, pass, touchdown, interception, etc)
                    string play = GeneratePlay();
                    // Adds the generated play to the play by play log that is displayed to the user 
                    AddPlay(play);

                    /* Reduces the game clock by a random number of seconds to simulate the time a real football play would take, 
                     while also giving me 15 - 20 plays each quarter */
                    gameSeconds -= rand.Next(10, 25); 

                    // Updates the gameclock and scoreboard after each play
                    UpdateGameClock();
                    UpdateScoreboard();
                    // Using another short delay so the simulation doesn't instantly run and gives the user time to read the play by play updates as they happen
                    await Task.Delay(200);
                }
                // Moves counter foward to the next quarter
                quarter++;
                // Resets the quarter clock
                gameSeconds = 600;

                // Checks if the current quarter still falls within the normal 1-4 quarters game length
                if (quarter <= 4)
                {
                    /* If the game falls within quarters 1-4, display a message in the play-by-play log announcing the start of the quarter,
                     "{quarter}" variable used to represent the number of current quarter */
                    AddPlay($"---Start of Quarter {quarter}---");
                }
                else
                {
                    /* If the quarter number is greater than 4, than the game has ended. Displays a message in the play-by-play log
                     indicating that the game play simulation has finished and the game is over.*/
                    AddPlay("Final Whistle! Game Over.");
                }
            }
        }
        // Event handler that runs when the "Start Game" button is clicked
        private async void btnStartSimulation_Click(object sender, EventArgs e)
        {
            // Clears any previous play-by-play
            rtbPlayByPlay.Clear();

            // Resets all the game stats
            homeScore = 0;
            awayScore = 0;
            passingYards = 0;
            rushingYards = 0;
            penaltyYards = 0;
            kickingYards = 0;
            sackedYards = 0;
            holdingYards = 0;
            QbYards = 0;
            turnovers = 0;
            Touchdown = 0;
            Safety = 0; 
            // Resets the state of the game
            quarter = 1;
            gameSeconds = 600;

            // Updates the scoreboard and game clock
            UpdateScoreboard();
            UpdateGameClock();
            // Starts the game play simulation 
            RunSimulation();
        }
        // Event handler that runs when the "Return to Main Menu" button is clicked
        private void btnReturn_Click(object sender, EventArgs e)
        {
            //Creates a new instance of the Main Menu form so it can be opened 
            frmMainMenu mainMenu = new frmMainMenu();
            // Closes the current form so only one form is visible 
            this.Close();
            // Displays the Main Menu form to user
            mainMenu.Show();

        }
        // Event handler that runs when the "Mini Games" button is clicked
        private void btnMini_Click(object sender, EventArgs e)
        {
            //Creates a new instance of the form 
            frmMiniGamesMenu miniGames = new frmMiniGamesMenu();
            // Closes the current form so only one form is visible
            this.Close();
            // Displays the Mini Games Menu form to user
            miniGames.Show();

            
        }
    }
}
