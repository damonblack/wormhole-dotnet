using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WormholeClient.Data;

namespace WormholeClient
{
	/// <summary>
	/// Summary description for ChatView.
	/// </summary>
	public class ChatView : System.Windows.Forms.Form
	{

		#region Private Components
		private WormholeClient		_owner;
		private Button				_bnSend;
		private TextBox				_tbSendBox;
		private TextBox				_tbChatBox;
		private System.Windows.Forms.GroupBox _gbPlayer;
			private System.Windows.Forms.PictureBox _pbIcon0;
			private System.Windows.Forms.PictureBox _pbIcon1;
			private System.Windows.Forms.PictureBox _pbIcon2;
			private System.Windows.Forms.PictureBox _pbIcon3;
			private System.Windows.Forms.PictureBox _pbIcon4;
			private System.Windows.Forms.PictureBox _pbIcon5;
			private System.Windows.Forms.PictureBox _pbIcon6;
			private System.Windows.Forms.PictureBox _pbIcon7;
			private System.Windows.Forms.CheckBox _cbPlayer0;
			private System.Windows.Forms.CheckBox _cbPlayer1;
			private System.Windows.Forms.CheckBox _cbPlayer2;
			private System.Windows.Forms.CheckBox _cbPlayer3;
			private System.Windows.Forms.CheckBox _cbPlayer4;
			private System.Windows.Forms.CheckBox _cbPlayer5;
			private System.Windows.Forms.CheckBox _cbPlayer6;
			private System.Windows.Forms.CheckBox _cbPlayer7;
			private System.Windows.Forms.CheckBox _cbAllPlayers;

		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region Methods
		public void ChatUpdate(string msg)
		{
			_tbChatBox.AppendText("\r\n" + msg);
		}

		
		public void UpdatePlayerList(GameData gameData)
		{
			if(gameData == null)
				return;
			PlayerList plist = gameData.PlayerList;
			for(int i = 0; i < plist.Count; i++)
			{
				Console.WriteLine("buttons initialized");
				_gbPlayer.Controls[i].Text = plist[i].Name;
				//_gbPlayer.Controls[i].Visible = true;
				_gbPlayer.Controls[i].Enabled = true;				
				//_gbPlayer.Controls[i+9].Visible = true;
			}
			//_gbPlayer.Controls[8].Visible = true;
			_gbPlayer.Controls[8].Enabled = true;
			Console.WriteLine("chat view initialized");
			this.Invalidate();
        }

