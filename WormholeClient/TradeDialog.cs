using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WormholeClient.Data;
using Messages;

namespace WormholeClient
{
	/// <summary>
	/// Summary description for TradeDialog.
	/// </summary>
	public class TradeDialog : System.Windows.Forms.Form
	{
		private GameData					_gameData;
		private WormholeClient				_client;
		private int							_thisPlayerID;
		private int							_targetPlayerID;
		private MsgTrade					_tradeMsg;
		private int							_exMin;
		private int							_exOrg;
		private int							_exEng;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown returnMineral;
		private System.Windows.Forms.NumericUpDown returnOrganic;
		private System.Windows.Forms.NumericUpDown returnEnergy;
		private System.Windows.Forms.NumericUpDown offerMineral;
		private System.Windows.Forms.NumericUpDown offerOrganic;
		private System.Windows.Forms.NumericUpDown offerEnergy;		
		private System.Windows.Forms.TextBox surplusMineral;		
		private System.Windows.Forms.TextBox surplusOrganic;
		private System.Windows.Forms.TextBox surplusEnergy;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button bnOffer;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button bnCancel;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button bnReject;
		private System.Windows.Forms.Button bnAccept;
		private System.Windows.Forms.Button bnIgnore;
		private System.Windows.Forms.TextBox chatBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// call this to open a dialog to formulate a new trade
		/// </summary>
		/// <param name="gameData"></param>
		/// <param name="client"></param>
		/// <param name="ID"></param>
		/// <param name="targetID"></param>
		public TradeDialog(GameData gameData, WormholeClient client, int ID, int targetID)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this._gameData = gameData;
			this._client = client;
			this._thisPlayerID = ID;
			this._targetPlayerID = targetID;
			this.Text += _gameData.PlayerList[targetID].Name;

			this._exMin = _gameData.PlayerList[ID].Fleet.ExMineral;
			this._exOrg = _gameData.PlayerList[ID].Fleet.ExOrganic;
			this._exEng = _gameData.PlayerList[ID].Fleet.ExEnergy;
		
			this.surplusMineral.Text = _exMin.ToString();
			this.surplusOrganic.Text = _exOrg.ToString();
			this.surplusEnergy.Text = _exEng.ToString();

			//this.offerMineral.Maximum = _exMin;
			//this.offerOrganic.Maximum = _exOrg;
			//this.offerEnergy.Maximum = _exEng;

			this.AcceptButton = this.bnOffer;
			this.bnAccept.Enabled = false;
			this.bnReject.Enabled = false;

			Fleet fleet = _gameData.PlayerList[ID].Fleet;

			for(int i = 0; i < fleet.Count; i++)
			{
				if(fleet[i].Type == Ship.ShipType.Trader)
				{
					Trader trader = (Trader)fleet[i];
					this.listBox1.Items.Add(trader.Trade);
				}
					
			}

		}
		/// <summary>
		/// call this to accept a trade message from another player
		/// Keep in mind this will be closed as a Modal dialog and will be returned
		/// as soon as an accept button is pressed. This will block this Players ability
		/// to accept other messages. hmmm, this could cause problem.. possible discon-
		/// nections?  
		/// </summary>
		/// <param name="trade"></param>
		/// <param name="gameData"></param>
		/// <param name="client"></param>
		/// <param name="thisPlayerID"></param>

		public TradeDialog(MsgTrade tradeMsg, GameData gameData, WormholeClient client, int thisPlayerID)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this._tradeMsg = tradeMsg;
			Trade trade = tradeMsg.Trade;
			this._gameData = gameData;
			this._client = client;
			this._thisPlayerID = thisPlayerID;
			if(this._thisPlayerID == tradeMsg.Trade.OfferingPartyID)
			{
				this._targetPlayerID = tradeMsg.Trade.AskedPartyID;
			}
			else
			{
				this._targetPlayerID = tradeMsg.Trade.OfferingPartyID;
			}
			this.Text += _gameData.PlayerList[_targetPlayerID].Name;

