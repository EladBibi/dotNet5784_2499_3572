using BO;
using PL.Engineer;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL;

/// <summary>
/// Interaction logic for CreateSchedule.xaml
/// </summary>
/// 
public partial class CreateSchedule : Window
{


    public DateTime StartDate
    {
        get { return (DateTime)GetValue(StartDateProperty); }
        set { SetValue(StartDateProperty, value); }
    }

    public static readonly DependencyProperty StartDateProperty =
      DependencyProperty.Register(nameof(StartDate), typeof(DateTime), typeof(CreateSchedule));















    public bool StartDateEnabled
    {
        get { return (bool)GetValue(StartDateEnabledProperty); }
        set { SetValue(StartDateEnabledProperty, value); }
    }

    public static readonly DependencyProperty StartDateEnabledProperty =
      DependencyProperty.Register(nameof(StartDateEnabled), typeof(bool), typeof(CreateSchedule));









    public BO.Task? Task
    {
        get { return (BO.Task)GetValue(TaskProperty); }
        set { SetValue(TaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskProperty =
 DependencyProperty.Register(nameof(Task), typeof(BO.Task), typeof(CreateSchedule));









    static readonly BlApi.IBl bl = BlApi.Factory.Get();

    public IEnumerable<BO.TaskInList> TasksList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }


    public static readonly DependencyProperty TaskListProperty =
      DependencyProperty.Register(nameof(TasksList), typeof(IEnumerable<BO.TaskInList>), typeof(CreateSchedule));




    bool X;

    public CreateSchedule()
    {
        X = true;
        StartDate = bl.Clock;
        StartDateEnabled = false;
        TasksList = bl.Task.ReadAll();
        InitializeComponent();
    }



   

    

    private void Add_Date(object sender, RoutedEventArgs e)
    {

        if (StartDate.Date < bl.Clock.Date)
        {
            MessageBox.Show("Error", "The date you entered has already passed",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        bl.SetDate(StartDate, "StartDate");

        StartDateEnabled = true;
        if (sender != null && sender is Button)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;
        }
        MessageBox.Show("Please enter the Scheduled Dates for the tasks by double click:");

    }




  

        private void add_date(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (sender is System.Windows.Controls.ListView listView)
            {
                id = ((TaskInList)listView.SelectedItem!).Id;
                new DatePicker("schedule",id).ShowDialog();
            }

        }
            private void create_schedule(object sender, RoutedEventArgs e)

    {
        if (bl.Task.Schedule_date() is true)
        {
            MessageBox.Show("Error", "You have not entered scheduled dates for all tasks!",
              MessageBoxButton.OK, MessageBoxImage.Error);
            return;

        }

      



        if (bl.GetDate("StartDate") == DateTime.MinValue)
        {
            MessageBox.Show("Error", "You have not entered start date for the project!",
          MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }




        X = false;
            this.Close();


    }




    private void create_schedule_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {

        if (X is true)
        { 

        MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the data?", "Exit",
         MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bl.SetDate(DateTime.MinValue, "StartDate");
                bl.Task.reset_schdule_date();
            }
            else
                e.Cancel = true;
    }
    
}




}







     











                      








