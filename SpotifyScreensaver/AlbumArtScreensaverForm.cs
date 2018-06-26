using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;

namespace SpotifyScreensaver
{
	public partial class AlbumArtScreensaverForm : Form
	{
		[DllImport("user32.dll")]
		static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

		private readonly bool _previewMode;
		private SpotifyLocalAPI _spotify;
		private Point _mouseLocation;

		private readonly int _albumArtWidth;
		private readonly int _albumArtHeight;

		private readonly int _minSpeed = 1;
		private readonly int _maxSpeed = 3;

		private static readonly Random Rnd = new Random();

		// Position
		private int _albumArtX, _albumArtY;
		// Velocity
		private int _albumArtVx, _albumArtVy;

		private Image _albumArtImage;

		public AlbumArtScreensaverForm(Screen screen)
		{
			InitializeComponent();
			Bounds = screen.Bounds;

			var artSize = screen.Bounds.Width/4;
			_albumArtWidth = artSize;
			_albumArtHeight = artSize;

			Cursor.Hide();
			TopMost = true;

			Initialize();
		}

		public AlbumArtScreensaverForm(IntPtr previewWndHandle)
		{
			InitializeComponent();
 
			// Set the preview window as the parent of this window
			SetParent(Handle, previewWndHandle);
 
			// Make this a child window so it will close when the parent dialog closes
			// GWL_STYLE = -16, WS_CHILD = 0x40000000
			SetWindowLong(Handle, -16, new IntPtr(GetWindowLong(Handle, -16) | 0x40000000));
 
			// Place our window inside the parent
			Rectangle parentRect;
			GetClientRect(previewWndHandle, out parentRect);
			Size = parentRect.Size;
			Location = new Point(0, 0);

			var artSize = parentRect.Size.Width/4;

			// Slow down animation. Reduce size.
			_maxSpeed = 1;
			_minSpeed = 1;
			_albumArtWidth = artSize;
			_albumArtHeight = artSize;

			Initialize();
			_previewMode = true;
		}

		private void Initialize()
		{
			_spotify = new SpotifyLocalAPI();

			// Try and connect to spotify
			if (!SpotifyConnect())
			{
				return;
			}

			_spotify.OnTrackChange += _spotify_OnTrackChange;
			SetAlbumArt(_spotify.GetStatus().Track);
			timerMove.Enabled = true;
		}

		private bool SpotifyConnect()
		{
			if (!SpotifyLocalAPI.IsSpotifyRunning())
			{
				return false;
			}
			if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
			{
				return false;
			}

			bool successful;

			try
			{
				successful = _spotify.Connect();
			}
			catch(Exception)
			{
				return false;
			}

			if (!successful)
			{
				return false;
			}

			_spotify.ListenForEvents = true;

			return true;
		}

		private void _spotify_OnTrackChange(object sender, TrackChangeEventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() => _spotify_OnTrackChange(sender, e)));
				return;
			}

			SetAlbumArt(e.NewTrack);
		}

		private void SetAlbumArt(Track track)
		{
			if (track == null)
			{
				_albumArtImage = null;
				return;
			}

			_albumArtImage = track.GetAlbumArt(AlbumArtSize.Size640);
		}

		private void AlbumArtScreensaverForm_Load(object sender, EventArgs e)
		{
			// Initialize form styling
			BackColor = Color.FromArgb(18, 18, 18);

			// Animate the album artwork
			_albumArtVx = Rnd.Next(_minSpeed, _maxSpeed);
			_albumArtVy = Rnd.Next(_minSpeed, _maxSpeed);
			_albumArtX = Rnd.Next(0, ClientSize.Width - _albumArtWidth);
			_albumArtY = Rnd.Next(0, ClientSize.Height - _albumArtHeight);

			// Use double buffering to reduce flicker.
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
			UpdateStyles();
		}

		private void AlbumArtScreensaverForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (_previewMode)
			{
				return;
			}

			if (!_mouseLocation.IsEmpty)
			{
				// Terminate if mouse is moved a significant distance
				if (Math.Abs(_mouseLocation.X - e.X) > 5 ||
					Math.Abs(_mouseLocation.Y - e.Y) > 5)
					Application.Exit();
			}

			// Update current mouse location
			_mouseLocation = e.Location;
		}

		private void AlbumArtScreensaverForm_MouseClick(object sender, MouseEventArgs e)
		{
			if (!_previewMode)
			{
				Application.Exit();
			}
		}

		private void timerMove_Tick(object sender, EventArgs e)
		{
			_albumArtX += _albumArtVx;
			if (_albumArtX < 0)
			{
				_albumArtVx = -_albumArtVx;
			}
			else if (_albumArtX + _albumArtWidth > ClientSize.Width)
			{
				_albumArtVx = -_albumArtVx;
			}

			_albumArtY += _albumArtVy;
			if (_albumArtY < 0)
			{
				_albumArtVy = -_albumArtVy;
			}
			else if (_albumArtY + _albumArtHeight > ClientSize.Height)
			{
				_albumArtVy = -_albumArtVy;
			}

			Refresh();
		}

		private void AlbumArtScreensaverForm_Paint(object sender, PaintEventArgs e)
		{
			if (_albumArtImage == null)
			{
				return;
			}

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.Clear(BackColor);
			e.Graphics.DrawImage(_albumArtImage, _albumArtX, _albumArtY, _albumArtWidth, _albumArtHeight);
		}

		private void AlbumArtScreensaverForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (!_previewMode)
			{
				Application.Exit();
			}
		}
	}
}
