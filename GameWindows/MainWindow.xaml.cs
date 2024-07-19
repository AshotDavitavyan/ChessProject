using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameWindows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private FirstGameWindow? _firstWindow;
    private SecondGameWindow? _secondWindow;
    private ThirdGameWindow? _thirdWindow;
    public MainWindow()
    {
        InitializeComponent();
        _firstWindow = null;
    }

    private void OpenGame1(object sender, RoutedEventArgs e)
    {
        if(_firstWindow is null)
            _firstWindow = new FirstGameWindow(this);
        if (_secondWindow is not null) _secondWindow.Close();
        if (_thirdWindow is not null) _thirdWindow.Close();
        Hide();
        _firstWindow.Show();
    }

    private void Button_Click_Exit(object sender, RoutedEventArgs e)
    {
        if (_firstWindow is not null) _firstWindow.Close();
        if (_secondWindow is not null) _secondWindow.Close();
        if (_thirdWindow is not null) _thirdWindow.Close();
        Close();
    }

    private void OpenGame2(object sender, RoutedEventArgs e)
    {
        if (_secondWindow is null)
            _secondWindow = new SecondGameWindow(this);
        if (_firstWindow is not null) _firstWindow.Close();
        if (_thirdWindow is not null) _thirdWindow.Close();
        Hide();
        _secondWindow.Show();
    }
    
    private void OpenGame3(object sender, RoutedEventArgs e)
    {
        if (_thirdWindow is null)
            _thirdWindow = new ThirdGameWindow(this);
        if (_firstWindow is not null) _firstWindow.Close();
        if (_secondWindow is not null) _secondWindow.Close();
        Hide();
        _thirdWindow.Show();
    }
}