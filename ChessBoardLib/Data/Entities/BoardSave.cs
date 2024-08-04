namespace ChessBoardLib.Data;

public class BoardSave
{
	public int GameId { get; set; }
	public int StateId { get; set; }
	public int UserId { get; set; }
	public DateTime SaveDate { get; set; }
}