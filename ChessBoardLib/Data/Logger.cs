namespace ChessBoardLib.Data;

using System.Data.SqlClient;

public static class Logger
{
	private static ChessMovesRepository _chessMovesRepository = new ();
	private static BoardStateRepository _boardStateRepository = new ();
	public static void Log(ChessMoves moves)
	{
		_chessMovesRepository.Save(moves);
	}

	public static void Log(BoardState state)
	{
		_boardStateRepository.Save(state);
	}
}