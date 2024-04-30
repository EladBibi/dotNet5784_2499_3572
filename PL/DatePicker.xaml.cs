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

namespace PL;

/// <summary>
/// Interaction logic for DatePicker.xaml
/// </summary>
public partial class DatePicker : Window
{
    static readonly BlApi.IBl bl = BlApi.Factory.Get();


   

    public DateTime schedule_start_date
    {
        get { return (DateTime)GetValue(schedule_start_dateProperty); }
        set { SetValue (schedule_start_dateProperty, value); }
    }


    public static readonly DependencyProperty schedule_start_dateProperty =
DependencyProperty.Register(nameof(schedule_start_date), typeof(DateTime), typeof(DatePicker));


    public string s_date
    {
        get { return (string)GetValue(s_dateProperty); }
        set { SetValue(s_dateProperty, value); }
    }


    public static readonly DependencyProperty s_dateProperty =
DependencyProperty.Register(nameof(s_date), typeof(string), typeof(DatePicker));

       



    int eng_Id;
    int id = 0;
    string Date;
    public DatePicker(string date, int i, int eng_id=0)
    {
        eng_Id = eng_id;
         Date = date;
        s_date = "Enter the " + date + " date of the task";
        id = i;
        schedule_start_date = DateTime.Now;
        InitializeComponent();
    }






    private void Add_Sdate(object sender, RoutedEventArgs e)

    {
        
        try
        {
            if (Date == "schedule")
                bl.Task.UpdateDate(schedule_start_date, id, "schedule");
            else
                if (Date == "start")
            {
                bl.Task.UpdateDate(schedule_start_date, id, "start");
             
            }



        }

        catch ( Exception ex)
        {
            MessageBox.Show("Error", ex.Message,
             MessageBoxButton.OK, MessageBoxImage.Error);
            if (Date == "Start")
                bl.Task.update_engineer_id(0, id);
            this.Close();
            return;
        }
       
            
            MessageBox.Show("The operation was successful");
        this.Close();


    }
}
