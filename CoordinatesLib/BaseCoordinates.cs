namespace CoordinatesLib;

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
		// if (posX > 7 || posX < 0 || posY > 7 || posY < 0)
		// 	throw new ArgumentException("Invalid position");
		this.posX = posX;
		this.posY = posY;
	}

	//Parameterized Constructor with string
	public BaseCoordinates(string positionString)
	{
		// if (positionString == null || positionString.Length != 2)
		// 	throw new ArgumentException("Invalid position");
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
			// if (value > 7 || value < 0)
			// 	throw new ArgumentException("Invalid position");
			posX = value;
		}
	}
	
	/// Property for the Y coordinate of the piece.
	public int PosY
	{
		get { return posY; }
		set
		{
			// if (value > 7 || value < 0)
			// 	throw new ArgumentException("Invalid position");
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

	public static BaseCoordinates operator +(BaseCoordinates c1, BaseCoordinates c2)
	{
		BaseCoordinates result = new BaseCoordinates();
		result.PosX = c1.PosX + c2.PosX;
		result.PosY = c1.PosY + c2.PosY;
		return result;
	}
	
	public static BaseCoordinates operator -(BaseCoordinates c1, BaseCoordinates c2)
	{
		BaseCoordinates result = new BaseCoordinates();
		result.PosX = c1.PosX - c2.PosX;
		result.PosY = c1.PosY - c2.PosY;
		return result;
	}

	public static BaseCoordinates operator -(BaseCoordinates c1)
	{
		BaseCoordinates result = new BaseCoordinates();
		result.PosX = -c1.PosX;
		result.PosY = -c1.PosY;
		return result;
	}

	public static (int diffX, int diffY) GetDiffs(BaseCoordinates c1, BaseCoordinates c2)
	{
		int diffX = c1.PosX - c2.PosX;
		int diffY = c1.PosY - c2.PosY;

		return (diffX, diffY);
	}
	
	public static (int diffX, int diffY) GetAbsDiffs(BaseCoordinates c1, BaseCoordinates c2)
	{
		int diffX = Math.Abs(c1.PosX - c2.PosX);
		int diffY = Math.Abs(c1.PosY - c2.PosY);

		return (diffX, diffY);
	}

	public override string ToString()
	{
		int posYValue = PosY + 1;
		char posXChar = (Char)(PosX + 'a');
		return $"{posXChar}{posYValue}";
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(posX, posY);
	}

	public override bool Equals(object? obj)
	{
		return obj is BaseCoordinates coordinates &&
		       posX == coordinates.posX &&
		       posY == coordinates.posY;
	}
}