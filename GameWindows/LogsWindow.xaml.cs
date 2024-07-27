using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows;
using ChessBoardLib.Data;

namespace GameWindows;

public partial class LogsWindow : Window
{
	public LogsWindow()
	{
		InitializeComponent();
	}

	public void DisplayLogs()
	{
		ChessMovesRepository rep = new ChessMovesRepository();
		DataTable dt = rep.GetAll();
		datagGrid.ItemsSource = dt.DefaultView;
	}
}