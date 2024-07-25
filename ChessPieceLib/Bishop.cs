using CoordinatesLib;

namespace ChessPieceLib;

public class Bishop : ChessPiece
{
    public Bishop(BaseCoordinates Cords, GameColor color) : base(Cords, 'B', color, 30)
    {
    }
	
    public override ChessPiece Clone()
    {
        Bishop clone = new Bishop(Cord, Color);
        foreach (BaseCoordinates move in _validMoves)
        {
            clone._validMoves.Add(new BaseCoordinates(move));
        }
        return clone;
    }

    public override bool IsMoveValid(BaseCoordinates destination)
    {
        (int diffX, int diffY) diffs = BaseCoordinates.GetAbsDiffs(Cord, destination);
        if (diffs is {diffX: 0, diffY: 0} || diffs.diffX != diffs.diffY)
            return false;
        return true;
    }

    public override BaseCoordinates GetValidVector(BaseCoordinates destination)
    {
        BaseCoordinates vector = new BaseCoordinates();
        (int diffX, int diffY) diffs = BaseCoordinates.GetDiffs(Cord, destination);
        vector.PosX = diffs.diffX > 0 ? -1 : 1;
        vector.PosY = diffs.diffY > 0 ? -1 : 1;
        return vector;
    }
    
    public override string ToString()
    {
        return "Bishop";
    }
}