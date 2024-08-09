using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using ChessBoardLib;
using ChessBoardLib.Data;
using ChessBoardLib.Services;
using ChessPieceLib;
using CoordinatesLib;
using ChessBotLib;
using MoveLib;

namespace GameWindows;

public partial class ThirdGameWindow : Window
{
	private bool _isLoaded;
	private int _userId;
	private ChessBoard _board;
	private Rectangle? _currentPosition;
	private BaseCoordinates? _position;
	private bool _isPieceClicked;
	private GameStateManager _gameStateManager;

	public ThirdGameWindow(int UserId)
	{
		_userId = UserId;
		InitializeComponent();
		InitializeGame();
		InitializeGameState();
	}
	
	public ThirdGameWindow(BoardSave boardSave)
	{
		_isLoaded = true;
		_userId = boardSave.UserId;
		InitializeComponent();
		InitializeGame();
		LoadBoardSave(boardSave);
	}
	
	private void InitializeGameState()
	{
		int stateId = GameServices.GetLastStateId() + 1;
		_gameStateManager = new GameStateManager
		{
			LastStateId = stateId,
			CurrentStateId = stateId,
			FirstStateId = stateId,
			CurrentGameId = GameServices.GetLastGameId() + 1
		};
	}

	private void InitializeGameState(BoardSave boardSave)
	{
		List<BoardState> states = GameServices.GetBoardStatesForGame(boardSave.GameId);
		int currentStateId = -1;
		int firstStateId = GameServices.GetLastStateId() + 1;
		int gameId = GameServices.GetLastGameId() + 1;
		foreach (BoardState state in states)
		{
			if (state.StateId == boardSave.StateId)
				currentStateId = GameServices.GetLastStateId() + 1;
			state.GameId = gameId;
			Logger.Log(state);
		}
		_gameStateManager = new GameStateManager
		{
			LastStateId = GameServices.GetLastStateId(),
			CurrentStateId = currentStateId,
			FirstStateId = firstStateId,
			CurrentGameId = gameId
		};

		Logger.Log(new BoardSave
		{
			GameId = _gameStateManager.CurrentGameId,
			StateId = _gameStateManager.FirstStateId,
			UserId = _userId
		});
	}

	private void InitializeGame()
	{
		ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
		_board = new ChessBoard();
		_currentPosition = null;
		_position = null;
		_isPieceClicked = false;
	}

	private void LoadBoardSave(BoardSave boardSave)
	{
		BoardState boardState = new GameServices().GetBoardState(boardSave);
		InitializeGameState(boardSave);
		_board.LoadBoardState(boardState);
		ChessBoardDisplayer.UpdateChessBoard(ChessBoardSquares, _board);
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
		
		_board.MakeMoveLog(new Move(_board.ActivePiece,
									_position, 
									_board[_position]), _gameStateManager.CurrentGameId, _gameStateManager.CurrentStateId);
		ChessBoardDisplayer.UpdateChessBoard(ChessBoardSquares, _board);
		_isPieceClicked = false;
		_board.ActivePiece = null;
		InstructionsLabel.Content = "Black's turn";
	}
	
