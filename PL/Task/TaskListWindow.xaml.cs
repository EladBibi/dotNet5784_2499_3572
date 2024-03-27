
using BO;
using PL.Task;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Globalization;
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
    








    private void delete_task(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? task = (sender as System.Windows.Controls.ListView)?.SelectedItem as BO.TaskInList;
        if(task is not null) {
        MessageBoxResult result = System.Windows.MessageBox.Show($"Are you sure you want to delete the task: {task.Id}", "Delete",
           MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            
                
                try
                {
                    bl.Task.Delete(task.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error", ex.Message,
                 MessageBoxButton.OK, MessageBoxImage.Error); 

                }
                TasksList = bl.Task.ReadAll();
            }

        }
    }
   




}
