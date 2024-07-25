using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using ChessBoardLib;
using ChessPieceLib;
using CoordinatesLib;
using ChessBotLib;
using MoveLib;

namespace GameWindows;

public partial class ThirdGameWindow : Window
{
	private readonly MainWindow _mainWindow;
	private ChessBoard _board;
	private Rectangle? _currentPosition;
	private BaseCoordinates? _position;
	private bool _isPieceClicked;
	
	public ThirdGameWindow(MainWindow mainWindow)
	{
		InitializeComponent();
		ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
		_mainWindow = mainWindow;
		_board = new ChessBoard();
		_currentPosition = null;
		_position = null;
		_isPieceClicked = false;
	}

	private bool IsMoveValid(ChessPiece piece, BaseCoordinates position)
	{
		foreach (BaseCoordinates move in piece.ValidMoves)
		{
			if (move == position)
				return true;
		}

		return false;
	}
	
	private void MovePlayersPiece()
	{
		
		_board.MakeMove(_board.ActivePiece, _position);
		ChessBoardSquares.Children.Clear();
		ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
		ChessBoardDisplayer.AddChessPiecesOnBoard(_board, ChessBoardSquares);
		_isPieceClicked = false;
		_board.ActivePiece = null;
		InstructionsLabel.Content = "Black's turn";
		if (!CheckGameState())
			GameOver();
	}
	
	private void ChessBoardSquares_MouseDown_GetPlayersMove(object sender, MouseButtonEventArgs e)
	{
		if (_board.WhoseTurn != GameColor.White)
			return;
		RegisterClickedPosition(sender, e);
		if (_isPieceClicked && _board.CanPieceGetToPosition(_board.ActivePiece, _position) && IsMoveValid(_board.ActivePiece, _position))
		{
			MovePlayersPiece();
			_board.UpdateValidMoves();
			BotsMove();
		}
		else if (_board.PieceManager.FindPieceOnPosition(_position) is not null && _board.PieceManager.FindPieceOnPosition(_position).Color == GameColor.White)
		{
			_isPieceClicked = true;
			_board.ActivePiece = _board.PieceManager.FindPieceOnPosition(_position);
			ChessBoardSquares.Children.Clear();
			ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
			ChessBoardDisplayer.AddChessPiecesOnBoard(_board, ChessBoardSquares);
			ChessBoardDisplayer.ShowValidMoves(_board, ChessBoardSquares);
		}
	}

	private void RegisterClickedPosition(object sender, MouseButtonEventArgs e)
	{
		Point position = e.GetPosition((IInputElement)sender);
		int posX = (int)position.X / 40;
		int posY = (int)position.Y / 40;
		_position = new BaseCoordinates(posX, posY);
	}

	private void SwitchButtonAddFunction(bool switchingToRemove)
	{
		switch (switchingToRemove)
		{
			case true:
				ButtonAdd.Content = "Add";
				ButtonAdd.Click -= Button_Click_Remove;
				ButtonAdd.Click += Button_Click_Add;
				_isPieceClicked = false;
				break;
			case false:
				_isPieceClicked = true;
				ButtonAdd.Content = "Remove";
				ButtonAdd.Click -= Button_Click_Add;
				ButtonAdd.Click += Button_Click_Remove;
				break;
		}
	}
	
	private void ChessBoardSquares_MouseDown_SetUpBoard(object sender, MouseButtonEventArgs e)
	{
		if (_currentPosition is not null)
			ChessBoardSquares.Children.Remove(_currentPosition);
		if (_isPieceClicked)
			SwitchButtonAddFunction(true);
		RegisterClickedPosition(sender, e);
		if (_board.PieceManager.FindPieceOnPosition(_position) is not null)
			SwitchButtonAddFunction(false);
		ChessBoardDisplayer.MarkThePosition(ChessBoardSquares, _position, ref _currentPosition);
	}

	private void Button_Click_Back(object sender, RoutedEventArgs e)
	{
		_mainWindow.Show();
		Hide();
	}
	
	private void Button_Click_Remove(object sender, RoutedEventArgs e)
	{
		_board.PieceManager.RemovePiece(_board.PieceManager.FindPieceOnPosition(_position));
		_board.AddInfluenceCoordinates();
		ChessBoardSquares.Children.Remove(_currentPosition);
		ChessBoardSquares.Children.Clear();
		ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
		ChessBoardDisplayer.AddChessPiecesOnBoard(_board, ChessBoardSquares);
		ButtonAdd.Content = "Add";
		ButtonAdd.Click -= Button_Click_Remove;
		ButtonAdd.Click += Button_Click_Add;
		_isPieceClicked = false;
	}

