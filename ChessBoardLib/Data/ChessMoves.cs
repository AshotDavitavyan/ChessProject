namespace ChessBoardLib.Data;

public class ChessMoves
{
	public int Id { get; set; }
	public string Piece { get; set; }
	public string Color { get; set; }
	public string WhereFrom { get; set; }
	public string WhereTo { get; set; }
	public string? Capture { get; set; }
	public DateTime Date { get; set; }
}