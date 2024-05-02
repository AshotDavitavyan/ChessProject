namespace ChessProject.ChessPieces;
using ChessProject.ChessBoards;
public class King : ChessPiece
{
	public King(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'K', color, 900)
	{
	}
	
	public override ChessPiece Clone()
	{
		return new King(Cord, Color);
	}
	
	/// <summary>
	/// Checks if the move to the new position is valid for a king.
	/// </summary>
	/// <returns>True if the move is valid, otherwise false.</returns>
	public override bool CanGetToPosition(BaseCoordinates coordinateToMoveTo, ChessBoard board)
	{
		CountDiffs(coordinateToMoveTo, out int diffX, out int diffY);
		if (diffX > 1 || diffY > 1)
			return (false);
		return (true);
	}
}