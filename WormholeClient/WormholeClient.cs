using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Messages;
using WormholeClient.Data;
using System.Threading;


namespace WormholeClient
{
	public class WormholeClient:System.Windows.Forms.Form
	{
		#region Private Fields
		private TestForm					_testForm;
		/// <summary>
		/// the client socket
		/// </summary>
		private TcpClient					_client;
		private string						_serverIP;
		private bool						_connected = false;
		private bool						_firstTime = true;
		private bool						_lastTurnSupport = true;
		private string						_userName;
		private TradeDialog					_tradeDialog = null;
		/// <summary>
		/// The main game info manager
		/// </summary>
		private GameModel					_model = null;
		/// <summary>
		/// client ID, assigned by server
		/// </summary>
		private int							_ID = -1;
		/// <summary>
		/// player ID, index of assigned playerslot
		/// </summary>
		private int							_playerID = -1;
		private string						_password = String.Empty;
		/// <summary>
		/// GameData object with complete state information for the last game update
		/// </summary>
		private GameData					_gameData;
		/// <summary>
		/// Player Data object representing this player
		/// </summary>
		private Player						_playerData;
		/// <summary>
		/// ugly hack for colony context menu, override eventargs instead
		/// </summary>
		private Ship						_lastSelectedShip;
		/// <summary>
		/// same as above :(
		/// </summary>
		private Token						_lastSelectedToken;
		/// <summary>
		/// Player name
		/// </summary>
		private string						_name;
		/// <summary>
		/// Object representing the map as a unit
		/// </summary>
		private MapBoard					_mapBoard;
		/// <summary>
		/// Anchor for grab/scroll function
		/// </summary>
		private Point						_grabPt;
		/// <summary>		
		/// Currently selected token
		/// </summary>
		private Token						_selectedToken = null;
		private byte[]						_headerBytes = new Byte[8];
		private byte[]						_msgBytes  = null;
		private MsgType						_msgType;
		private int							_msgBytesExpected = 0;
		private int							_msgBytesReceived;
		/// <summary>
		/// Colony Ship Context Menu items
		/// </summary>
		private ContextMenu				_colonyContextMenu;
		private MenuItem					_miBuildBase;
		private MenuItem					_miAbandonBase;
		private MenuItem					_miBuildShips;
		private MenuItem					_miDecommissionColony;
		private ContextMenu				_shipContextMenu;
		private MenuItem					_miDecommissionShip;
		private MenuItem					_miCancel;
		private ContextMenu				_systemInfoMenu;
		private MenuItem					_systemName;
		private MenuItem					_miDash;
		private MenuItem					_systemType;
		private MenuItem					_minerals;
		private MenuItem					_organics;
		private MenuItem					_energy;
		/// <summary>
		/// chat window
		/// </summary>
		private ChatView					_chatView;
		/// <summary>
		/// Player logs view window
		/// </summary>
		private SystemLogView				_systemLogView;
		/// <summary>
		/// MainMenu
		/// </summary>
		private MainMenu			mainMenu1;
		private MenuItem				_miServer;	
		private MenuItem					_miConnect;
		private MenuItem					_miChat;
		private MenuItem				_miSystemLogs;
		private MenuItem				_miNextTurn;
		/// <summary>
		/// icon keyed to players color
		/// </summary>
		private Icon						_icon = null;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem _miGameInfo;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem _miLabelRI;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem _miMinerals;
		private System.Windows.Forms.MenuItem _miOrganics;
		private System.Windows.Forms.MenuItem _miEnergy;
		private System.Windows.Forms.MenuItem _miTradeManager;
		
		/// <summary>		
		/// Required designer variable
		/// </summary>
		private System.ComponentModel.Container components = null;
	
		#endregion
		#region Properties
		public int PlayerID
		{
			get {return _playerID;}
		}
		public string Password
		{
			set{_password = value;}
		}
		public string PlayerName
		{
			get{return this._userName;}
			set
			{
				this._userName = value;
				_playerData.Name = value;
			}

		}
		public GameData GameData
		{
			get{return _gameData;}
		}
		public TradeDialog TradeDialog
		{
			set{this._tradeDialog = value;}
		}
		#endregion
		#region Constructors and Initialization
		/// <summary>
		/// Default constructor
		/// </summary>
		public WormholeClient()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			//  Sets up Double Buffering for cleaner screen refresh
			//
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint , true);	
			this.SetStyle(ControlStyles.UserPaint, true);


