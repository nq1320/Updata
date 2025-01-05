namespace Binance
{
	partial class Settings
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Connection", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Connection");
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
						"Feed Status",
						"Not Running"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
			this.tbSettingsTabs = new System.Windows.Forms.TabControl();
			this.tpGeneral = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.cbGeneralLogLevel = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tpDiagnostics = new System.Windows.Forms.TabPage();
			this.lvStats = new System.Windows.Forms.ListView();
			this.chStatsName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chStatsValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tsSpring = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsVersion = new System.Windows.Forms.ToolStripStatusLabel();
			this.btRestartFeed = new System.Windows.Forms.Button();
			this.tbSettingsTabs.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tpDiagnostics.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbSettingsTabs
			// 
			this.tbSettingsTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbSettingsTabs.Controls.Add(this.tpGeneral);
			this.tbSettingsTabs.Controls.Add(this.tpDiagnostics);
			this.tbSettingsTabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbSettingsTabs.Location = new System.Drawing.Point(3, 4);
			this.tbSettingsTabs.Name = "tbSettingsTabs";
			this.tbSettingsTabs.SelectedIndex = 0;
			this.tbSettingsTabs.Size = new System.Drawing.Size(400, 99);
			this.tbSettingsTabs.TabIndex = 0;
			// 
			// tpGeneral
			// 
			this.tpGeneral.Controls.Add(this.groupBox3);
			this.tpGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tpGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpGeneral.Size = new System.Drawing.Size(392, 73);
			this.tpGeneral.TabIndex = 0;
			this.tpGeneral.Text = "General Settings";
			this.tpGeneral.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.cbGeneralLogLevel);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Location = new System.Drawing.Point(6, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(380, 61);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Diagnostics";
			// 
			// cbGeneralLogLevel
			// 
			this.cbGeneralLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbGeneralLogLevel.FormattingEnabled = true;
			this.cbGeneralLogLevel.Items.AddRange(new object[] {
						"Trace",
						"Verbose",
						"Debug",
						"Info",
						"Warning",
						"Error",
						"Always",
						"Off"});
			this.cbGeneralLogLevel.Location = new System.Drawing.Point(66, 22);
			this.cbGeneralLogLevel.Name = "cbGeneralLogLevel";
			this.cbGeneralLogLevel.Size = new System.Drawing.Size(158, 21);
			this.cbGeneralLogLevel.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Log Level";
			// 
			// tpDiagnostics
			// 
			this.tpDiagnostics.Controls.Add(this.btRestartFeed);
			this.tpDiagnostics.Location = new System.Drawing.Point(4, 22);
			this.tpDiagnostics.Name = "tpDiagnostics";
			this.tpDiagnostics.Padding = new System.Windows.Forms.Padding(3);
			this.tpDiagnostics.Size = new System.Drawing.Size(392, 73);
			this.tpDiagnostics.TabIndex = 1;
			this.tpDiagnostics.Text = "Diagnostics";
			this.tpDiagnostics.UseVisualStyleBackColor = true;
			// 
			// lvStats
			// 
			this.lvStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvStats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.chStatsName,
						this.chStatsValue});
			this.lvStats.GridLines = true;
			listViewGroup1.Header = "Connection";
			listViewGroup1.Name = "lvgConnection";
			this.lvStats.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
						listViewGroup1});
			this.lvStats.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvStats.HideSelection = false;
			listViewItem1.Group = listViewGroup1;
			listViewItem2.Group = listViewGroup1;
			this.lvStats.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
						listViewItem1,
						listViewItem2});
			this.lvStats.Location = new System.Drawing.Point(3, 109);
			this.lvStats.Name = "lvStats";
			this.lvStats.Size = new System.Drawing.Size(396, 97);
			this.lvStats.TabIndex = 2;
			this.lvStats.UseCompatibleStateImageBehavior = false;
			this.lvStats.View = System.Windows.Forms.View.Details;
			// 
			// chStatsName
			// 
			this.chStatsName.Text = "";
			this.chStatsName.Width = 171;
			// 
			// chStatsValue
			// 
			this.chStatsValue.Text = "";
			this.chStatsValue.Width = 220;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
						this.tsSpring,
						this.tsVersion});
			this.statusStrip1.Location = new System.Drawing.Point(0, 209);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(403, 22);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "ssStatus";
			// 
			// tsSpring
			// 
			this.tsSpring.Name = "tsSpring";
			this.tsSpring.Size = new System.Drawing.Size(212, 17);
			this.tsSpring.Spring = true;
			// 
			// tsVersion
			// 
			this.tsVersion.Name = "tsVersion";
			this.tsVersion.Size = new System.Drawing.Size(176, 17);
			this.tsVersion.Text = "Version major.date.time.revision";
			// 
			// btRestartFeed
			// 
			this.btRestartFeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btRestartFeed.Location = new System.Drawing.Point(303, 6);
			this.btRestartFeed.Name = "btRestartFeed";
			this.btRestartFeed.Size = new System.Drawing.Size(83, 23);
			this.btRestartFeed.TabIndex = 0;
			this.btRestartFeed.Text = "Restart Feed";
			this.btRestartFeed.UseVisualStyleBackColor = true;
			this.btRestartFeed.Click += new System.EventHandler(this.btRestartFeed_Click);
			// 
			// Settings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(403, 231);
			this.Controls.Add(this.lvStats);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.tbSettingsTabs);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Settings";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Updata Feed Settings";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
			this.Load += new System.EventHandler(this.Settings_Load);
			this.tbSettingsTabs.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tpDiagnostics.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tbSettingsTabs;
		private System.Windows.Forms.TabPage tpGeneral;
		private System.Windows.Forms.ListView lvStats;
		private System.Windows.Forms.ColumnHeader chStatsName;
		private System.Windows.Forms.ColumnHeader chStatsValue;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsSpring;
		private System.Windows.Forms.ToolStripStatusLabel tsVersion;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ComboBox cbGeneralLogLevel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabPage tpDiagnostics;
		private System.Windows.Forms.Button btRestartFeed;
	}
}