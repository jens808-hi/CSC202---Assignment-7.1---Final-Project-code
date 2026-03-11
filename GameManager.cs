/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files and json serialization */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WinFormsAppFINAL
{

    /* Centralized game manager for Bet Blitz so all forms can access it to track player's balance, place bets, track wins & losses, manage player profiles, watch a football game 
	 simulation, manage award winnings for mini-games, logging messages, and save/load leaderboard data */
    public static class GameManager
	{
        // Player's total money currently available 
        public static decimal PlayerBalance = 100;
		// Current bet placed by the player 
		public static decimal CurrentBet = 0;
		// Tracks the number of games the player has won
		public static int Wins = 0;
        // Tracks the number of games the player has lost
        public static int Losses = 0;

		// Stores the players profile object 
		public static PlayerProfile? CurrentPlayer;
		// Triggered when the players data changes 
        public static event Action? OnPlayerDataChanged;
        // Triggered when something should be written to the game log, serves as an indicator that something might be wrong even if no errors are thrown (debugging)
        public static event Action<string>?OnGameLog;
        // File name where Leaderboard data (players stats) is stored 
        public static string LeaderboardFile = "leaderboard.json"; 

		// Method that runs when a player creates or selects a profile and loads their stats into Game Manager
		public static void SetPlayer(PlayerProfile profile)
		{
			// Assigns the profiles as a currently active player 
			CurrentPlayer = profile;
			// Loads the data from players profile into Game Manager 
			PlayerBalance = profile.Balance;
			Wins = profile.Wins;
			Losses = profile.Losses;

            // Tells the user interface that the player's data changed
            NotifyPlayerDataChanged();
		} 

		// Method that sends a messages to any form listening of game events
		public static void Log(string message)
		{
			// The form listens for messages from an event, sends a message and triggers the event. "?" prevents it from crashing if nothing is listening
			OnGameLog?.Invoke(message);
		}

		// Method that adds a player player update event 
		public static void NotifyPlayerDataChanged()
		{
			// Checks if an event has any listeners 
			OnPlayerDataChanged?.Invoke(); 
		}

		// Method used to place a bet if the player has enough balance and deducts the bet amount 
		public static bool PlaceBet(decimal amount)
		{
			{
				// Checks if the player has enough money to place a bet
				if (amount <= PlayerBalance)
				{
					// Stores the bet amount
					CurrentBet = amount;
					// Deducts the bet amount from players balance 
					PlayerBalance -= amount;
					// Tells the user interface to update the players balance 
					NotifyPlayerDataChanged();

					// This indicates the bet was placed succesfully
					return true;
				}
				// Player ran out of money, bet couldn't be placed 
				return false;
			}
		}
	

         // Method to award player winnings after a successful bet 
        public static void Payout(decimal multiplier)
		{
			// Calculate winnings (bet amount x multiplier)
			decimal winnings = CurrentBet * multiplier;
			// Add winnings to players balance
			PlayerBalance += winnings;
			// Resets the bet to 0
			CurrentBet = 0;
			// Increases the win counter
			Wins++;

            // Saves the updated Leaderboard results to json file
            SaveLeaderboard();

			// Tells the user interface to update the players balance counter
			NotifyPlayerDataChanged();
        }

		// Method to reset bets when player loses
		public static void ResetBet()
		{
			// Player loses the bet, resets current bet to 0
			CurrentBet = 0;
			// Increases the losses counter 
			Losses++;
            // Saves the updated Leaderboard results to json file
            SaveLeaderboard();
		}
		// Method that runs when the player wins or loses a mini game, awards 
		public static void AwardMiniGame(decimal reward, bool success)
		{
			// Adds a reward to player's balance
			PlayerBalance += reward;
			// Updates the player's stats depending on success
			if (success) Wins++;
			else Losses++;

			// Saves the results to the Leaderboard
			SaveLeaderboard();
			// Updates the user interface
            NotifyPlayerDataChanged();
        }

		// Saves the Leaderboard, storing the players data to a JSON file
		public static void SaveLeaderboard()
		{
			try
			{	// Creates an object to hold the players balance, wins and losses
				var data = new LeaderboardData()
				{
					Balance = PlayerBalance,
					Wins = Wins,
					Losses = Losses
				};

				// Converts the object to JSON text with indenation to make it easier to read
				string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                //  Writes to the JSON string to the file and overwrites any exisiting data and
                File.WriteAllText(LeaderboardFile, json);
			}
			catch (Exception ex) 
			{
                // Logs any exceptons that occur while saving to console and returns false, displaying error message to player 
                Console.WriteLine("Error, Leaderboard data could not be saved: " + ex.Message);
            }
		}

		// Loads Leaderboard data from saved JSON file and loads the players stats
		public static void LoadLeaderboard()
		{
			try
			{	// Checks if the file exists
				if (File.Exists(LeaderboardFile))
				{
					// Reads the JSON text 
					string json = File.ReadAllText(LeaderboardFile);
					// Converts JSON string back into the object 
					var data = JsonSerializer.Deserialize<LeaderboardData>(json);
					// Reloads the players stats into Game Manager
					PlayerBalance = data.Balance;
					Wins = data.Wins;
					Losses = data.Losses;

                    // Tells the user interface 
                    NotifyPlayerDataChanged();
                }
			}
            // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            catch (Exception ex) 
            {
				// Logs any exceptons that occur while saving to console and returns false, displaying error message to player 
				Console.WriteLine("Error, Leaderboard data could not be loaded: " + ex.Message);
            }
        }

		// Used to store Leaderboard information for saving and loading JSON
		public class LeaderboardData
		{
			public decimal Balance { get; set; } // Player's current balance
			public int Wins { get; set; } // Number of mini-games won
			public int Losses { get; set; } // Number of mini-games lost
		}
	}
	
}
