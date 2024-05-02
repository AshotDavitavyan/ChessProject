using static System.Console;

namespace ChessProject.ChessPieces;

/// <summary>
/// Static class for parsing chess piece information.
/// </summary>
public static class ChessPieceParser
{
	/// <summary>
	/// Takes a position on the chessboard from the user.
	/// </summary>
	/// <returns>The position</returns>
	public static string TakePositionOnBoard()
	{
		WriteLine($"Enter a valid position on the chessboard (a-h)(1-8)");
		string? piecePosition = ReadLine();
		return piecePosition;
	}
	
	/// <summary>
	/// Takes the type of chess piece from the user.
	/// </summary>
	/// <returns>The valid character, or throws an exception.</returns>
	public static char TakePieceType()
	{
		string validPieceCharacters = "KQRNBP";

		WriteLine("Enter the first letter of the chess piece.");
		if (!char.TryParse(ReadLine(), out char selectedPieceCharacter) ||
		    validPieceCharacters.IndexOf(selectedPieceCharacter) == -1)
			throw new ArgumentException("Invalid piece character.");
		return selectedPieceCharacter;
	}
	
	
	/// <summary>
	/// Creates a chess piece based on the given coordinates, piece character, and color.
	/// </summary>
	/// <param name="coordinates">The coordinates of the piece on the chessboard.</param>
	/// <param name="selectedPieceCharacter">The character representing the type of piece (e.g., 'K' for King, 'Q' for Queen).</param>
	/// <param name="color">The color of the piece.</param>
	/// <returns>The created chess piece.</returns>
	/// <exception cref="ArgumentException">Thrown when an invalid piece character is provided.</exception>	
	static ChessPiece CreatePiece(BaseCoordinates coordinates, char selectedPieceCharacter, EPieceColor color)
	{
		switch (selectedPieceCharacter)
		{
			case 'K':
				return (new King(coordinates, color));
			case 'Q':
				return (new Queen(coordinates, color));
			case 'R':
				return (new Rook(coordinates, color));
			case 'N':
				return (new Knight(coordinates, color));
			case 'B':
				return (new Bishop(coordinates, color));
			case 'P':
				return (new Pawn(coordinates, color));
			default:
				throw new ArgumentException("Invalid piece character.");
		}
	}
	
	public static ChessPiece ParsePieceInfo(EPieceColor color)
	{
		try 
		{
			string piecePosition = TakePositionOnBoard();
			char selectedPieceCharacter = TakePieceType();
			return CreatePiece(new BaseCoordinates(piecePosition), selectedPieceCharacter, color);
		}
		catch (ArgumentException ex)
		{
			WriteLine(ex.Message + " Please try again.");
			return ParsePieceInfo(color);
		}
	}
}