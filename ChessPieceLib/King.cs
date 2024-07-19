using CoordinatesLib;

namespace ChessPieceLib;

public class King : ChessPiece
{
	public King(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'K', color, 0)
	{
	}
	
	public override ChessPiece Clone()
	{
		King clone = new King(Cord, Color);
		foreach (BaseCoordinates move in _validMoves)
		{
			clone._validMoves.Add(new BaseCoordinates(move));
		}
		return clone;
	}

	public override BaseCoordinates GetValidVector(BaseCoordinates destination)
	{
		BaseCoordinates cords = new BaseCoordinates();
		(int diffX, int diffY) diffs = BaseCoordinates.GetDiffs(Cord, destination);
		cords.PosX = -diffs.diffX;
		cords.PosY = -diffs.diffY;
		return cords;
	}

	public override bool IsMoveValid(BaseCoordinates destination)
	{
		(int diffX, int diffY) diffs = BaseCoordinates.GetAbsDiffs(Cord, destination);
		if ((diffs.diffX > 1 || diffs.diffY > 1) || diffs is { diffX: 0, diffY: 0 })
			return (false);
		return (true);
	}

	public override string ToString()
	{
		return "King";
	}
}