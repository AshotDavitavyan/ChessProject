using System.Reflection.PortableExecutable;

namespace ChessProject.ChessPieces;
using ChessProject.ChessBoards;

public class Bishop : ChessPiece, ICanBeBlocked
{
	public Bishop(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'B', color, 30)
	{
	}
	
	public override ChessPiece Clone()
	{
		return new Bishop(Cord, Color);
	}
	
	/// <summary>
	/// Checks if the move to the new position is valid for a bishop.
	/// </summary>
	/// <returns>True if the move is valid, otherwise false.</returns>
	public override bool CanGetToPosition(BaseCoordinates coordinateToMoveTo, ChessBoard board)
	{
		CountDiffs(coordinateToMoveTo, out int diffX, out int diffY);
		if (diffX != diffY || IsPathBlockedByPiece(coordinateToMoveTo, board))
			return (false);
		return (true);
	}
	
	/// <summary>
	/// Determines whether the path from the current position to the specified coordinate is blocked by other pieces on the chessboard.
	/// </summary>
	/// <param name="coordinate">The coordinates of the target position.</param>
	/// <param name="board">The current chessboard state.</param>
	/// <returns>
	///   <c>true</c> if the path is blocked by other pieces; otherwise, <c>false</c>.
	/// </returns>
	public bool IsPathBlockedByPiece(BaseCoordinates coordinate, ChessBoard board)
	{
		BaseCoordinates coordinateCopy = new BaseCoordinates(coordinate);
		
		while (coordinateCopy.PosX != Cord.PosX && coordinateCopy.PosY != Cord.PosY)
		{
			ChessPiece? pieceOnPosition = board.IsPieceOnPosition(coordinateCopy);
			if (pieceOnPosition != null)
			{
				if (!(coordinateCopy == coordinate && pieceOnPosition.Color != Color))
					return true;
			}
			coordinateCopy.PosX += Cord.PosX < coordinateCopy.PosX ? -1 : 1;
			coordinateCopy.PosY += Cord.PosY < coordinateCopy.PosY ? -1 : 1;
		}

		return false;
	}
}