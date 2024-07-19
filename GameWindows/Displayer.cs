using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChessBoardLib;
using CoordinatesLib;
using System.Windows.Shapes;
using ChessPieceLib;

namespace GameWindows;

/// <summary>
/// Provides methods to display the chessboard and its influence areas.
/// </summary>
public static class ChessBoardDisplayer
{
	public static void PaintChessSquares(Grid chessBoardSquares)
	{
		Color lightColor = Colors.LightGoldenrodYellow;
		Color darkColor = Colors.SaddleBrown;

		for (int row = 0; row < 8; row++)
		{
			for (int col = 0; col < 8; col++)
			{
				Rectangle square = new Rectangle();
				square.Fill = (row + col) % 2 == 0 ? new SolidColorBrush(darkColor) : new SolidColorBrush(lightColor);

				chessBoardSquares.Children.Add(square);
				Grid.SetRow(square, row);
				Grid.SetColumn(square, col);
			}
		}
	}
	
	public static void MarkThePosition(Grid chessBoardSquares, BaseCoordinates position, ref Rectangle? currentPositionRectangle)
	{
		Rectangle square = new Rectangle();
		square.Fill = Brushes.Red;
		chessBoardSquares.Children.Add(square);
		square.Width = 40;
		square.Height = 40;
		currentPositionRectangle = square;
		square.Opacity = 0.5;
		Grid.SetColumn(square, position.PosX);
		Grid.SetRow(square, position.PosY);
	}
	
	public static void MarkThePosition(Grid chessBoardSquares, BaseCoordinates position)
	{
		Rectangle square = new Rectangle();
		square.Fill = Brushes.Red;
		chessBoardSquares.Children.Add(square);
		square.Width = 40;
		square.Height = 40;
		square.Opacity = 0.5;
		Grid.SetColumn(square, position.PosX);
		Grid.SetRow(square, position.PosY);
	}
	
	public static void MarkThePosition(Grid chessBoardSquares, BaseCoordinates position, SolidColorBrush color)
	{
		Rectangle square = new Rectangle();
		square.Fill = color;
		chessBoardSquares.Children.Add(square);
		square.Width = 40;
		square.Height = 40;
		Grid.SetColumn(square, position.PosX);
		Grid.SetRow(square, position.PosY);
	}
	
	public static Rectangle? GetRectangleAtGridPosition(int column, int row, Grid chessBoardSquares)
	{
		foreach (Rectangle rectangle in chessBoardSquares.Children)
		{
			if (Grid.GetRow(rectangle) == row &&
			    Grid.GetColumn(rectangle) == column)
			{
				return rectangle;
			}
		}
		return null;
	}
	
	public static string GenerateTheUri(ChessPiece piece)
	{
		return ("C:\\Users\\ashot\\RiderProjects\\ChessProjectWpf\\images\\" + piece.ToString() + piece.Color.ToString() + ".png");
	}

	public static void IterateAndAdd(List<ChessPiece> piecesList, Grid chessBoardSquares)
	{
		foreach (ChessPiece piece in piecesList)
		{
			Image pieceI = new Image();
			string uri = GenerateTheUri(piece);
			pieceI.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri));
			pieceI.Width = 40;
			pieceI.Height = 40;
			pieceI.HorizontalAlignment = HorizontalAlignment.Center;
			pieceI.VerticalAlignment = VerticalAlignment.Center;
			chessBoardSquares.Children.Add(pieceI);
			Grid.SetRow(pieceI, piece.Cord.PosY);
			Grid.SetColumn(pieceI, piece.Cord.PosX);
		}
	}
	
	public static void ShowValidMoves(ChessBoard board, Grid chessBoardSquares)
	{
		foreach (BaseCoordinates move in board.ActivePiece.ValidMoves)
		{
			Rectangle square = ChessBoardDisplayer.GetRectangleAtGridPosition(move.PosX, move.PosY, chessBoardSquares);
			square.Fill = Brushes.Green;
		}
	}

	public static void AddChessPiecesOnBoard(ChessBoard board, Grid chessBoardSquares)
	{
		IterateAndAdd(board.WhitePieces, chessBoardSquares);
		IterateAndAdd(board.BlackPieces, chessBoardSquares);
	}
	
	public static void ShowInfluenceCoordinates(ChessPiece piece, ChessBoard board, Grid chessBoardSquares)
	{
		foreach (InfluenceCoordinates coordinate in board.InfluenceCoordinates)
		{
			if (board.CanPieceGetToPosition(piece, coordinate))
			{
				Rectangle? rectangle = GetRectangleAtGridPosition(coordinate.PosX, coordinate.PosY, chessBoardSquares);
				if (rectangle is null)
					MessageBox.Show(coordinate.PosX.ToString() + ", " + coordinate.PosY.ToString());
				rectangle.Fill = new SolidColorBrush(Colors.Green);
			}
		}
	}
	
	public static void UpdateChessBoard(Grid chessBoardSquares, ChessBoard board)
	{
		chessBoardSquares.Children.Clear();
		PaintChessSquares(chessBoardSquares);
		AddChessPiecesOnBoard(board, chessBoardSquares);
	}
}