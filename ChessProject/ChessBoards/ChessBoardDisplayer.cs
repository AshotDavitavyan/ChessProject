using static System.Console;
using ChessProject.ChessPieces;

namespace ChessProject.ChessBoards;

/// <summary>
/// Provides methods to display the chessboard and its influence areas.
/// </summary>
public static class ChessBoardDisplayer
{
	/// <summary>
	/// Displays the description of the colors used on the chessboard.
	/// </summary>
	public static void DisplayBoardDescription()
	{
		BackgroundColor = ConsoleColor.Yellow;
		Write($"  ");
		ResetColor();
		WriteLine(": Whites");
		BackgroundColor = ConsoleColor.DarkBlue;
		Write($"  ");
		ResetColor();
		WriteLine(": Blacks");
	}
	
	/// <summary>
	/// Determines if a position is reachable and sets the background color accordingly.
	/// </summary>
	/// <param name="cords">The coordinates of the position.</param>
	/// <param name="board">The current chessboard state.</param>
	public static void IsPositionReachable(BaseCoordinates cords, ChessBoard board)
	{
		ConsoleColor[] pieceColors = { ConsoleColor.DarkBlue, ConsoleColor.Yellow, ConsoleColor.Green };
	
		foreach (InfluenceCoordinates influence in board.InfluenceCoordinates)
		{
			if (influence == cords)
			{
				BackgroundColor = pieceColors[(int)influence.Color];
				return;
			}
		}
	}

	/// <summary>
	/// Displays the chessboard with influence areas highlighted.
	/// </summary>
	/// <param name="board">The current chessboard state.</param>
	public static void DisplayChessBoardInfluence(ChessBoard board)
	{
		string columnLetters = new string(" a b c d e f g h");
		DisplayBoardDescription();
		ConsoleColor[] backgroundColors = { ConsoleColor.Black, ConsoleColor.White };
		ConsoleColor[] pieceColors = { ConsoleColor.Yellow, ConsoleColor.DarkBlue};
		WriteLine("------Here is your chessboard!------");
		WriteLine($"        {columnLetters}");
		for (int y = 0; y < board.Size.Rows; y++)
		{
			Write($"       {y+1} ");
			for (int x = 0; x < board.Size.Columns; x++)
			{
				BaseCoordinates currentCords = new BaseCoordinates(x, y);
				BackgroundColor = backgroundColors[(y+x) % 2];

				IsPositionReachable(currentCords, board);
				Write("  ");
				BackgroundColor = ConsoleColor.Black;
			}
			Write($" {y+1}");
			WriteLine();
		}
		WriteLine($"        {columnLetters}");
	}

	/// <summary>
	/// Displays the chessboard with pieces and their colors.
	/// </summary>
	/// <param name="board">The current chessboard state.</param>
	public static void DisplayChessBoard(ChessBoard board)
	{
		string columnLetters = new string(" a b c d e f g h");
		DisplayBoardDescription();
		ConsoleColor[] backgroundColors = { ConsoleColor.Black, ConsoleColor.White };
		ConsoleColor[] pieceColors = { ConsoleColor.Yellow, ConsoleColor.DarkBlue};
		WriteLine("------Here is your chessboard!------");
		WriteLine($"        {columnLetters}");
		for (int y = 0; y < board.Size.Rows; y++)
		{
			Write($"       {y+1} ");
			for (int x = 0; x < board.Size.Columns; x++)
			{
				BaseCoordinates currentCords = new BaseCoordinates(x, y);
				BackgroundColor = backgroundColors[(y+x) % 2];
				ChessPiece? pieceOnPosition = board.IsPieceOnPosition(currentCords);
				if (pieceOnPosition is not null)
				{
					ForegroundColor = pieceColors[(int)pieceOnPosition.Color];
					Write($"{pieceOnPosition.Type} ");
					ResetColor();
					continue;
				}
				Write("  ");
				BackgroundColor = ConsoleColor.Black;
			}
			Write($" {y+1}");
			WriteLine();
		}
		WriteLine($"        {columnLetters}");
	}

}