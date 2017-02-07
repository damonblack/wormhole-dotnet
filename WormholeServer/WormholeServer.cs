using System;
using System.Drawing;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Serialization;

using WormholeClient;
using WormholeClient.Data;
using Messages;



namespace WormholeServer
{
	/// <summary>
	/// The Server owns a model object that reflects the relevent game data 
	/// from turn to turn. The Server will facilitate communication between 
	/// the model and players as well as manage a chat interface for player
	/// to playre communication. WormHoleServer is based on Form and acts as 
	/// the start up form for the game. The Server will start up with an 
	/// active chat interface allowing players to examing the newly created 
	/// game map before beginning the game, possibly allowing vetoing 
	/// undesirable maps. Players will be asked for a password on their first 
	/// turn after downloading the map. When the turn is ended, the orders will 
	/// be processed by the Server and the game data updated. As each player then
	/// enters orders for the next turn after and agreed upon amount of time for
	/// diplomacy. Turns proceed as rounds of players entering orders until a 
	/// victory condition is achieved.
	///  </summary>
	public class WormholeServer : System.Windows.Forms.Form
	{
		#region Private Fields
		/// <summary>
		/// Desperate, uninformed attempt to keep the fucking sockets
		/// from timing out.
		/// </summary>
		public System.Windows.Forms.Timer				_timer = new System.Windows.Forms.Timer();
		/// <summary>
		/// Number of clients connected
		/// </summary>
		private int						_numClients = 0;
		/// <summary>
		/// The Game, save GameData member to a file to save the game
		/// </summary>
		private GameModel				_model = null;
		/// <summary>
		/// Object representing the map as a unit
		/// </summary>
		private MapBoard				_mapBoard;
		private GameData				_gameData;

		/// <summary>
		/// Array for eight players
		/// </summary>
		private ClientHost[]			_clientHostArray = new ClientHost[8];
		private ClientHost[]			_playerIDArray = new ClientHost[8];
		/// <summary>
		/// Thread listening for new connections
		/// </summary>
		private Thread					_listenerThread;
		/// <summary>
		/// Primary server socket
		/// </summary>
		private TcpListener				_listener;
		/// <summary>
		/// Anchor for grab/scroll function
		/// </summary>
		private Point					_grabPt;

		//Menu Items
		private MainMenu				mainMenu1;
		private MenuItem				miGame;
		private MenuItem				miNewGame;
		private MenuItem				miOpenGame;
		//private MenuItem				miNextTurn;
		private MenuItem				miSaveGame;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region Constructors and Clean up
		/// <summary>
		/// Default constructor
		/// </summary>
		public WormholeServer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_timer.Interval = 10000;
			_timer.Tick += new EventHandler(OnTimerTick);
			_timer.Enabled = true;
			//
			//  Sets up Double Buffering for cleaner screen refresh
			//
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint , true);	
			this.SetStyle(ControlStyles.UserPaint, true);		

			_listenerThread = new Thread(new ThreadStart(StartListen));
			_listenerThread.Start();
			Console.WriteLine("Socket Server Listening");
			
