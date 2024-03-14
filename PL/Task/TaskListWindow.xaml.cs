
using BO;
using PL.Task;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();
   
    public IEnumerable<BO.TaskInList> TasksList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }


    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register(nameof(TasksList), typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow));








    private void Status_Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       TasksList = ((status == BO.Status.None) ?
            bl?.Task.ReadAll()! : bl.Task.ReadAll(item => bl.Task.getstatus(item) == status)!);
    }

    public BO.Status status { get; set; } = BO.Status.None;

    public TaskListWindow()
    {
        TasksList = bl.Task.ReadAll();
        

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

        TasksList = bl.Task.ReadAll();
    }


    



}
