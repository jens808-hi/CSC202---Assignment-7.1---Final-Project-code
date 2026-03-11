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

            // Shows the Throw Football mini-game
            throwForm.Show();

            // Closes the form
            this.Close();
        }
        private void btnCatchRun_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form for the Catch & Run mini-game
            frmCatchRun catchForm = new frmCatchRun();

            // Shows the Catch & Run mini-game
            catchForm.Show();

            // Closes the form
            this.Close();
        }
        private void btnBlackjack_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form for the Blackjack mini-game
            frmBlackjack blackjackForm = new frmBlackjack();

            // Shows the Blackjack mini-game
            blackjackForm.Show();

            // Closes the form
            this.Close();
        }
        private void btnRoulette_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the form for the Roulette mini-game
            frmRoulette rouletteForm = new frmRoulette();

            // shows the Roulette mini-game
            rouletteForm.Show();

            // Closes the form
            this.Close();
        }
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