			try
			{
				Assembly thisAssembly = Assembly.GetAssembly(this.GetType());
				Stream imageStream = thisAssembly.GetManifestResourceStream("WormholeClient.Wormhole.ico");
				this.Icon = new Icon(imageStream);
			}
			catch
			{
				MessageBox.Show("Wormhole.ico not found.");
			}
			Invalidate();
		}	 
		#endregion
		#region Methods
		public void RequestPlayerID(int ID, string password)
		{
			NameAndID nameAndID = new NameAndID(ID,this.PlayerName,password);
			try
			{
				lock(_client.GetStream())
				{
					NetworkStream str = _client.GetStream();
					IFormatter fmt = new BinaryFormatter();
					MemoryStream memStr = new MemoryStream();
					fmt.Serialize(memStr,nameAndID);
					byte[] data = memStr.GetBuffer();
					//get header info in bytes
					int memSize = (int)memStr.Length;		
					byte[] type = BitConverter.GetBytes((int)MsgType.PlayerIDRequest);
					byte[] size = BitConverter.GetBytes(memSize);
					byte[] header = new Byte[8];
					size.CopyTo(header,0);
					type.CopyTo(header,4);
					//send header
					str.Write(header,0,8);
					//send data
					str.Write(data,0,memSize);
					str.Flush();
					memStr.Close();
				}
			}
			catch
			{
				this.ReConnect();
			}
		}
		public void SendText(string text)
		{
			BitArray ba = new BitArray(8);
			MsgText msgText = new MsgText(text,ba);
			
			try
			{
				lock(_client.GetStream())
				{
					//Get stream
					NetworkStream str = _client.GetStream();
					IFormatter fmt = new BinaryFormatter();
					MemoryStream memStr = new MemoryStream();
					//Serialize object to the memorystream for sizing
					fmt.Serialize(memStr,msgText);
					byte[] data = memStr.GetBuffer();
					//get header info in bytes
					int memSize = (int)memStr.Length;
					byte[] size = BitConverter.GetBytes(memSize);	
					byte[] type = BitConverter.GetBytes((int)MsgType.Text);
					byte[] header = new Byte[8];
					size.CopyTo(header,0);
					type.CopyTo(header,4);

					
					//send header
					str.Write(header,0,8);
					//send data
					str.Write(data,0,memSize);
					str.Flush();
					memStr.Close();
				}
			}
			catch
			{
				this.ReConnect();
			}
		}

		public	void SendText(string text, BitArray ba)
		{
			MsgText msgText = new MsgText(text,ba);
			
			try
			{
				lock(_client.GetStream())
				{
					//Get stream
					NetworkStream str = _client.GetStream();
					IFormatter fmt = new BinaryFormatter();
					MemoryStream memStr = new MemoryStream();
					//Serialize object to the memorystream for sizing
					fmt.Serialize(memStr,msgText);
					byte[] data = memStr.GetBuffer();
					//get header info in bytes
					int memSize = (int)memStr.Length;
					byte[] size = BitConverter.GetBytes(memSize);	
					byte[] type = BitConverter.GetBytes((int)MsgType.Text);
					byte[] header = new Byte[8];
					size.CopyTo(header,0);
					type.CopyTo(header,4);

					
					//send header
					str.Write(header,0,8);
					//send data
					str.Write(data,0,memSize);
					str.Flush();
					memStr.Close();
				}
			}
			catch
			{
				this.ReConnect();
			}

		}
		public void SendTradeMsg(MsgTrade msgTrade)
		{
			try
			{
				lock(_client.GetStream())
				{
					//Get stream
					NetworkStream str = _client.GetStream();
					IFormatter fmt = new BinaryFormatter();
					MemoryStream memStr = new MemoryStream();
					//Serialize object to the memorystream for sizing
					fmt.Serialize(memStr,msgTrade);
					byte[] data = memStr.GetBuffer();
					//get header info in bytes
					int memSize = (int)memStr.Length;
					byte[] size = BitConverter.GetBytes(memSize);	
					byte[] type = BitConverter.GetBytes((int)MsgType.Trade);
					byte[] header = new Byte[8];
					size.CopyTo(header,0);
					type.CopyTo(header,4);
					Console.WriteLine("Trade message sent");
					
					//send header
					str.Write(header,0,8);
					//send data
					str.Write(data,0,memSize);
					str.Flush();
					memStr.Close();
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}
		}
		public void SendBusyMsg(int targetID)
		{

		}
		public void SetUpTrade(Trade trade, int otherPlayerID)
		{
			Trader trader = new Trader(this.PlayerID, otherPlayerID, trade);
			this._playerData.Fleet.AddSpecialShip(trader);
		}
		public void CancelTrade(Trade trade)
		{
			if(trade.AskedPartyID == this.PlayerID)
				this._playerData.Fleet.RemoveTradeShip(trade.AskedShipID);
			else
				this._playerData.Fleet.RemoveTradeShip(trade.OfferingShipID);
			this.UpdateResourceInfo();
		}
		public void InitView()
		{
			Thread thread = Thread.CurrentThread;
			Console.WriteLine("Current thread from Init View: " + thread.Name);
			Console.WriteLine("Current threadstate from Init View: " + thread.ThreadState);


			Point offset = Point.Empty;
			int size = 25;
			bool update = false;
			if(_mapBoard != null)
			{
				offset = _mapBoard.Offset;
				size = _mapBoard.HexSize;
				update = true;
			}
			_mapBoard = new MapBoard(_gameData, _playerID);
			if(update)
			{
				_mapBoard.Offset = offset;
				_mapBoard.HexSize = size;
			}
			else
				_mapBoard.HexSize = ClientSize.Width/(_gameData.DataMap.Width * 2);
			if(_playerID != -1)
			{
				_playerData = _gameData.PlayerList[_playerID];
				this._miSystemLogs.Enabled = true;
				this._miNextTurn.Enabled = true;
				this._miNextTurn.Text = "&Ready!";
			}
            this.PrepBoard();
			this.Invalidate();

		}

		public void Disconnect()
		{
			this._connected=false;
			if(_client != null)
				_client.Close();
			this._miConnect.Enabled = true;
		}

		#endregion
		#region OnEvent Form Overrides
		/// <summary>
		/// Sets scroll postion, paints map and tokens
		/// </summary>
		/// <param name="grfx"></param>
		protected override void OnPaint(PaintEventArgs pea)
		{
			if(_model == null)
			{
				base.OnPaint(pea);
				return;
			}
			Graphics grfx = pea.Graphics;
			_mapBoard.Offset = AutoScrollPosition;
			_mapBoard.DrawMap(grfx);		
			_mapBoard.PaintTokens(grfx);
			
		}
		/// <summary>
		/// Hit test tokens and map, and set grab point for grab/scrolling
		/// </summary>
		/// <param name="mea"></param>
		protected override void OnMouseDown(MouseEventArgs mea)
		{
			if(_model == null)
				return;
			_selectedToken = _mapBoard.HitTestTokens(mea.X,mea.Y);
			if(_selectedToken != null)
			{
				Font font = this.Font;
				Brush brush = Brushes.Beige;
				if(_selectedToken.GetType() == typeof(MoveableToken))
				{
					if(this._miNextTurn.Text == "&Wait!")
					{
						MessageBox.Show("Click 'Wait!' to change your orders.");
						_selectedToken = null;
						return;
					}

					if(!_playerData.Fleet.FleetSupported && mea.Button == MouseButtons.Left)
					{
						string deficit = (_playerData.Fleet.ExMineral + "/" + 
											_playerData.Fleet.ExOrganic + "/" +
											_playerData.Fleet.ExEnergy);
						_selectedToken = null;
						MessageBox.Show("Fleet not supported! \r\n" + deficit);
						return;
					}
					_lastSelectedToken = _selectedToken;
					Ship ship = (Ship)_selectedToken.Source;
					_lastSelectedShip = ship;
					if(mea.Button == MouseButtons.Right)
					{
						StarSystem localSystem = _gameData.DataMap[ship.HexLocation.X,ship.HexLocation.Y].System;
						//If this is a Colony ship or a base, at a starsystem we control
						// and it started the turn here and has not yet been assigned an action
						if((ship.Type == Ship.ShipType.Colony || ship.Type == Ship.ShipType.Base)&& localSystem != null &&
							localSystem.Owner == _playerData.ID && ship.HomeHex == ship.HexLocation && ship.Acted == false)
						{
							//	mi 0 = Base
							//  mi 1 = Abandon
							//	mi 2 = Build Ships
							//  mi 3 = DecommissionColony
							if(ship.Type == Ship.ShipType.Base)
							{
								this._colonyContextMenu.MenuItems[0].Enabled = false;
								this._colonyContextMenu.MenuItems[1].Enabled = true;
								this._colonyContextMenu.MenuItems[2].Enabled = true;
								this._colonyContextMenu.MenuItems[3].Enabled = true;
							}
							else if(_playerData.Fleet.ExMineral>=35 &&
								_playerData.Fleet.ExOrganic>=25 &&
								_playerData.Fleet.ExEnergy>=40)
							{
								this._colonyContextMenu.MenuItems[0].Enabled = true;
								this._colonyContextMenu.MenuItems[1].Enabled = false;
								this._colonyContextMenu.MenuItems[2].Enabled = false;
								this._colonyContextMenu.MenuItems[3].Enabled = true;
							}
							else
							{
								this._colonyContextMenu.MenuItems[0].Enabled = false;
								this._colonyContextMenu.MenuItems[1].Enabled = false;
								this._colonyContextMenu.MenuItems[2].Enabled = false;
								this._colonyContextMenu.MenuItems[3].Enabled = true;
							}
							this._colonyContextMenu.Show(this,new Point(mea.X,mea.Y));												
						}
						else
						{
							this._shipContextMenu.Show(this,new Point(mea.X,mea.Y));
						}
						return;
					}
					
					_mapBoard.ShowRange(_selectedToken.HomeHexIndex.X,_selectedToken.HomeHexIndex.Y,ship.Range);
					Invalidate();
					return;
				}
				if(mea.Button == MouseButtons.Right)
				{
					if(this.PlayerID == -1)
						return;
					if(_selectedToken.Source.GetType() != typeof(StarSystem))
						return;
					StarSystem star = (StarSystem)_selectedToken.Source;
					SystemInfo sysInfo = _playerData.SystemLog[star.Index];
					string name = sysInfo.Name;
					if(name.Substring(0,3)=="WMA" && star.Owner == this._playerID)
					{
						SystemName dlg = new SystemName();
						DialogResult result = dlg.ShowDialog();
						if(result == DialogResult.OK && dlg.SysName != "")
						{
							name = dlg.SysName;
							RenameSystem(name, star.Index);
						}
					}
					this._systemName.Text = name + "(" + sysInfo.InfluencePercentage + ")";
					this._systemType.Text = sysInfo.Type;
					this._miDash.Text = "-";
					this._minerals.Text = sysInfo.Mineral + "/" + sysInfo.MineralCollected;
					this._organics.Text = sysInfo.Organic + "/" + sysInfo.OrganicCollected;
					this._energy.Text = sysInfo.Energy + "/" + sysInfo.EnergyCollected;
					this._systemInfoMenu.Show(this, new Point(mea.X,mea.Y));
					_selectedToken = null;
					return;
					
				}
				
			}

			Hex hex = _mapBoard.HitTestMap(mea.X,mea.Y);
			if(hex == null)
				return;
			else
			{
				Point pt = AutoScrollPosition;
				_grabPt = new Point(mea.X - pt.X ,mea.Y - pt.Y);
			}
			Invalidate();
		}
		/// <summary>
		/// Process dragging for moveable tokens, else implement grab scroll
		/// </summary>
		/// <param name="mea"></param>
		protected override void OnMouseMove(MouseEventArgs mea)
		{
			if(_model == null)
				return;
			// if selected Token is moveable, drag it, if able
			if(_selectedToken != null && _selectedToken.GetType()== typeof(MoveableToken) )
			{ 
				Graphics grfx = CreateGraphics();
				MoveableToken token = (MoveableToken)_selectedToken;
				token.Move(grfx,mea.X,mea.Y,this);
				grfx.Dispose();
				//Invalidate();
				return;
			}
			if(_grabPt != Point.Empty)
			{  
				Point pt = new Point(_grabPt.X - mea.X, _grabPt.Y - mea.Y);
				AutoScrollPosition = pt;
				Invalidate();
			}	
		}
		/// <summary>
		/// Set grab point to null and reset hit-selections.  
		/// Update moved token locations
		/// </summary>
		/// <param name="mea"></param>
		protected override void OnMouseUp(MouseEventArgs mea)
		{
			
			if(_model == null)
				return;
			//
			// If we moved a ship set it down in its new location if that new location 
			// is within range
			//
			if(_selectedToken != null && _selectedToken.GetType()== typeof(MoveableToken))
			{
				MoveableToken token = (MoveableToken)_selectedToken;
				Hex hex = _mapBoard.HitTestMap(mea.X,mea.Y);
				if(hex != null && hex.Highlighted)//legal hexes should be hightlighted by 
					_mapBoard.MoveToken(hex, token);//range finder
				_mapBoard.ReSetMap();
			}
			
			_mapBoard.ClearRange();
			
			//_mapBoard.SelectedHex = null;	
			_selectedToken = null;
			_grabPt = Point.Empty;
			Invalidate();		
		}
		/// <summary>
		/// Zoom map
		/// </summary>
		/// <param name="mea"></param>
		protected override void OnMouseWheel(MouseEventArgs mea)
		{
			if(_model == null)
				return;
			Point pt = MousePosition;
			pt = this.PointToClient(pt);
			Point offset = AutoScrollPosition;
			if(mea.Delta < 0 && !base.HScroll && !base.VScroll)
				return;
			Size oldMapSize = _mapBoard.MapPixelSize;
			int oldSize = _mapBoard.HexSize;
			if(oldSize > 55) oldSize = 55;
			_mapBoard.HexSize += mea.Delta/(120 - (2*oldSize));
			AutoScrollMinSize = _mapBoard.MapPixelSize;
			int x = ((pt.X - offset.X)* _mapBoard.MapPixelSize.Width) / oldMapSize.Width - pt.X;
			int y = ((pt.Y - offset.Y)* _mapBoard.MapPixelSize.Height) / oldMapSize.Height - pt.Y;
			offset.X = x;
			offset.Y = y;
			Invalidate(); 
			AutoScrollPosition = offset;
		}
		protected override void OnClosing(CancelEventArgs cea)
		{
			DialogResult d = MessageBox.Show("Closing this window will disconnect you and end \nyour game session, do you wish to proceeed?",
				"Disconnect?",MessageBoxButtons.OKCancel);
			if(d != DialogResult.OK)
			{
				cea.Cancel = true;
				return;
			}
			this.Disconnect();
			if(_systemLogView != null)
				_systemLogView.Close();
		}
		protected override void OnKeyDown(KeyEventArgs kea)
		{
			if(kea.KeyCode == Keys.Up || kea.KeyCode == Keys.Down)
			{
				Point pt = MousePosition;
				pt = this.PointToClient(pt);
				Point offset = AutoScrollPosition;
				if(kea.KeyCode == Keys.Down && (!base.HScroll || !base.VScroll || _model == null))
					return;
				Size oldMapSize = _mapBoard.MapPixelSize;
				int oldSize = _mapBoard.HexSize;
				if(oldSize > 55) oldSize = 55;
				if(kea.KeyCode == Keys.Up)
					_mapBoard.HexSize += 120/(120 - (2*oldSize));
				else
					_mapBoard.HexSize -= 120/(120 - (2*oldSize));
				AutoScrollMinSize = _mapBoard.MapPixelSize;
				int x = ((pt.X - offset.X)* _mapBoard.MapPixelSize.Width) / oldMapSize.Width - pt.X;
				int y = ((pt.Y - offset.Y)* _mapBoard.MapPixelSize.Height) / oldMapSize.Height - pt.Y;
				offset.X = x;
				offset.Y = y;
				Invalidate(); 
				AutoScrollPosition = offset;
			}
		}
		#endregion
		#region Event Handlers
		#endregion
		#region Utilities
		private void OpenTradeDialog()
		{
			_tradeDialog.Show();
		}
		/// <summary>
		/// Handler for the header AsyncCallback. The strategy will be to precede every
		/// message from the client with a 4 byte header that will be converted
		/// to an int describing the size in bytes of the following msg. A second
		/// asynchronous call will follow with a buffer equal to the size of the 
		/// expected message, with repeated calls if necessary til the message is 
		/// complete. The GetMessage method will the restart the process with another
		/// call to listen for the next header.
		/// </summary>
		/// <param name="ar"></param>
		private void UpdateResourceInfo()
		{
			if(_playerData == null)
				return;
			this._miMinerals.Text = "Minerals:    " + this._playerData.ResourcePool.Mineral + 
				" - " + this._playerData.Fleet.SupMineral + " = " +
				this._playerData.Fleet.ExMineral;
			this._miOrganics.Text = "Organics:   " + this._playerData.ResourcePool.Organic + 
				" - " + this._playerData.Fleet.SupOrganic + " = " +
				this._playerData.Fleet.ExOrganic;
			this._miEnergy.Text = "Energy:      " + this._playerData.ResourcePool.Energy + 
				" - " + this._playerData.Fleet.SupEnergy + " = " +
				this._playerData.Fleet.ExEnergy;
		}
		private void GetStreamHeader(IAsyncResult ar)
		{
			//
			// Get the info from the header and restart the BeginRead
			// until the entire object is retrieved, then get next header
			// 
			//	Check to make sure this is a valid response, if not reconnect to 
			//  Server and download gamedata as appropriate
			//
			_msgType = MsgType.Invalid;
			int intCount;
			try
			{
				//Lock the Client Stream
				lock(_client.GetStream())
				{
					intCount = _client.GetStream().EndRead(ar);
				}
				if(intCount<8)
				{
					//If a value less than 4 received that means that 
					//client disconnected or this is not a valid header
					_client.Close();
					Console.WriteLine("socket closed..intCount for header = " + intCount + " Attempting Reconnect");
					this.ReConnect();
					// here is where we need to deal with erroneous messages
					// either by reconnecting or re-initiating the current 
					// message.
				}
				//store the size of expected object
				_msgBytesExpected = BitConverter.ToInt32(_headerBytes,0);
				_msgType = (MsgType)BitConverter.ToInt32(_headerBytes,4);
				//reset the amount msg byte counter
				_msgBytesReceived = 0;
	
				//create a new buffer for expected message
				_msgBytes = new Byte[_msgBytesExpected];
				//go get it
				lock(_client.GetStream())
				{
					AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
					_client.GetStream().BeginRead(_msgBytes,0,_msgBytesExpected,GetStreamMsgCallback,null);
				}
			}
			catch
			{
				_client.Close();
				Console.WriteLine("socket closed (exception in get header) attempting reconnect");
				this.ReConnect();
			}
		}

		private void GetStreamMsg(IAsyncResult ar)
		{
			int intCount = 0;
			try
			{
				lock(_client.GetStream())
				{
					intCount = _client.GetStream().EndRead(ar);
				}
				if(intCount < 1)
				{
					//If a value less than 1 received that means that 
					//client disconnected
					_client.Close();
					Console.WriteLine("socket closed in client, get message");
				}

				_msgBytesReceived += intCount;
				_msgBytesExpected -= intCount;
				//if we dpn't have the whole message
				if(_msgBytesExpected > 0)
				{
					Console.WriteLine("Need to get more bytes");
					//get the next packet and append it to our buffer
					lock(_client.GetStream())
					{
						AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
						_client.GetStream().BeginRead(_msgBytes,_msgBytesReceived,_msgBytesExpected,
							GetStreamMsgCallback,null);
					}					
				}
				else
				{
					//otherwise, get the next header
					lock(_client.GetStream())
					{
						AsyncCallback GetStreamHeaderCallback = new AsyncCallback(GetStreamHeader);
						_client.GetStream().BeginRead(_headerBytes,0,8,GetStreamHeaderCallback,null);
					}

					//Maybe this shouldn't be called from here
					//Consider an event?
					ProcessMessage(_msgBytes);

				}
			}
			catch(Exception e)
			{
				Console.WriteLine("socket closed, exception from GetStreamMsg " + e);
				_client.Close();
				this.ReConnect();
			}
		}

		/// <summary>
		/// Figured out that this is must be running in a seperate thread from 
		/// the main app.(Which makes sense, given that its all triggered by a 
		/// call back function from the AsyncSocket call) So the form I'm trying 
		/// to create only has a message loop running in this thread, so when I 
		/// try to go and reference it from my variable _tradeDialog in my client 
		/// form, it doesn't work. Will try an event to trigger the creation of 
		/// the modeless trade dialog in the main thread.
		/// 
		/// Or not. this approach produced exactly the same results, a half 
		/// drawn window stalled. The rest of the program functional however.
		/// </summary>
		/// <param name="data"></param>
		private void ProcessMessage(byte [] data)
		{
			//Deserialize message
			IFormatter fmt = new BinaryFormatter();
			MemoryStream memHolder = new MemoryStream(data,0,data.Length);
			memHolder.Position = 0;

			switch(_msgType)
			{
				case MsgType.Ping:
				{
					PingRespond();
					break;
				}
				case MsgType.Text:
				{
					MsgText msg = (MsgText)fmt.Deserialize(memHolder);
					//if this is the first message assign it to client name
					if(_firstTime)
					{
						_firstTime = false;
						_chatView.ChatUpdate("Connected");
						Console.WriteLine("ChatUpdate first time called");
						_connected = true;
					}
					else
						_chatView.ChatUpdate(msg.Text);
					break;
				}
				case MsgType.Init:
				{
					_gameData = (GameData)fmt.Deserialize(memHolder);
					_model = new GameModel(_gameData);
					_chatView.UpdatePlayerList(_gameData);
					_gameData = _model.Update();
					this.InitTradeMenu();
					this.UpdateResourceInfo();
					this.InitView();
					break;
				}
				// 
				//  This is the connection ID // NOT the index for Playerlist
				//
				case MsgType.ID:
				{
					_ID = BitConverter.ToInt32(data,0);
					Console.WriteLine("Id set to " + _ID);	
					break;
				}
				case MsgType.Orders:
				{
					try
					{
						ArrayList orders = (ArrayList)fmt.Deserialize(memHolder);
						_model.NewOrders(orders);
					}
					catch(Exception e)			
					{
						Console.WriteLine(e);
						MessageBox.Show("Map Update from server failed");
						return;
					}					
					_gameData = _model.Update();
					this.UpdateResourceInfo();
					InitView();

					break;
				}
				case MsgType.PlayerID:
				{
					NameAndID nameid = (NameAndID)fmt.Deserialize(memHolder);
					string name = nameid.Name;
					int id = nameid.ID;
					if(name == this._userName)
					{
						this._playerID = id;
						_gameData.PlayerList[id].Password = this._password;
						this._miTradeManager.MenuItems[id].Enabled = false;
					}
                    _gameData.PlayerList[id].Name = name;
					_chatView.UpdatePlayerList(_gameData);
					this._miTradeManager.MenuItems[id].Text = name;
					this.UpdateResourceInfo();					
					this.InitView();
					break;
				}
				case MsgType.SystemName:
				{
					MsgSystemName sysName = (MsgSystemName)fmt.Deserialize(memHolder);
					_playerData.SystemLog[sysName.Index].Name = sysName.Name;
					break;
				}
				case MsgType.Trade:
				{
					MsgTrade tradeMsg = (MsgTrade)fmt.Deserialize(memHolder);
					int otherPlayerID;
					switch(tradeMsg.Type)
					{
						case MsgTrade.TradeMsgType.Offer:
						case MsgTrade.TradeMsgType.Message:
						{
							otherPlayerID = tradeMsg.Trade.OfferingPartyID;
							//if player is already in negotiations with offering player update
							if(_tradeDialog != null && _tradeDialog.ActivePlayerID == otherPlayerID)
							{
								_tradeDialog.UpdateTradeMsg(tradeMsg);
							}
							//otherwise open new trade dialog (modal in this thread, not sure if this will work)
							else
							{
								string playerName = _gameData.PlayerList[tradeMsg.Trade.OfferingPartyID].Name;
								DialogResult dlgResult = MessageBox.Show(playerName + " requests Trade Talks.","",
									MessageBoxButtons.OKCancel);
								if(DialogResult == DialogResult.Cancel)
									return;
								Console.WriteLine("Offer received, attempting to open trade window");
								_tradeDialog = new TradeDialog(tradeMsg,_gameData,this,
									this._playerID);
								_tradeDialog.ShowDialog();
							}
							break;
						}
						case MsgTrade.TradeMsgType.Busy:
						{
							string name = _gameData.PlayerList[tradeMsg.Trade.AskedPartyID].Name;
							MessageBox.Show(name + " is busy right now. \nTry again later.");
							break;
						}
						case MsgTrade.TradeMsgType.Accept:
						{
							//forward to tradedialog if open
							if(this.PlayerID == tradeMsg.Trade.OfferingPartyID)
								otherPlayerID = tradeMsg.Trade.AskedPartyID;
							else
								otherPlayerID = tradeMsg.Trade.OfferingPartyID;
							if(_tradeDialog.ActivePlayerID == otherPlayerID)
							{
								_tradeDialog.UpdateTradeMsg(tradeMsg);
							}
							else
								this.SetUpTrade(tradeMsg.Trade, otherPlayerID);
							break;
						}
						case MsgTrade.TradeMsgType.Reject:
						{
							//forward to tradedialog if open
							if(_tradeDialog.ActivePlayerID == tradeMsg.Trade.AskedPartyID)
								_tradeDialog.UpdateTradeMsg(tradeMsg);
							break;
						}
						case MsgTrade.TradeMsgType.Cancel:
						{
							if(this.PlayerID == tradeMsg.Trade.OfferingPartyID)
								otherPlayerID = tradeMsg.Trade.AskedPartyID;
							else
								otherPlayerID = tradeMsg.Trade.OfferingPartyID;
							this.CancelTrade(tradeMsg.Trade);
							if(_tradeDialog != null && _tradeDialog.ActivePlayerID == otherPlayerID)
								_tradeDialog.UpdateTradeMsg(tradeMsg);
							else
								MessageBox.Show(this._gameData.PlayerList[otherPlayerID].Name +
													" has cancelled a trade agreement.");
							break;		
						}
					}
					break;
				}		
			}
			memHolder.Close();
		}
		private void InitTradeMenu()
		{
			MenuItem[] items = new MenuItem[this._gameData.PlayerList.Count];
			for(int i = 0; i < this._gameData.PlayerList.Count; i++)
			{
				items[i] = new MenuItem();
				items[i].Text = this._gameData.PlayerList[i].Name;
				items[i].Index = i;
				items[i].Click += new EventHandler(OnTradeMenuClick);
				if(this.PlayerID == i)
					items[i].Enabled = false;
			}
			this._miTradeManager.MenuItems.AddRange(items);
		}
		private void RenameSystem(string name, int index)
		{
			//send this players fleet data to the server
			try
			{
				lock(_client.GetStream())
				{
					MsgSystemName sysName = new MsgSystemName(name, index);
					NetworkStream str = _client.GetStream();
					IFormatter fmt = new BinaryFormatter();
					MemoryStream memStr = new MemoryStream();
					//Serialize object to the memorystream for sizing
					fmt.Serialize(memStr,sysName);
					byte[] data = memStr.GetBuffer();
					//get header info in bytes
					int memSize = (int)memStr.Length;
					byte[] size = BitConverter.GetBytes(memSize);	
					byte[] type = BitConverter.GetBytes((int)MsgType.SystemName);
					byte[] header = new Byte[8];
					size.CopyTo(header,0);
					type.CopyTo(header,4);		
					//send header
					str.Write(header,0,8);
					//send data
					str.Write(data,0,memSize);
					str.Flush();
					memStr.Close();
				}
			}
			catch(Exception e)
			{
				MessageBox.Show("Send New name fails");
				Console.WriteLine(e);
				this.Disconnect();
				this.ReConnect();
			}
		}
		private void SendOrders()
		{
			//send this players fleet data to the server
			try
			{
				lock(_client.GetStream())
				{
					Fleet fleet = _playerData.Fleet;
					if(fleet == null)
						MessageBox.Show("No Fleet in playerData");
					//Get stream
					NetworkStream str = _client.GetStream();
					IFormatter fmt = new BinaryFormatter();
					MemoryStream memStr = new MemoryStream();
					//Serialize object to the memorystream for sizing
					fmt.Serialize(memStr,fleet);
					byte[] data = memStr.GetBuffer();
					//get header info in bytes
					int memSize = (int)memStr.Length;
					byte[] size = BitConverter.GetBytes(memSize);	
					byte[] type = BitConverter.GetBytes((int)MsgType.Orders);
					byte[] header = new Byte[8];
					size.CopyTo(header,0);
					type.CopyTo(header,4);
					
					//send header
					str.Write(header,0,8);
					//send data
					str.Write(data,0,memSize);
					str.Flush();
					memStr.Close();
				}
			}
			catch(Exception e)
			{
				MessageBox.Show("Send Orders fails");
				Console.WriteLine(e);
				this.Disconnect();
				this.ReConnect();
			}
		}
		private void CancelOrders()
		{
			try
			{
				//Get stream
				NetworkStream str = _client.GetStream();
				
				//Serialize object to the memorystream for sizing
				int dummy = 334;
				byte[] data = BitConverter.GetBytes(dummy);
				//get header info in bytes
				byte[] size = BitConverter.GetBytes(4);	
				byte[] type = BitConverter.GetBytes((int)MsgType.Cancel);
				byte[] header = new Byte[8];
				size.CopyTo(header,0);
				type.CopyTo(header,4);
					
				//send header
				str.Write(header,0,8);
				//send data
				str.Write(data,0,data.Length);
				str.Flush();
			}
			catch
			{
				this.Disconnect();
				this.ReConnect();
			}
		}
		private void PrepBoard()
		{
			AutoScrollMinSize = _mapBoard.MapPixelSize;
		}
		private void PingRespond()
		{
			try
			{
				lock(_client.GetStream())
				{
					NetworkStream str = _client.GetStream();
					byte[] type = BitConverter.GetBytes((int)MsgType.Ping);
					byte[] size = BitConverter.GetBytes(4);//size of filler bytes..just to fit protocol
					byte[] data = BitConverter.GetBytes(4);//filler bytes
					byte[] header = new Byte[8];
					size.CopyTo(header,0);
					type.CopyTo(header,4);
					//send header
					str.Write(header,0,8);
					//send data
					str.Write(data,0,4);

					str.Flush();
				}
			}
			catch
			{
				this.Disconnect();
			}
		}
		/// <summary>
		/// Fire this whenever we're disconnected rather than crashing the client
		/// </summary>
		private void ReConnect()
		{
			Console.WriteLine("Attempting Reconnection");
			if(_client != null)
				_client.Close();
			try
			{
				_client = new TcpClient(_serverIP,5220);
			}
			catch
			{
				MessageBox.Show("Unable to maintain connection");
				return;
			}
			//Id and name here will still be right, just need to set them
			// on the client host.
			SendText(_userName);
			RequestPlayerID(_playerID,_password);
			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WormholeClient));
			this._colonyContextMenu = new System.Windows.Forms.ContextMenu();
			this._miBuildBase = new System.Windows.Forms.MenuItem();
			this._miAbandonBase = new System.Windows.Forms.MenuItem();
			this._miBuildShips = new System.Windows.Forms.MenuItem();
			this._miDecommissionColony = new System.Windows.Forms.MenuItem();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this._miServer = new System.Windows.Forms.MenuItem();
			this._miConnect = new System.Windows.Forms.MenuItem();
			this._miChat = new System.Windows.Forms.MenuItem();
			this._miSystemLogs = new System.Windows.Forms.MenuItem();
			this._miNextTurn = new System.Windows.Forms.MenuItem();
			this._miLabelRI = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this._miMinerals = new System.Windows.Forms.MenuItem();
			this._miOrganics = new System.Windows.Forms.MenuItem();
			this._miEnergy = new System.Windows.Forms.MenuItem();
			this._miTradeManager = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this._miGameInfo = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this._systemInfoMenu = new System.Windows.Forms.ContextMenu();
			this._systemName = new System.Windows.Forms.MenuItem();
			this._miDash = new System.Windows.Forms.MenuItem();
			this._systemType = new System.Windows.Forms.MenuItem();
			this._minerals = new System.Windows.Forms.MenuItem();
			this._organics = new System.Windows.Forms.MenuItem();
			this._energy = new System.Windows.Forms.MenuItem();
			this._shipContextMenu = new System.Windows.Forms.ContextMenu();
			this._miDecommissionShip = new System.Windows.Forms.MenuItem();
			this._miCancel = new System.Windows.Forms.MenuItem();
			// 
			// _colonyContextMenu
			// 
			this._colonyContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							   this._miBuildBase,
																							   this._miAbandonBase,
																							   this._miBuildShips,
																							   this._miDecommissionColony});
			// 
			// _miBuildBase
			// 
			this._miBuildBase.Enabled = false;
			this._miBuildBase.Index = 0;
			this._miBuildBase.Text = "Build Base";
			this._miBuildBase.Click += new System.EventHandler(this.miBuildBaseOnClick);
			// 
			// _miAbandonBase
			// 
			this._miAbandonBase.Enabled = false;
			this._miAbandonBase.Index = 1;
			this._miAbandonBase.Text = "Abandon Base";
			this._miAbandonBase.Click += new System.EventHandler(this.miAbandonBaseOnClick);
			// 
			// _miBuildShips
			// 
			this._miBuildShips.Enabled = false;
			this._miBuildShips.Index = 2;
			this._miBuildShips.Text = "Build Ships";
			this._miBuildShips.Click += new System.EventHandler(this.miBuildShipsHandler);
			// 
			// _miDecommissionColony
			// 
			this._miDecommissionColony.Index = 3;
			this._miDecommissionColony.Text = "Decommission Colony";
			this._miDecommissionColony.Click += new System.EventHandler(this.miDecommissionOnClick);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this._miServer,
																					  this._miSystemLogs,
																					  this._miLabelRI,
																					  this._miTradeManager,
																					  this.menuItem1,
																					  this._miNextTurn});
			// 
			// _miServer
			// 
			this._miServer.Index = 0;
			this._miServer.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this._miConnect,
																					  this._miChat});
			this._miServer.Text = "&Server";
			// 
			// _miConnect
			// 
			this._miConnect.Index = 0;
			this._miConnect.Text = "Connect";
			this._miConnect.Click += new System.EventHandler(this.miConnect_Click);
			// 
			// _miChat
			// 
			this._miChat.Index = 1;
			this._miChat.Text = "Chat";
			this._miChat.Click += new System.EventHandler(this.miChat_Click);
			// 
			// _miSystemLogs
			// 
			this._miSystemLogs.Enabled = false;
			this._miSystemLogs.Index = 1;
			this._miSystemLogs.Text = "System &Logs!";
			this._miSystemLogs.Click += new System.EventHandler(this.miSystemLogs_Click);
			// 
			// _miNextTurn
			// 
			this._miNextTurn.Enabled = false;
			this._miNextTurn.Index = 5;
			this._miNextTurn.Text = "&Ready!";
			this._miNextTurn.Click += new System.EventHandler(this.miNextTurn_Click);
			// 
			// _miLabelRI
			// 
			this._miLabelRI.Index = 2;
			this._miLabelRI.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem2,
																					   this.menuItem4,
																					   this._miMinerals,
																					   this._miOrganics,
																					   this._miEnergy});
			this._miLabelRI.Text = "Resource Info";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Collected - Support = Excess";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// _miMinerals
			// 
			this._miMinerals.Index = 2;
			this._miMinerals.Text = "Minerals";
			// 
			// _miOrganics
			// 
			this._miOrganics.Index = 3;
			this._miOrganics.Text = "Organics";
			// 
			// _miEnergy
			// 
			this._miEnergy.Index = 4;
			this._miEnergy.Text = "Energy";
			// 
			// _miTradeManager
			// 
			this._miTradeManager.Index = 3;
			this._miTradeManager.Text = "Trade Talks";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 4;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this._miGameInfo,
																					  this.menuItem3});
			this.menuItem1.Text = "&Help";
			// 
			// _miGameInfo
			// 
			this._miGameInfo.Index = 0;
			this._miGameInfo.Text = "&Game Info";
			this._miGameInfo.Click += new System.EventHandler(this.miGameInfo_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "&About";
			// 
			// _systemInfoMenu
			// 
			this._systemInfoMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this._systemName,
																							this._miDash,
																							this._systemType,
																							this._minerals,
																							this._organics,
																							this._energy});
			// 
			// _systemName
			// 
			this._systemName.Index = 0;
			this._systemName.Text = "";
			// 
			// _miDash
			// 
			this._miDash.Index = 1;
			this._miDash.Text = "";
			// 
			// _systemType
			// 
			this._systemType.Index = 2;
			this._systemType.Text = "";
			// 
			// _minerals
			// 
			this._minerals.Index = 3;
			this._minerals.Text = "";
			// 
			// _organics
			// 
			this._organics.Index = 4;
			this._organics.Text = "";
			// 
			// _energy
			// 
			this._energy.Index = 5;
			this._energy.Text = "";
			// 
			// _shipContextMenu
			// 
			this._shipContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this._miDecommissionShip,
																							 this._miCancel});
			// 
			// _miDecommissionShip
			// 
			this._miDecommissionShip.Index = 0;
			this._miDecommissionShip.Text = "Decommission";
			this._miDecommissionShip.Click += new System.EventHandler(this.miDecommissionOnClick);
			// 
			// _miCancel
			// 
			this._miCancel.Index = 1;
			this._miCancel.Text = "";
			// 
			// WormholeClient
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(720, 431);
			this.Menu = this.mainMenu1;
			this.Name = "WormholeClient";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Wormhole Client";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

		}
			
		#endregion

		#region Main
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run( new WormholeClient() );
		}
		#endregion

		#region MenuHandlers
		private void miAbandonBaseOnClick(object sender, EventArgs ea)
		{
			if(MessageBox.Show("Are you sure you want to abandon this Star Base?",
				"Confirm",MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				Colony colony = (Colony)_lastSelectedShip;
				colony.Type = Ship.ShipType.Colony;
				colony.Base = false;
				this._playerData.Fleet.Support(this._playerData.ResourcePool);
				this.UpdateResourceInfo();
			}
		}
		private void miBuildBaseOnClick(object sender, EventArgs ea)
		{
			Colony colony = (Colony)_lastSelectedShip;
			colony.Type = Ship.ShipType.Base;
			colony.Base = true;
			this._playerData.Fleet.Support(this._playerData.ResourcePool);
			this.UpdateResourceInfo();
		}
		private void miBuildShipsHandler(object obj, EventArgs ea)
		{
			ShipBuilder _shipBuilder = new ShipBuilder(_playerID,this._playerData);
			DialogResult result = _shipBuilder.ShowDialog();
			if(result == DialogResult.OK)
			{
				if(_shipBuilder.Selection != Ship.ShipType.None)
				{
					_playerData.Fleet.Add(_shipBuilder.Selection,_lastSelectedShip.HexLocation);
					_playerData.Fleet.Support(_playerData.ResourcePool);
					this.UpdateResourceInfo();
					_lastSelectedShip.Acted = true;
				}
				_shipBuilder.Dispose();
			}
		}
		private void miSystemLogs_Click(object obj, EventArgs ea)
		{
			if(_systemLogView != null)
			{
				_systemLogView.Close();
				_systemLogView.Dispose();
				_systemLogView = null;
				return;
			}
	
			_systemLogView = new SystemLogView(_playerData);
			_systemLogView.Icon = _icon;
			_systemLogView.Owner = this;
			_systemLogView.Show();		
		}
		private void miDecommissionOnClick(object obj, EventArgs ea)
		{
			if(MessageBox.Show("Are you sure you want to order this unit to retire?",
				"Confirm",MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				_mapBoard.RemoveToken(_lastSelectedToken);
				_lastSelectedToken = null;
				_playerData.Fleet.Remove(_lastSelectedShip);
				_playerData.Fleet.Support(_playerData.ResourcePool); 
				_lastSelectedShip = null;
				this.UpdateResourceInfo();
				Invalidate();
			}
		}
		private void miConnect_Click(object sender, System.EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;
			ConnectDialog dlg = new ConnectDialog();
			dlg.ShowDialog(this);
			if(dlg.DialogResult != DialogResult.OK)
				return;
			_serverIP = dlg.TextIP;
			try
			{
				_client = new TcpClient(_serverIP,5220);
				mi.Enabled = false;
				_connected = true;
			}
			catch
			{
				MessageBox.Show("Connection Failed");
				return;
			}

			_userName = dlg.ClientName;

			_chatView = new ChatView(this);
			_chatView.Show();

			

			//Start Receiving the Messages
			AsyncCallback GetStreamHeaderCallback = new AsyncCallback(GetStreamHeader);
			_client.GetStream().BeginRead(_headerBytes,0,8,GetStreamHeaderCallback,null);
			//go get the first msg header

			//send username to server
			SendText(_userName);
		
		}
		private void miNextTurn_Click(object sender, System.EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;
			if(mi.Text == "&Ready!")
			{
				if(!this._lastTurnSupport && !this._playerData.Fleet.FleetSupported)
				{
					MessageBox.Show("Fleet must be supported before advancing! Decommission a unit.");
					return;
				}
				if(MessageBox.Show("Are you ready for the next turn?","Send Orders",MessageBoxButtons.OKCancel)
					== DialogResult.OK)
				{
					SendOrders();
					mi.Text= "&Wait!";
				}
				this._lastTurnSupport = this._playerData.Fleet.FleetSupported;
			}
			else
			{
				CancelOrders();
				mi.Text= "&Ready!";
			}
		}
		private void miChat_Click(object sender, System.EventArgs e)
		{
			if(_chatView != null)
			{

				_chatView.UpdatePlayerList(_gameData);
				_chatView.Show();				
			}
			else
			{
				Thread thread = Thread.CurrentThread;
				Console.WriteLine("Current thread from Chat_Click: " + thread.Name);
				Console.WriteLine("Current threadstate from Chat_Click: " + thread.ThreadState);

				_chatView = new ChatView(this);
				_chatView.UpdatePlayerList(_gameData);
				_chatView.Show();
			}
		}
		private void miGameInfo_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("Wormhole My Ass\r\n\r\nFleet Support:		Mineral/Organic/Energy\r\n\r\nColony Ship		15/25/10\r\n" +
									"Scout			15/10/35\r\nDefender			60/20/25\r\nStar Base			50/50/50\r\n" +
									"\r\nBase Fleet Resources	80/85/95\r\n\r\nSystem Resource Ranges:\r\n\r\n" +
									"Red Giant			20-50/5-25/15-35\r\n\r\nYellow			15-35/20-50/5-25\r\n\r\n" +
									"White Dwarf		5-25/15-35/20-50");
		}
		private void OnTradeMenuClick(object sender, EventArgs e)
		{
			Console.WriteLine("Thread from Main Menu: " + Thread.CurrentThread.Name);
			if(this._miNextTurn.Text == "&Wait!")
			{
				MessageBox.Show("You can't enter trade negotiations once orders have \n\rbeen entered. Cancle orders and try again.");
				return;
			}
			MenuItem item = (MenuItem)sender;
			if(item.Text == "Open")
				return;
			if(_tradeDialog == null)
			{
				_tradeDialog = new TradeDialog(_gameData,this,this._playerID,item.Index);
				_tradeDialog.Show();
			}
			else
				MessageBox.Show("You may only open one \nTrade Window at a time.");
		}

		#endregion


	}
}