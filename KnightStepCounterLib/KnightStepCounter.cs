using CoordinatesLib;

namespace KnightStepCounterLib;

public static class KnightStepCounter
{
	/// <summary>
	/// Checks if the given position is outside the chessboard or already visited.
	/// </summary>
	/// <param name="posX">The X-coordinate of the position to check.</param>
	/// <param name="posY">The Y-coordinate of the position to check.</param>
	/// <param name="visited">A boolean matrix representing visited positions on the chessboard.</param>
	/// <returns>True if the position is outside the chessboard or already visited; otherwise, false.</returns>
	private static bool IsOutsideOrVisited(int posX, int posY, bool[ , ] visited)
	{
		if (!(posX >= 0 && posX <= 7 && posY >= 0 && posY <= 7) || visited[posX, posY])
			return (true);
		return (false);
	}

	/// <summary>
	/// Finds the minimum number of steps for a knight to reach the target position.
	/// </summary>
	/// <param name="cords">An array containing the start and target positions of the knight.</param>
	/// <param name="validPath">A reference to store the valid path found by the method.</param>
	/// <returns>The minimum number of steps required for the knight to reach the target position.</returns>
	private static int CalculateSteps(PathfinderCoordinates startPosition, PathfinderCoordinates endPosition, ref PathfinderCoordinates validPath)
	{
		List<PathfinderCoordinates> cordStartList = new List<PathfinderCoordinates>();
		cordStartList.Add(new PathfinderCoordinates(startPosition));
		PathfinderCoordinates cordTarget = new PathfinderCoordinates(endPosition);
		int[] dx = { -2, -1, 1, 2, -2, -1, 1, 2 };
		int[] dy = { -1, -2, -2, -1, 1, 2, 2, 1 };
		bool[ , ] visited = new bool[8, 8];
		visited[startPosition.PosX, startPosition.PosY] = true;
		return CalculateMinimumSteps(0, ref validPath, cordStartList, cordTarget, dx, dy, visited);
	}

	/// <summary>
	/// Refreshes the list of coordinates to explore based on the current position and the possible moves of the knight.
	/// </summary>
	/// <param name="coordinatesList">A list containing coordinates to explore.</param>
	/// <param name="dx">Array of possible X-coordinate changes for the knight's move.</param>
	/// <param name="dy">Array of possible Y-coordinate changes for the knight's move.</param>
	/// <param name="visited">A boolean matrix representing visited positions on the chessboard.</param>
	private static void RefreshListCoordinates(List<PathfinderCoordinates> coordinatesList, int[] dx, int[] dy, bool[ , ] visited)
	{
		int currentIndex = 0;
		int size = coordinatesList.Count;
		
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (coordinatesList[0] is null)
					continue;
				int newX = coordinatesList[0].PosX + dx[j];
				int newY = coordinatesList[0].PosY + dy[j];
				if (IsOutsideOrVisited(newX, newY, visited))
					continue;
				PathfinderCoordinates newPosition = new PathfinderCoordinates(newX, newY, coordinatesList[0]);
				coordinatesList.Add(newPosition);
				visited[newX, newY] = true;
				currentIndex++;
			}
			coordinatesList.RemoveAt(0);
		}
	}
	
	/// <summary>
	/// Recursively calculates the minimum steps required for the knight to reach the target position.
	/// </summary>
	/// <param name="depth">The current depth of recursion.</param>
	/// <param name="validPath">A reference to store the valid path found by the method.</param>
	/// <param name="coordinatesList">A list containing coordinates to explore.</param>
	/// <param name="targetPosition">The target position the knight needs to reach.</param>
	/// <param name="dx">Array of possible X-coordinate changes for the knight's move.</param>
	/// <param name="dy">Array of possible Y-coordinate changes for the knight's move.</param>
	/// <param name="visited">A boolean matrix representing visited positions on the chessboard.</param>
	/// <returns>The minimum number of steps required for the knight to reach the target position.</returns>
	private static int CalculateMinimumSteps(int depth, ref PathfinderCoordinates validPath, List<PathfinderCoordinates> coordinatesList,
		PathfinderCoordinates targetPosition, int[] dx, int[] dy, bool[,] visited)
	{
		for (int i = 0; i < coordinatesList.Count; i++)
		{
			if (coordinatesList[i] == targetPosition)
			{
				validPath = coordinatesList[i];
				return (depth);
			}
		}
		RefreshListCoordinates(coordinatesList, dx, dy, visited);
		return (CalculateMinimumSteps(depth + 1, ref validPath, coordinatesList, targetPosition, dx, dy, visited));
	}

	/// <summary>
	/// Creates a list of base coordinates from a valid path.
	/// </summary>
	/// <param name="validPath">The valid path containing coordinates.</param>
	/// <returns>A list of base coordinates extracted from the valid path.</returns>
	private static List<BaseCoordinates> CreateListWithValidPath(PathfinderCoordinates validPath)
	{
		List<BaseCoordinates> validCoordinatesList = new List<BaseCoordinates>();
		
		while (validPath is not null)
		{
			validCoordinatesList.Add(validPath);
			validPath = validPath.prevCoordinates;
		}

		return validCoordinatesList;
	}
	
	/// <summary>
	/// Starts the knight pathfinding game.
	/// </summary>
	public static List<BaseCoordinates> Start(PathfinderCoordinates startPosition, PathfinderCoordinates endPosition)
	{
		PathfinderCoordinates? validPath = new PathfinderCoordinates(startPosition.PosX, startPosition.PosY, null);
		int steps = CalculateSteps(startPosition, endPosition, ref validPath);
		List<BaseCoordinates> validCoordinatesList = CreateListWithValidPath(validPath);
		return validCoordinatesList;
	}
}