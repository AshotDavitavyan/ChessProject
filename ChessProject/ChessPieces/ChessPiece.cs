namespace ChessProject.ChessPieces;
using ChessBoards;

/// <summary>
/// Abstract class representing a chess piece.
/// </summary>
public abstract class ChessPiece
{
	protected char _type;
	protected BaseCoordinates _cord;
	protected EPieceColor _color;
	protected int _value;

	// Parameterized constructor
	protected ChessPiece(BaseCoordinates cord, char type, EPieceColor color, int value)
	{
		Cord = new BaseCoordinates(cord);
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
	/// Checks if the move to the new position is valid for the chess piece.
	/// </summary>
	/// <param name="coordinateToMoveTo">The new coordinates for the chess piece.</param>
	/// <param name="board">The chess board.</param>
	/// <returns>True if the move is valid, otherwise false.</returns>
	public abstract bool CanGetToPosition(BaseCoordinates coordinateToMoveTo, ChessBoard board);
	
	/// <summary>
	/// Creates a deep copy of the current chess piece.
	/// </summary>
	/// <returns>A new instance of the chess piece with the same state as the current one.</returns>
	public abstract ChessPiece Clone();
	
	/// <summary>
	/// Counts the differences between the current position and the new position.
	/// </summary>
	/// <param name="coordinateToMoveTo"> The coordinate from which the counting will be done. </param>
	/// <param name="diffX"></param>
	/// <param name="diffY"></param>
	public void CountDiffs(BaseCoordinates coordinateToMoveTo, out int diffX, out int diffY)
	{
		diffX = Math.Abs(coordinateToMoveTo.PosX - _cord.PosX);
		diffY = Math.Abs(coordinateToMoveTo.PosY - _cord.PosY);
	}
	
	/// <summary>
	/// Moves the chess piece to the new position.
	/// </summary>
	/// <param name="newPosition">The new coordinates for the chess piece.</param>
	/// <param name="board">The chess board.</param>
	public void Move(BaseCoordinates newPosition, ChessBoard board)
	{
		if (CanGetToPosition(newPosition, board) == false)
			throw new InvalidOperationException();
		_cord = new BaseCoordinates(newPosition);
	}
}