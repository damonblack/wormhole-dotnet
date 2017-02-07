using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using WormholeClient.Data;

namespace WormholeClient
{
	/// <summary>
	/// Summary description for SystemLog.
	/// </summary>
	[Serializable] 
	public class SystemLog
	{
		#region Private Fields
		private int						_playerID;
		private SystemInfo[]			_aSysInfo;
		#endregion
		#region Properties
		public int Count
		{
			get{return _aSysInfo.GetUpperBound(0) + 1;}
		}
		public SystemInfo this[int index]
		{
			get
			{
				return _aSysInfo[index];
			}
		}
		#endregion
		#region Constructors and Initialization
		public SystemLog(SystemList sysList, int ID)
		{
			_playerID = ID;
			_aSysInfo = new SystemInfo[sysList.Count];
			for(int i = 0; i < sysList.Count; i++)
			{
				_aSysInfo[i] = new SystemInfo(sysList[i].MapLocation,sysList[i].Type);
			}
		}
		#endregion
		#region Methods
		public void AddInfo(DataHex dataHex)
		{
			StarSystem system = dataHex.System;
			if(system == null)
			{
				return;
			}
			_aSysInfo[system.Index].AddResourceInfo(system);
		}
		public void Update(SystemList sysList)
		{
			for( int i = 0; i < sysList.Count; i++ )
			{
				int infPercentage = sysList[i].GetInfluencePercentage(_playerID);
				_aSysInfo[i].InfPercent = infPercentage;
				if(!sysList[i].HasCollected(_playerID))
				{
					_aSysInfo[i].ClearCollectedResources();
				}

			}
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
