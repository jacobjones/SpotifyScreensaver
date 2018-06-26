using System;
using System.Windows.Forms;

namespace SpotifyScreensaver
{
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
