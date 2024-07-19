using CoordinatesLib;

namespace ChessPieceLib;

public class Knight : ChessPiece
{
	public Knight(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'N', color, 30)
	{
	}
	
	public override ChessPiece Clone()
	{
		Knight clone = new Knight(Cord, Color);
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
		if (diffs is not {diffX: 2, diffY: 1} && diffs is not {diffX: 1, diffY: 2})
			return (false);
		return (true);
	}

	public override string ToString()
	{
		return "Knight";
	}
}