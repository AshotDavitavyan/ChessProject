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
}