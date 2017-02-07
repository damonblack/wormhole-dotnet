using System;
using System.Drawing;

namespace WormholeClient.Data
{
	/// <summary>
	/// DataHex. basic data structure for game
	/// </summary>
	[Serializable]
	public class DataHex
	{
		#region Private Fields
		/// <summary>
		/// StarSystem data assigned at game creation by DataMap
		/// </summary>
		protected StarSystem		_system = null;
		/// <summary>
		/// map index location
		/// assigned at creation
		/// </summary>
		protected Point				_index;
		#endregion
		#region Properties
		public Point Index
		{
			get{return _index;}
		}
			public StarSystem System
		{
			get{ return _system; }
			set{ _system = value; }
		}
		#endregion
		#region Constructors and Initialization
		/// <summary>
		/// made this public so Hex could use it as copy constructor
		/// </summary>
		public DataHex(){}
		public DataHex(int x, int y)
		{
			_index = new Point(x,y);
			//Console.Write("DataHex created at index " + x + "," + y + "  ");
		}
		#endregion
	}
}
