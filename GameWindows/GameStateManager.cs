using System.Windows.Media;
using ChessBoardLib.Data;
using ChessBoardLib.Services;

namespace GameWindows;

public class GameStateManager
{
	public int LastStateId { get; set; } = 1;
	public int CurrentStateId { get; set; } = 1;
	public int FirstStateId { get; set; } = 1;
	public int CurrentGameId { get; set; } = 1;
	
	public void IncrementIds()
	{
		LastStateId = GameServices.GetLastStateId() + 1;
		CurrentStateId = GameServices.GetLastStateId() + 1;
	}

	public BoardState?  GoBack()
	{
		int previousStateId = -1;
		if (CurrentStateId == FirstStateId)
			return null;
		Dictionary<int, BoardState> boardStates = new BoardStateRepository().GetWhiteStates(CurrentGameId);
		foreach (var state in boardStates)
		{
			if (state.Key == CurrentStateId)
				break;
			previousStateId = state.Key;
		}
		CurrentStateId = previousStateId;
		return boardStates[previousStateId];
	}

	public BoardState? GoForward()
	{
		if (CurrentStateId == LastStateId)
			return null;
		bool found = false;
		Dictionary<int, BoardState> boardStates = new BoardStateRepository().GetWhiteStates(CurrentGameId);
		foreach (var state in boardStates)
		{
			if (state.Key == CurrentStateId)
			{
				found = true;
				continue;
			}
			if (found)
			{
				CurrentStateId = state.Key;
				break;
			}
		}
		return boardStates[CurrentStateId];
	}

	public void CheckAndUpdateStateIds()
	{
		if (LastStateId != CurrentStateId)
		{
			new BoardStateRepository().DeleteGameLogsStartingFrom(CurrentStateId + 1, CurrentGameId);
			LastStateId = CurrentStateId;
		}
	}
}