		#endregion
		#region Constructors and Initialization
		public ChatView(WormholeClient owner)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_owner = owner;
            
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

		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ChatView));
			this._tbSendBox = new System.Windows.Forms.TextBox();
			this._tbChatBox = new System.Windows.Forms.TextBox();
			this._bnSend = new System.Windows.Forms.Button();
			this._pbIcon0 = new System.Windows.Forms.PictureBox();
			this._pbIcon1 = new System.Windows.Forms.PictureBox();
			this._pbIcon2 = new System.Windows.Forms.PictureBox();
			this._pbIcon3 = new System.Windows.Forms.PictureBox();
			this._pbIcon4 = new System.Windows.Forms.PictureBox();
			this._pbIcon5 = new System.Windows.Forms.PictureBox();
			this._pbIcon6 = new System.Windows.Forms.PictureBox();
			this._pbIcon7 = new System.Windows.Forms.PictureBox();
			this._gbPlayer = new System.Windows.Forms.GroupBox();
			this._cbPlayer0 = new System.Windows.Forms.CheckBox();
			this._cbPlayer1 = new System.Windows.Forms.CheckBox();
			this._cbPlayer2 = new System.Windows.Forms.CheckBox();
			this._cbPlayer3 = new System.Windows.Forms.CheckBox();
			this._cbPlayer4 = new System.Windows.Forms.CheckBox();
			this._cbPlayer5 = new System.Windows.Forms.CheckBox();
			this._cbPlayer6 = new System.Windows.Forms.CheckBox();
			this._cbPlayer7 = new System.Windows.Forms.CheckBox();
			this._cbAllPlayers = new System.Windows.Forms.CheckBox();
			this._gbPlayer.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tbSendBox
			// 
			this._tbSendBox.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._tbSendBox.Location = new System.Drawing.Point(8, 328);
			this._tbSendBox.Multiline = true;
			this._tbSendBox.Name = "_tbSendBox";
			this._tbSendBox.Size = new System.Drawing.Size(216, 40);
			this._tbSendBox.TabIndex = 0;
			this._tbSendBox.Text = "";
			// 
			// _tbChatBox
			// 
			this._tbChatBox.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._tbChatBox.BackColor = System.Drawing.SystemColors.Window;
			this._tbChatBox.Location = new System.Drawing.Point(8, 8);
			this._tbChatBox.Multiline = true;
			this._tbChatBox.Name = "_tbChatBox";
			this._tbChatBox.ReadOnly = true;
			this._tbChatBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._tbChatBox.Size = new System.Drawing.Size(150, 312);
			this._tbChatBox.TabIndex = 1;
			this._tbChatBox.Text = "";
			// 
			// _bnSend
			// 
			this._bnSend.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this._bnSend.Location = new System.Drawing.Point(232, 328);
			this._bnSend.Name = "_bnSend";
			this._bnSend.Size = new System.Drawing.Size(112, 40);
			this._bnSend.TabIndex = 2;
			this._bnSend.Text = "Send";
			this._bnSend.Click += new System.EventHandler(this._bnSend_Click);
			// 
			// _pbIcon0
			// 
			this._pbIcon0.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pbIcon0.Enabled = false;
			this._pbIcon0.Image = ((System.Drawing.Bitmap)(resources.GetObject("_pbIcon0.Image")));
			this._pbIcon0.Location = new System.Drawing.Point(8, 28);
			this._pbIcon0.Name = "_pbIcon0";
			this._pbIcon0.Size = new System.Drawing.Size(24, 24);
			this._pbIcon0.TabIndex = 11;
			this._pbIcon0.TabStop = false;
			// 
			// _pbIcon1
			// 
			this._pbIcon1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pbIcon1.Enabled = false;
			this._pbIcon1.Image = ((System.Drawing.Bitmap)(resources.GetObject("_pbIcon1.Image")));
			this._pbIcon1.Location = new System.Drawing.Point(8, 60);
			this._pbIcon1.Name = "_pbIcon1";
			this._pbIcon1.Size = new System.Drawing.Size(24, 24);
			this._pbIcon1.TabIndex = 12;
			this._pbIcon1.TabStop = false;
			// 
			// _pbIcon2
			// 
			this._pbIcon2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pbIcon2.Enabled = false;
			this._pbIcon2.Image = ((System.Drawing.Bitmap)(resources.GetObject("_pbIcon2.Image")));
			this._pbIcon2.Location = new System.Drawing.Point(8, 92);
			this._pbIcon2.Name = "_pbIcon2";
			this._pbIcon2.Size = new System.Drawing.Size(24, 24);
			this._pbIcon2.TabIndex = 13;
			this._pbIcon2.TabStop = false;
			// 
			// _pbIcon3
			// 
			this._pbIcon3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pbIcon3.Enabled = false;
			this._pbIcon3.Image = ((System.Drawing.Bitmap)(resources.GetObject("_pbIcon3.Image")));
			this._pbIcon3.Location = new System.Drawing.Point(8, 124);
			this._pbIcon3.Name = "_pbIcon3";
			this._pbIcon3.Size = new System.Drawing.Size(24, 24);
			this._pbIcon3.TabIndex = 14;
			this._pbIcon3.TabStop = false;
			// 
			// _pbIcon4
			// 
			this._pbIcon4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pbIcon4.Enabled = false;
			this._pbIcon4.Image = ((System.Drawing.Bitmap)(resources.GetObject("_pbIcon4.Image")));
			this._pbIcon4.Location = new System.Drawing.Point(8, 156);
			this._pbIcon4.Name = "_pbIcon4";
			this._pbIcon4.Size = new System.Drawing.Size(24, 24);
			this._pbIcon4.TabIndex = 15;
			this._pbIcon4.TabStop = false;
			// 
			// _pbIcon5
			// 
			this._pbIcon5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pbIcon5.Enabled = false;
			this._pbIcon5.Image = ((System.Drawing.Bitmap)(resources.GetObject("_pbIcon5.Image")));
			this._pbIcon5.Location = new System.Drawing.Point(8, 188);
			this._pbIcon5.Name = "_pbIcon5";
			this._pbIcon5.Size = new System.Drawing.Size(24, 24);
			this._pbIcon5.TabIndex = 16;
			this._pbIcon5.TabStop = false;
			// 
			// _pbIcon6
			// 
			this._pbIcon6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pbIcon6.Enabled = false;
			this._pbIcon6.Image = ((System.Drawing.Bitmap)(resources.GetObject("_pbIcon6.Image")));
			this._pbIcon6.Location = new System.Drawing.Point(8, 220);
			this._pbIcon6.Name = "_pbIcon6";
			this._pbIcon6.Size = new System.Drawing.Size(24, 24);
			this._pbIcon6.TabIndex = 17;
			this._pbIcon6.TabStop = false;
			// 
			// _pbIcon7
			// 
			this._pbIcon7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pbIcon7.Enabled = false;
			this._pbIcon7.Image = ((System.Drawing.Bitmap)(resources.GetObject("_pbIcon7.Image")));
			this._pbIcon7.Location = new System.Drawing.Point(8, 252);
			this._pbIcon7.Name = "_pbIcon7";
			this._pbIcon7.Size = new System.Drawing.Size(24, 24);
			this._pbIcon7.TabIndex = 18;
			this._pbIcon7.TabStop = false;
			// 
			// _gbPlayer
			// 
			this._gbPlayer.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._gbPlayer.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this._cbPlayer0,
																					this._cbPlayer1,
																					this._cbPlayer2,
																					this._cbPlayer3,
																					this._cbPlayer4,
																					this._cbPlayer5,
																					this._cbPlayer6,
																					this._cbPlayer7,
																					this._cbAllPlayers,
																					this._pbIcon0,
																					this._pbIcon1,
																					this._pbIcon2,
																					this._pbIcon3,
																					this._pbIcon4,
																					this._pbIcon5,
																					this._pbIcon6,
																					this._pbIcon7});
			this._gbPlayer.Location = new System.Drawing.Point(168, 0);
			this._gbPlayer.Name = "_gbPlayer";
			this._gbPlayer.Size = new System.Drawing.Size(256, 320);
			this._gbPlayer.TabIndex = 20;
			this._gbPlayer.TabStop = false;
			this._gbPlayer.Text = "Fleet Commanders";
			// 
			// _cbPlayer0
			// 
			this._cbPlayer0.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbPlayer0.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbPlayer0.Enabled = false;
			this._cbPlayer0.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbPlayer0.Location = new System.Drawing.Point(40, 28);
			this._cbPlayer0.Name = "_cbPlayer0";
			this._cbPlayer0.Size = new System.Drawing.Size(124, 24);
			this._cbPlayer0.TabIndex = 19;
			this._cbPlayer0.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbPlayer0.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// _cbPlayer1
			// 
			this._cbPlayer1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbPlayer1.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbPlayer1.Enabled = false;
			this._cbPlayer1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbPlayer1.Location = new System.Drawing.Point(40, 60);
			this._cbPlayer1.Name = "_cbPlayer1";
			this._cbPlayer1.Size = new System.Drawing.Size(124, 24);
			this._cbPlayer1.TabIndex = 20;
			this._cbPlayer1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbPlayer1.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// _cbPlayer2
			// 
			this._cbPlayer2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbPlayer2.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbPlayer2.Enabled = false;
			this._cbPlayer2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbPlayer2.Location = new System.Drawing.Point(40, 92);
			this._cbPlayer2.Name = "_cbPlayer2";
			this._cbPlayer2.Size = new System.Drawing.Size(124, 24);
			this._cbPlayer2.TabIndex = 21;
			this._cbPlayer2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbPlayer2.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// _cbPlayer3
			// 
			this._cbPlayer3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbPlayer3.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbPlayer3.Enabled = false;
			this._cbPlayer3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbPlayer3.Location = new System.Drawing.Point(40, 124);
			this._cbPlayer3.Name = "_cbPlayer3";
			this._cbPlayer3.Size = new System.Drawing.Size(124, 24);
			this._cbPlayer3.TabIndex = 22;
			this._cbPlayer3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbPlayer3.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// _cbPlayer4
			// 
			this._cbPlayer4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbPlayer4.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbPlayer4.Enabled = false;
			this._cbPlayer4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbPlayer4.Location = new System.Drawing.Point(40, 156);
			this._cbPlayer4.Name = "_cbPlayer4";
			this._cbPlayer4.Size = new System.Drawing.Size(124, 24);
			this._cbPlayer4.TabIndex = 23;
			this._cbPlayer4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbPlayer4.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// _cbPlayer5
			// 
			this._cbPlayer5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbPlayer5.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbPlayer5.Enabled = false;
			this._cbPlayer5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbPlayer5.Location = new System.Drawing.Point(40, 188);
			this._cbPlayer5.Name = "_cbPlayer5";
			this._cbPlayer5.Size = new System.Drawing.Size(124, 24);
			this._cbPlayer5.TabIndex = 24;
			this._cbPlayer5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbPlayer5.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// _cbPlayer6
			// 
			this._cbPlayer6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbPlayer6.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbPlayer6.Enabled = false;
			this._cbPlayer6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbPlayer6.Location = new System.Drawing.Point(40, 220);
			this._cbPlayer6.Name = "_cbPlayer6";
			this._cbPlayer6.Size = new System.Drawing.Size(124, 24);
			this._cbPlayer6.TabIndex = 25;
			this._cbPlayer6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbPlayer6.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// _cbPlayer7
			// 
			this._cbPlayer7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbPlayer7.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbPlayer7.Enabled = false;
			this._cbPlayer7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbPlayer7.Location = new System.Drawing.Point(40, 252);
			this._cbPlayer7.Name = "_cbPlayer7";
			this._cbPlayer7.Size = new System.Drawing.Size(124, 24);
			this._cbPlayer7.TabIndex = 26;
			this._cbPlayer7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbPlayer7.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// _cbAllPlayers
			// 
			this._cbAllPlayers.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._cbAllPlayers.Appearance = System.Windows.Forms.Appearance.Button;
			this._cbAllPlayers.Enabled = false;
			this._cbAllPlayers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._cbAllPlayers.Location = new System.Drawing.Point(40, 284);
			this._cbAllPlayers.Name = "_cbAllPlayers";
			this._cbAllPlayers.Size = new System.Drawing.Size(124, 24);
			this._cbAllPlayers.TabIndex = 27;
			this._cbAllPlayers.Text = "All Players";
			this._cbAllPlayers.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._cbAllPlayers.CheckedChanged += new System.EventHandler(this._cbPlayer_CheckedChanged);
			// 
			// ChatView
			// 
			this.AcceptButton = this._bnSend;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(352, 372);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._gbPlayer,
																		  this._bnSend,
																		  this._tbChatBox,
																		  this._tbSendBox});
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(704, 400);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(304, 400);
			this.Name = "ChatView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Chat Window";
			this._gbPlayer.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnClosing(CancelEventArgs cea)
		{
			cea.Cancel = true;
			Hide();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Return)
			{
				//Fillout out a bit array signalling who gets the message
				BitArray baPlayers = new BitArray(8);
				for(int i = 0; i < 8; i++)
				{
					CheckBox cb = (CheckBox)_gbPlayer.Controls[i];
					if(cb.Checked == true)
						baPlayers[i] = true;
				}
				if(_tbSendBox.Text != "")
					_owner.SendText(_tbSendBox.Text, baPlayers);
				_tbSendBox.Clear();	
			}
		}
		private void _bnSend_Click(object sender, System.EventArgs e)
		{
			//Fillout out a bit array signalling who gets the message
			BitArray baPlayers = new BitArray(8);
			for(int i = 0; i < 8; i++)
			{
				CheckBox cb = (CheckBox)_gbPlayer.Controls[i];
				if(cb.Checked == true)
					baPlayers[i] = true;
			}
			if(_tbSendBox.Text != "" )
				_owner.SendText(_tbSendBox.Text, baPlayers);
			_tbSendBox.Clear();	
		}
		private void _cbPlayer_CheckedChanged(object sender, System.EventArgs e)
		{
			CheckBox cbPlayer = (CheckBox)sender;
			if(cbPlayer.TabIndex == _owner.PlayerID + 19)
				cbPlayer.Checked = true;
			//if this is the All Players button toggle all check boxes
			if(cbPlayer.Text == "All Players")
			{
				CheckBox cb;
				//if any slots are still open, return
				for(int i = 0; i < 8; i++)
				{
					cb = (CheckBox)_gbPlayer.Controls[i];
					//if any slots are open, check them all so that chat can 
					//occur before the game begins
					if(cb.Text == "Open")
					{
						return;
					}
				}
				//otherwise toggle all the boxes
				for(int a = 0; a < 9; a++)
				{
					cb = (CheckBox)_gbPlayer.Controls[a];
					if(cbPlayer.Checked == true || a == _owner.PlayerID)						
						cb.Checked = true;
					else
						cb.Checked = false;
				}
				return;
			}

			//
			//	Note: all permission is checked here, server should only check to see
			//  if a particular ID has alread been assigned to another client
			//

			//If this client has not been assigned a player, check for password
			if(_owner.PlayerID == -1)
			{
				//Get Id associated with this checkbox
				int ID = cbPlayer.TabIndex - 19;
				//if no password is currently assigned for this playerslot
				//offer player chance to create one
				if(_owner.GameData.PlayerList[ID].Password == "")
				{
					NewPasswordForm newDlg = new NewPasswordForm();
					newDlg.ShowDialog(this);
					if(newDlg.DialogResult == DialogResult.OK)
					{ 
						if(newDlg.Password1 == newDlg.Password2)
							_owner.RequestPlayerID(ID,newDlg.Password2);
						else
							MessageBox.Show("Passwords must match!");
					}
				}
				else
				{
					//Get password from player
					PasswordForm dlg = new PasswordForm();
					dlg.ShowDialog(this);
					if(dlg.DialogResult == DialogResult.OK)				
					{
						//confirm the Password
						if(dlg.Password == _owner.GameData.PlayerList[ID].Password)
						{
							Console.WriteLine("Password from previous game: " + _owner.GameData.PlayerList[ID].Password);
							_owner.Password = dlg.Password;
							_owner.RequestPlayerID(ID,dlg.Password);
						}
						else
						{
							MessageBox.Show("Incorrect Password!");
						}
					}
				}				
			}
		}
		protected override void OnLoad(EventArgs ea)
		{
			this.ActiveControl = this._tbSendBox;
		}
	}
}
