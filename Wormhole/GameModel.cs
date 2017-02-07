using System;
using System.Drawing;
using System.Collections;
using WormholeClient.Data;

namespace WormholeClient
{
	/// <summary>
	/// Class to manage all persisitent data for the game.
	/// Should be able to accept orders from players and referee 
	/// turns, returning updated map info to view class(es). This 
	/// should be stored independent of graphical representation.
	/// </summary>
	public class GameModel
	{
		#region Private Fields
		private GameData			_gameData;
		private ArrayList			_orders;
		#endregion
		#region Properties
		#endregion
		#region Constructors and Initialization
		private GameModel(){}
		/// <summary>
		/// Create a new game
		/// </summary>
		/// <param name="players"></param>
		/// <param name="mapWidth"></param>
		/// <param name="mapHeight"></param>
		/// <param name="systems"></param>
		public GameModel(int players, int mapWidth, int mapHeight, int systems)
		{
			Point wormhole = new Point(mapWidth/2, mapHeight/2);
			SystemList systemList = new SystemList(systems,players);
			PlayerList playerList = new PlayerList(players,wormhole);
			DataMap dataMap = new DataMap(mapWidth, mapHeight, systemList, wormhole);
			_gameData = new GameData( dataMap, systemList, playerList);
			playerList.GenLogs(systemList);
			
		}
		/// <summary>
		/// Create a game from previously saved GameData object
		/// </summary>
		/// <param name="gameData"></param>
		public GameModel(GameData gameData) 
		{
			_gameData = gameData;
		}

		#endregion
		#region Methods
		public GameData Update()
		{
			return _gameData;
		}
		public void NewOrders(ArrayList orders)
		{
			_orders = orders;
			ProcessTurn();
		}
		public void AssignName(string name, int playerIndex)
		{
			_gameData.PlayerList[playerIndex].Name = name;
		}
		#endregion
		#region Utility Functions
		/// <summary>
		/// Process player orders and update gamedata
		/// </summary>
		private void ProcessTurn()
		{
			_gameData.SetupTurn();
			_gameData.UpdateFleets(_orders);
			CalculateInfluence();
			PerformTasks(); 
			UpdateLogs();
			SupportFleets();
			//Update gamedata
		}
		/// <summary>
		/// Cycles through players fleets, distributing influence points
		/// within their respective ranges. Then triggers each system to 
		/// calculate if controlling influence has been established
		/// </summary>
		private void CalculateInfluence()
		{
			//Cycle through each Players Fleet, retrieving the range of hexes
			//affected and then giving the ship power to each system as influence
			int count = _gameData.PlayerList.Count;
			for(int ID = 0; ID < count; ID ++)
			{
				Fleet fleet = _gameData.PlayerList[ID].Fleet;
				RangeFinder influenceHexes = _gameData.DataMap.RangeFinder();
				for(int i = 0; i < fleet.Count; i++)
				{
					Ship ship = fleet[i];
					ArrayList hexes = influenceHexes.Find(ship.HexLocation.X,ship.HexLocation.Y,
						ship.Range);
					foreach(DataHex hex in hexes)
					{
						StarSystem system = hex.System;
						if(system != null && system.Type != StarSystem.SystemType.Empty)
						{
								system.AddInfluence(ID,ship.Power);
						}
					}
				}
			}
			for(int i = 0; i < _gameData.SystemList.Count; i++)
			{
				_gameData.SystemList[i].ResolveInfluence();
			}
		}
		/// <summary>
		/// Triggers each ship to determine assigned task and perform it.
		/// </summary>
		private void PerformTasks()
		{
			//Cycle through each fleet, performing task for each
			//ship as ordered.
			int count = _gameData.PlayerList.Count;
			for(int ID = 0; ID < count; ID ++)
			{
				Fleet fleet = _gameData.PlayerList[ID].Fleet;
				ResourcePool resPool= _gameData.PlayerList[ID].ResourcePool;
				//retrieve the system log and send it along to record info
				SystemLog sysLog = _gameData.PlayerList[ID].SystemLog;
				for(int i = 0; i < fleet.Count; i++)
				{
					fleet[i].DoTask(resPool,sysLog,_gameData.DataMap);
				}
			}
		}
		/// <summary>
		/// Updates player available information and manages resource
		/// usage.
		/// </summary>
		private void UpdateLogs()
		{
			SystemList sysList = _gameData.SystemList;
			int count = _gameData.PlayerList.Count;
			for(int i = 0; i < count; i++)
				_gameData.PlayerList[i].SystemLog.Update(sysList);


		}
		private void SupportFleets()
		{
			for(int i = 0; i < _gameData.PlayerList.Count; i++)
			{
				//for each player calculate support for each ship
				_gameData.PlayerList[i].Fleet.Support(_gameData.PlayerList[i].ResourcePool);
				for(int x = 0; x < _gameData.PlayerList[i].Fleet.Count; x++)
					//for each ship set acted to false, allowing new actions
					_gameData.PlayerList[i].Fleet[x].Acted = false;
			}
				
		}
		#endregion
	}
}
