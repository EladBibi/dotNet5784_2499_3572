using PL.Engineer;
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
using BlApi;
using PL.Task;

namespace PL;

/// <summary>
/// Interaction logic for Admin.xaml
/// </summary>
public partial class Admin : Window
{

    private readonly IBl bl = BlApi.Factory.Get();



    public bool FINISH
    {
        get { return (bool)GetValue(FINISHProperty); }
        set { SetValue(FINISHProperty, value); }
    }

    public static readonly DependencyProperty FINISHProperty =
      DependencyProperty.Register(nameof(FINISH), typeof(bool), typeof(Admin));










    public bool is_all_schedule
    {
        get { return (bool)GetValue(is_all_scheduleProperty); }
        set { SetValue(is_all_scheduleProperty, value); }
    }

    public static readonly DependencyProperty is_all_scheduleProperty =
      DependencyProperty.Register(nameof(is_all_schedule), typeof(bool), typeof(Admin));

    public bool gantt
    {
        get { return (bool)GetValue(ganttProperty); }
        set { SetValue(ganttProperty, value); }
    }

    public static readonly DependencyProperty ganttProperty =
      DependencyProperty.Register(nameof(gantt), typeof(bool), typeof(Admin));








   

    public Admin()
    {
        is_all_schedule = bl.Task.Schedule_date();
        FINISH = bl.Task.finish_project() && bl.GetDate("FinishDate") == DateTime.MinValue;
        gantt = !is_all_schedule;
        InitializeComponent();
        
    }

    private void GanttClick(object sender, RoutedEventArgs e) => new GanttWindow().Show();

    private void btnEngineer_Click(object sender, RoutedEventArgs e)
    { new EngineerListWindow().Show(); }

    private void btTask_Click(object sender, RoutedEventArgs e)
    { new TaskListWindow().ShowDialog();
        is_all_schedule = bl.Task.Schedule_date();
        gantt = !is_all_schedule;
    }

    private void Init_Data(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to initialize the data?", "Init",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
            Factory.Get().InitializeDB();
        is_all_schedule = bl.Task.Schedule_date();
        gantt = !is_all_schedule;
    }

    private void TaskClick(object sender, RoutedEventArgs e)=> new TaskListWindow().Show();
    

        
       
    

        private void Reset_Data(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to rest the data?", "Reset",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
            Factory.Get().ResetDB();
        is_all_schedule = bl.Task.Schedule_date();
    }

    private void Create_Schedule(object sender, RoutedEventArgs e)
    {
        if (bl.Task.time_required() is true)
        {
            MessageBox.Show("Error", "You have not entered required effort time for all tasks!",
              MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        new CreateSchedule().ShowDialog();
        is_all_schedule = bl.Task.Schedule_date();
        gantt = !bl.Task.Schedule_date();
       
    }

   
    private void Finish_Project(object sender, RoutedEventArgs e)
    {
        bl.SetDate(DateTime.Now,"FinishDate");
        MessageBox.Show("Congratulations!");
        this.Close();
       
    }






}
