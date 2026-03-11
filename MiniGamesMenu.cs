/* Using built-in libraries for Windows Forms and the tools within the app, including labels, buttons, images, text boxes, message boxes, list boxes, panels, 
 sound effects and to read/write files */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsAppFINAL
{
    // Represents an additional form in my project
    public partial class frmMiniGamesMenu : Form
    {
        // Runs when the form is first created 
        public frmMiniGamesMenu()
        {
            // Initializes all from controls 
            InitializeComponent();
        }
        private void btnThrowFootball_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form for the Throw Football mini-game
            frmThrowFootball throwForm = new frmThrowFootball();

            // Hides the current form so only one form is visible
            this.Hide();

            // Shows the Throw Football mini-game
            throwForm.ShowDialog();

            // Closes the form
            this.Close();
        }
        private void btnCatchRun_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form for the Catch & Run mini-game
            frmCatchRun catchForm = new frmCatchRun();

            // Hides the current form so only one form is visible
            this.Hide();

            // Shows the Catch & Run mini-game
            catchForm.ShowDialog();

            // Closes the form
            this.Close();
        }
        private void btnBlackjack_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form for the Blackjack mini-game
            frmBlackjack blackjackForm = new frmBlackjack();

            // Hides the current form so only one form is visible
            this.Hide();

            // Shows the Blackjack mini-game
            blackjackForm.ShowDialog();

            // Closes the form
            this.Close();
        }
        private void btnRoulette_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form for the Roulette mini-game
            frmRoulette rouletteForm = new frmRoulette();

            // Hides the current form so only one form is visible
            this.Hide();

            // Displays the Mini Games Menu form to user
            rouletteForm.ShowDialog();

            // Closes the form
            this.Close();
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            //Creates a new instance of the Main Menu form so it can be opened 
            frmMainMenu mainMenu = new frmMainMenu();

            // Hides the current form so only one form is visible
            this.Hide();

            // Displays the Mini Games Menu form to user
            mainMenu.ShowDialog();

            // Closes the form
            this.Close();

        }
    }

}
