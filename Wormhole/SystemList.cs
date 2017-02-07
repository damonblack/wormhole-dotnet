using System;

namespace WormholeClient.Data
{
	/// <summary>
	/// List of Systems in the region
	/// </summary>
	[Serializable]
	public class SystemList
	{
		#region Private Fields
		/// <summary>
		/// Total systems
		/// </summary>
		private int						_systemCount;
		/// <summary>	
		/// array holding systems
		/// </summary>
		private StarSystem[]			_systemArray;
		#endregion
		#region Properties
		public int Count
		{
			get{ return _systemArray.GetUpperBound(0)+1;}
		}
		public StarSystem this[int index]
		{
			get{return _systemArray[index];}
		}
		#endregion
		#region Constructors and Initialization
		/// <summary>
		/// SystemList created here, map seeded in ?
		/// </summary>
		/// <param name="systemCount"></param>
		public SystemList(int systemCount, int players)
		{
			_systemCount = systemCount;
			_systemArray = new StarSystem[systemCount];
			for(int i = 0; i < systemCount; i++)
				_systemArray[i]= new StarSystem(players,i);
		}
		#endregion
		#region Methods
		#endregion
		#region Events
		#endregion
		#region EventHandlers
		#endregion
		#region Utility Functions
		#endregion

	}
}
