/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files */
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using WinFormsAppFINAL;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WinFormsAppFINAL
{
    // Form created to allow the player to view, select, and create profiles
    public partial class ProfileSelect : Form
    {

        // Runs when the form is created
        public ProfileSelect()
        {
            // Initializes all user interface controls
            InitializeComponent();
        }

        // Method that runs when the form loads
        private void ProfileSelect_Load(object sender, EventArgs e)
        {
            try
            {
                // Calls the ProfileManager to loads all existing player profiles from json file into memory
                ProfileManager.LoadProfiles();

                // Populates the list box with profile UserIDs
                LoadProfilesIntoList();
            }
            catch (Exception ex) // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            {
                // If an error ocurrs while loading profiles, write the error to the console
                Console.WriteLine("Error finding profile: " + ex.Message);
                // Displays error message to the player
                MessageBox.Show("Error finding profile: " + ex.Message);
            }
        }

        // Populates the listbox with usernames from ProfileManager
        private void LoadProfilesIntoList()
        {
            try
            {
                // Clears any exisitiing items from the listbox
                lstProfiles.Items.Clear();

                // Loops through each Players Profile stored in the Profile Manager.profiles
                foreach (var profile in ProfileManager.Profiles)
                {
                    // Adds the username to the listbox 
                    lstProfiles.Items.Contains(profile.UserID);
                    {
                        // Adds the player's UserId to the list box so the player can select it.
                        lstProfiles.Items.Add(profile.UserID);
                    }
                }
            }
            // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            catch (Exception ex)
            {
                // If an error ocurrs while loading profiles, write the error to the console
                Console.WriteLine("Error finding profile: " + ex.Message);
                // Displays error message to the player
                MessageBox.Show("Error finding profile: " + ex.Message);
            }
        }

        // Method that run when the "Select Profile" button is clicked
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                // Checks to make sure the player has selected a profile in the Listbox 
                if (lstProfiles.SelectedItem != null)
                {
                    // Converts the selected ListBox item into a string representing the UserID
                    string selectedUserId = lstProfiles.SelectedItem.ToString()!;
                    // Retreives the full PlayerProfile object using the selected UserID
                    var profile = ProfileManager.GetProfile(selectedUserId);

                    // Checks to ensure the profile was found 
                    if (profile != null)
                    {
                        // Loads the selected player profile into the GameManager
                        GameManager.SetPlayer(profile);
                        // Displays a confirmation message to the player showing the selected profile
                        MessageBox.Show($"Profile {profile.UserID} selected!\nBalance: {profile.Balance}");

                        // Sets the Dialog Resutl to ok so the calling form knows the selection was successful
                        this.DialogResult = DialogResult.OK;
                        
                    }
                }
            }
            // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            catch (Exception ex)
            {
                // If an error ocurrs while loading profiles, write the error to the console
                Console.WriteLine("Error selecting profile: " + ex.Message);
                // Displays error message to the player
                MessageBox.Show("Error selecting profile: " + ex.Message);
            }
        }
        // Method that run when the "Create Profile" button is clicked
        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            try
            {
                // Gets the input text entered by the player in the textbox, removes any extra spaces before after the username
                string userId = txtNewProfile.Text.Trim();
                // Checks to make sure the player actually entered a username 
                if (!string.IsNullOrEmpty(userId))
                {
                    // Creates a new PlayerProfile object and assigns default values
                    var newProfile = new PlayerProfile()
                    {
                        UserID = userId, // Sets the UserID to the name entered by the player
                        Balance = 100, // Gives new players a starting balance of 100 coins
                        Wins = 0, // New profiles start with 0 wins
                        Losses = 0, // New profiles start with 0 losses
                        ProfileImagePath = "" // No profile image is assigned at this part of the creation in the form 
                    };

                    // Adds the newly created profile to the ProfileManager and saves the profile to the json file
                    ProfileManager.AddProfile(newProfile);
                    // Adds the new username to the listbox so the player can immediately select it 
                    lstProfiles.Items.Add(newProfile.UserID);
                    // Clears the textbox so te player can enter another profile if needed
                    txtNewProfile.Clear();
                }
            }
            // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            catch (Exception ex)
            {
                // If an error ocurrs while loading profiles, write the error to the console
                Console.WriteLine("Error creating profile: " + ex.Message);
                // Displays error message to the player
                MessageBox.Show("Error selecting profile: " + ex.Message);
            }
        }

        // Runs when the "Return to Main Menu" button is clicked
        private void btnReturn_Click(object sender, EventArgs e)
        {
            //Creates a new instance of the Main Menu form 
            frmMainMenu mainMenu = new frmMainMenu();

            // Shows the main menu form 
            mainMenu.Show();

            // Closes the form
            this.Close();
        }
    }
}
    