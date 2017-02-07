using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Text;
using System.Data;
using System.Collections;
using WormholeClient.Data;
using Messages;

namespace WormholeServer
{
	//	Public delegates
	//		- these are used to hook up the Client objects(server side) with the 
	//		ServerView class. The Server registers to Client with delegates
	//		matching these signatures, i.e. these act as wrappers for the
	//		Servers callback functions that wish to handle the matching

	//	Client events
	public delegate void ConnectDelegate(object sender, EventArgs e);
	public delegate void DisconnectDelegate(object sender, EventArgs e);
	public delegate void MessageDelegate(object sender, MessageEventArgs e);
	public delegate void ReadyDelegate(object sender, EventArgs e);
	public delegate void IDRequestDelegate(object sender, IDRequestEventArgs e);
	public delegate void RenameSystemDelegate(object sender, SystemNameEventArgs e);
	public delegate void TradeMessageDelegate(object sender, TradeMessageEventArgs e);

	/// <summary>
	/// The basic object representing each client connected to the server
	/// </summary>
	public class ClientHost
	{
		//
		//	Events for server notification of client activities
		//
		public event ConnectDelegate eConnected;
		public event DisconnectDelegate eDisconnected;
		public event MessageDelegate eMessageReceived;
		public event ReadyDelegate eReady;
		public event IDRequestDelegate eIDRequest;
		public event RenameSystemDelegate eRenameSystem;
		public event TradeMessageDelegate eTradeMessage;

		#region Private Fields
		
		public static ArrayList		_orders = new ArrayList(8);
		private Fleet				_fleet;
		private bool				_ready = false;

		private int					_clientID;
		private int					_playerID = -1;
		private bool				_connected = false;
		private bool				_firstTime = true;
		//byte counters for message retrieval
		private int					_msgBytesExpected = 0;
		private int					_msgBytesReceived;
		//storage buffers for message processing
		private byte[]				_headerBytes = new Byte[8];
		private byte[]				_msgBytes  = null;
		private MsgType				_msgType;

		private string 				_userName = null;
	
		private TcpClient			_clientSocket;

		#endregion
		#region Properties
		public int PlayerID
		{
			get {return _playerID;}
			set {_playerID = value;}
		}
		public bool Ready
		{
			get{return _ready;}
		}
		public string UserName
		{
			get{return _userName;}
			set{_userName = value;}
		}

		public int ID
		{
			get{return _clientID;}
			set{_clientID = value;}
		}

