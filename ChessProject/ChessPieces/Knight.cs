namespace ChessProject.ChessPieces;
using ChessProject.ChessBoards;

public class Knight : ChessPiece
{
	public Knight(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'N', color, 30)
	{
	}
	
	public override ChessPiece Clone()
	{
		return new Knight(Cord, Color);
	}

	/// <summary>
	/// Checks if the move to the new position is valid for a knight.
	/// </summary>
	/// <returns>True if the move is valid, otherwise false.</returns>
	public override bool CanGetToPosition(BaseCoordinates coordinateToMoveTo, ChessBoard board)
	{
		CountDiffs(coordinateToMoveTo, out int diffX, out int diffY);
		if (!((diffX == 2 && diffY == 1) || (diffX == 1 && diffY == 2)))
			return (false);
		return (true);
	}
}