using CoordinatesLib;

namespace ChessPieceLib;

/// <summary>
/// Abstract class representing a chess piece.
/// </summary>
public abstract class ChessPiece
{
	protected char _type;
	protected BaseCoordinates _cord;
	protected GameColor _color;
	protected int _value;
	protected List<BaseCoordinates> _validMoves;
	
	// Parameterized constructor
	protected ChessPiece(BaseCoordinates cord, char type, GameColor color, int value)
	{
		Cord = new BaseCoordinates(cord);
		_validMoves = new List<BaseCoordinates>();
		Type = type;
		Color = color;
		Value = value;
	}
	
	public GameColor Color
	{
		get { return _color; }
		set
		{
			if (value != GameColor.Black && value != GameColor.White)
				throw new InvalidOperationException("Invalid color.");
			_color = value;
		}
	}
	
	public static bool operator ==(ChessPiece c1, ChessPiece c2)
	{
		return c1.Cord == c2.Cord && c1.Color == c2.Color && c1.Type == c2.Type;
	}

	public static bool operator !=(ChessPiece c1, ChessPiece c2)
	{
		return !(c1 == c2);
	}


	public List<BaseCoordinates> ValidMoves
	{
		get { return _validMoves; }
		set { _validMoves = value; }
	}

	public char Type
	{
		get { return _type; }
		set { this._type = value;  }
	}

	public BaseCoordinates Cord
	{
		get { return _cord; }
		set { _cord = value; }
	}

	public int Value
	{
		get { return _value; }
		set { this._value = value; }
	}
	
	/// <summary>
	/// Creates a deep copy of the current chess piece.
	/// </summary>
	/// <returns>A new instance of the chess piece with the same state as the current one.</returns>
	public abstract ChessPiece Clone();

	public abstract BaseCoordinates GetValidVector(BaseCoordinates destination);

	public abstract bool IsMoveValid(BaseCoordinates destination);
	
	public abstract override string ToString();
	
	/// <summary>
	/// Moves the chess piece to the new position.
	/// </summary>
	/// <param name="newPosition">The new coordinates for the chess piece.</param>
	public virtual void Move(BaseCoordinates newPosition)
	{
		_cord = new BaseCoordinates(newPosition);
	}
}