namespace ChessProject.ChessPieces;
using ChessProject.ChessBoards;

public class Queen : ChessPiece, ICanBeBlocked
{
	public Queen(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'Q', color, 90)
	{
	}
	
	public override ChessPiece Clone()
	{
		return new Queen(Cord, Color);
	}
	
	public override bool CanGetToPosition(BaseCoordinates coordinateToMoveTo, ChessBoard board)
	{
		CountDiffs(coordinateToMoveTo, out int diffX, out int diffY);
		if ((diffX > 0  && diffY > 0) && (diffX != diffY) || IsPathBlockedByPiece(coordinateToMoveTo, board))
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
		if (coordinateCopy.PosX == Cord.PosX && coordinateCopy.PosY != Cord.PosY)
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

		else if (coordinateCopy.PosY == Cord.PosY && coordinateCopy.PosX != Cord.PosX)
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
		
		else if (Math.Abs(Cord.PosX - coordinateCopy.PosX) == Math.Abs(Cord.PosY - coordinateCopy.PosY))
		{
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
		}
		return false;
	}
}