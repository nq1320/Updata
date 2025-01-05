namespace Binance
{
	partial class TradeDialog
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
			this.btSubmitSell = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lbDataProvider = new System.Windows.Forms.Label();
			this.lbPlatform = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.nudOrderAmount = new System.Windows.Forms.NumericUpDown();
			this.nudOrderPrice = new System.Windows.Forms.NumericUpDown();
			this.btSubmitBuy = new System.Windows.Forms.Button();
			this.lbInstrument = new System.Windows.Forms.Label();
			this.Instrument = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudOrderAmount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOrderPrice)).BeginInit();
			this.SuspendLayout();
			// 
			// btSubmitSell
			// 
			this.btSubmitSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btSubmitSell.Location = new System.Drawing.Point(109, 124);
			this.btSubmitSell.Name = "btSubmitSell";
			this.btSubmitSell.Size = new System.Drawing.Size(94, 29);
			this.btSubmitSell.TabIndex = 0;
			this.btSubmitSell.Text = "Sell";
			this.btSubmitSell.UseVisualStyleBackColor = true;
			this.btSubmitSell.Click += new System.EventHandler(this.btSubmitSell_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Data Provider:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Trading Platform:";
			// 
			// lbDataProvider
			// 
			this.lbDataProvider.AutoSize = true;
			this.lbDataProvider.Location = new System.Drawing.Point(101, 30);
			this.lbDataProvider.Name = "lbDataProvider";
			this.lbDataProvider.Size = new System.Drawing.Size(46, 13);
			this.lbDataProvider.TabIndex = 3;
			this.lbDataProvider.Text = "Provider";
			// 
			// lbPlatform
			// 
			this.lbPlatform.AutoSize = true;
			this.lbPlatform.Location = new System.Drawing.Point(101, 51);
			this.lbPlatform.Name = "lbPlatform";
			this.lbPlatform.Size = new System.Drawing.Size(45, 13);
			this.lbPlatform.TabIndex = 4;
			this.lbPlatform.Text = "Platform";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 74);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(46, 13);
			this.label5.TabIndex = 5;
			this.label5.Text = "Amount:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 100);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(34, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "Price:";
			// 
			// nudOrderAmount
			// 
			this.nudOrderAmount.Location = new System.Drawing.Point(60, 72);
			this.nudOrderAmount.Name = "nudOrderAmount";
			this.nudOrderAmount.Size = new System.Drawing.Size(143, 20);
			this.nudOrderAmount.TabIndex = 7;
			// 
			// nudOrderPrice
			// 
			this.nudOrderPrice.Location = new System.Drawing.Point(60, 98);
			this.nudOrderPrice.Name = "nudOrderPrice";
			this.nudOrderPrice.Size = new System.Drawing.Size(143, 20);
			this.nudOrderPrice.TabIndex = 8;
			// 
			// btSubmitBuy
			// 
			this.btSubmitBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btSubmitBuy.Location = new System.Drawing.Point(11, 124);
			this.btSubmitBuy.Name = "btSubmitBuy";
			this.btSubmitBuy.Size = new System.Drawing.Size(94, 29);
			this.btSubmitBuy.TabIndex = 9;
			this.btSubmitBuy.Text = "Buy";
			this.btSubmitBuy.UseVisualStyleBackColor = true;
			this.btSubmitBuy.Click += new System.EventHandler(this.btSubmitBuy_Click);
			// 
			// lbInstrument
			// 
			this.lbInstrument.AutoSize = true;
			this.lbInstrument.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbInstrument.Location = new System.Drawing.Point(101, 9);
			this.lbInstrument.Name = "lbInstrument";
			this.lbInstrument.Size = new System.Drawing.Size(29, 13);
			this.lbInstrument.TabIndex = 11;
			this.lbInstrument.Text = "InId";
			// 
			// Instrument
			// 
			this.Instrument.AutoSize = true;
			this.Instrument.Location = new System.Drawing.Point(8, 9);
			this.Instrument.Name = "Instrument";
			this.Instrument.Size = new System.Drawing.Size(59, 13);
			this.Instrument.TabIndex = 10;
			this.Instrument.Text = "Instrument:";
			// 
			// TradeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(213, 164);
			this.Controls.Add(this.lbInstrument);
			this.Controls.Add(this.Instrument);
			this.Controls.Add(this.btSubmitBuy);
			this.Controls.Add(this.nudOrderPrice);
			this.Controls.Add(this.nudOrderAmount);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lbPlatform);
			this.Controls.Add(this.lbDataProvider);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btSubmitSell);
			this.Name = "TradeDialog";
			((System.ComponentModel.ISupportInitialize)(this.nudOrderAmount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudOrderPrice)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btSubmitSell;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbDataProvider;
		private System.Windows.Forms.Label lbPlatform;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown nudOrderAmount;
		private System.Windows.Forms.NumericUpDown nudOrderPrice;
		private System.Windows.Forms.Button btSubmitBuy;
		private System.Windows.Forms.Label lbInstrument;
		private System.Windows.Forms.Label Instrument;
	}
}