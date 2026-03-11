using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsAppFINAL
{
    public partial class frmRoulette : Form
    {
        public frmRoulette()
        {
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            //Creates a new instance of the form 
            frmMainMenu mainMenu = new frmMainMenu();

            // Hides the current form so only one form is visible
            this.Hide();

            // Shows the main menu 
            mainMenu.ShowDialog();

            // Closes the current mini-game form
            this.Close();
        }
    }
}
