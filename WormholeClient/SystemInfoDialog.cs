using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WormholeClient
{
	/// <summary>
	/// Summary description for SystemInfoDialog.
	/// </summary>
	public class SystemInfoDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label _lblSystemName;
		private System.Windows.Forms.Label _lblMinerals;
		private System.Windows.Forms.Label _lblOrganics;
		private System.Windows.Forms.Label _lblEnergy;
		private System.Windows.Forms.Label _lblSystemInfo;
		private System.Windows.Forms.Button _bnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SystemInfoDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SystemInfoDialog));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this._lblSystemName = new System.Windows.Forms.Label();
			this._lblMinerals = new System.Windows.Forms.Label();
			this._lblOrganics = new System.Windows.Forms.Label();
			this._lblEnergy = new System.Windows.Forms.Label();
			this._lblSystemInfo = new System.Windows.Forms.Label();
			this._bnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.BackgroundImage")));
			this.pictureBox1.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(160, 48);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(88, 88);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "Minerals:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 24);
			this.label3.TabIndex = 3;
			this.label3.Text = "Organics:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 24);
			this.label4.TabIndex = 4;
			this.label4.Text = "Energy:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lblSystemName
			// 
			this._lblSystemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._lblSystemName.Location = new System.Drawing.Point(72, 16);
			this._lblSystemName.Name = "_lblSystemName";
			this._lblSystemName.Size = new System.Drawing.Size(184, 24);
			this._lblSystemName.TabIndex = 5;
			this._lblSystemName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _lblMinerals
			// 
			this._lblMinerals.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._lblMinerals.Location = new System.Drawing.Point(96, 48);
			this._lblMinerals.Name = "_lblMinerals";
			this._lblMinerals.Size = new System.Drawing.Size(48, 24);
			this._lblMinerals.TabIndex = 6;
			this._lblMinerals.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _lblOrganics
			// 
			this._lblOrganics.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._lblOrganics.Location = new System.Drawing.Point(96, 80);
			this._lblOrganics.Name = "_lblOrganics";
			this._lblOrganics.Size = new System.Drawing.Size(48, 24);
			this._lblOrganics.TabIndex = 7;
			this._lblOrganics.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _lblEnergy
			// 
			this._lblEnergy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._lblEnergy.Location = new System.Drawing.Point(96, 110);
			this._lblEnergy.Name = "_lblEnergy";
			this._lblEnergy.Size = new System.Drawing.Size(48, 24);
			this._lblEnergy.TabIndex = 8;
			this._lblEnergy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _lblSystemInfo
			// 
			this._lblSystemInfo.Location = new System.Drawing.Point(16, 152);
			this._lblSystemInfo.Name = "_lblSystemInfo";
			this._lblSystemInfo.Size = new System.Drawing.Size(232, 64);
			this._lblSystemInfo.TabIndex = 9;
			this._lblSystemInfo.Text = "(lots more info here later, specials, improvements etc...)";
			// 
			// _bnOK
			// 
			this._bnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._bnOK.Location = new System.Drawing.Point(104, 224);
			this._bnOK.Name = "_bnOK";
			this._bnOK.Size = new System.Drawing.Size(56, 24);
			this._bnOK.TabIndex = 10;
			this._bnOK.Text = "OK";
			// 
			// SystemInfoDialog
			// 
			this.AcceptButton = this._bnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(264, 252);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._bnOK,
																		  this._lblSystemInfo,
																		  this._lblEnergy,
																		  this._lblOrganics,
																		  this._lblMinerals,
																		  this._lblSystemName,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.pictureBox1});
			this.MaximizeBox = false;
			this.Name = "SystemInfoDialog";
			this.Text = "Star System Information";
			this.ResumeLayout(false);

		}
		#endregion




	}
}
