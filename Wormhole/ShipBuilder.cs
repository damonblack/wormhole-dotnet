using System;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WormholeClient.Data;

namespace WormholeClient
{
	/// <summary>
	/// ShipBuilder dialog raised from context menu.
	/// </summary>
	public class ShipBuilder : System.Windows.Forms.Form
	{

		#region Private Fields
		private int									_playerID;
		private Player								_playerData;
		private System.Windows.Forms.PictureBox		pictureBox2;
		private System.Windows.Forms.PictureBox		pictureBox3;
		private System.Windows.Forms.PictureBox		pictureBox1;
		private System.Windows.Forms.Button			_bnOK;
		private System.Windows.Forms.Button			_bnCancel;
		private System.Windows.Forms.GroupBox		_rbGroupBox;
		private System.Windows.Forms.RadioButton	radioButton1;
		private System.Windows.Forms.RadioButton	radioButton2;
		private System.Windows.Forms.RadioButton	radioButton3;
		private System.Windows.Forms.Label			label1;
		private System.Windows.Forms.Label			label2;
		private System.Windows.Forms.Label			label3;
		private System.Windows.Forms.Label			label4;
		private System.Windows.Forms.Label			label5;
		private System.Windows.Forms.Label			label6;
		private System.Windows.Forms.Label			label7;
		private System.Windows.Forms.Label			label8;
		private System.Windows.Forms.Label			_lblExcessResources;
		private System.Windows.Forms.Label			_lblUsedResources;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region Properties
		public Ship.ShipType Selection
		{
			get
			{
				if(radioButton1.Checked == true)
					return Ship.ShipType.Colony;
				if(radioButton2.Checked == true)
					return Ship.ShipType.Scout;
				if(radioButton3.Checked == true)
					return Ship.ShipType.Defender;
				else
					return Ship.ShipType.None;
			}
		}
		#endregion
		#region Constructors and Initialization
		public ShipBuilder(int ID, Player playerData)
		{
			InitializeComponent();
			Assembly thisAssembly = Assembly.GetAssembly(this.GetType());
			Stream imageStream = thisAssembly.GetManifestResourceStream
						("Wormhole.Resources.ShipColony" + ID + ".ico");
			this.pictureBox1.Image = Image.FromStream(imageStream);
			imageStream = thisAssembly.GetManifestResourceStream
				("Wormhole.Resources.ShipScout" + ID + ".ico");
			this.pictureBox2.Image = Image.FromStream(imageStream);
			imageStream = thisAssembly.GetManifestResourceStream
				("Wormhole.Resources.ShipDefender" + ID + ".ico");
			this.pictureBox3.Image = Image.FromStream(imageStream);

			int MinTotal = playerData.Fleet.SupMineral;
			int OrgTotal = playerData.Fleet.SupOrganic;
			int EngTotal = playerData.Fleet.SupEnergy;
			int exMin = playerData.Fleet.ExMineral;
			int exOrg = playerData.Fleet.ExOrganic;
			int exEng = playerData.Fleet.ExEnergy;

			this._lblExcessResources.Text = exMin.ToString() + "/" + exOrg.ToString() + "/"
												+ exEng.ToString();
			this._lblUsedResources.Text = MinTotal.ToString() + "/" + OrgTotal.ToString() + "/" 
												+ EngTotal.ToString();
			if(exMin < (int)Ship.MineralSupport.Colony || exOrg < (int)Ship.OrganicSupport.Colony 
					|| exEng < (int)Ship.EnergySupport.Colony)
				this.radioButton1.Enabled = false;
			if(exMin < (int)Ship.MineralSupport.Scout || exOrg < (int)Ship.OrganicSupport.Scout 
					|| exEng < (int)Ship.EnergySupport.Scout)
				this.radioButton2.Enabled = false;
			if(exMin < (int)Ship.MineralSupport.Defender || exOrg < (int)Ship.OrganicSupport.Defender
					|| exEng < (int)Ship.EnergySupport.Defender)
				this.radioButton3.Enabled = false;

		}
		#endregion
		#region Methods
		#endregion
		#region Events
		#endregion
		#region EventHandlers
		#endregion
		#region Utility Functions
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
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ShipBuilder));
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this._bnOK = new System.Windows.Forms.Button();
			this._bnCancel = new System.Windows.Forms.Button();
			this._rbGroupBox = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this._lblExcessResources = new System.Windows.Forms.Label();
			this._lblUsedResources = new System.Windows.Forms.Label();
			this._rbGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackColor = System.Drawing.Color.Black;
			this.pictureBox2.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox2.BackgroundImage")));
			this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox2.Location = new System.Drawing.Point(176, 8);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(160, 160);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.BackColor = System.Drawing.Color.Black;
			this.pictureBox3.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox3.BackgroundImage")));
			this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox3.Location = new System.Drawing.Point(344, 8);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(160, 160);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox3.TabIndex = 2;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Black;
			this.pictureBox1.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.BackgroundImage")));
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(160, 160);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// _bnOK
			// 
			this._bnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this._bnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._bnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._bnOK.Location = new System.Drawing.Point(360, 360);
			this._bnOK.Name = "_bnOK";
			this._bnOK.Size = new System.Drawing.Size(136, 32);
			this._bnOK.TabIndex = 6;
			this._bnOK.Text = "OK";
			// 
			// _bnCancel
			// 
			this._bnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this._bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._bnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._bnCancel.Location = new System.Drawing.Point(360, 312);
			this._bnCancel.Name = "_bnCancel";
			this._bnCancel.Size = new System.Drawing.Size(136, 32);
			this._bnCancel.TabIndex = 7;
			this._bnCancel.Text = "Cancel";
			// 
			// _rbGroupBox
			// 
			this._rbGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.label5,
																					  this.label4,
																					  this.label3,
																					  this.label2,
																					  this.label1,
																					  this.radioButton3,
																					  this.radioButton2,
																					  this.radioButton1,
																					  this.label6});
			this._rbGroupBox.Location = new System.Drawing.Point(8, 168);
			this._rbGroupBox.Name = "_rbGroupBox";
			this._rbGroupBox.Size = new System.Drawing.Size(496, 120);
			this._rbGroupBox.TabIndex = 8;
			this._rbGroupBox.TabStop = false;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(184, 70);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(144, 16);
			this.label5.TabIndex = 7;
			this.label5.Text = "Support:  M15 O10 E35";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(16, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(152, 34);
			this.label4.TabIndex = 6;
			this.label4.Text = "Support:  M15(50) O25(50) E10(50)";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(352, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Power/Range        5/2";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(184, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Power/Range         1/4";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Power/Range   2/1 (7/0)";
			// 
			// radioButton3
			// 
			this.radioButton3.Location = new System.Drawing.Point(352, 24);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(128, 24);
			this.radioButton3.TabIndex = 2;
			this.radioButton3.Text = "Defender";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(184, 24);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(128, 24);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "Scout Ship";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(16, 24);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(128, 24);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.Text = "Colony Ship";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(352, 70);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(136, 16);
			this.label6.TabIndex = 9;
			this.label6.Text = "Support:  M60 O20 E25";
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.label7.Location = new System.Drawing.Point(8, 304);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(224, 40);
			this.label7.TabIndex = 9;
			this.label7.Text = "Resources Supporting Fleet:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.label8.Location = new System.Drawing.Point(8, 352);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(224, 40);
			this.label8.TabIndex = 10;
			this.label8.Text = "Available for New Ship:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lblExcessResources
			// 
			this._lblExcessResources.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this._lblExcessResources.Location = new System.Drawing.Point(240, 352);
			this._lblExcessResources.Name = "_lblExcessResources";
			this._lblExcessResources.Size = new System.Drawing.Size(112, 40);
			this._lblExcessResources.TabIndex = 11;
			this._lblExcessResources.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _lblUsedResources
			// 
			this._lblUsedResources.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this._lblUsedResources.Location = new System.Drawing.Point(240, 304);
			this._lblUsedResources.Name = "_lblUsedResources";
			this._lblUsedResources.Size = new System.Drawing.Size(112, 40);
			this._lblUsedResources.TabIndex = 12;
			this._lblUsedResources.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ShipBuilder
			// 
			this.AcceptButton = this._bnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
			this.CancelButton = this._bnCancel;
			this.ClientSize = new System.Drawing.Size(512, 398);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._lblUsedResources,
																		  this._lblExcessResources,
																		  this.label8,
																		  this.label7,
																		  this._rbGroupBox,
																		  this._bnCancel,
																		  this._bnOK,
																		  this.pictureBox3,
																		  this.pictureBox2,
																		  this.pictureBox1});
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ShipBuilder";
			this.ShowInTaskbar = false;
			this.Text = "ShipBuilder";
			this.TopMost = true;
			this._rbGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion









	}
}
