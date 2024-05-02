namespace ChessProject.ChessPieces;
using ChessBoards;

/// <summary>
/// Interface for pieces that can be blocked.
/// </summary>
public interface ICanBeBlocked
{
	bool IsPathBlockedByPiece(BaseCoordinates coordinate, ChessBoard board);
}