namespace ChessProject.ChessPieces;
using ChessBoards;

public class Rook : ChessPiece, ICanBeBlocked
{
	public Rook(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'R', color, 50)
	{
	}

	public override ChessPiece Clone()
	{
		return new Rook(Cord, Color);
	}

	/// <summary>
	/// Checks if the move to the new position is valid for a rook.
	/// </summary>
	/// <returns>True if the move is valid, otherwise false.</returns>
	public override bool CanGetToPosition(BaseCoordinates coordinateToMoveTo, ChessBoard board)
	{
		CountDiffs(coordinateToMoveTo, out int diffX, out int diffY);
		if ((diffX > 0 && diffY > 0) || IsPathBlockedByPiece(coordinateToMoveTo, board))
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

		if (Cord.PosX == coordinateCopy.PosX)
		{
			while (Cord.PosY != coordinateCopy.PosY)
			{
				ChessPiece? pieceOnPosition = board.IsPieceOnPosition(coordinateCopy);
				if (pieceOnPosition != null)
				{
					if (!(coordinateCopy == coordinate && pieceOnPosition.Color != Color))
						return true;
				}
				coordinateCopy.PosY += Cord.PosY < coordinateCopy.PosY ? -1 : 1;
			}
		}
		else if (Cord.PosY == coordinateCopy.PosY)
		{
			while (Cord.PosX != coordinateCopy.PosX)
			{
				ChessPiece? pieceOnPosition = board.IsPieceOnPosition(coordinateCopy);
				if (pieceOnPosition != null)
				{
					if (!(coordinateCopy == coordinate && pieceOnPosition.Color != Color))
						return true;
				}
				coordinateCopy.PosX += Cord.PosX < coordinateCopy.PosX ? -1 : 1;
			}
		}
		return false;
	}
}