	private bool ValidatePieceInfo(ComboBoxItem selectedPiece, ComboBoxItem selectedPieceColor)
	{
		if (selectedPiece is null)
		{
			MessageBox.Show("Pick a ChessPiece.");
			return false;
		} 
		if (_position is null)
		{
			MessageBox.Show("Pick a position on the chessboard");
			return false;
		} 
		if (selectedPieceColor is null)
		{
			MessageBox.Show("Pick a Color");
			return false;
		}

		return true;
	}

	private void RegisterThePiece(ComboBoxItem selectedPiece, ComboBoxItem selectedPieceColor)
	{
		string selectedPieceName = selectedPiece.Name;
		GameColor pieceColor = selectedPieceColor.Name == "White" ? GameColor.White : GameColor.Black;
		_board.PieceManager.AddPiece(ChessPieceParser.CreatePiece(_position, selectedPieceName, pieceColor));
		ChessBoardDisplayer.AddChessPiecesOnBoard(_board, ChessBoardSquares); ////// Make a AddPieceOnBoard function
		ChessBoardSquares.Children.Remove(_currentPosition);
		_position = null;
	}
	
	private void Button_Click_Add(object sender, RoutedEventArgs e)
	{
		var selectedPiece = PieceOfChoice.SelectedItem as ComboBoxItem;
		var selectedPieceColor = ColorOfPiece.SelectedItem as ComboBoxItem;
		if (!ValidatePieceInfo(selectedPiece, selectedPieceColor))
			return;
		RegisterThePiece(selectedPiece, selectedPieceColor);
	}
	
	private bool CheckKings()
	{
		int whiteKingCount = 0;
		int blackKingCount = 0;
		
		foreach (ChessPiece piece in _board.PieceManager.WhitePieces)
		{
			if (piece is King)
			{
				whiteKingCount++;
			}
		}
		foreach (ChessPiece piece in _board.PieceManager.BlackPieces)
		{
			if (piece is King)
			{
				blackKingCount++;
			}
		}
		return whiteKingCount == 1 && blackKingCount == 1;
	}

	private bool CheckGameState()
	{
		GameState gameState = _board.GetGameState();
		switch (gameState)
		{
			case GameState.Mate:
				MessageBox.Show($"Mate for {_board.WhoseTurn}s, game over.");
				return false;
			case GameState.Stalemate:
				MessageBox.Show($"Stalemate for {_board.WhoseTurn}s, game over.");
				return false;
			case GameState.Check:
				MessageBox.Show($"Check for {_board.WhoseTurn}s.");
				break;
		}
		return true;
	}

	private void GameOver()
	{
		ChessBoardSquares.MouseDown -= ChessBoardSquares_MouseDown_GetPlayersMove;
	}
	
	private void BotsMove()
	{
		ChessBot.MakeMove(_board);
		_board.UpdateValidMoves();
		ChessBoardSquares.Children.Clear();
		ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
		ChessBoardDisplayer.AddChessPiecesOnBoard(_board, ChessBoardSquares);
		InstructionsLabel.Content = "White's turn";
		if (!CheckGameState())
			GameOver();
	}

	private void ChangeButtonStartFunction()
	{
		switch (ButtonStart.Content)
		{
			case "Start":
			ButtonStart.Content = "Restart";
			ButtonStart.Click -= Button_Click_Start;
			ButtonStart.Click += Button_Click_Restart;
			break;
			case "Restart":
			ButtonStart.Content = "Start";
			ButtonStart.Click -= Button_Click_Restart;
			ButtonStart.Click += Button_Click_Start;
			break;
		}
	}

	private void RemoveUnnecessaryComponents()
	{
		ChangeButtonStartFunction();
		Buttons.Children.Remove(ButtonAdd);
		ParentGrid.Children.Remove(GeneralInfo);
		InstructionsLabel.Content = "White's turn";
	}

	private void Button_Click_Restart(object sender, RoutedEventArgs e)
	{
		ChangeButtonStartFunction();
		ChessBoardSquares.Children.Clear();
		ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
		_board = new ChessBoard();
		_isPieceClicked = false;
		_currentPosition = null;
		_position = null;
		ChessBoardSquares.MouseDown -= ChessBoardSquares_MouseDown_GetPlayersMove;
		ChessBoardSquares.MouseDown += ChessBoardSquares_MouseDown_SetUpBoard;
		Buttons.Children.Add(ButtonAdd);
		ParentGrid.Children.Add(GeneralInfo);
	}
	
	private void Button_Click_Start(object sender, RoutedEventArgs e)
	{
		if (!CheckKings())
		{
			MessageBox.Show("You need to have one king for each color");
			return;
		}
		ChessBoardSquares.MouseDown -= ChessBoardSquares_MouseDown_SetUpBoard;
		ChessBoardSquares.MouseDown += ChessBoardSquares_MouseDown_GetPlayersMove;
		RemoveUnnecessaryComponents();
		_board.UpdateInfluenceCoordinates();
		_board.UpdateValidMoves();
		if (!CheckGameState())
			GameOver();
	}
}