			this.offerMineral.Value = trade.AskedMineral;
			this.offerOrganic.Value = trade.AskedOrganic;
			this.offerEnergy.Value = trade.AskedEnergy;

			this.returnMineral.Value = trade.OfferedMineral;
			this.returnOrganic.Value = trade.OfferedOrganic;
			this.returnEnergy.Value = trade.OfferedEnergy;

			this._exMin = _gameData.PlayerList[_thisPlayerID].Fleet.ExMineral - trade.AskedMineral + trade.OfferedMineral;
			this._exOrg = _gameData.PlayerList[_thisPlayerID].Fleet.ExOrganic - trade.AskedOrganic + trade.OfferedOrganic;
			this._exEng = _gameData.PlayerList[_thisPlayerID].Fleet.ExEnergy - trade.AskedEnergy + trade.OfferedEnergy;

			this.surplusMineral.Text = _exMin.ToString();
			this.surplusOrganic.Text = _exOrg.ToString();
			this.surplusEnergy.Text = _exEng.ToString();

			if(tradeMsg.Type == MsgTrade.TradeMsgType.Offer)
			{
				this.bnCancel.Enabled = false;
				this.bnOffer.Enabled = false;
				this.bnReject.Enabled = true;
				this.bnAccept.Enabled = true;
				this.bnIgnore.Enabled = true;
				this.bnIgnore.Text = "Busy, Try Later";
			}
			if(_exMin < 0 || _exOrg < 0 || _exEng < 0)
			{
				this.bnAccept.Enabled = false;
			}
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

