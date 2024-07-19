using CoordinatesLib;

namespace ChessPieceLib;

public class Pawn : ChessPiece
{
	public Pawn(BaseCoordinates Cords, EPieceColor color) : base(Cords, 'P', color, 10)
	{
	}
	
	public override ChessPiece Clone()
	{
		Pawn clone = new Pawn(Cord, Color);
		foreach (BaseCoordinates move in _validMoves)
		{
			clone._validMoves.Add(new BaseCoordinates(move));
		}
		return clone;
	}

	public override BaseCoordinates GetValidVector(BaseCoordinates destination)
	{
		throw new NotImplementedException();
	}

	public override bool IsMoveValid(BaseCoordinates destination)
	{
		throw new NotImplementedException();
	}

	public override string ToString()
	{
		return "Pawn";
	}
}