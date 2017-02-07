using System;
using System.Drawing;


namespace WormholeClient.Data
{
	/// <summary>
	/// Summary description for Scout.
	/// </summary>
	[Serializable]
	public class Scout:Ship
	{
		#region Private Fields
		
		#endregion
		#region Properties
	
		#endregion
		#region Constructors and Initialization
		public Scout( int ownerID, Point homeHex):base( ownerID, homeHex)
		{
			_type = ShipType.Scout;
			_range = BaseRange.Scout;
			_power = BasePower.Scout;
			_defaultTask = Task.Survey;
			_currentTask = _defaultTask;
			_mineralUsed = (int)MineralSupport.Scout;
			_organicUsed = (int)OrganicSupport.Scout;
			_energyUsed = (int)EnergySupport.Scout;
		}
		#endregion
		#region Methods
		public override void DoTask(ResourcePool resPool, SystemLog sysLog,DataMap dataMap)
		{
			switch(_currentTask)
			{
				case Task.Survey:
				{
					sysLog.AddInfo(dataMap[this.HexLocation.X,this.HexLocation.Y]);
					this._acted = true;
					break;
				}
				case Task.Move:
				{
					break;
				}
				default:
				{
					this._acted = true;
					sysLog.AddInfo(dataMap[this.HexLocation.X,this.HexLocation.Y]);
					break;
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
