namespace ChessProject.ChessPieces;
using ChessProject.ChessBoards;

public class Pawn : ChessPiece
{
	public Pawn(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'P', color, 10)
	{
	}
	
	public override ChessPiece Clone()
	{
		return new Pawn(Cord, Color);
	}
	
	/// <summary>
	/// Checks if the move to the new position is valid for a pawn.
	/// </summary>
	/// <returns>True if the move is valid, otherwise false.</returns>
	public override bool CanGetToPosition(BaseCoordinates coordinateToMoveTo, ChessBoard board)
	{
		CountDiffs(coordinateToMoveTo, out int diffX, out int diffY);
		if (diffX > 2 || diffY > 0)
			return (false);
		return (true);
	}
}