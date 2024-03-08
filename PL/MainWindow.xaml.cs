



namespace PL;

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PL.Engineer;
using BlApi;
using System.Windows.Threading;









/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    bool first = true;

    public string CurrentTime
    {
        get { return (string)GetValue(CurrentTimeProperty); }
        set { SetValue(CurrentTimeProperty, value); }
    }

    public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

  
       


public DateTime Date
    {
        get { return (DateTime)GetValue(DateProperty); }
        set { SetValue(DateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register("Date", typeof(DateTime), typeof(MainWindow));


    DispatcherTimer timer = new DispatcherTimer();
    public MainWindow()
    {
        InitializeComponent();
        

        // Create a DispatcherTimer instance
        

        // Set the interval for the timer (1 second in this case)
        timer.Interval = TimeSpan.FromSeconds(1);

        // Define the event handler for the Tick event
        timer.Tick += Timer_Tick!;

        // Start the timer
        timer.Start();


       

        Date = DateTime.Now;
       
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        // Update the CurrentTime property with the current time
        CurrentTime = DateTime.Now.ToString();
    }


    private void Is_Admin(object sender, RoutedEventArgs e) => new Admin().Show();



    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Do you want to save the data?", "Exit",
           MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.No)
            Factory.Get().ResetDB();
    }

    

    private void ChangeDate(object sender, RoutedEventArgs e)
    {
        timer.Stop();
        if (first is true)
        {
            Date = DateTime.Now.Date;
            first = false;
        }
        try
        {
            if (sender is Button button)
            {
                switch (button.Content)
                {
                    case "Add Day":
                        Date = Date.AddDays(1);
                        



                        break;

                    case "Add Month":
                        Date = Date.AddMonths(1);
                        break;

                    case "Add Year":
                        Date = Date.AddYears(1);
                        break;

                    default: break;
                }
            }
        }
        catch { }
       
       CurrentTime= Date.ToString();

    }



}
















