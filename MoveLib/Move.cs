using ChessPieceLib;
using CoordinatesLib;

namespace MoveLib;


/// <summary>
/// Represents a move made by a chess piece on the chessboard.
/// </summary>
public class Move
{
	private ChessPiece _piece;
	private BaseCoordinates _destination;
	private int _moveValue;
	
	//Parameterized Constructor
	public Move(ChessPiece piece, BaseCoordinates destination, ChessPiece? capturedPiece)
	{
		_piece = piece.Clone();
		_destination = new BaseCoordinates(destination);
		_moveValue += capturedPiece is null ? 0 : capturedPiece.Value;
	}

	public override string ToString()
	{
		return $"{_piece.ToString()} from {_piece.Cord.ToString()} to {_destination.ToString()}";
	}

	public ChessPiece Piece
	{
		get => _piece;
	}
	
	public BaseCoordinates Destination
	{
		get => _destination;
	}

	public int MoveValue
	{
		get => _moveValue;
	}
}