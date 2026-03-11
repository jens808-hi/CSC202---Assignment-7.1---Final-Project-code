/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files and json serialization */
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsAppFINAL
{
    /* Manages and stores all player profiles and saves/loads them to json. Uses methods to add, remove, and get profiles */
    public static class ProfileManager
    {
        // Uses json file to store all player profiles
        private static string ProfilesFile = "profiles.json";

        // List of all loaded player profiles and initializes as an empty list to prevent null errors 
        public static List<PlayerProfile> Profiles { get; private set; } = new List<PlayerProfile>();

        // Adds a new player profile and saves all profiles to json
        public static void AddProfile(PlayerProfile profile)
        {
            // Only add if the object is not null 
            if (profile != null)
            {
                // Adds to programs memory 
                Profiles.Add(profile);
                // Saves updated profile list to json file
                SaveProfiles();
            }
        }

        // Gets a profile by UserID string of the profile
        public static PlayerProfile? GetProfile(string userId)
        {
            // Searches the profiles list for a profile whose UserID matches 
            return Profiles.Find(profile => profile.UserID == userId);
        }

        // Saves current list of all profiles to JSON
        public static void SaveProfiles()
        {
            // Tries to run the code safely but if an error happens, catch block will handle it
            try
            {
                // Converts the profiles list to json text, uses indentation to make it easier to read
                string json = JsonSerializer.Serialize(Profiles, new JsonSerializerOptions { WriteIndented = true });
                // Writes the json string to the file, overwriting any existing file
                File.WriteAllText(ProfilesFile, json);
            }
            // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            catch (Exception ex) 
            {
                // Logs any exceptons that occur while saving to console and returns false, displaying error message to player 
                Console.WriteLine("Error saving profiles: " + ex.Message);
            }
        }

        // Method to remove a profile by the UserID string and updates the json file. 
        public static bool RemoveProfile(string userId)
        {

            // Tries to run the code safely but if an error happens, catch block will handle it
            try
            {
                // Tries to fiind the profile with the matching UserID
                var profile = GetProfile(userId);
                if (profile != null)
                {
                    // Removes profile from the list 
                    Profiles.Remove(profile);
                    // Saves the updated list back to json 
                    SaveProfiles();
                    // Returns true in succession 
                    return true;
                }
                // Returns false if the profile is not found 
                return false;
            }
            // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            catch (Exception ex)
            {
                // Logs any exceptons that occur while saving to console and returns false, displaying error message to player 
                Console.WriteLine("Error removing profile: " + ex.Message);
                return false;
            }
        }

        // Loads profiles from JSON
        public static void LoadProfiles()
        {

            // Tries to run the code safely but if an error happens, catch block will handle it
            try
            {
                // Checks if the json file exists
                if (!File.Exists(ProfilesFile))
                {
                    // Reads all json text from the file 
                    string json = File.ReadAllText(ProfilesFile);
                    // Converts json back into a list 
                    Profiles = JsonSerializer.Deserialize<List<PlayerProfile>>(json) ?? new List<PlayerProfile>();
                }
            }
            // Runs if any error occurs inside the try block and stores it in ex to show details of the error
            catch (Exception ex)
            {
                // Logs any exceptons that occur while saving to console and returns false, displaying error message to player 
                Console.WriteLine("Error loading profiles: " + ex.Message);
            }
        }

    }

}









