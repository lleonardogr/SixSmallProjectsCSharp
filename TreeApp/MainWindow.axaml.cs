using Avalonia.Controls;
using Avalonia.Interactivity;
using Trees;

namespace TreeApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        // Initialization code if needed
    }
    
    private void OrgChartsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var orgChartsWindow = new OrgCharts();
        orgChartsWindow.Show();
        this.Hide();
    }
    
    private void SortedTreesButton_OnClick(object sender, RoutedEventArgs e)
    {
        var sortedTreesWindow = new SortedTrees();
        sortedTreesWindow.Show();
        this.Hide();
    }
}
