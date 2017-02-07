using System;
using System.Drawing;
using WormholeClient;

namespace WormholeClient.Data
{
	/// <summary>
	/// Summary description for Player.
	/// </summary>
	[Serializable]
	public class Player
	{
		#region Private Fields
		/// <summary>
		/// ID number of player, index into playerlist
		/// </summary>
		private int				_id;
		/// <summary>
		/// Player name as entered by player on first turn
		/// </summary>
		private string			_name = "Open";
		private string			_password = "";
		/// <summary>
		/// Fleet of ships controlled by player
		/// </summary>
		private Fleet			_fleet;
		/// <summary>
		/// Resources collected by the player currently
		/// </summary>
		private ResourcePool	_resourcePool = new ResourcePool();
		private SystemLog		_systemLog = null;
		#endregion
		#region Properties
		public Fleet Fleet
		{
			get{ return _fleet;}
			set{ _fleet = value; }
		}
		public int ID
		{
			get{ return _id; }
			set{ _id = value; }
		}
		public string Name
		{
			get{ return _name; }
			set{_name = value;}
		}
		public ResourcePool ResourcePool
		{
			get{return _resourcePool;}
		}
		public SystemLog SystemLog
		{
			get{return _systemLog;}
		}
		public string Password
		{
			get{return _password;}
			set{_password = value;}
		}

		#endregion 
		#region Constructors and Initialization
		public Player(int id,Point wormhole)
		{
			_id = id;
			_fleet = new Fleet(_id, wormhole);
		}
		#endregion
		#region Methods
		public void GenLog(SystemList sysList, int ID)
		{
			_systemLog = new SystemLog(sysList,ID);
		}

		#endregion
		#region Events
		#endregion
		#region EventHandlers
		#endregion
		#region Utility Functions
		#endregion
	}
}
