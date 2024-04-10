using BO;
using DO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for Engineers_Window.xaml
/// </summary>
public partial class Engineers_Window : Window
{

    public IEnumerable<BO.TaskInList> TasksList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TasksListProperty); }
        set { SetValue(TasksListProperty, value); }
    }

    public static readonly DependencyProperty TasksListProperty =
        DependencyProperty.Register(nameof(TasksList), typeof(IEnumerable<BO.TaskInList>), typeof(Engineers_Window));



    public IEnumerable<BO.TaskInList> TasksList_done
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TasksList_doneProperty); }
        set { SetValue(TasksList_doneProperty, value); }
    }

    public static readonly DependencyProperty TasksList_doneProperty =
        DependencyProperty.Register(nameof(TasksList_done), typeof(IEnumerable<BO.TaskInList>), typeof(Engineers_Window));









    int Id = 0;

    static readonly BlApi.IBl bl = BlApi.Factory.Get();
    public Engineers_Window(int id)
    {
        Id = id;
        TasksList = bl.Task.ReadAll(k => k.EngineerId == id && bl.Task.getstatus(k) != BO.Status.Done);
        TasksList_done = bl.Task.ReadAll(k => k.EngineerId == id &&  bl.Task.getstatus(k) == BO.Status.Done);
        InitializeComponent();

    }


    private void open_tasks_list(object sender, RoutedEventArgs e)
    {
        new SuggestedTasks(Id).ShowDialog();
        TasksList = bl.Task.ReadAll(k => k.EngineerId == Id && bl.Task.getstatus(k) != BO.Status.Done);

    }

    private void finish_task(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show($"Are you sure you finished the task?"
             , "Finish task",
             MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {

            if (sender is System.Windows.Controls.ListView listView)
            {
                if ((TaskInList)listView.SelectedItem is not null)
                {
                    int id = ((TaskInList)listView.SelectedItem).Id;
                    bl.Task.finish_task(id);
                    TasksList = bl.Task.ReadAll(k => k.EngineerId == Id && bl.Task.getstatus(k) != BO.Status.Done);
                    TasksList_done = bl.Task.ReadAll(k => k.EngineerId == Id && bl.Task.getstatus(k) == BO.Status.Done);


                }
            }
        }
    }


}


    
   




 
