using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using ChessBoardLib;
using ChessPieceLib;
using CoordinatesLib;

namespace GameWindows;

	public partial class FirstGameWindow : Window
    {
	    private Rectangle? _currentPositionElement;
		private MainWindow _mainWindow;
		private ChessBoard _board;
		private BaseCoordinates? _position;
		private bool _hasGameStarted;
		private bool _isPieceClicked;
        public FirstGameWindow(MainWindow window)
        {
            InitializeComponent();
			_currentPositionElement = null;
			_mainWindow = window;
			_board = new ChessBoard();
			_position = null;
			_hasGameStarted = false;
			_isPieceClicked = false;
			ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
        }
        
		private void Button_Click_Back(object sender, RoutedEventArgs e)
		{
			_mainWindow.Show();
			Hide();
		}

		private void MoveThePiece()
		{
			if (_position == _board.ActivePiece.Cord)
				return;
			_board.MakeMove(_board.ActivePiece, _position);
			ChessBoardDisplayer.UpdateChessBoard(ChessBoardSquares, _board);
			_isPieceClicked = false;
			_board.ActivePiece = null;
		}
		
		private void SetActivePieceAndShowValidMoves()
		{
			_isPieceClicked = true;
			_board.ActivePiece = _board[_position];
			ChessBoardDisplayer.ShowInfluenceCoordinates(_board.ActivePiece, _board, ChessBoardSquares);
		}
		
		private void ChessBoardSquares_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (_currentPositionElement is not null)
				ChessBoardSquares.Children.Remove(_currentPositionElement);
			Point position = e.GetPosition((IInputElement)sender);
			int posX = (int)position.X / 40;
			int posY = (int)position.Y / 40;
			_position = new BaseCoordinates(posX, posY);
			
			if (_isPieceClicked && _board.CanPieceGetToPosition(_board.ActivePiece, _position))
				MoveThePiece();
			else if (!_hasGameStarted)
				ChessBoardDisplayer.MarkThePosition(ChessBoardSquares, _position, ref _currentPositionElement);
			else if (_hasGameStarted && _board[_position] is not null)
				SetActivePieceAndShowValidMoves();
		}

		public void RegisterThePiece(ComboBoxItem selectedComboBoxItem)
		{
			string selectedPieceName = selectedComboBoxItem.Name;
			_board.PieceManager.AddPiece(ChessPieceParser.CreatePiece(_position, selectedPieceName, GameColor.White));
			_board.AddInfluenceCoordinates();
			ChessBoardDisplayer.AddChessPiecesOnBoard(_board, ChessBoardSquares);
		}

		public void AddRestartButton()
		{
			ParentGrid.Children.Remove(GeneralInfo);
			ChessBoardSquares.Children.Remove(_currentPositionElement);
			ParentGrid.Children.Remove(InstructionsGrid);
			_hasGameStarted = true;
			StartButton.Click -= Button_Click_Start;
			StartButton.Content = "Restart";
			StartButton.Click += ButtonClick_Restart;
		}

		public void ButtonClick_Restart(object sender, RoutedEventArgs e)
		{
			ChessBoardSquares.Children.Clear();
			ChessBoardDisplayer.PaintChessSquares(ChessBoardSquares);
			_board = new ChessBoard();
			_position = null;
			_currentPositionElement = null;
			_hasGameStarted = false;
			_isPieceClicked = false;
			StartButton.Click -= ButtonClick_Restart;
			StartButton.Click += Button_Click_Start;
			StartButton.Content = "Start";
			ParentGrid.Children.Add(GeneralInfo);
			ParentGrid.Children.Add(InstructionsGrid);
		}
		
		private void Button_Click_Start(object sender, RoutedEventArgs e)
		{
			var selectedComboBoxItem = PieceOfChoice.SelectedItem as ComboBoxItem;

			if (selectedComboBoxItem == null)
			{
				MessageBox.Show("Pick a ChessPiece.");
				return;
			} else if (_position is null)
			{
				MessageBox.Show("Pick a position on the chessboard");
				return;
			}
			RegisterThePiece(selectedComboBoxItem);
			AddRestartButton();
		}
	}