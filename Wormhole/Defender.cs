using System;
using System.Drawing;

namespace WormholeClient.Data
{
	/// <summary>
	/// Summary description for Defender.
	/// </summary>
	[Serializable]	 
	public class Defender:Ship
	{
		#region Constructors and Initialization
		public Defender( int ownerID, Point homeHex):base( ownerID, homeHex)
		{
            _type = ShipType.Defender;
			_range = BaseRange.Defender;
			_power = BasePower.Defender;
			_defaultTask = Task.Survey;
			_currentTask = Task.Survey;
			_mineralUsed =(int) MineralSupport.Defender;
			_organicUsed = (int)OrganicSupport.Defender;
			_energyUsed = (int)EnergySupport.Defender;
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
					sysLog.AddInfo(dataMap[this.HexLocation.X,this.HexLocation.Y]);
					this._acted = true;
					break;
				}
			}
		}
		#endregion
	}
}
