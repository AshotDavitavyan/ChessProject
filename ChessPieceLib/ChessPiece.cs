using CoordinatesLib;

namespace ChessPieceLib;

/// <summary>
/// Abstract class representing a chess piece.
/// </summary>
public abstract class ChessPiece
{
	protected char _type;
	protected BaseCoordinates _cord;
	protected EPieceColor _color;
	protected int _value;
	protected List<BaseCoordinates> _validMoves;
	
	// Parameterized constructor
	protected ChessPiece(BaseCoordinates cord, char type, EPieceColor color, int value)
	{
		Cord = new BaseCoordinates(cord);
		_validMoves = new List<BaseCoordinates>();
		Type = type;
		Color = color;
		Value = value;
	}
	
	public EPieceColor Color
	{
		get { return _color; }
		set
		{
			if (value != EPieceColor.Black && value != EPieceColor.White)
				throw new InvalidOperationException("Invalid color.");
			_color = value;
		}
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
	
	/// <summary>
	/// Moves the chess piece to the new position.
	/// </summary>
	/// <param name="newPosition">The new coordinates for the chess piece.</param>
	public void Move(BaseCoordinates newPosition)
	{
		_cord = new BaseCoordinates(newPosition);
	}
}