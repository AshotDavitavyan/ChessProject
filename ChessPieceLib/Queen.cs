using System.Drawing;
using CoordinatesLib;

namespace ChessPieceLib;

public class Queen : ChessPiece
{
	public Queen(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'Q', color, 90)
	{
	}
	
	public override ChessPiece Clone()
	{
		Queen clone = new Queen(Cord, Color);
		foreach (BaseCoordinates move in _validMoves)
		{
			clone._validMoves.Add(new BaseCoordinates(move));
		}
		return clone;
	}
	
	public override BaseCoordinates GetValidVector(BaseCoordinates destination)
	{
		BaseCoordinates vector = new BaseCoordinates();
		(int diffX, int diffY) diffs = BaseCoordinates.GetDiffs(Cord, destination);
		if (diffs.diffX == 0)
			vector.PosY = diffs.diffY > 0 ? -1 : 1;
		else if (diffs.diffY == 0)
			vector.PosX = diffs.diffX > 0 ? -1 : 1;
		else
		{
			vector.PosX = diffs.diffX > 0 ? -1 : 1;
			vector.PosY = diffs.diffY > 0 ? -1 : 1;
		}
		return vector;
	}

	public override bool IsMoveValid(BaseCoordinates destination)
	{
		(int diffX, int diffY) diffs = BaseCoordinates.GetAbsDiffs(Cord, destination);
		if ((diffs is { diffX: > 0, diffY: > 0 } && 
		     diffs.diffX != diffs.diffY) || 
		     diffs is {diffX: 0, diffY: 0})
			return (false);
		return (true);
	}
	
	public override string ToString()
	{
		return "Queen";
	}
}