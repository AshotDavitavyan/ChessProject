using ChessPieceLib;
using CoordinatesLib;

namespace GameWindows;

/// <summary>
/// Static class for parsing chess piece information.
/// </summary>
public static class ChessPieceParser
{
	/// <summary>
	/// Creates a chess piece based on the given coordinates, piece character, and color.
	/// </summary>
	/// <param name="coordinates">The coordinates of the piece on the chessboard.</param>
	/// <param name="selectedPieceCharacter">The character representing the type of piece (e.g., 'K' for King, 'Q' for Queen).</param>
	/// <param name="color">The color of the piece.</param>
	/// <returns>The created chess piece.</returns>
	/// <exception cref="ArgumentException">Thrown when an invalid piece character is provided.</exception>	
	public static ChessPiece CreatePiece(BaseCoordinates coordinates, string pieceName, GameColor color)
	{
		switch (pieceName)
		{
			case "King":
				return (new King(coordinates, color));
			case "Queen":
				return (new Queen(coordinates, color));
			case "Rook":
				return (new Rook(coordinates, color));
			case "Knight":
				return (new Knight(coordinates, color));
			case "Bishop":
				return (new Bishop(coordinates, color));
			case "Pawn":
				return (new Pawn(coordinates, color));
			default:
				throw new ArgumentException("Invalid piece character.");
		}
	}
}