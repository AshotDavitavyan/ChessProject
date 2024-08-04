namespace ChessBoardLib.Data;

public class BoardState
{
	public int StateId { get; set; }
	public int GameId { get; set; }
	public string WhoseTurn { get; set; }
	public string WhitePieces { get; set; }
	public string BlackPieces { get; set; }
}