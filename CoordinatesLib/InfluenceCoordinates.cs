namespace CoordinatesLib;

public class InfluenceCoordinates : BaseCoordinates
{
    private GameColor _color;
	
    public InfluenceCoordinates(int posX, int posY, GameColor color) : base(posX, posY)
    {
        _color = color;
    }

    public InfluenceCoordinates(InfluenceCoordinates copy) : base(copy)
    {
        _color = copy.Color;
    }
	
    public InfluenceCoordinates(BaseCoordinates cords, GameColor color) : base(cords)
    {
        _color = color;
    }
	
    public GameColor Color
    {
        get { return _color; }
        set { _color = value; }
    }
}
