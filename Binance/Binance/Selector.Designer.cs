namespace Binance
{
	partial class Selector
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btSelect = new System.Windows.Forms.Button();
			this.tbInstrument = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btSelect
			// 
			this.btSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btSelect.Location = new System.Drawing.Point(310, 12);
			this.btSelect.Name = "btSelect";
			this.btSelect.Size = new System.Drawing.Size(80, 23);
			this.btSelect.TabIndex = 0;
			this.btSelect.Text = "Select";
			this.btSelect.UseVisualStyleBackColor = true;
			this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
			// 
			// tbInstrument
			// 
			this.tbInstrument.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbInstrument.Location = new System.Drawing.Point(102, 14);
			this.tbInstrument.Name = "tbInstrument";
			this.tbInstrument.Size = new System.Drawing.Size(202, 20);
			this.tbInstrument.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Enter Instrument";
			// 
			// Selector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbInstrument);
			this.Controls.Add(this.btSelect);
			this.MinimumSize = new System.Drawing.Size(405, 255);
			this.Name = "Selector";
			this.Size = new System.Drawing.Size(405, 255);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btSelect;
		private System.Windows.Forms.TextBox tbInstrument;
		private System.Windows.Forms.Label label1;
	}
}
