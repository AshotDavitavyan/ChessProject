using System.Windows;
using ChessBoardLib.Data;
using ChessBoardLib.Services;

namespace GameWindows;

public partial class LoginWindow : Window
{
	public LoginWindow()
	{
		InitializeComponent();
	}

	private void ButtonLogin_Click(object sender, RoutedEventArgs e)
	{
		String username = TBUsername.Text;
		String password = TBGameId.Text;
		if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
		{
			MessageBox.Show("Please fill in all fields");
			return;
		}
		var user = new GameServices().GetUser(username);
		if (user is null)
		{
			MessageBox.Show("User not found, creating a new user...");
			User newUser = new User{UserName = username, Password = password};
			new UsersRepository().Save(newUser);
			new WelcomeWindow(newUser).Show();
			Close();
			return;
		}
		WelcomeWindow welcomeWindow = new WelcomeWindow(user);
		welcomeWindow.Show();
		Close();
	}
}