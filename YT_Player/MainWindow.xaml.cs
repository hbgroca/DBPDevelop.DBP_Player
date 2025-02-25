using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;

namespace YT_Player;

public partial class MainWindow : Window
{
    private string userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DBP_Player", "DBP_Player_YT");
    private bool isResizing = false;
    private Point startPoint;

    public MainWindow()
    {
        InitializeComponent();
        InitializeAsync();
    }

    // Initialize WebView2
    private async void InitializeAsync()
    {
        var options = new CoreWebView2EnvironmentOptions();
        var environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder, options);

        await webView.EnsureCoreWebView2Async(environment);
        webView.CoreWebView2.Navigate("https://music.youtube.com/");
    }


    // Drag and drop window
    private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }


    // Resize window
    private void ResizeGrip_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            isResizing = true;
            startPoint = e.GetPosition(this);
            Mouse.Capture((UIElement)sender);
        }
    }

    private void ResizeGrip_MouseMove(object sender, MouseEventArgs e)
    {
        if (isResizing)
        {
            Point currentPoint = e.GetPosition(this);
            double deltaX = currentPoint.X - startPoint.X;
            double deltaY = currentPoint.Y - startPoint.Y;

            Width = Math.Max(300, Width + deltaX);
            Height = Math.Max(425, Height + deltaY); 

            startPoint = currentPoint;
        }
    }

    private void ResizeGrip_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            isResizing = false;
            Mouse.Capture(null);
        }
    }


    // Close window
    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    // Minimize window
    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}