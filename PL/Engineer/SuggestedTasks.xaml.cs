using BO;
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
/// Interaction logic for SuggestedTasks.xaml
/// </summary>
public partial class SuggestedTasks : Window
{
  

        public bool startdate
    {
        get { return (bool)GetValue(startdateProperty); }
        set { SetValue(startdateProperty, value); }
    }

    public static readonly DependencyProperty startdateProperty =
        DependencyProperty.Register(nameof(startdate), typeof(bool), typeof(SuggestedTasks));



    public IEnumerable<BO.TaskInList> TasksList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TasksListProperty); }
        set { SetValue(TasksListProperty, value); }
    }

    public static readonly DependencyProperty TasksListProperty =
        DependencyProperty.Register(nameof(TasksList), typeof(IEnumerable<BO.TaskInList>), typeof(SuggestedTasks));



    int Id = 0;
    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public SuggestedTasks(int id)
    {
        Id = id;
        startdate = bl.GetDate("StartDate") != DateTime.MinValue;

        TasksList = bl.Task.ReadAll(k=> bl.Task.list_task_for_engineer(k,id));
        InitializeComponent();
    }



   
    private void start_task(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to start work on the task?", "Task",
                   MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            int id = 0;
            if (sender is System.Windows.Controls.ListView listView)
            {
                id = ((TaskInList)listView.SelectedItem!).Id;
                try { bl.Task.update_engineer_id(Id, id); }
                
                 catch (Exception ex)
                {
                    MessageBox.Show("Error", ex.Message,
                     MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                bl.Task.UpdateDate(DateTime.Now.Date, id, "start");
                MessageBox.Show("The operation was successful");
                this.Close();


            }


        }

    }

    private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {

    }
}
