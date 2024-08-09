using System.Windows;
using System.Windows.Documents;
using ChessBoardLib.Data;
using ChessBoardLib.Services;

namespace GameWindows;

public partial class WelcomeWindow : Window
{
	private User _user;
	public WelcomeWindow(User user)
	{
		InitializeComponent();
		WelcomeLabel.Content = "Welcome," + user.UserName + "!";
		_user = user;
	}

	private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
	{
		new ThirdGameWindow(_user.UserId).Show();
		Close();
	}

	private void ButtonLoadGame_Click(object sender, RoutedEventArgs e)
	{
		List<BoardSave> saves = new GameServices().GetSaves(_user.UserId);
		new LoadGameWindow(saves).Show();
		Close();
	}
}