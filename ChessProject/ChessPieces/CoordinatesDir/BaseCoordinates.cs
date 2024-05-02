namespace ChessProject.ChessPieces;

#pragma warning disable CS0660
#pragma warning disable CS0661

/// <summary>
/// Represents a set of coordinates on a chessboard.
/// </summary>
public class BaseCoordinates
{
	protected int posX;
	protected int posY;
	
	//Default Constructor
	public BaseCoordinates()
	{
		posX = 0;
		posY = 0;
	}

	//Parameterized Constructor with int
	public BaseCoordinates(int posX, int posY)
	{
		if (posX > 7 || posX < 0 || posY > 7 || posY < 0)
			throw new ArgumentException("Invalid position");
		this.posX = posX;
		this.posY = posY;
	}

	//Parameterized Constructor with string
	public BaseCoordinates(string positionString)
	{
		if (positionString == null || positionString.Length != 2)
			throw new ArgumentException("Invalid position");
		PosX = positionString[0] - 'a';
		PosY = positionString[1] - '1';
	}
	
	//Copy Constructor
	public BaseCoordinates(BaseCoordinates other)
	{
		this.posX = other.posX;
		this.posY = other.posY;
	}
	
	/// Property for the X coordinate of the piece.
	public int PosX
	{
		get { return posX; }
		set
		{
			if (value > 7 || value < 0)
				throw new ArgumentException("Invalid position");
			posX = value;
		}
	}
	
	/// Property for the Y coordinate of the piece.
	public int PosY
	{
		get { return posY; }
		set
		{
			if (value > 7 || value < 0)
				throw new ArgumentException("Invalid position");
			posY = value;
		}
	}

	/// <summary>
	/// Determines whether two "Coordinates" instances are equal.
	/// </summary>
	/// <param name="c1">The first <see cref="BaseCoordinates"/> to compare.</param>
	/// <param name="c2">The second <see cref="BaseCoordinates"/> to compare.</param>
	/// <returns>True if the instances are equal; otherwise, false.</returns>
	public static bool operator ==(BaseCoordinates c1, BaseCoordinates c2)
	{
		return c1.posX == c2.posX && c1.posY == c2.posY;
	}

	/// <summary>
	/// Determines whether two "Coordinates" instances are not equal.
	/// </summary>
	/// <param name="c1">The first <see cref="BaseCoordinates"/> to compare.</param>
	/// <param name="c2">The second <see cref="BaseCoordinates"/> to compare.</param>
	/// <returns>True if the instances are not equal; otherwise, false.</returns>
	public static bool operator !=(BaseCoordinates c1, BaseCoordinates c2)
	{
		return !(c1 == c2);
	}
}

#pragma warning restore CS0660
#pragma warning restore CS0661