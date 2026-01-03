using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace UnrealProjectHub.Controls;

public partial class LogsOverlay
{
    private bool _isExpanded;

    public ObservableCollection<string> Logs
    {
        get => (ObservableCollection<string>)GetValue(LogsProperty);
        set => SetValue(LogsProperty, value);
    }
        
    public static readonly DependencyProperty LogsProperty = DependencyProperty.Register(
        nameof(Logs),
        typeof(ObservableCollection<string>),
        typeof(LogsOverlay),
        new PropertyMetadata(null, RegisterLastLogRefresh)
    );

    private const float ExpandedHeight = 160;
    private const float DefaultHeigh = 32;
    
    public LogsOverlay()
    {
        InitializeComponent();
    }

    private void RefreshLastLog(object? _, NotifyCollectionChangedEventArgs __)
    {
        RefreshLastLog();
    }

    private static void RegisterLastLogRefresh(DependencyObject dependency, DependencyPropertyChangedEventArgs arguments)
    {
        LogsOverlay control = (LogsOverlay)dependency;

        if (arguments.OldValue is ObservableCollection<string> oldLogs)
        {
            oldLogs.CollectionChanged -= control.RefreshLastLog;
        }

        if (arguments.NewValue is ObservableCollection<string> newLogs)
        {
            newLogs.CollectionChanged += control.RefreshLastLog;
        }

        control.RefreshLastLog();
    }

    private void RefreshLastLog()
    {
        if (Logs.Count == 0)
        {
            LogsList.ItemsSource = null;
            LastLogText.Text = "";
            return;
        }

        LastLogText.Text = Logs.Last();
        LogsList.ItemsSource = Logs;
    }

    private void ToggleOverlayExpansion(object _, RoutedEventArgs __)
    {
        _isExpanded = !_isExpanded;

        if (_isExpanded)
        {
            CollapsedView.Visibility = Visibility.Collapsed;
            ExpandedView.Visibility = Visibility.Visible;
            Root.Height = ExpandedHeight;
        }
        else
        {
            ExpandedView.Visibility = Visibility.Collapsed;
            CollapsedView.Visibility = Visibility.Visible;
            Root.Height = DefaultHeigh;
        }
    }
}