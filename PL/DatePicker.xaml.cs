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


   

    public DateTime scheduledate
    {
        get { return (DateTime)GetValue(scheduledateProperty); }
        set { SetValue (scheduledateProperty, value); }
    }


    public static readonly DependencyProperty scheduledateProperty =
DependencyProperty.Register(nameof(scheduledate), typeof(DateTime), typeof(DatePicker));



    

       



    int id = 0;

    public DatePicker(int i)
    {

        id = i;
        scheduledate = DateTime.Now;
        InitializeComponent();
    }






    private void Add_Sdate(object sender, RoutedEventArgs e)

    {
        
        try
        {
            bl.Task.UpdateDate(scheduledate, id);
        }

        catch ( Exception ex)
        {
            MessageBox.Show("Error", ex.Message,
             MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        MessageBox.Show("The operation was successful");
        this.Close();


    }
}