		#endregion
		#region Constructors and Initialization
		public ClientHost(TcpClient client)
		{
			_clientSocket = client;	
		}
		#endregion
		#region Methods
		public void Connect()
		{
			//Start Receiving the Messages
			AsyncCallback GetStreamHeaderCallback = new AsyncCallback(GetStreamHeader);
			_clientSocket.GetStream().BeginRead(_headerBytes,0,8,GetStreamHeaderCallback,null);
			//go get the first msg header
		}
		public void Ping()
		{

			try
			{
				lock(_clientSocket.GetStream())
				{
					NetworkStream str = _clientSocket.GetStream();
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

			
		public void SendID(int id, string name)
		{
			NameAndID nameid = new NameAndID(id, name);
	
			lock(_clientSocket.GetStream())
			{
				//Get stream
				NetworkStream str = _clientSocket.GetStream();
				IFormatter fmt = new BinaryFormatter();
				MemoryStream memStr = new MemoryStream();
				//Serialize object to the memorystream for sizing
				fmt.Serialize(memStr,nameid);
				byte[] data = memStr.GetBuffer();
				//get header info in bytes
				int memSize = (int)memStr.Length;
				byte[] size = BitConverter.GetBytes(memSize);	
				byte[] type = BitConverter.GetBytes((int)MsgType.PlayerID);
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
 
		/// <summary>
		/// Send a text message to a player chat screen
		/// </summary>
		/// <param name="msg"></param>
		public void SendText(string msg)
		{
			MsgText msgText = new MsgText(msg);
			lock(_clientSocket.GetStream())
			{
				//Get stream
				NetworkStream str = _clientSocket.GetStream();
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
				Console.WriteLine("reply from server sent: " + msgText.Text);
				
				//send header
				str.Write(header,0,8);
				//send data
				str.Write(data,0,memSize);
				str.Flush();
				memStr.Close();

			}
		}
		public void SendSystemName(MsgSystemName sysName)
		{
			lock(_clientSocket.GetStream())
			{
				//Get stream
				NetworkStream str = _clientSocket.GetStream();
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
		/// <summary>
		/// Initializes Client Views with GameData
		/// </summary>
		/// <param name="data"></param>
		public void Init(GameData gameData)
		{
			//send new game info to a client
			lock(_clientSocket.GetStream())
			{
				//Get stream
				NetworkStream str = _clientSocket.GetStream();
				IFormatter fmt = new BinaryFormatter();
				MemoryStream memStr = new MemoryStream();
				//Serialize object to the memorystream for sizing
				fmt.Serialize(memStr,gameData);
				byte[] data = memStr.GetBuffer();
				//get header info in bytes
				int memSize = (int)memStr.Length;
				byte[] size = BitConverter.GetBytes(memSize);	
				byte[] type = BitConverter.GetBytes((int)MsgType.Init);
				byte[] header = new Byte[8];
				size.CopyTo(header,0);
				type.CopyTo(header,4);
				
				//send header
				str.Write(header,0,8);
				//send data
				str.Write(data,0,memSize);
				//
				//	Send ID
				//
				//reset type
				type = BitConverter.GetBytes((int)MsgType.ID);
				size = BitConverter.GetBytes(4);
				data = BitConverter.GetBytes(_clientID);
				size.CopyTo(header,0);
				type.CopyTo(header,4);
				//send header
				str.Write(header,0,8);
				//send data
				str.Write(data,0,4);

				str.Flush();
				memStr.Close();

			}

		}
		/// <summary>
		/// Sends latest collated orders to client
		/// </summary>
		public void SendUpdate()
		{
			lock(_clientSocket.GetStream())
			{
				//Get stream
				NetworkStream str = _clientSocket.GetStream();
				IFormatter fmt = new BinaryFormatter();
				MemoryStream memStr = new MemoryStream();
				//Serialize object to the memorystream for sizing
				fmt.Serialize(memStr,ClientHost._orders);
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
				this._ready = false;
			}
			
		}
		public void Disconnect()
		{
			this._connected=false;
			_clientSocket.Close();
			Console.WriteLine("Client socket for slot " + _clientID + " closing, disconnecting");
		}
	
		public void SendTradeMsg(MsgTrade tradeMsg)
		{
			try
			{
				lock(_clientSocket.GetStream())
				{
					//Get stream
					NetworkStream str = _clientSocket.GetStream();
					IFormatter fmt = new BinaryFormatter();
					MemoryStream memStr = new MemoryStream();
					//Serialize object to the memorystream for sizing
					fmt.Serialize(memStr,tradeMsg);
					byte[] data = memStr.GetBuffer();
					//get header info in bytes
					int memSize = (int)memStr.Length;
					byte[] size = BitConverter.GetBytes(memSize);	
					byte[] type = BitConverter.GetBytes((int)MsgType.Trade);
					byte[] header = new Byte[8];
					size.CopyTo(header,0);
					type.CopyTo(header,4);
					Console.WriteLine("memsize = "+memSize);
					
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

		#endregion
		#region Utilities
		/// <summary>
		/// Handler for the header AsyncCallback. The strategy will be to precede every
		/// message from the client with a 8 byte header that will be converted
		/// to an int describing the size in bytes of the following msg and another int 
		/// for the message type. A second asynchronous call will follow with a buffer 
		/// equal to the size of the expected message, with repeated calls if necessary 
		/// til the message is complete. The GetMessage method will the restart the process 
		/// with another call to listen for the next header.
		/// </summary>
		/// <param name="ar"></param>
		private void GetStreamHeader(IAsyncResult ar)
		{
			//
			// Get the info from the header and restart the BeginRead
			// until the entire object is retrieved, then get next header
			// 
			_msgType = MsgType.Invalid;
			int intCount;
			try
			{
				//Lock the Client Stream
				lock(_clientSocket.GetStream())
				{
					if(_clientSocket == null)
					{
						Console.WriteLine("_clientSocket is null");
						return;
					}
					intCount = _clientSocket.GetStream().EndRead(ar);
				}
				Console.WriteLine("intCount in header = " + intCount);
				if(intCount<8)
				{
					//If a value less than 8 received that means that 
					//client disconnected or this is not a valid header
					_clientSocket.Close();
					Console.WriteLine("socket for " + _clientID + " closing in GetStreamHeader:: intCount = " + intCount);
					//raise the Disconnected Event
					if(eDisconnected!=null)
					{	
						EventArgs e = new EventArgs();
						eDisconnected(this, e);
					}
				}
				//store the size of expected object
				_msgBytesExpected = BitConverter.ToInt32(_headerBytes,0);
				_msgType = (MsgType)BitConverter.ToInt32(_headerBytes,4);
				//reset the amount msg byte counter
				_msgBytesReceived = 0;
	
				//create a new buffer for expected message
				_msgBytes = new Byte[_msgBytesExpected];
				//go get it
				lock(_clientSocket.GetStream())
				{
					AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
					_clientSocket.GetStream().BeginRead(_msgBytes,0,_msgBytesExpected,GetStreamMsgCallback,null);
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("Exception thrown from GetStreamHeader" + ex);
				if(_clientSocket != null)
				{
					_clientSocket.Close();
					Console.WriteLine("socket for " + _clientID + " closing in GetStreamHeader");
				}
				if(eDisconnected!=null)
				{
					EventArgs e = new EventArgs();
					eDisconnected(this, e);
				}
			}
		}

		private void GetStreamMsg(IAsyncResult ar)
		{
			int intCount = 0;

			try
			{
				lock(_clientSocket.GetStream())
				{
					intCount = _clientSocket.GetStream().EndRead(ar);
					Console.WriteLine("EndRead called, receiving " + intCount 
												+ " bytes, "+ _msgBytesExpected + 
													" expected");
				}
				
				if(intCount < 1)
				{
					//If a value less than 1 received that means that 
					//client disconnected
					_clientSocket.Close();
					Console.WriteLine("socket for slot " + _clientID + " closing Get Stream MSG");
					//raise the Disconnected Event
					if(eDisconnected!=null)
					{	
						EventArgs e = new EventArgs();
						eDisconnected(this, e);
					}
				}

				_msgBytesReceived += intCount;
				_msgBytesExpected -= intCount;
				//if we don't have the whole message
				if(_msgBytesExpected > 0)
				{
					Console.WriteLine("Need more bytes");
					//get the next packet and append it to our buffer
					lock(_clientSocket.GetStream())
					{
						AsyncCallback GetStreamMsgCallback = new AsyncCallback(GetStreamMsg);
						_clientSocket.GetStream().BeginRead(_msgBytes,_msgBytesReceived,_msgBytesExpected,
							GetStreamMsgCallback,null);
					}
					Console.WriteLine("read more message bytes " + _msgBytesExpected + " left");
					
				}
				else
				{
					//otherwise, get the next header
					lock(_clientSocket.GetStream())
					{
						AsyncCallback GetStreamHeaderCallback = new AsyncCallback(GetStreamHeader);
						_clientSocket.GetStream().BeginRead(_headerBytes,0,8,GetStreamHeaderCallback,null);
					}
					Console.WriteLine("host listening for next header");
					ProcessMessage(_msgBytes);
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("Exception thrown from Get message " + ex + " //  socket closing");
				_clientSocket.Close();
				if(eDisconnected!=null)
				{
					EventArgs e = new EventArgs();
					eDisconnected(this, e);
				}
			}
		}

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
					Console.WriteLine("Pong");
					break;
				}
				case MsgType.Text:
				{
					MsgText msg = (MsgText)fmt.Deserialize(memHolder);
					//if this is the first message assign it to client name
					if(_firstTime)
					{
						_userName = msg.Text;
						_firstTime = false;
						_connected = true;
						if(eConnected!=null)
						{
							EventArgs e = new EventArgs();
							eConnected(this, e);
						}
					}
					else if(eMessageReceived != null && _connected)
					{
						//Else raise the MessageReceived Event 
						//and pass the message along
						MessageEventArgs e = new MessageEventArgs(msg);
						eMessageReceived(this,e);
					}
					break;
				}
				case MsgType.Orders:
				{
					//
					//	TODO: make sure it isn't possible to get more
					//     than one set of orders in the list for a 
					//		single player
					// 

					//accept fleet data from player and add to 
					//arraylist of fleetdata
					if(_fleet != null && _orders.Contains(_fleet))
					{
						Console.WriteLine("Old fleet orders removed for player " + this._clientID);
						_orders.Remove(_fleet);
					}
					Console.WriteLine("Attempting to Deserialize Orders from player");
					this._fleet = (Fleet)fmt.Deserialize(memHolder);
					ClientHost._orders.Add(_fleet);
					Console.WriteLine("Fleet added to server orders");
					this._ready = true;
					if(eReady != null)
					{
						EventArgs e = new EventArgs();
						eReady(this,e);
					}
					break;
				}
				case MsgType.Cancel:
				{
					ClientHost._orders.Remove(this._fleet);
					Console.WriteLine("Fleet orders removed for player " + this._clientID);
					this._ready = false;
					break;
				}
				case MsgType.PlayerIDRequest:
				{
					NameAndID nameAndID = (NameAndID)fmt.Deserialize(memHolder);
					int id = nameAndID.ID;
					string password = nameAndID.Password;
					if(eIDRequest != null)
					{ 
						Console.WriteLine(_userName + " requests player ID(event fired)");
						IDRequestEventArgs e = new IDRequestEventArgs(id,password);
						eIDRequest(this,e);
					}
					break;
				}
				case MsgType.SystemName:
				{
					MsgSystemName sysName = (MsgSystemName)fmt.Deserialize(memHolder);
					if(this.eRenameSystem != null)
					{
						SystemNameEventArgs mea = new SystemNameEventArgs(sysName);
						this.eRenameSystem(this,mea);
					}
					break;
				}
				case MsgType.Trade:
				{
					MsgTrade tradeMsg = (MsgTrade)fmt.Deserialize(memHolder);
					if(this.eTradeMessage != null)
					{
						TradeMessageEventArgs ea = new TradeMessageEventArgs(tradeMsg);
						this.eTradeMessage(this,ea);
					}
					break;
				}

			}
			memHolder.Close();
			//Determine message type
			//Carry out message instructions
		}
		#endregion
	}


	/// <summary>
	/// Custon Event Args for messages
	/// Expand this to include game update and requests
	/// </summary>
	public class MessageEventArgs : EventArgs
	{
		private MsgText _msg;
		public MessageEventArgs(MsgText msg)
		{
			_msg = msg;
		}
		public MsgText Message
		{
			get{return _msg;}
			set{_msg = value;}	
		}
	}
	/// <summary>
	/// Request for player ID assignment
	/// </summary>
	public class IDRequestEventArgs : EventArgs
	{
		//index into PlayerList array in gameData
		private int _id;
		private string _password;
		public string Password
		{
			get{return _password;}
		}
		public int ID
		{
			get{return _id;}
		}
		public IDRequestEventArgs(int id, string password)
		{
			_id = id;
			_password = password;
		}
	}
	public class SystemNameEventArgs : EventArgs
	{
		private MsgSystemName _sysName;
		public MsgSystemName SysName
		{
			get{return _sysName;}
		}
		public SystemNameEventArgs(MsgSystemName sysName)
		{
			_sysName = sysName;
		}
	}
	public class TradeMessageEventArgs : EventArgs
	{
		private MsgTrade _tradeMsg;
		public MsgTrade TradeMsg
		{
			get{return _tradeMsg;}
		}
		public TradeMessageEventArgs(MsgTrade tradeMsg)
		{
			_tradeMsg = tradeMsg;
		}
	}
}

