namespace ChessProject;

using static System.Console;
using ChessProject.ChessPieces;
using System.Collections.Generic;

/// <summary>
/// Class responsible for finding the path for a knight in a chessboard.
/// </summary>
static class KnightStepCounter
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
	/// Checks if the provided position string is a valid chessboard position.
	/// </summary>
	/// <param name="position">The position string to check.</param>
	private static void CheckPosition(string? position)
	{
		if (position == null || position.Length != 2)
		{
			throw new ArgumentException("Invalid path");
		}
	}

	/// <summary>
	/// Finds the minimum number of steps for a knight to reach the target position.
	/// </summary>
	/// <param name="cords">An array containing the start and target positions of the knight.</param>
	/// <param name="validPath">A reference to store the valid path found by the method.</param>
	/// <returns>The minimum number of steps required for the knight to reach the target position.</returns>
	private static int CalculateSteps(PathfinderCoordinates[] coordinatesArray, ref PathfinderCoordinates validPath)
	{
		List<PathfinderCoordinates> cordStartList = new List<PathfinderCoordinates>();
		cordStartList.Add(new PathfinderCoordinates(coordinatesArray[0]));
		PathfinderCoordinates cordTarget = new PathfinderCoordinates(coordinatesArray[1]);
		int[] dx = { -2, -1, 1, 2, -2, -1, 1, 2 };
		int[] dy = { -1, -2, -2, -1, 1, 2, 2, 1 };
		bool[ , ] visited = new bool[8, 8];
		visited[coordinatesArray[0].PosX, coordinatesArray[0].PosY] = true;
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
	/// Parses user input to get the start and end positions of the knight.
	/// </summary>
	/// <param name="coordinates">An array to store the start and target positions of the knight.</param>
	/// <param name="startPosition">A string to store the text version of the start position of the knight.</param>
	/// <param name="endPosition">A string to store the text version of the end position of the knight.</param>
	private static void ParseUserInput(PathfinderCoordinates[] coordinates, string startPosition, string endPosition)
	{
		string[] state = new string[] {"start", "end"};
		string[] positions = new string[] { startPosition, endPosition };
		int i = 0;

		WriteLine("-----------Welcome to Pathfinder!-----------");
		while (i < 2)
		{
			WriteLine($"Type in the {state[i]} coordinates of the Knight");
			string? position = ReadLine();
			try
			{
				CheckPosition(position);
				positions[i] = new string(position);
				coordinates[i] = new PathfinderCoordinates(position[0] - 'a', position[1] - '1', null);
			}
			catch(ArgumentException ex)
			{
				WriteLine(ex.Message);
				continue;
			}
			i++;
		}
		startPosition = positions[0];
		endPosition = positions[1];
	}

	/// <summary>
	/// Displays the chessboard with a marker at the specified current coordinates.
	/// </summary>
	/// <param name="currentCords">The current coordinates to mark on the chessboard.</param>
	private static void DisplayChessBoard(BaseCoordinates currentCords)
	{
		string columnLetters = new string(" a b c d e f g h");
		WriteLine($" {columnLetters}");
		ConsoleColor[] backgroundColors = {ConsoleColor.Black, ConsoleColor.White};
		for (int y = 0; y < 8; y++){
			BackgroundColor = ConsoleColor.Black;
			Write($"{y + 1} ");
			for (int x = 0; x < 8; x++){
				BackgroundColor = backgroundColors[(y+x) % 2];
				if (currentCords.PosX == x && currentCords.PosY == y){
					BackgroundColor = ConsoleColor.Green;
					Write($"  ");
					continue;
				}
				Write("  ");
			}
			BackgroundColor = ConsoleColor.Black;
			Write($" {y + 1}");
			WriteLine();
		}
		WriteLine($" {columnLetters}");
		WriteLine("--------------------");
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
	public static void Start(){
		PathfinderCoordinates[] cords = new PathfinderCoordinates[2];
		string startPosition = string.Empty;
		string endPosition = string.Empty;

		while (true)
		{
			ParseUserInput(cords, startPosition, endPosition);
			PathfinderCoordinates? validPath = new PathfinderCoordinates(cords[0].PosX, cords[0].PosY, null);
			int steps = CalculateSteps(cords, ref validPath);
			WriteLine($"Least amount of steps to move the Knight from {startPosition} to {endPosition} is {steps}");
			WriteLine("-------------------------------------------------------------------------");
			List<BaseCoordinates> validCoordinatesList = CreateListWithValidPath(validPath);
			for (int i = validCoordinatesList.Count - 1; i >= 0; i--)
				DisplayChessBoard(validCoordinatesList[i]);
			WriteLine("Write RETRY to try another one or BACK to go back to game choice dialogue");
			string? answer = ReadLine();
			switch (answer)
			{
				case null:
				case string value when value != "RETRY" && value != "BACK":
					WriteLine("Unknown command going back to the game choice dialogue.");
					return;
				case "BACK":
					return;
				case "RETRY":
					continue;
			}
		}
	}
}
