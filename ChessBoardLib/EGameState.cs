namespace ChessBoardLib;

/// <summary>
/// Represents the possible states of a chess game.
/// </summary>
public enum EGameState
{
	Normal,
	Check,
	Mate,
	Stalemate,
	Draw
}