using System;
using System.Drawing;

namespace WormholeClient.Data
{
	/// <summary>
	/// Summary description for MapObject.
	/// </summary>
	[Serializable]
	public class MapObject
	{
		#region Private Fields
		protected Point		_hexLocation;
		#endregion
		#region Properties
		public Point HomeHex
		{
			get{return _hexLocation;}
			set{_hexLocation = value;}
		}
		#endregion
		#region Constructors and Initialization
		public MapObject()
		{
			_hexLocation = new Point(0,0);
		}
		public MapObject(Point hexLocation)
		{
			_hexLocation = hexLocation;
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
