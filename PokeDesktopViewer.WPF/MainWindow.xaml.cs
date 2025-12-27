using PokeDesktopViewer.WPF.ViewModels;
using System.Windows;

namespace PokeDesktopViewer.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainViewModel();
        DataContext = viewModel;
    }
}