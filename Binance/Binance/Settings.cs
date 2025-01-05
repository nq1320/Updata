using System;
using System.Windows.Forms;
using UAPI;

namespace Binance
{
	/// <summary>
	/// This dialog is displayed when the user requests the Feed Settings from either UA or from the System Tray menu.
	/// Once displayed there would be Diagnostic display and controls for Configuration of your Feed.
	/// </summary>
	public partial class Settings : Form
	{
		/// <summary>
		/// This Timer is used to update diagnostic display on your form.
		/// Update controls and displays in the <see cref="StatsQueryUpdateTimer_Tick(object, EventArgs)"/> event.
		/// </summary>
		Timer StatsQueryUpdateTimer = new Timer();

		/// <summary>
		/// The Settings Dialog is created at startup and destroyed during shutdown.
		/// It is merely displayed or hidden as needed.
		/// This flag is set when the dialog should be destroyed <see cref="Binance.Shutdown"/>
		/// </summary>
		public bool ShuttingDown = false;

		/// <summary>
		/// When the Feed is built in Debug Mode, this flag is set to true and allows a Diagnostics tab to be visible.
		/// This allows for debugging Feed behaviour from your Settings Dialog while hiding it from users in Release Mode.
		/// </summary>
		bool ShowDiagnostics = true;

		public Settings()
		{
			InitializeComponent();
			Text = $"Updata {FeedConfig.Name()} Settings";
		}

		private void Settings_Load(object sender, EventArgs e)
		{
			try
			{
				// Display the Version of the Feed Assembly on ToolStrip
				tsVersion.Text = string.Format("Version {0}", Binance.I.Version);

				// Set the current Log Level to be Displayed
				cbGeneralLogLevel.SelectedIndex = cbGeneralLogLevel.FindString(Log.Level.ToString());

#if DEBUG
				// Only show the Diagnostics Tab in Debug Mode
				ShowDiagnostics = true;
#endif

				if (ShowDiagnostics)
				{
					tbSettingsTabs.SelectedIndex = tbSettingsTabs.TabCount - 1;
				}
				else
				{
					// Remove the Diagnostics Tab in Release Mode
					var DiagnosticsTabPage = tbSettingsTabs.TabPages["tpDiagnostics"];
					if (DiagnosticsTabPage != null)
						tbSettingsTabs.TabPages.Remove(DiagnosticsTabPage);
				}

				// Setup the Stats Output Timer
				StatsQueryUpdateTimer_Tick(this, new EventArgs());
				StatsQueryUpdateTimer.Tick += StatsQueryUpdateTimer_Tick;
				StatsQueryUpdateTimer.Interval = 1000;
				StatsQueryUpdateTimer.Start();
			}
			catch (Exception x)
			{
				// Any Exceptions to be displayed in place of the Version output
				tsVersion.Text = x.Message;
			}
		}

		/// <summary>
		/// Update your Dialog Display at the Timer Tick Frequency
		/// </summary>
		void StatsQueryUpdateTimer_Tick(object sender, EventArgs e)
		{
			lvStats.Items[1].SubItems[1].Text = $"Connected {DateTime.Now}";
		}

		/// <summary>
		/// When the Form is closed, during operation, apply the settings and just hide it.
		/// At the time of shutdown, the form is allowed to fully close.
		/// </summary>
		private void Settings_FormClosing(object sender, FormClosingEventArgs e)
		{			
			if (cbGeneralLogLevel.SelectedItem != null)
			{
				// When the settings form is closed, apply the Log Level
				if (Enum.TryParse<LogLevel>(cbGeneralLogLevel.Items[cbGeneralLogLevel.SelectedIndex].ToString(), out var Level))
					Log.Level = Level;
			}

			// If it is not shutting down then just hide the form and cancel the Close event
			if (!ShuttingDown)
			{
				Hide();
				e.Cancel = true;
			}
			else
			{
				// Stop the Timer from Ticking and potentially trying to use controls that are disposed before the form Closes
				StatsQueryUpdateTimer.Stop();
			}
		}

		private void cbGeneralLogLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			// As the user changes the Log Level, apply it to the Log output from here on
			if (cbGeneralLogLevel.SelectedItem != null)
			{
				if (Enum.TryParse<LogLevel>(cbGeneralLogLevel.Items[cbGeneralLogLevel.SelectedIndex].ToString(), out var Level))
					Log.Level = Level;
			}
		}

		private void btRestartFeed_Click(object sender, EventArgs e)
		{
			// Request to UA to restart the Feed
			Binance.I.raise_OnRestartMe();
		}
	}
}
