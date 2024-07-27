namespace ChessBoardLib.Data;

using System.Data.SqlClient;

public static class Logger
{
	private static ChessMovesRepository _repository = new ChessMovesRepository();
	public static void Log(ChessMoves moves)
	{
		_repository.Save(moves);
	}
}