			try
			{
				Assembly thisAssembly = Assembly.GetAssembly(this.GetType());
				Stream imageStream = thisAssembly.GetManifestResourceStream
					("WormholeServer.Wormhole.ico");
				this.Icon = new Icon(imageStream);
			}
			catch
			{
				MessageBox.Show("Couldn't find Wormhole.ico");
			}

		} 
		public void StartListen()
		{
			try
			{
				//
				//	Consider a dialog displaying IP and allowing custom
				//	port selection.
				//

				//	Start the tcpListener
				_listener = new TcpListener(5220);
				_listener.Start();
				do
				{
					//
					//	Here we'll need to figure out how to handle new clients
					//  taking into consideration options we'll offer to new 
					//  connections -- i.e. joining game in progress, re-starting
					//  saved game, choosing slot... passwords?. Lets consider
					//	keeping the Guid/Hashtable stategy for now as a way of 
					//  positively identifying players/clients, perhaps assigning
					//  Clients to Player objects later in the process
					//
					ClientHost newClient = new ClientHost(_listener.AcceptTcpClient());
					if(newClient != null)
						Console.WriteLine("Client Connected");
					//Attach the Delegates
					newClient.eDisconnected+= new DisconnectDelegate(OnDisconnected);
					newClient.eConnected+=new ConnectDelegate(this.OnConnected);
					newClient.eMessageReceived+=new MessageDelegate(OnMessageReceived);
					newClient.eReady+=new ReadyDelegate(OnReady);
					newClient.eIDRequest+=new IDRequestDelegate(OnIDRequest);
					newClient.eRenameSystem+=new RenameSystemDelegate(OnRenameSystem);
					newClient.eTradeMessage+=new TradeMessageDelegate(OnTradeMessage);
					//Connect to the clients
					newClient.Connect();
					
				}
				while(true);
			}
			catch
			{
				_listener.Stop();
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
		private void WormholeServer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_listener.Stop();
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
				return;
			//if a game is currently running, DrawMap and paint tokens					   
			Graphics grfx = pea.Graphics;
			_mapBoard.Offset = AutoScrollPosition;
			_mapBoard.DrawMap(grfx);		
			_mapBoard.PaintTokens(grfx); 
		}
	
		/// <summary>
		/// Set grab point for grab/scrolling
		/// </summary>
		/// <param name="mea"></param>
		protected override void OnMouseDown(MouseEventArgs mea)
		{
			if(_model == null)
				return;
			Point pt = AutoScrollPosition;
			_grabPt = new Point(mea.X - pt.X ,mea.Y - pt.Y);
		}
		/// <summary>
		/// Implement grab scroll
		/// </summary>
		/// <param name="mea"></param>
		protected override void OnMouseMove(MouseEventArgs mea)
		{
			if(_model == null)
				return;
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
			_grabPt = Point.Empty;
		}
		/// <summary>
		/// Zoom map
		/// </summary>
		/// <param name="mea"></param>
		protected override void OnMouseWheel(MouseEventArgs mea)
		{
			if(_model == null)
				return;
			//
			//  Re setting the hex size should trigger recalibration 
			// of all map map items
			//
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
		#endregion*/
		#region Events
		#endregion

		#region EventHandlers
		public void OnTimerTick(object sender, EventArgs ea)
		{
			foreach(ClientHost client in _clientHostArray)
			{
				if(client != null)
					client.Ping();
			}
			Console.WriteLine("Pinging for fun and profit");

		}
		/// <summary>
		/// event fired every time a player indicates the are ready
		/// if all are ready, triggers the next turn
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnReady(object sender, EventArgs e)
		{
			CheckReady();
		}
		/// <summary>
		/// Event Handler for Connected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnConnected(object sender, EventArgs e)
		{
			_numClients++;
			bool assigned = false;
			//Get the client that raised the event
			ClientHost temp = (ClientHost)sender;
			//Add the client to the first empty slot
			for(int i = 0; i < 8; i++)
			{
				if(_clientHostArray[i] == null)
				{
					temp.ID = i;
					_clientHostArray[i] = temp;
					assigned = true;
					break;
				}
			}
			if(!assigned)
			{
				temp.SendText("Sorry, server is full");
				temp.Disconnect();
				return;
			}
			
			Console.WriteLine(temp.UserName + " connected at slot " + temp.ID);
			//loop through each client and announce the 
			//client connected
			foreach(ClientHost client in _clientHostArray)
			{
				if(client != null)
				{
					client.SendText(temp.UserName + " connected at slot " + temp.ID);
					Console.WriteLine("Server SendText called");
				}
			}
		}

		/// <summary>
		/// Event Handler for Disconnteced
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnDisconnected(object sender, EventArgs e)
		{
			_numClients--;
			ClientHost temp = (ClientHost)sender;
			_clientHostArray[temp.ID] = null;
			Console.WriteLine(temp.UserName + " disconnected at slot " + temp.ID);
			foreach(ClientHost client in _clientHostArray)
			{
				if(client != null)
					client.SendText(temp.UserName + " disconnected from slot " + temp.ID);
			}

		}

		/// <summary>
		/// Event Handler for MessageReceived
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnMessageReceived(object sender, MessageEventArgs e)
		{
			//Message sender client
			ClientHost temp = (ClientHost)sender;
			Console.WriteLine(temp.UserName + ": " + e.Message.Text);
			//for each valid client
			for(int i = 0; i < 8; i++)
			{
				if(_clientHostArray[i] != null)
				{
					int ID = _clientHostArray[i].PlayerID;
					//if they are in the bit array of targets
					if(ID == -1 || e.Message.PlayerTargets[ID] == true)
						_clientHostArray[i].SendText(temp.UserName + ":\r\n    " + e.Message.Text);
				}
			}
		}
		public void OnRenameSystem(object sender, SystemNameEventArgs e)
		{
			PlayerList pList = _gameData.PlayerList;
			//update playerlogs here for saving purposes
			for(int a = 0; a < pList.Count; a++)
			{
				pList[a].SystemLog[e.SysName.Index].Name = e.SysName.Name;
			}
			//signal players to update their logs
			for(int i = 0; i < 8; i++)
			{
				if(_clientHostArray[i] != null)
				{
					_clientHostArray[i].SendSystemName(e.SysName);
				}
			}
		}
		public void OnIDRequest(object sender, IDRequestEventArgs e)
		{
			ClientHost clientHost = (ClientHost)sender;
			GameData gameData = _model.Update();
			int id = e.ID;
			string password = e.Password;
			//
			//	Permission is checked at client level, here we just check to see
			//   if the player ID has already been assigned to another client
			//

			//Cycle through Client Hosts, if any have requested ID, 
			//do nothing - no update, probably should send a rejection notice at 
			// some point, we'll see if its necessary		
			foreach(ClientHost tempClient in _clientHostArray)
			{
				if(tempClient != null && tempClient.PlayerID == id)
					return;
			}
			this._playerIDArray[id] = clientHost;
			gameData.PlayerList[id].Name = clientHost.UserName;
			gameData.PlayerList[id].Password = password;
			clientHost.PlayerID = id;
			Console.WriteLine(clientHost.UserName +  " is assigned slot " + id);
			foreach(ClientHost tempClient in _clientHostArray)
			{
				if(tempClient != null)
					tempClient.SendID(id,clientHost.UserName);
			}
		}
		/// <summary>
		/// Serves as confirmation for accepting and cancelling trades sending out duplicate
		/// notices to each party when an acceptance or cancelation notice is received from
		/// one of the trade partners
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnTradeMessage(object sender, TradeMessageEventArgs e)
		{
			MsgTrade tradeMsg = e.TradeMsg;
			switch(tradeMsg.Type)
			{
				case MsgTrade.TradeMsgType.Accept:
				
					//assign an ID
					//tradeMsg.Trade.TradeID = _gameData.NextTradeID;
					//add to list
					//this._tradeList.Add(tradeMsg.Trade);
					//send confirmation
					//this._playerIDArray[tradeMsg.Trade.OfferingPartyID].SendTradeMsg(tradeMsg);
					//this._playerIDArray[tradeMsg.Trade.AskedPartyID].SendTradeMsg(tradeMsg);
					//break;
				
				case MsgTrade.TradeMsgType.Cancel:
				{
					//int removeID = tradeMsg.Trade.TradeID; //this info will now be kept on
					//find trade								//and transfered with the trade
					/*foreach(object trade in this._tradeList)
					{
						Trade t = (Trade)trade;
						//Remove trade from trade list
						if(t.TradeID == removeID)
							_tradeList.Remove(trade);
					}*/
					//Notify each trade partner of the cancellation
					this._playerIDArray[tradeMsg.Trade.OfferingPartyID].SendTradeMsg(tradeMsg);
					this._playerIDArray[tradeMsg.Trade.AskedPartyID].SendTradeMsg(tradeMsg);
					break;
				}
				case MsgTrade.TradeMsgType.Offer:
				case MsgTrade.TradeMsgType.Reject:
				case MsgTrade.TradeMsgType.Busy:
				case MsgTrade.TradeMsgType.Message:
				{
					//forward tradeMsg to target player
					this._playerIDArray[tradeMsg.Trade.AskedPartyID].SendTradeMsg(tradeMsg);
					break;
				}
			}


		}
		#endregion
		#region Utility Functions
		private void PrepBoard()
		{
			if(_model == null)
				return;
			AutoScrollMinSize = _mapBoard.MapPixelSize;
		}
		private void InitializePlayers()
		{
			GameData data = _model.Update();
			foreach(ClientHost client in _clientHostArray)
			{
				if(client != null)
					client.Init(data);
			}
		}
		private void CheckReady()
		{
			bool ready = true;
			foreach(ClientHost client in _clientHostArray)
			{
				if(client != null && client.Ready == false)
					ready = false;
			}
			if(ready)
				NextTurn();
		}
		private void NextTurn()
		{

			
			foreach(ClientHost client in _clientHostArray)
			{
				if(client != null)
					client.SendUpdate();
			}
			_model.NewOrders(ClientHost._orders);
			_gameData = _model.Update();
			this.InitView();
		}
		private void InitView()
		{
			Point offset = Point.Empty;
			int size = 25;
			if(_mapBoard != null)
			{
				offset = _mapBoard.Offset;
				size = _mapBoard.HexSize;
			}
			_mapBoard = new MapBoard(_gameData, -1);
			_mapBoard.Offset = offset;
			_mapBoard.HexSize = size;
			this.PrepBoard();
			this.Invalidate();
		}
		#endregion		

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WormholeServer));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.miGame = new System.Windows.Forms.MenuItem();
			this.miNewGame = new System.Windows.Forms.MenuItem();
			this.miOpenGame = new System.Windows.Forms.MenuItem();
			this.miSaveGame = new System.Windows.Forms.MenuItem();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.miGame});
			// 
			// miGame
			// 
			this.miGame.Index = 0;
			this.miGame.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.miNewGame,
																				   this.miOpenGame,
																				   this.miSaveGame});
			this.miGame.Text = "&Game";
			// 
			// miNewGame
			// 
			this.miNewGame.Index = 0;
			this.miNewGame.Text = "&New Game";
			this.miNewGame.Click += new System.EventHandler(this.miNewGame_Click);
			// 
			// miOpenGame
			// 
			this.miOpenGame.Index = 1;
			this.miOpenGame.Text = "&Open Saved Game";
			this.miOpenGame.Click += new System.EventHandler(this.miOpenGame_Click);
			// 
			// miSaveGame
			// 
			this.miSaveGame.Enabled = false;
			this.miSaveGame.Index = 2;
			this.miSaveGame.Text = "&Save Current Game";
			this.miSaveGame.Click += new System.EventHandler(this.miSaveGame_Click);
			// 
			// WormholeServer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(792, 553);
			this.Menu = this.mainMenu1;
			this.Name = "WormholeServer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Wormhole Server";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.WormholeServer_Closing);

		}
		#endregion

		#region Main

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run( new WormholeServer() );
		}
		#endregion
		#region Menu Handlers
		private void miNewGame_Click(object sender, System.EventArgs e)
		{
			if(_model != null)
			{
				DialogResult result = MessageBox.Show(this,"Do you wish to discard the current game?" + 
					"\n(changes since last save will be lost)", "Confirm", MessageBoxButtons.OKCancel);
				if(result != DialogResult.OK)
					return;
			}

			//create the dialog and show it
			NewGameDialog dlg = new NewGameDialog();
			dlg.ShowDialog(this);
			if(dlg.DialogResult != DialogResult.OK)
				return;

			int NumberOfPlayers = dlg.NumberOfPlayers;
			int SizeOfMap = dlg.SizeOfMap;

			if(NumberOfPlayers > _numClients)
			{
				MessageBox.Show("Not enough players are connected");
				return;
			}

					
			//Create new game with parameters from the New Game dialog
			_model = new GameModel(NumberOfPlayers,SizeOfMap,SizeOfMap,(SizeOfMap)*(SizeOfMap)/17);
			//number of systems, we'll get 
			//this from the new game 
			//dialog eventually
			_mapBoard = new MapBoard(_model.Update(), -1 );
			_mapBoard.HexSize = ClientSize.Width/(SizeOfMap * 2);
		
			InitializePlayers();

			//miNextTurn.Text = "&Next Turn!";
			miSaveGame.Enabled = true;
			PrepBoard();
			Invalidate();

		}
		private void miNextTurn_Click(object sender, System.EventArgs e)
		{
			/*
			if(_model == null ) 
				return;
			//Transfer players latest order to model
			for(int i = 0; i < _playerViewList.Count; i++)
			{
				Orders orders = _playerViewList[i].LatestOrders;
				_model.NewOrders(orders, i);
			}
			//Tell the model to process them
			_model.TurnOver();
			//Tell the playerviews to update from the model
			for(int i = 0; i < _playerViewList.Count; i++)
			{
				_playerViewList[i].UpdateView(_model.Update());//model.Update provides a 
				_playerViewList[i].Invalidate();			   //gamedata object for updating
			}
			//record our scroll and zoom status, so we don't jump on update
			Point offset = _mapBoard.Offset;
			int size = _mapBoard.HexSize;
			//Update the map for the controller view
			_mapBoard = new MapBoard(_model.Update(), -1 );
			_mapBoard.Offset = offset;
			_mapBoard.HexSize = size;
			Invalidate();*/
		}
		private void miOpenGame_Click(object sender, System.EventArgs e)
		{
			if(_model != null)
			{
				DialogResult result = MessageBox.Show(this,"Do you wish to discard the current game?" + 
					"\n(changes since last save will be lost)", "Confirm", MessageBoxButtons.OKCancel);
				if(result != DialogResult.OK)
					return;
			}

			GameData gameData = null;

			Stream stream;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			
			openFileDialog.Filter = "Wormhole Saves(*.whs)|*.whs" ;
			openFileDialog.RestoreDirectory = true ;

			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if((stream = openFileDialog.OpenFile())!= null)
				{
					IFormatter formatter = new BinaryFormatter();
					gameData = (GameData) formatter.Deserialize(stream);			
					stream.Close();
					if(gameData == null)
						return;
				}
			}
			else
				return;
	
			int NumberOfPlayers = gameData.PlayerList.Count;
					
			//Create new game with parameters from the New Game dialog
			_model = new GameModel(gameData);
			//number of systems, we'll get 
			//this from the new game 
			//dialog eventually
			_mapBoard = new MapBoard(_model.Update(), -1 );
			_mapBoard.HexSize = ClientSize.Width/(gameData.DataMap.Width * 2);
			InitializePlayers();

			//miNextTurn.Text = "&Next Turn!";
			miSaveGame.Enabled = true;
			PrepBoard();
			Invalidate();	
		}

		private void miSaveGame_Click(object sender, System.EventArgs e)
		{
			Stream stream ;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
 
			saveFileDialog.Filter = "Wormhole Saves(*.whs)|*.whs";
			saveFileDialog.RestoreDirectory = true ;

			GameData gameData = _model.Update();
 
			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if((stream = saveFileDialog.OpenFile()) != null)
				{
					IFormatter formatter = new BinaryFormatter();
					formatter.Serialize(stream, gameData);
					stream.Close();
				}
			}
		}
		#endregion
	}
}
	
