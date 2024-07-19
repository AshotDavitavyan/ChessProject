namespace CoordinatesLib;

public class InfluenceCoordinates : BaseCoordinates
{
    private EPieceColor _color;
	
    public InfluenceCoordinates(int posX, int posY, EPieceColor color) : base(posX, posY)
    {
        _color = color;
    }

    public InfluenceCoordinates(InfluenceCoordinates copy) : base(copy)
    {
        _color = copy.Color;
    }
	
    public InfluenceCoordinates(BaseCoordinates cords, EPieceColor color) : base(cords)
    {
        _color = color;
    }
	
    public EPieceColor Color
    {
        get { return _color; }
        set { _color = value; }
    }
}
