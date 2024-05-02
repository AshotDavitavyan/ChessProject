namespace ChessProject.ChessBoards;

public struct BoardSize
{
	private const int _rows = 8;
	private const int _columns = 8;
	
	public int Rows
	{
		get { return _rows; }
	}
	
	public int Columns
	{
		get { return _columns; }
	}
}