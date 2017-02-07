using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using WormholeClient;
namespace WormholeClient.Data
{
	/// <summary>
	/// Basic graphicl object on the map.
	/// (looking into using multiple images for scrolling, resizing
	/// at appropriate intervals)
	/// </summary>
	public class Token
	{
		#region Private Fields
		protected Point		_hexIndex;
		/// <summary>
		/// pixel client area location of upper left corner
		/// </summary>
		protected Point		_location;
		/// <summary>
		/// Loaded bitmap image for token
		/// </summary>
		protected Image		_icon;
		/// <summary>
		/// Bounding rectangle for token image
		/// </summary>
		protected Rectangle	_rect;
		/// <summary>
		/// smaller rectange for more precise hit testing
		/// </summary>
		protected Rectangle _hitRect;
		/// <summary>
		/// object the token is attached to
		/// </summary>
		private MapObject		_source;
		#endregion
		#region Properties
		public int Width
		{
			get{return _rect.Width;}
		}
		public int Height
		{
			get{return _rect.Height;}
		}
		/// <summary>
		/// Gets/sets current pixel client area location
		/// </summary>
		public Point Location
		{
			get{return _location;}
			set
			{
				_location = value;
				_rect = new Rectangle(_location,_icon.Size);
				_hitRect = new Rectangle(_location.X + _icon.Size.Width/3, _location.Y + _icon.Size.Height/3,
											_icon.Size.Width/3, _icon.Size.Height/3);
			}
		}
		public MapObject Source
		{
			get{ return _source; }
			set{ _source = value; }
		}
		public Point HomeHexIndex
		{
			get
			{
				return _source.HomeHex;
			}
		}
			
		#endregion
		#region Constructors and Initialization
		/// <summary>
		/// Accepts filename of image to load
		/// </summary>
		/// <param name="filename"></param>
		public Token(string filename, Point hexIndex)
		{
			try
			{
				Assembly thisAssembly = Assembly.GetAssembly(this.GetType());
				Stream imageStream = thisAssembly.GetManifestResourceStream("Wormhole.Resources."+filename);
				_icon = Image.FromStream(imageStream);

			}
			catch
			{
				MessageBox.Show("Cannot find "+ filename);
			}
			_location = new Point(0,0);//must be set at location later
										// by home hex.
			_hexIndex = hexIndex;
			_rect = new Rectangle(_location,_icon.Size);
			_hitRect = new Rectangle(_location.X + _icon.Size.Width/3, _location.Y + _icon.Size.Height/3,
				_icon.Size.Width/3, _icon.Size.Height/3);

		}
		/// <summary>
		/// Accepts filename of image and intial pixel client area location
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Token(string filename, int x, int y) //actual pixel location
		{											//Is this ever used??, if not, trash 
													//it, its confusing having two ways 
													//to initially locate a token
			try 
			{
				Assembly thisAssembly = Assembly.GetAssembly(this.GetType());
				Stream imageStream = thisAssembly.GetManifestResourceStream("Wormhole.Resources."+filename);
				_icon = Image.FromStream(imageStream);

			}
			catch
			{
				MessageBox.Show("Cannot find "+filename);
			}
			_location = new Point(x,y);
			_rect = new Rectangle(_location,_icon.Size);
			_hitRect = new Rectangle(_location.X + _icon.Size.Width/3,_location.Y +
				_icon.Size.Height/3,_icon.Size.Width/3,_icon.Size.Height/3);
		
		}
		#endregion
		#region Public Methods
		/// <summary>
		/// Hit test x,y pixel coordinates in bounding rectangle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public virtual bool HitTest(int x, int y)//pixel location
		{
			Point point = new Point(x,y);
			return _hitRect.Contains(point);		
		}
		/// <summary>
		/// Paints token image at current location
		/// </summary>
		/// <param name="e"></param>
		public void Draw(Graphics e)
		{
			e.DrawImage(_icon,_location.X,_location.Y);
			//Console.WriteLine("Draw called?");
		}
		public void Draw(Graphics e, Point point)
		{
			e.DrawImage(_icon, point.X - _icon.Width/2,
				point.Y - _icon.Height/2);
		}
		#endregion
	}
	/// <summary>
	/// Sub class of Token for dragging capability -- ships, etc.
	/// </summary>
	public class MoveableToken:Token
	{
		#region Private Fields
		/// <summary>
		/// offset of mousecursor when selected
		/// </summary>
		private Point			_offset = Point.Empty;

		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets home hex index for a moveable token
		/// </summary>
		public new Point HomeHexIndex
		{
			get
			{
				Ship ship = (Ship)base.Source;
				return ship.HomeHex;
			}
		}
		/// <summary>
		/// gets/sets tokens current hex index
		/// </summary>
		public Point HexIndex
		{
			get{ return _hexIndex; }
			set
			{ 
				_hexIndex = value;
				Ship ship = (Ship)base.Source;
				ship.HexLocation = value;
			}
		}

		#endregion
		#region Constructors and Initialization
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public MoveableToken(string filename, Point hexLocation):base(filename,hexLocation)
		{
			_hitRect = new Rectangle(_location.X + _icon.Size.Width/4,_location.Y + 
				_icon.Size.Height/4,_icon.Size.Width/2,_icon.Size.Height/2);
		}
		#endregion
		#region Public Methods
		/// <summary>
		/// Override of Hit test to set offset for cursor dragging
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public override bool HitTest(int x, int y)
		{
			Point point = new Point(x,y);
			if(_hitRect.Contains(point))
			{
				_offset.X = point.X - _location.X;
				_offset.Y = point.Y - _location.Y;
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Drags selected object
		/// </summary>
		/// <param name="e"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="form"></param>
		public void Move(Graphics e, int x, int y, Form form)
		{
			Rectangle oldrect = _rect;
			_location.X = x - _offset.X;
			_location.Y = y - _offset.Y;
			_rect = new Rectangle(_location,_icon.Size);
			_hitRect = new Rectangle(_location.X + _icon.Size.Width/3,_location.Y + 
				_icon.Size.Height/3,_icon.Size.Width/3,_icon.Size.Height/3);
			form.Invalidate(oldrect);
			Draw(e);
		
		}
		#endregion
	}
}
	