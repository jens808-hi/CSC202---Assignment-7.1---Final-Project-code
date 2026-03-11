/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Forms;

namespace WinFormsAppFINAL
{
    // Form that handles placing single or parlay bets in the game
    public partial class frmPlaceBets : Form
    {
        Random rand = new Random();
        // Array list to store all bets placed by the player
        private List<Bet> Bets = new List<Bet>();
        // File path for saving/loading the bets 
        private string BetsFile = "bets.json";

        // Determines bet 
        public class Bet
        {
            public string BetType { get; set; } = "Single"; // Single or Parlay bet 
            public decimal Amount { get; set; } = 0; // Bet amount 
            public bool? Won { get; set; } = null; // null means bet hasn't completed 
        }
        public frmPlaceBets()
        {
            // Initializes the forms controls
            InitializeComponent();
        }
        // Runs when the form loads 
        private void frmPlaceBets_Load(object sender, EventArgs e)
        {
            // Loads existing bets from file if it exists
            LoadBets();

            // Sets the minimum and maximum values for numeric input of bet amount 
            numericBetAmount.Minimum = 1;
            numericBetAmount.Maximum = GameManager.PlayerBalance;

            // Populates the combo box with bet type options 
            comboBetType.Items.Clear(); // Clears any existing bets
            comboBetType.Items.Add("Single"); // Single bet 
            comboBetType.Items.Add("Parlay"); // Parlay bet
            comboBetType.SelectedIndex = 0; // Default 

            // Updates the balance label to reflect current player balance 
            UpdateBalanceLabel();
        }

        // Runs when the "Place Bet" button is clicked 
        private void btnPlaceBet_Click(object sender, EventArgs e)
        {
            // Gets the amount of money the player wants to bet from the numerica control 
            decimal amount = numericBetAmount.Value;
            // Gets the type of bet from the combo box ("Single" or "Parlay") 
            string type = comboBetType.SelectedItem?.ToString() ?? "Single";

            // Ensures player has enough balance
            if (!GameManager.PlaceBet(amount))
            {
                // Displays message warning player 
                MessageBox.Show("Insufficient balance!");
                // Ends here if they can't afford to vet 
                return;
            }

            // Create new bet and add to array
            Bet newBet = new Bet
            {
                // Sets the type of bet
                BetType = type,
                // Sets the bet amount 
                Amount = amount
            };
            // Adds this bet to the array list of bets 
            Bets.Add(newBet);

            // Saves all current bets to the json file
            SaveBets();
            // Displays message to the player that the bet was successful 
            MessageBox.Show($"Placed a {type} bet of {amount:C}");
            // Updates the balance label to reflect the new balance 
            UpdateBalanceLabel();
        }

        // Simulates completing all bets whent the "Complete Bets" button is clicked 
        private void btnCompleteBets_Click(object sender, EventArgs e)
        {
            // Loops through every bet in the array 
            foreach (var bet in Bets)
            {
                // Only processes bets that have not yet been completed
                if (bet.Won == null)
                {
                    // Stores whether the bet was won 
                    bool win;
                    // Multiplier for the payout
                    decimal multiplier;

                    // Determins the outcome based on the type of bet 
                    if (bet.BetType == "Single")
                    {
                        // 50% chance to win 
                        win = rand.Next(0, 2) == 0;
                        // Doubles the money if won 
                        multiplier = 2;
                    }
                    else
                    {
                        // 33% chance to win 
                        win = rand.Next(0, 3) == 2;
                        // Triples the money if won 
                        multiplier = 3;
                    }
                    // Stores the outcome in the Bet object 
                    bet.Won = win;
                    // Updates GameManager based on outcome 
                    if (win)
                        // Adds winnings to players balance
                        GameManager.Payout(multiplier);
                    else
                        // Resets bet if player loses
                        GameManager.ResetBet();
                }
            }
            SaveBets();
            MessageBox.Show("Bets completed! Check your balance.");
            UpdateBalanceLabel(); 

            }
            private void UpdateBalanceLabel()
                {
                    lblBalance.Text = $"Balance: {GameManager.PlayerBalance:C}";
                    numericBetAmount.Maximum = GameManager.PlayerBalance;
                }
            
            private void SaveBets()
            {
                try
                {
                    // Serializes the bets list to json text 
                    string json = JsonSerializer.Serialize(SaveBets, new JsonSerializerOptions { WriteIndented = true });
                    // Writes the json text to the file 
                    File.WriteAllText(BetsFile, json);
                }
                catch (Exception ex) 
                {
                // Shows error message if file cannot be saved 
                MessageBox.Show("Error saving bets: " + ex.Message);
            }
        }

        private void LoadBets()
        {
            try
            {
                if (File.Exists(BetsFile))
                {
                    string json = File.ReadAllText(BetsFile);
                    Bets = JsonSerializer.Deserialize<List<Bet>>(json) ?? new List<Bet>(); 
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error loading bets: " + ex.Message); 
            }
        }
        private void btnReturn_Click (object sender, EventArgs e)
        {
            // Returns to Main Menu 
            this.Close();
        }
    }
}