		public int ActivePlayerID
		{
			get{return this._targetPlayerID;}
		}
		public void UpdateTradeMsg(MsgTrade tradeMsg)
		{
			this._tradeMsg = tradeMsg;
			switch(tradeMsg.Type)
			{
				case MsgTrade.TradeMsgType.Accept:
				{
					this._client.SetUpTrade(tradeMsg.Trade,this.ActivePlayerID);
					this.listBox1.Items.Add(tradeMsg.Trade);
					this.chatBox.AppendText("\r\nTrade agreement confirmed");
					break;
				}
				case MsgTrade.TradeMsgType.Reject:
				{
					this.chatBox.AppendText(tradeMsg.Msg);
					Console.WriteLine("Rejection received");
					break;
				}
				case MsgTrade.TradeMsgType.Message:
				{
					this.chatBox.AppendText(tradeMsg.Msg);
					break;
				}
				case MsgTrade.TradeMsgType.Offer:
				{
					Trade trade = tradeMsg.Trade;
					this.offerMineral.Value = trade.AskedMineral;
					this.offerOrganic.Value = trade.AskedOrganic;
					this.offerEnergy.Value = trade.AskedEnergy;
					this.returnMineral.Value = trade.OfferedMineral;
					this.returnOrganic.Value = trade.OfferedOrganic;
					this.returnEnergy.Value = trade.OfferedEnergy;
					this.bnAccept.Enabled = true;
					this.bnReject.Enabled = true;
					this.bnCancel.Enabled = false;
					this.bnOffer.Enabled = false;
					break;
				}
				case MsgTrade.TradeMsgType.Cancel:
				{
					this.listBox1.Items.Clear();
					this.bnCancel.Enabled = false;
					Fleet fleet = _gameData.PlayerList[_thisPlayerID].Fleet;
					for(int i = 0; i < fleet.Count; i++)
					{
						if(fleet[i].Type == Ship.ShipType.Trader)
						{
							Trader trader = (Trader)fleet[i];
							this.listBox1.Items.Add(trader.Trade);
						}	
					}
					this.chatBox.AppendText("\r\nTrade canceled");
					Console.WriteLine("Cancel message received by dialog box");
					int newExMin = this._exEng + (int)this.returnMineral.Value - (int)this.offerMineral.Value;
					this.surplusEnergy.Text = newExMin.ToString();	
					int newExOrg = this._exOrg + (int)this.returnOrganic.Value - (int)this.offerOrganic.Value;
					this.surplusEnergy.Text = newExOrg.ToString();
					int newExEng = this._exEng + (int)this.returnEnergy.Value - (int)this.offerEnergy.Value;
					this.surplusEnergy.Text = newExEng.ToString();
					break;
				}
			}
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.returnMineral = new System.Windows.Forms.NumericUpDown();
			this.returnOrganic = new System.Windows.Forms.NumericUpDown();
			this.returnEnergy = new System.Windows.Forms.NumericUpDown();
			this.offerMineral = new System.Windows.Forms.NumericUpDown();
			this.offerOrganic = new System.Windows.Forms.NumericUpDown();
			this.offerEnergy = new System.Windows.Forms.NumericUpDown();
			this.surplusMineral = new System.Windows.Forms.TextBox();
			this.surplusOrganic = new System.Windows.Forms.TextBox();
			this.surplusEnergy = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.bnOffer = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.bnCancel = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.bnIgnore = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.bnReject = new System.Windows.Forms.Button();
			this.bnAccept = new System.Windows.Forms.Button();
			this.chatBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.offerMineral)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.returnEnergy)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.offerEnergy)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.returnOrganic)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.offerOrganic)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.returnMineral)).BeginInit();
			this.SuspendLayout();
			// 
			// offerMineral
			// 
			this.offerMineral.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.offerMineral.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.offerMineral.Location = new System.Drawing.Point(127, 283);
			this.offerMineral.Maximum = new System.Decimal(new int[] {
																		 200,
																		 0,
																		 0,
																		 0});
			this.offerMineral.Name = "offerMineral";
			this.offerMineral.Size = new System.Drawing.Size(48, 24);
			this.offerMineral.TabIndex = 5;
			this.offerMineral.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.offerMineral.ValueChanged += new System.EventHandler(this.offerMineral_ValueChanged);
			// 
			// returnEnergy
			// 
			this.returnEnergy.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.returnEnergy.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.returnEnergy.Location = new System.Drawing.Point(257, 244);
			this.returnEnergy.Maximum = new System.Decimal(new int[] {
																		 200,
																		 0,
																		 0,
																		 0});
			this.returnEnergy.Name = "returnEnergy";
			this.returnEnergy.Size = new System.Drawing.Size(48, 24);
			this.returnEnergy.TabIndex = 6;
			this.returnEnergy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.returnEnergy.ValueChanged += new System.EventHandler(this.returnEnergy_ValueChanged);
			// 
			// offerEnergy
			// 
			this.offerEnergy.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.offerEnergy.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.offerEnergy.Location = new System.Drawing.Point(257, 283);
			this.offerEnergy.Maximum = new System.Decimal(new int[] {
																		200,
																		0,
																		0,
																		0});
			this.offerEnergy.Name = "offerEnergy";
			this.offerEnergy.Size = new System.Drawing.Size(48, 24);
			this.offerEnergy.TabIndex = 7;
			this.offerEnergy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.offerEnergy.ValueChanged += new System.EventHandler(this.offerEnergy_ValueChanged);
			// 
			// returnOrganic
			// 
			this.returnOrganic.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.returnOrganic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.returnOrganic.Location = new System.Drawing.Point(192, 244);
			this.returnOrganic.Maximum = new System.Decimal(new int[] {
																		  200,
																		  0,
																		  0,
																		  0});
			this.returnOrganic.Name = "returnOrganic";
			this.returnOrganic.Size = new System.Drawing.Size(48, 24);
			this.returnOrganic.TabIndex = 8;
			this.returnOrganic.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.returnOrganic.ValueChanged += new System.EventHandler(this.returnOrganic_ValueChanged);
			// 
			// offerOrganic
			// 
			this.offerOrganic.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.offerOrganic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.offerOrganic.Location = new System.Drawing.Point(192, 283);
			this.offerOrganic.Maximum = new System.Decimal(new int[] {
																		 200,
																		 0,
																		 0,
																		 0});
			this.offerOrganic.Name = "offerOrganic";
			this.offerOrganic.Size = new System.Drawing.Size(48, 24);
			this.offerOrganic.TabIndex = 9;
			this.offerOrganic.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.offerOrganic.ValueChanged += new System.EventHandler(this.offerOrganic_ValueChanged);
			// 
			// returnMineral
			// 
			this.returnMineral.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.returnMineral.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.returnMineral.Location = new System.Drawing.Point(127, 244);
			this.returnMineral.Maximum = new System.Decimal(new int[] {
																		  200,
																		  0,
																		  0,
																		  0});
			this.returnMineral.Name = "returnMineral";
			this.returnMineral.Size = new System.Drawing.Size(48, 24);
			this.returnMineral.TabIndex = 10;
			this.returnMineral.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.returnMineral.ValueChanged += new System.EventHandler(this.returnMineral_ValueChanged);
			// 
			// surplusEnergy
			// 
			this.surplusEnergy.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.surplusEnergy.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.surplusEnergy.Location = new System.Drawing.Point(265, 329);
			this.surplusEnergy.Name = "surplusEnergy";
			this.surplusEnergy.Size = new System.Drawing.Size(32, 24);
			this.surplusEnergy.TabIndex = 11;
			this.surplusEnergy.Text = "0";
			this.surplusEnergy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// surplusMineral
			// 
			this.surplusMineral.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.surplusMineral.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.surplusMineral.Location = new System.Drawing.Point(135, 329);
			this.surplusMineral.Name = "surplusMineral";
			this.surplusMineral.Size = new System.Drawing.Size(32, 24);
			this.surplusMineral.TabIndex = 12;
			this.surplusMineral.Text = "0";
			this.surplusMineral.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// surplusOrganic
			// 
			this.surplusOrganic.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.surplusOrganic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.surplusOrganic.Location = new System.Drawing.Point(200, 329);
			this.surplusOrganic.Name = "surplusOrganic";
			this.surplusOrganic.Size = new System.Drawing.Size(32, 24);
			this.surplusOrganic.TabIndex = 13;
			this.surplusOrganic.Text = "0";
			this.surplusOrganic.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label1.Location = new System.Drawing.Point(123, 220);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 14;
			this.label1.Text = "Minerals";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label2
			// 
			this.label2.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label2.Location = new System.Drawing.Point(186, 220);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 23);
			this.label2.TabIndex = 15;
			this.label2.Text = "Organics";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label3.Location = new System.Drawing.Point(257, 220);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 23);
			this.label3.TabIndex = 16;
			this.label3.Text = "Energy";
			// 
			// label4
			// 
			this.label4.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label4.Location = new System.Drawing.Point(8, 244);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 24);
			this.label4.TabIndex = 17;
			this.label4.Text = "they provide";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label5.Location = new System.Drawing.Point(8, 282);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 24);
			this.label5.TabIndex = 18;
			this.label5.Text = "we provide";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label6.Location = new System.Drawing.Point(8, 329);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 24);
			this.label6.TabIndex = 19;
			this.label6.Text = "Surplus :";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// bnOffer
			// 
			this.bnOffer.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.bnOffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bnOffer.Location = new System.Drawing.Point(324, 252);
			this.bnOffer.Name = "bnOffer";
			this.bnOffer.Size = new System.Drawing.Size(100, 24);
			this.bnOffer.TabIndex = 20;
			this.bnOffer.Text = "Offer";
			this.bnOffer.Click += new System.EventHandler(this.bnOffer_Click);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.label7.Location = new System.Drawing.Point(136, 2);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(160, 21);
			this.label7.TabIndex = 22;
			this.label7.Text = "Active Trades";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// bnCancel
			// 
			this.bnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.bnCancel.Enabled = false;
			this.bnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bnCancel.Location = new System.Drawing.Point(324, 221);
			this.bnCancel.Name = "bnCancel";
			this.bnCancel.Size = new System.Drawing.Size(100, 24);
			this.bnCancel.TabIndex = 24;
			this.bnCancel.Text = "Cancel Trade";
			this.bnCancel.Click += new System.EventHandler(this.bnCancel_Click);
			// 
			// label9
			// 
			this.label9.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.label9.Location = new System.Drawing.Point(8, 220);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(108, 20);
			this.label9.TabIndex = 25;
			this.label9.Text = "Proposal:";
			// 
			// bnIgnore
			// 
			this.bnIgnore.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.bnIgnore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bnIgnore.Location = new System.Drawing.Point(324, 345);
			this.bnIgnore.Name = "bnIgnore";
			this.bnIgnore.Size = new System.Drawing.Size(100, 24);
			this.bnIgnore.TabIndex = 26;
			this.bnIgnore.Text = "Nevermind";
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listBox1.ItemHeight = 16;
			this.listBox1.Location = new System.Drawing.Point(2, 22);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(428, 116);
			this.listBox1.TabIndex = 27;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// bnReject
			// 
			this.bnReject.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.bnReject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bnReject.Location = new System.Drawing.Point(324, 283);
			this.bnReject.Name = "bnReject";
			this.bnReject.Size = new System.Drawing.Size(100, 24);
			this.bnReject.TabIndex = 4;
			this.bnReject.Text = "Reject";
			this.bnReject.Click+= new EventHandler(this.bnReject_Click);
			// 
			// bnAccept
			// 
			this.bnAccept.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.bnAccept.Enabled = false;
			this.bnAccept.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bnAccept.Location = new System.Drawing.Point(324, 314);
			this.bnAccept.Name = "bnAccept";
			this.bnAccept.Size = new System.Drawing.Size(100, 24);
			this.bnAccept.TabIndex = 3;
			this.bnAccept.Text = "Accept";
			this.bnAccept.Click += new System.EventHandler(this.bnAccept_Click_1);
			// 
			// chatBox
			// 
			this.chatBox.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.chatBox.BackColor = System.Drawing.SystemColors.Window;
			this.chatBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chatBox.Location = new System.Drawing.Point(2, 142);
			this.chatBox.Multiline = true;
			this.chatBox.Name = "chatBox";
			this.chatBox.ReadOnly = true;
			this.chatBox.Size = new System.Drawing.Size(428, 71);
			this.chatBox.TabIndex = 0;
			this.chatBox.Text = "";
			// 
			// TradeDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(432, 376);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listBox1,
																		  this.bnIgnore,
																		  this.label9,
																		  this.bnCancel,
																		  this.label7,
																		  this.bnOffer,
																		  this.label6,
																		  this.label5,
																		  this.label4,
																		  this.label3,
																		  this.label2,
																		  this.label1,
																		  this.surplusOrganic,
																		  this.surplusMineral,
																		  this.surplusEnergy,
																		  this.returnMineral,
																		  this.offerOrganic,
																		  this.returnOrganic,
																		  this.offerEnergy,
																		  this.returnEnergy,
																		  this.offerMineral,
																		  this.bnReject,
																		  this.bnAccept,
																		  this.chatBox});
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(464, 464);
			this.MinimumSize = new System.Drawing.Size(404, 304);
			this.Name = "TradeDialog";
			this.Text = "Trade Talks with ";
			((System.ComponentModel.ISupportInitialize)(this.offerMineral)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.returnEnergy)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.offerEnergy)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.returnOrganic)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.offerOrganic)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.returnMineral)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void offerMineral_ValueChanged(object sender, System.EventArgs e)
		{
			int newExMin = this._exMin - (int)this.offerMineral.Value + (int)this.returnMineral.Value;
			this.surplusMineral.Text = newExMin.ToString();
			if(newExMin >= 0)
				this.bnOffer.Enabled = true;
			else
				this.bnOffer.Enabled = false;
			this.bnAccept.Enabled = false;
			this.bnReject.Enabled = false;
		}

		private void offerOrganic_ValueChanged(object sender, System.EventArgs e)
		{
			int newExOrg = this._exOrg - (int)this.offerOrganic.Value + (int)this.returnOrganic.Value;
			this.surplusOrganic.Text = newExOrg.ToString();
			if(newExOrg >= 0)
				this.bnOffer.Enabled = true;
			else
				this.bnOffer.Enabled = false;
			this.bnAccept.Enabled = false;
			this.bnReject.Enabled = false;
		}

		private void offerEnergy_ValueChanged(object sender, System.EventArgs e)
		{
			int newExEng = this._exEng - (int)this.offerEnergy.Value + (int)this.returnEnergy.Value;
			this.surplusEnergy.Text = newExEng.ToString();
			if(newExEng >= 0)
				this.bnOffer.Enabled = true;
			else
				this.bnOffer.Enabled = false;
			this.bnAccept.Enabled = false;
			this.bnReject.Enabled = false;
		}

		private void returnMineral_ValueChanged(object sender, System.EventArgs e)
		{
			int newExMin = this._exMin + (int)this.returnMineral.Value - (int)this.offerMineral.Value;
			this.surplusMineral.Text = newExMin.ToString();
			if(newExMin >= 0)
				this.bnOffer.Enabled = true;
			else
				this.bnOffer.Enabled = false;
			this.bnAccept.Enabled = false;
			this.bnReject.Enabled = false;
		}

		private void returnOrganic_ValueChanged(object sender, System.EventArgs e)
		{
			int newExOrg = this._exOrg + (int)this.returnOrganic.Value - (int)this.offerOrganic.Value;
			this.surplusOrganic.Text = newExOrg.ToString();	
			if(newExOrg >= 0)
				this.bnOffer.Enabled = true;
			else
				this.bnOffer.Enabled = false;
			this.bnAccept.Enabled = false;
			this.bnReject.Enabled = false;
		}
		private void returnEnergy_ValueChanged(object sender, System.EventArgs e)
		{
			int newExEng = this._exEng + (int)this.returnEnergy.Value - (int)this.offerEnergy.Value;
			this.surplusEnergy.Text = newExEng.ToString();	
			if(newExEng >= 0)
				this.bnOffer.Enabled = true;
			else
				this.bnOffer.Enabled = false;
			this.bnAccept.Enabled = false;
			this.bnReject.Enabled = false;
		}

		private void bnOffer_Click(object sender, System.EventArgs e)
		{
			this.bnOffer.Enabled = false;
			Trade trade = new Trade(this._thisPlayerID,Trader.NextTradeShipID,this._client.PlayerName,this._targetPlayerID,
				(int)this.offerMineral.Value,(int)this.offerOrganic.Value,(int)this.offerEnergy.Value,
				(int)this.returnMineral.Value,(int)this.returnOrganic.Value,(int)this.returnEnergy.Value);
			this._tradeMsg = new MsgTrade(MsgTrade.TradeMsgType.Offer,trade,String.Empty);
			this._client.SendTradeMsg(_tradeMsg);
		}
		private void bnCancel_Click(object sender, System.EventArgs e)
		{
			Trade trade = (Trade)this.listBox1.SelectedItem;
			this.bnCancel.Enabled = false;
			if(trade == null)
				return;
			MsgTrade tradeMsg = new MsgTrade(MsgTrade.TradeMsgType.Cancel, trade, String.Empty);
			this._client.SendTradeMsg(tradeMsg);
		}
		private void bnReject_Click(object sender, System.EventArgs e)
		{
			this._tradeMsg.Type = MsgTrade.TradeMsgType.Reject;
			this._tradeMsg.Msg = "\n\rTrade rejected by " + this._client.PlayerName;
			this._client.SendTradeMsg(this._tradeMsg);		
		}

		private void bnAccept_Click_1(object sender, System.EventArgs e)
		{
			Console.WriteLine("Accept button clicked");
			this.bnAccept.Enabled = false;
			this.bnReject.Enabled = false;
			this.bnOffer.Enabled = false;
			this._tradeMsg.Trade.AskedShipID = Trader.NextTradeShipID;
			this._tradeMsg.Trade.AskedName = this._client.PlayerName;
			this._tradeMsg.Type = MsgTrade.TradeMsgType.Accept;
			this._client.SendTradeMsg(this._tradeMsg);
		}

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.bnCancel.Enabled = true;
		}


		protected override void OnClosed(EventArgs ea)
		{
			_client.TradeDialog = null;
			this.Dispose();
		}
	}
}