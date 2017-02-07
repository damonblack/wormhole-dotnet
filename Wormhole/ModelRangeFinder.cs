using System;
using System.Drawing;
using System.Collections;


namespace WormholeClient.Data
{
	/// <summary>
	/// Class for determining legal moves for a given ship.
	/// Accepts mapsize in constructor
	/// </summary>
	public class RangeFinder
	{
		#region Private Fields
		private DataHex[,]			_hexArray;
		#endregion
		#region Properties
		#endregion
		#region Constructors and Initialization
		public RangeFinder(DataHex [,] hexArray)
		{
			_hexArray =  hexArray;
		}
		#endregion
		#region Methods
		public ArrayList Find(int X, int Y, int range)
		{
			ArrayList list = new ArrayList();
			int beg = X - range;
			int end = X + range;
			for(int y = 0; y <= range; y++)
			{
				for(int x = beg; x <= end; x++)
				{
					if(x<0||x>_hexArray.GetUpperBound(0))
						continue;
					if(!(Y-y<0))
					{
						if(!list.Contains(_hexArray[x,Y-y]))
							list.Add(_hexArray[x,Y-y]);
					}
					if(!(Y+y>_hexArray.GetUpperBound(1)))
					{
						if(!list.Contains(_hexArray[x,Y+y]))
							list.Add(_hexArray[x,Y+y]);
					}
				}
				if(Y%2==0)
				{
					if(y%2==0)
						end--;
					else
						beg++;
				}
				else
				{
					if(y%2==0)
						beg++;
					else
						end--;
				}
			}
			return list;
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


