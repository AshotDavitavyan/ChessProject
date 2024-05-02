using ChessProject.ChessPieces;

namespace ChessProject;
using static Console;
using ChessBoards;

/// <summary>
/// Provides methods to start and manage a chess game.
/// </summary>
public static class ChessGame
{
	/// <summary>
	/// Parses and adds chess pieces to the board for the specified color.
	/// </summary>
	/// <param name="board">The chess board to add pieces to.</param>
	/// <param name="color">The color of the pieces to be added.</param>
	static void ParsePieces(ChessBoard board, EPieceColor color)
	{
		WriteLine($"Creating the {color} Pieces...");
		while (true)
		{
			try
			{
				board.AddPiece(ChessPieceParser.ParsePieceInfo(color));
			}
			catch (ArgumentException ex)
			{
				WriteLine(ex.Message);
				continue;
			}
			WriteLine("Chess piece added successfully, would you like to add another piece? (y/n)");
			if (ReadLine() != "y")
				break;
		}
	}

	/// <summary>
	/// Gets the color of the player whose turn it is and sets it on the chess board.
	/// </summary>
	/// <param name="board">The chess board to set the player's turn.</param>
	static private void GetWhoseTurn(ChessBoard board)
	{
		WriteLine("Whose turn is it? White or Black?");
		while (true)
		{
			string? turn = ReadLine();
			if (turn == null || (turn != "White" && turn != "Black"))
				WriteLine("Invalid input, please enter White or Black.");
			else if (turn == "Black")
			{
				board.WhoseTurn = EPieceColor.Black;
				break;
			}
			else if (turn == "White")
			{
				board.WhoseTurn = EPieceColor.White;
				break;
			}
		}
	}
	
	/// <summary>
	/// Starts a new chess game.
	/// </summary>
	public static void Start()
	{
		ChessBoard board = new ChessBoard();
		ParsePieces(board, EPieceColor.White);
		ParsePieces(board, EPieceColor.Black);
		board.AddInfluenceCoordinates();
		GetWhoseTurn(board);
		board.TheGame();
	}
}