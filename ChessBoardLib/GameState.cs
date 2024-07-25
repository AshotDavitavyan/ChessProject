namespace ChessBoardLib;

/// <summary>
/// Represents the possible states of a chess game.
/// </summary>
public enum GameState
{
	Normal,
	Check,
	Mate,
	Stalemate,
	Draw
}