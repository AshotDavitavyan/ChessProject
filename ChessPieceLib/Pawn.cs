using CoordinatesLib;

namespace ChessPieceLib;

public class Pawn : ChessPiece
{
	private int _stepCount;
	private bool enPassant;
	public Pawn(BaseCoordinates Cords, GameColor color) : base(Cords, 'P', color, 10)
	{
		_stepCount = 0;
	}

	public int StepCount
	{
		get => _stepCount;
		set => _stepCount = value;
	}
	
	public bool EnPassant
	{
		get => enPassant;
		set => enPassant = value;
	}
	
	public override ChessPiece Clone()
	{
		Pawn clone = new Pawn(Cord, Color);
		clone.StepCount = _stepCount;
		foreach (BaseCoordinates move in _validMoves)
			clone._validMoves.Add(new BaseCoordinates(move));
		return clone;
	}

	public override BaseCoordinates GetValidVector(BaseCoordinates destination)
	{
		(int diffX, int diffY) diffs = BaseCoordinates.GetDiffs(destination, Cord);
		BaseCoordinates vector = new BaseCoordinates(diffs.diffX, diffs.diffY);
		return vector;
	}

	public override bool IsMoveValid(BaseCoordinates destination)
	{
		(int diffX, int diffY) diffs = BaseCoordinates.GetDiffs(Cord, destination);
		int yCofficient = Color == GameColor.White ? 1 : -1;
		diffs.diffY *= yCofficient;
		
		if (diffs.diffX == 0 && diffs.diffY == 1)
			return true;
		if (diffs.diffX == 0 && diffs.diffY == 2 && _stepCount == 0)
			return true;
		if (Math.Abs(diffs.diffX) == 1 && diffs.diffY == 1)
			return true;
		return false;
	}

	public override void Move(BaseCoordinates destination)
	{
		if (_stepCount == 0 && Math.Abs(Cord.PosY - destination.PosY) == 2)
			enPassant = true;
		else
			enPassant = false;
		_stepCount++;
		Cord = new BaseCoordinates(destination);
	}

	public override string ToString()
	{
		return "Pawn";
	}
}
