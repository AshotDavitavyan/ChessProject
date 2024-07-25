using CoordinatesLib;

namespace ChessPieceLib;

public class Rook : ChessPiece
{
	public Rook(BaseCoordinates Cords, GameColor color) : base(Cords, 'R', color, 50)
	{
	}

	public override ChessPiece Clone()
	{
		Rook clone = new Rook(Cord, Color);
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
		if (diffs.diffX == 0)
			cords.PosY = diffs.diffY > 0 ? -1 : 1;
		else
			cords.PosX = diffs.diffX > 0 ? -1 : 1;
		return cords;
	}

	public override bool IsMoveValid(BaseCoordinates destination)
	{
		(int diffX, int diffY) diffs = BaseCoordinates.GetAbsDiffs(Cord, destination);
		if (diffs is { diffX: > 0, diffY: > 0 } or {diffX: 0, diffY: 0})
			return (false);
		return (true);
	}

	public override string ToString()
	{
		return "Rook";
	}

}