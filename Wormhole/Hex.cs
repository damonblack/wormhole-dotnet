using System;
using System.Drawing;
using System.Windows.Forms;

namespace WormholeClient.Data
{
	/// <summary>
	/// Basic graphic unit for map. Initialized each turn with 
	/// data from model.  
	/// </summary>
	public class Hex:DataHex
	{
		#region Private Fields
		private Token			_systemToken = null;
		
		/// <summary>
		/// List of local Tokens
		/// Hex will be responsible for updating locations
		/// on zoom or scroll.
		/// </summary>
		private TokenList		_tokenList = new TokenList();
		/// <summary>
		/// Default size
		/// </summary>
		private  int			_size;
		/// <summary>
		/// Base hex color
		/// </summary>
		private static Pen		_pen = new Pen(Color.DarkSlateGray,0);
		/// <summary>
		/// highlighted hex color
		/// </summary>
		private static Pen		_hlpen = new Pen(Color.SlateGray,0);
		private bool			_highlighted = false;
		/// <summary>
		/// pixel location of hex center, client area coordinates
		/// </summary>
		private Point			_center = Point.Empty;
		/// <summary>
		/// Corner Points for Hexagon
		/// </summary>
		private Point[]		_aPoints = new Point[7];
		private Point[]     _aTokenAnchors = new Point[8];
		#endregion
		#region Properties
		public Token SystemToken
		{
			get{return _systemToken;}
			set{_systemToken = value;}
		}
		public new Point Index
		{
			get{return _index;}
			set{_index = value;}
		}
		public TokenList TokenList
		{
			get{return _tokenList;}
		}
		public StarSystem StarSystem
		{
			get { return _system; } 
		}
		/// <summary>
		/// Sets/gets size of hexes. Minimum 10 pixels.
		/// </summary>
		public  int HexSize
		{
			set
			{
				_size = value;
				SetPoints();
			}
			get{ return _size; }
		}
		/// <summary>
		/// Gets/Sets Center. Set triggers resetting corner points.
		/// </summary>
		public Point Center
		{
			get
			{
				return _center;
			}
			set
			{
				_center = value;
				SetPoints();
			}
		}
		/// <summary>
		/// gets or sets highlight
		/// </summary>
		public bool Highlighted
		{
			get{ return _highlighted; }
			set{ _highlighted = value;}
		}
		#endregion
		#region Constructors and Initialization
		/// <summary>
		/// Default constructor
		/// </summary>
		private Hex()
		{
		}
		/// <summary>
		/// Create Hex centered at pixel location x,y
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Hex(int x, int y):base(x,y)
		{
			Center = new Point(x,y);					
		}
		/// <summary>
		/// Construct from Model info
		/// </summary>
		/// <param name="dataHex"></param>
		public Hex(DataHex dataHex)
		{
			_system = dataHex.System;
		}
		#endregion
		#region Methods
		/// <summary>
		/// uhh... guess
		/// </summary>
		/// <param name="e"></param>
		public void Draw(Graphics e)
		{
			if(_highlighted)
                e.DrawLines(_hlpen,_aPoints);
			else
				e.DrawLines(_pen,_aPoints);
			
		}
		/// <summary>
		/// Resets corner points after zoom or scroll
		/// </summary>
		public void SetPoints()
		{
			int x = _center.X;
			int y = _center.Y;
			_aPoints[0] = new Point(x -_size, y -_size/2);
			_aPoints[1] = new Point(x, y -_size);
			_aPoints[2] = new Point(x +_size, y -_size/2);
			_aPoints[3] = new Point(x +_size, y +_size/2);
			_aPoints[4] = new Point(x, y +_size);
			_aPoints[5] = new Point(x -_size, y +_size/2);
			_aPoints[6] = _aPoints[0];

			_aTokenAnchors[0]=new Point(x,y-_size/2);
			_aTokenAnchors[1]=new Point(x,y+_size/2);
			_aTokenAnchors[2]=new Point(x+(5*_size)/8,y);
			_aTokenAnchors[3]=new Point(x-(5*_size)/8,y);
			_aTokenAnchors[4]=new Point(x+(3*_size)/8,y-(3*_size)/8);
			_aTokenAnchors[5]=new Point(x-(3*_size)/8,y+(3*_size)/8);
			_aTokenAnchors[6]=new Point(x+(3*_size)/8,y+(3*_size)/8);
			_aTokenAnchors[7]=new Point(x-(3*_size)/8,y-(3*_size)/8);

			if(_systemToken != null)
				_systemToken.Location = new Point(_center.X - _systemToken.Width/2,
													_center.Y - _systemToken.Height/2);

			if(_tokenList != null)
			{
				for(int i = 0; i < _tokenList.Count; i++)
				{
					Token token = _tokenList[i];
					token.Location = new Point(_aTokenAnchors[i%8].X - token.Width/2,
												_aTokenAnchors[i%8].Y - token.Height/2);
				}
			}
		}
		#endregion
		#region Utility Functions		
		#endregion
	}
}
