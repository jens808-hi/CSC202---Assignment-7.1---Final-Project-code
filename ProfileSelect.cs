/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsAppFINAL;

namespace WinFormsAppFINAL
{
    // 
    public partial class ProfileSelect: Form
    {
        public ProfileSelect()
        {
            InitializeComponent();
        }

        
        private void ProfileSelect_Load(object sender, EventArgs e)
        {
            try
            {
                // Loads all profiles from JSON
                ProfileManager.LoadProfiles();

                // Populates the list box with profile UserIDs
                lstProfiles.Items.Clear();

                foreach (var profile in ProfileManager.Profiles)
                {
                    lstProfiles.Items.Add(profile.UserID);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding profile: " + ex.Message);
            }

        }

        // Method that run when the "Select Profile" button is clicked
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstProfiles.SelectedItem != null)
                {
                    string selectedUserId = lstProfiles.SelectedItem.ToString()!;
                    var profile = ProfileManager.GetProfile(selectedUserId);

                    if (profile != null)
                    {
                        // Loads it into the GameManager
                        GameManager.SetPlayer(profile);
                        MessageBox.Show($"Profile {profile.UserID} selected!\nBalance: {profile.Balance}");

                        // Closes the form
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error selecting profile: " + ex.Message);
            }
        }
        private void btnCreateProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string userId = txtNewProfile.Text.Trim();
                if (!string.IsNullOrEmpty(userId))
                {
                    var newProfile = new PlayerProfile()
                    {
                        UserID = userId,
                        Balance = 100, // Starting balance 
                        Wins = 0,
                        Losses = 0,
                        ProfileImagePath = ""
                    };

                    ProfileManager.AddProfile(newProfile);
                    lstProfiles.Items.Add(newProfile.UserID);
                    txtNewProfile.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating profile: " + ex.Message);
            }
        }
    }
}

