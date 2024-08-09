using ChessBoardLib.Data;

namespace ChessBoardLib.Services;

public class GameServices
{
	public BoardState GetBoardState(BoardSave save)
	{
		return new BoardStateRepository().GetById(save.StateId, save.GameId);
	}
	
	public List<BoardSave> GetSaves(int userId)
	{
		return new BoardStateRepository().GetByUserId(userId);
	}
	public User GetUser(string username)
	{
		return new UsersRepository().GetUser(username);
	}
	
	public IEnumerable<BoardSave> GetSessions(int userId)
	{
		return new BoardStateRepository().GetByUserId(userId);
	}

	public static int GetLastGameId()
	{
		return new BoardSavesRepository().GetLastId();
	}

	public static List<BoardState> GetBoardStatesForGame(int gameId)
	{
		return new BoardStateRepository().GetAllByGameId(gameId);
	}

	public static BoardState? GetLastState(int gameId)
	{
		return new BoardStateRepository().GetLastStateForGame(gameId);
	}
	
	public static BoardState? GetFirstState(int gameId)
	{
		return new BoardStateRepository().GetFirstStateForGame(gameId);
	}

	public static int GetLastStateId()
	{
		return new BoardStateRepository().GetLastStateId();
	}

	public static int GetFirstStateId(int boardSaveGameId)
	{
		BoardState? firstState = GetFirstState(boardSaveGameId);
		return firstState?.StateId ?? 0;
	}
}