
using BO;
using PL.Task;
using System.Collections.ObjectModel;
using System.Windows;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public ObservableCollection<BO.TaskInList> TasksList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register(nameof(TasksList), typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow));

    public TaskListWindow()
    {
        TasksList = new(bl.Task.ReadAll());

        InitializeComponent();

    }

    private void EditTask(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.ListView listView)
        {
            int id = ((TaskInList)listView.SelectedItem!).Id;
            new EditTask(id).ShowDialog();
        }
        else
            new EditTask().ShowDialog();

        TasksList = new(bl.Task.ReadAll());
    }
}
