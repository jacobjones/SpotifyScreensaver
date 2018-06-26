namespace SpotifyScreensaver
{
	partial class AlbumArtScreensaverForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timerMove = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// timerMove
			// 
			this.timerMove.Interval = 5;
			this.timerMove.Tick += new System.EventHandler(this.timerMove_Tick);
			// 
			// AlbumArtScreensaverForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(804, 723);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "AlbumArtScreensaverForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.AlbumArtScreensaverForm_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.AlbumArtScreensaverForm_Paint);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AlbumArtScreensaverForm_KeyUp);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AlbumArtScreensaverForm_MouseClick);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AlbumArtScreensaverForm_MouseMove);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timerMove;
	}
}

