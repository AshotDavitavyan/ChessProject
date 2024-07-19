namespace CoordinatesLib;

public class PathfinderCoordinates : BaseCoordinates
{
	public PathfinderCoordinates? prevCoordinates;
	
	//Default Constructor
	public PathfinderCoordinates(int posX, int posY, PathfinderCoordinates? previous) : base(posX, posY)
	{
		prevCoordinates = previous;
	}
	
	//Parameterized Constructor
	public PathfinderCoordinates(PathfinderCoordinates other)
	{
		this.prevCoordinates = other.prevCoordinates is null ? null : new PathfinderCoordinates(other.prevCoordinates);
		this.posX = other.posX;
		this.posY = other.posY;
	}
}