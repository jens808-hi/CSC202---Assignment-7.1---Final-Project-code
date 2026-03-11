using System;

namespace WinFormsAppFINAL
{
	public class PlayerProfile
	{
		public string UserID { get; set; }	
		public string AccountID { get; set; }
		public decimal Balance { get; set; }
		public int Wins { get; set; }
		public int Losses { get; set; }
		public string ProfileImagePath { get; set; }
	}
}
