namespace ChessBoardLib.Data;

public static class Logger
{
	private static ChessMovesRepository _chessMovesRepository = new ();
	private static BoardStateRepository _boardStateRepository = new ();
	private static BoardSavesRepository _boardSavesRepository = new ();
	public static void Log(ChessMoves moves)
	{
		_chessMovesRepository.Save(moves);
	}

	public static void Log(BoardState state)
	{
		_boardStateRepository.Save(state);
	}

	public static void Log(BoardSave save)
	{
		_boardSavesRepository.Save(save);
	}
}