using Avalonia.Controls;
using Avalonia.Interactivity;
using Trees;

namespace TreeApp;

public partial class SortedTrees : Window
{
    public SortedTrees()
    {
        InitializeComponent();
    }
    
    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }


    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        
    }
}