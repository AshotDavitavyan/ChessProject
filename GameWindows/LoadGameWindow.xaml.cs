using System.Windows;
using System.Windows.Controls;
using ChessBoardLib.Data;
using ChessBoardLib.Services;

namespace GameWindows;

public partial class LoadGameWindow : Window
{
	public LoadGameWindow(List<BoardSave> saves)
	{
		InitializeComponent();
		FillTheComboBox(saves);
	}
	
	private void FillTheComboBox(List<BoardSave> saves)
	{
		foreach (var save in saves)
		{
			ComboBoxItem item = new ComboBoxItem
			{
				Content = save.GameId + ", " + save.SaveDate,
				Tag = save
			};
			SavesComboBox.Items.Add(item);
		}
	}

	private void ButtonLoad_Click(object sender, RoutedEventArgs e)
	{
		if (SavesComboBox.SelectedItem is ComboBoxItem item)
		{
			BoardSave save = (BoardSave)item.Tag;
			new ThirdGameWindow(save).Show();
			Close();
		}
		else
			MessageBox.Show("Please select a save to load.");
	}
}