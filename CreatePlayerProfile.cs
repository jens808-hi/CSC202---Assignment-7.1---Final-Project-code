/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files */
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsAppFINAL
{
    /* Form allows players to create a new player profile for the game. The player enters a username and uploads a profile picture for their Avatar. 
     The program automatically generates a unique Account ID so there are no duplicates, a starting balance and inital wins/losses stat. Once the profile
     is created, the data is saved using the ProfileManager and is loaded into the GameManager so the player can immediately start playing  */
    public partial class CreatePlayerProfile : Form
    {
        // Stores the file path of the profile image the player selects
        string selectedImagePath = "";
        // Stores the auto-generated account ID created by the program 
        string generatedAccountID = "";
        // Stores the PlayerProfile created in this form, so the calling form can access it
        public PlayerProfile CreatedProfile { get; private set; }
        // Random number generator used to create random Account IDs
        Random rand = new Random();

        // // Runs when the form is first created 
        public CreatePlayerProfile()
        {
            // Initializes all from controls 
            InitializeComponent();
        }

        // Method that runs automatically when the form appears
        private void CreatePlayerProfile_Load(object sender, EventArgs e)
        {
            // Loads existing profiles so program can check for duplicate IDs
            ProfileManager.LoadProfiles();

            // Generates a unique account ID
            generatedAccountID = GenerateUniqueAccountID();

            // Displays unique account ID in the textbox
            txtAccountID.Text = GenerateUniqueAccountID();
        }
        // Method that generates a random 5-digit account ID
        private string GenerateUniqueAccountID()
        {
            // Variable used to temporarily store generated ID
            string newID;

            do // Loop continues until a unique ID is created 
            {
                // Generates a random 5 digit number between 10000 - 99999
                newID = rand.Next(10000, 100000).ToString();

                // Loop keeps generating a new ID if the ID already exists in the Profiles list. 
            } while (ProfileManager.Profiles.Exists(p => p.AccountID == newID));

            // Returns the unique ID once it passes the second check
            return newID;
        }


        // Method that runs the the "Create Profile" button is clicked
        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            // Tries to run the code safely but if an error happens, catch block will handle it
            try
            {
                // Validates username
                if (string.IsNullOrWhiteSpace(txtUserName.Text))
                {
                    MessageBox.Show("Enter a username.");
                    return;
                }

                // Checks if username already exists
                if (ProfileManager.Profiles.Exists(profile => profile.UserID == txtUserName.Text))
                {
                    MessageBox.Show("This username already exists.");
                    return;
                }

                // Create the new Player Profile
                PlayerProfile profile = new PlayerProfile()
                {
                    UserID = txtUserName.Text.Trim(), // Sets the UserId to text input recieved from the player 
                    AccountID = generatedAccountID, // Sets the AccountID to text generated for players bank
                    Balance = 100, // Initial balance given to player when a new profile is created
                    Wins = 0, // Sets the wins counter to 0 when a new profile is created
                    Losses = 0, // Sets the losses counter to 0 when a new profile is created 
                    ProfileImagePath = selectedImagePath // Stores the file path of the selected profile image
                };

                // Gives other forms the ability to access this profile
                CreatedProfile = profile;

                // Saves the newly created profile to the json file using Profile Manager
                ProfileManager.AddProfile(profile);

                // Sets the profile as an active player profile in the Game Manager 
                GameManager.SetPlayer(profile);

                // Displays a confirmation message of player profile info 
                MessageBox.Show(
                    $"Profile Created!\n\n +" +
                    $"Username: {profile.UserID}\n" +
                    $"Account ID: {profile.AccountID}\n" +
                    $"Starting Balance: {profile.Balance} FB Coins");

                // Closes the profile creation form and returns the player to the previous screen 
                this.Close();
            }
            catch (Exception ex) // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            {
                // Logs any exceptons that occur while saving to console and returns false, displaying error message to player 
                Console.WriteLine("Error creating profile: " + ex.Message);
                // Displays error message 
                MessageBox.Show("Error creating profile: " + ex.Message);

            }
        }

        // Method that runs when the "Upload image" button is clicked, opening the file dialog to let the player select a profile picture
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            // Tries to run the code safely but if an error happens, catch block will handle it
            try
            {
                // Creates and opens a new windows file picker dialog 
                OpenFileDialog dialog = new OpenFileDialog();

                // Only allows common image file types 
                dialog.Filter = "Image Files|*.png;*.jpg;*.jpeg";

                // Shows dialog to the player and checks if they selected a file
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Stores the file path in selectedImage Path variable 
                    selectedImagePath = dialog.FileName;
                    // Displays the selected image in the picturebox control named picAvatar
                    picAvatar.ImageLocation = selectedImagePath;
                }
            }
            catch (Exception ex) // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            {
                // Logs any exceptons that occur while saving to console and returns false, displaying error message to player 
                Console.WriteLine("Error selecting image: " + ex.Message);
                // Displays error message to player
                MessageBox.Show($"Error selecting image: " + ex.Message);
            }
        }
    }
}




                


