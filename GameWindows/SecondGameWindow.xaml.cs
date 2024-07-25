using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ChessBoardLib;
using ChessPieceLib;
using CoordinatesLib;
using KnightStepCounterLib;

namespace GameWindows;

public partial class SecondGameWindow : Window
{
	private MainWindow _mainWindow;
	private PathfinderCoordinates? _positionStart;
	private PathfinderCoordinates? _positionEnd;
	private ChessBoard _board;
	
	public SecondGameWindow(MainWindow mainWindow)
	{
		InitializeComponent();
		ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
		_positionStart = null;
		_positionEnd = null;
		_board = new ChessBoard();
		_mainWindow = mainWindow;
	}

	private void ChessBoardSquares_MouseDown(object sender, MouseButtonEventArgs e)
	{
		Point position = e.GetPosition((IInputElement)sender);
		int posX = (int)position.X / 40;
		int posY = (int)position.Y / 40;
		if (_positionStart is null)
		{
			_positionStart = new PathfinderCoordinates(posX, posY, null);
			InstructionsLabel.Content = "Select the end position for the knight.";
			ChessBoardDisplayer.MarkThePosition(ChessBoardSquares, _positionStart);
			return;
		}
		if (_positionEnd is null)
		{
			_positionEnd = new PathfinderCoordinates(posX, posY, null);
			InstructionsLabel.Content = "Click Start button to start.";
			ChessBoardDisplayer.MarkThePosition(ChessBoardSquares, _positionEnd);
		}
	}
	
	private void RestartButton_Click(object sender, RoutedEventArgs e)
	{
		_positionStart = null;
		_positionEnd = null;
		InstructionsLabel.Content = "Select the start position for the knight.";
		ChessBoardSquares.Children.Clear();
		ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
		_board = new ChessBoard();
		StartButton.Click -= RestartButton_Click;
		StartButton.Click += StartButton_Click;
		StartButton.Content = "Start";
	}

	private void SwichStartButtonToRestartButton()
	{
		StartButton.Click -= StartButton_Click;
		StartButton.Click += RestartButton_Click;
		StartButton.Content = "Restart";
	}
	
	private void VisualiseThePath(List<BaseCoordinates> path)
	{
		for (int i = path.Count - 2; i >= 0; i--)
		{
			ChessBoardDisplayer.MarkThePosition(ChessBoardSquares, path[i], Brushes.Green);
			ChessBoardDisplayer.MarkThePosition(ChessBoardSquares, _positionStart, Brushes.Red);
			ChessBoardDisplayer.MarkThePosition(ChessBoardSquares, _positionEnd, Brushes.Red);
			MessageBox.Show($"Moving the piece to {path[i].PosX}, {path[i].PosY}.");
			_board.MakeMove(_board.PieceManager.WhitePieces[0], path[i]);
			ChessBoardDisplayer.UpdateChessBoard(ChessBoardSquares, _board);
		}
		MessageBox.Show($"{path.Count-1} steps.");
	}
	
	private void StartButton_Click(object sender, RoutedEventArgs e)
	{
		if (_positionStart is null || _positionEnd is null)
		{
			MessageBox.Show("Please select a start and end position.");
			return;
		}
		SwichStartButtonToRestartButton();
		List<BaseCoordinates> validPath = KnightStepCounter.Start(_positionStart, _positionEnd);
		_board.PieceManager.AddPiece(new Knight(validPath[^1], GameColor.White));
		ChessBoardDisplayer.AddChessPiecesOnBoard(_board, ChessBoardSquares);
		VisualiseThePath(validPath);
	}

	private void ButtonBack_Click(object sender, RoutedEventArgs e)
	{
		_mainWindow.Show();
		this.Hide();
	}
}