	private void ChessBoardSquares_MouseDown_GetPlayersMove(object sender, MouseButtonEventArgs e)
	{
		if (_board.WhoseTurn != GameColor.White)
			return;
		RegisterClickedPosition(sender, e);
		if (_isPieceClicked && _board.CanPieceGetToPosition(_board.ActivePiece, _position) && IsMoveValid(_board.ActivePiece, _position))
		{
			_gameStateManager.CheckAndUpdateStateIds();

			_gameStateManager.IncrementIds();
			MovePlayersPiece();
			_board.UpdateValidMoves();
			if (!CheckGameState())
				GameOver();
			_gameStateManager.IncrementIds();
			BotsMove();
			_board.UpdateValidMoves();
			if (!CheckGameState())
				GameOver();
		}
		else if (_board[_position] is not null && _board[_position].Color == GameColor.White)
		{
			_isPieceClicked = true;
			_board.ActivePiece = _board[_position];
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
		if (_board[_position] is not null)
			SwitchButtonAddFunction(false);
		ChessBoardDisplayer.MarkThePosition(ChessBoardSquares, _position, ref _currentPosition);
	}

	private void Button_Click_Back(object sender, RoutedEventArgs e)
	{
		new BoardSavesRepository().DeleteGameSave(_gameStateManager.CurrentGameId);
		new BoardStateRepository().DeleteGameStates(_gameStateManager.CurrentGameId);
		Close();
	}
	
	private void Button_Click_Remove(object sender, RoutedEventArgs e)
	{
		_board.PieceManager.RemovePiece(_board[_position]);
		_board.AddInfluenceCoordinates();
		ChessBoardSquares.Children.Remove(_currentPosition);
		ChessBoardDisplayer.UpdateChessBoard(ChessBoardSquares, _board);
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
				whiteKingCount++;
		}
		foreach (ChessPiece piece in _board.PieceManager.BlackPieces)
		{
			if (piece is King)
				blackKingCount++;
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
		ChessBot.Think(_board);
		_board.MakeMoveLog(ChessBot.Think(_board), _gameStateManager.CurrentGameId, _gameStateManager.CurrentStateId);
		ChessBoardDisplayer.UpdateChessBoard(ChessBoardSquares, _board);
		InstructionsLabel.Content = "White's turn";
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
		new ChessMovesRepository().Clear();
		new BoardSavesRepository().DeleteGameLog(_gameStateManager.CurrentGameId);
		new BoardStateRepository().DeleteGameLogsStartingFrom(_gameStateManager.FirstStateId, _gameStateManager.CurrentGameId);
	}

	private void SaveGame(object sender, RoutedEventArgs e)
	{
		List<BoardState> states = GameServices.GetBoardStatesForGame(_gameStateManager.CurrentGameId);
		int gameId = GameServices.GetLastGameId() + 1;
		int currentStateId = -1;
		foreach (BoardState state in states)
		{
			if (state.StateId == _gameStateManager.CurrentStateId) 
				currentStateId = GameServices.GetLastStateId() + 1;
			state.GameId = gameId;
			Logger.Log(state);
		}
		Logger.Log(new BoardSave
		{
			GameId = gameId,
			StateId = currentStateId,
			UserId = _userId
		});
		MessageBox.Show("Game saved successfully!");
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
		ButtonAdd.Content = "Save";
		ButtonAdd.Click -= Button_Click_Add;
		ButtonAdd.Click += SaveGame;
		RemoveUnnecessaryComponents();
		_board.UpdateInfluenceCoordinates();
		_board.UpdateValidMoves();
		if (!_isLoaded)
		{
			_board.LogCurrentBoardState(_gameStateManager.CurrentGameId, _gameStateManager.CurrentStateId);
			Logger.Log(new BoardSave
			{
				GameId = GameServices.GetLastGameId() + 1,
				StateId = GameServices.GetLastStateId(),
				UserId = _userId,
				SaveDate = DateTime.Now
			});
		}
		_gameStateManager.CurrentGameId = GameServices.GetLastGameId();
		if (!CheckGameState())
			GameOver();
	}

	private void ButtonLog_OnClick(object sender, RoutedEventArgs e)
	{
		LogsWindow logsWindow = new LogsWindow();
		logsWindow.DisplayLogs();
		logsWindow.Show();
	}

	private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
	{
		BoardState? stateToLoad = _gameStateManager.GoBack();
		if (stateToLoad is null)
			return;
		_board.LoadBoardState(stateToLoad);
		ChessBoardDisplayer.UpdateChessBoard(ChessBoardSquares, _board);
	}

	private void ButtonForward_OnClick(object sender, RoutedEventArgs e)
	{
		BoardState? stateToLoad = _gameStateManager.GoForward();
		if (stateToLoad is null)
			return;
		_board.LoadBoardState(stateToLoad);
		ChessBoardDisplayer.UpdateChessBoard(ChessBoardSquares, _board);
	}
}