using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WormholeClient.Data;

namespace WormholeClient
{
	/// <summary>
	/// NewGameDialog.
	/// </summary>
	public class NewGameDialog : System.Windows.Forms.Form
	{
        
		public int NumberOfPlayers
		{
			get{ return (int)_numberOfPlayers.Value; }
		}
		public int SizeOfMap
		{
			get{ return (int)_sizeOfMap.Value; }
		}
		/// <summary>
		/// Required designer variable.
		/// </summary>
		
		private System.Windows.Forms.NumericUpDown _numberOfPlayers;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown _sizeOfMap;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Button _cancelButton;

		System.ComponentModel.Container components = null;

		public NewGameDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._numberOfPlayers = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this._sizeOfMap = new System.Windows.Forms.NumericUpDown();
			this._okButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this._numberOfPlayers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._sizeOfMap)).BeginInit();
			this.SuspendLayout();
			// 
			// _numberOfPlayers
			// 
			this._numberOfPlayers.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._numberOfPlayers.Location = new System.Drawing.Point(200, 11);
			this._numberOfPlayers.Maximum = new System.Decimal(new int[] {
																			 8,
																			 0,
																			 0,
																			 0});
			this._numberOfPlayers.Minimum = new System.Decimal(new int[] {
																			 2,
																			 0,
																			 0,
																			 0});
			this._numberOfPlayers.Name = "_numberOfPlayers";
			this._numberOfPlayers.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._numberOfPlayers.Size = new System.Drawing.Size(48, 32);
			this._numberOfPlayers.TabIndex = 0;
			this._numberOfPlayers.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this._numberOfPlayers.Value = new System.Decimal(new int[] {
																		   2,
																		   0,
																		   0,
																		   0});
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.label1.Location = new System.Drawing.Point(8, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(192, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "Number of Players";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.label2.Location = new System.Drawing.Point(8, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(184, 32);
			this.label2.TabIndex = 2;
			this.label2.Text = "Size of Map";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _sizeOfMap
			// 
			this._sizeOfMap.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._sizeOfMap.Location = new System.Drawing.Point(200, 64);
			this._sizeOfMap.Maximum = new System.Decimal(new int[] {
																	   50,
																	   0,
																	   0,
																	   0});
			this._sizeOfMap.Minimum = new System.Decimal(new int[] {
																	   10,
																	   0,
																	   0,
																	   0});
			this._sizeOfMap.Name = "_sizeOfMap";
			this._sizeOfMap.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._sizeOfMap.Size = new System.Drawing.Size(48, 32);
			this._sizeOfMap.TabIndex = 3;
			this._sizeOfMap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this._sizeOfMap.Value = new System.Decimal(new int[] {
																	 25,
																	 0,
																	 0,
																	 0});
			// 
			// _okButton
			// 
			this._okButton.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._okButton.Location = new System.Drawing.Point(144, 112);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(104, 32);
			this._okButton.TabIndex = 4;
			this._okButton.Text = "OK";
			this._okButton.Click += new System.EventHandler(this._okButton_Click);
			// 
			// _cancelButton
			// 
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._cancelButton.Location = new System.Drawing.Point(16, 112);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(104, 32);
			this._cancelButton.TabIndex = 5;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
			// 
			// NewGameDialog
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(8, 21);
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(258, 151);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._cancelButton,
																		  this._okButton,
																		  this._sizeOfMap,
																		  this.label2,
																		  this.label1,
																		  this._numberOfPlayers});
			this.Font = new System.Drawing.Font("Lucida Blackletter", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewGameDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create a New Game";
			((System.ComponentModel.ISupportInitialize)(this._numberOfPlayers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._sizeOfMap)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void _okButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		
		}

		private void _cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		
		}

	}
}
