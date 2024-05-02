namespace ChessProject.ChessBoards;
using ChessPieces;

/// <summary>
/// Represents a move made by a chess piece on the chessboard.
/// </summary>
public class Move
{
	private ChessPiece _piece;
	private BaseCoordinates _position;
	private BaseCoordinates _destination;
	
	//Parameterized Constructor
	public Move(ChessPiece piece, BaseCoordinates position, BaseCoordinates destination)
	{
		_piece = piece;
		_position = position;
		_destination = destination;
	}
	
	public ChessPiece Piece
	{
		get { return _piece; }
	}
	
	public BaseCoordinates Position
	{
		get { return _position; }
	}
	
	public BaseCoordinates Destination
	{
		get { return _destination; }
	}
}