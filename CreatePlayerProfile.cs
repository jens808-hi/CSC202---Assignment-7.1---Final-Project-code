/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files */
using Microsoft.VisualBasic;
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
    /* Form used to create a players profile by entering a username, generating an Account ID for digital currency/rewards, profile picture upload
     and creates the players profile from the information provided */
    public partial class CreatePlayerProfile : Form
    {
        // Stores the file path of the profile image the player selects
        string selectedImagePath = "";

        // // Runs when the form is first created 
        public CreatePlayerProfile()
        {
            // Initializes all from controls 
            InitializeComponent();
        }

        // Method that runs the the "Create Profile" button is clicked
        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            // Tries to run the code safely but if an error happens, catch block will handle it
            try
            {
                // Checks if the username field is empty 
                if (txtUserName.Text == "")
                {
                    // Displays message to player to enter username 
                    MessageBox.Show("Enter a username.");
                    return;
                }
                // Used to create the players profile object using input recieved from the player
                PlayerProfile profile = new PlayerProfile
                {
                    UserID = txtUserName.Text, // Sets the UserId to text input recieved from the player 
                    AccountID = txtAccountID.Text, // Sets the AccountID to text generated for players bank
                    Balance = 100, // Initial balance given to player when a new profile is created
                    Wins = 0, // Sets the wins counter to 0 when a new profile is created
                    Losses = 0, // Sets the losses counter to 0 when a new profile is created 
                    ProfileImagePath = selectedImagePath // Sets the profile image to the selected image path 
                };

                // Saves the newly created profile to the json file using Profile Manager
                ProfileManager.AddProfile(profile);
                // Sets the profile as an active player profile in the Game Manager 
                GameManager.SetPlayer(profile);
                // Displays a confirmation message to the player 
                MessageBox.Show("Profile Created!");

                // Closes the profile creation form and returns the player to the previous screen 
                this.Close();
            }
            catch (Exception ex) // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            {
                // Logs any exceptons that occur while saving to console and returns false, displaying error message to player 
                Console.WriteLine("Error creating profile: " + ex.Message);
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
            }
        }
    }
}
