using ChessPieceLib;
using CoordinatesLib;

namespace ChessBoardLib;

public static class PawnHelper
{
	public static bool IsMoveValid(ChessPiece pawn, BaseCoordinates destination, ChessBoard board)
	{
		(int diffX, int diffY) diffs = BaseCoordinates.GetAbsDiffs(pawn.Cord, destination);
		
		if (diffs is {diffX: 1, diffY: 1})
		{
			ChessPiece? piece = board[destination];
			if (piece is not null && piece.Color != pawn.Color)
				return true;
			return false;
		}
		return true;
	}
}