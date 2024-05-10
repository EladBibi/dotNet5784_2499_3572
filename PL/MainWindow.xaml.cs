



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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Markup;
using PL.Task;









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

  
       




    // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DateProperty =
        DependencyProperty.Register("Date", typeof(DateTime), typeof(MainWindow));


    DispatcherTimer timer = new DispatcherTimer();
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static public bool first_login { get; set; }
    DateTime SistemDate;
    
    public MainWindow()
    {
        first_login = true;
        InitializeComponent();
       
        SistemDate = s_bl.Get_Sistem_Date();


        // Create a DispatcherTimer instance


        // Set the interval for the timer (1 second in this case)
        timer.Interval = TimeSpan.FromSeconds(1);

        // Define the event handler for the Tick event
        timer.Tick += Timer_Tick!;

        // Start the timer
        timer.Start();       
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        // Update the CurrentTime property with the current time
        s_bl.AddSecond();
        SistemDate = SistemDate.AddSeconds(1);
        CurrentTime = SistemDate.ToString();
    }


    


    private void Is_Admin(object sender, RoutedEventArgs e)
    {
        if (first_login is true)
           
            new LoginWindow().ShowDialog();





        else

            new Admin().Show();






    }


    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
       

      if (s_bl.getDataBase() == "xml")
        {

            MessageBoxResult result_1 = MessageBox.Show("Do you want to save the data?", "Exit",
               MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result_1 == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            
                if (result_1 == MessageBoxResult.No)
                    Factory.Get().ResetDB();

            MessageBoxResult result = MessageBox.Show("Do you want to save the date?", "Exit",
         MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                s_bl.Set_Sistem_Date(s_bl.Clock);
            else
                s_bl.Set_Sistem_Date(DateTime.Now);

        }
    }





    private void is_engineer(object sender, RoutedEventArgs e)
    {
        string userInput = "";
        int id;
        
        
            userInput = Microsoft.VisualBasic.Interaction.InputBox("Please enter your id:", "Enter", "");
        if (userInput != "")
        {
            if (int.TryParse(userInput, out id) is false || s_bl.Task.check_id_engineer(id) is true)
            {
                MessageBox.Show("Error", "The data you entered is incorrect",
    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            new Engineers_Window(id).Show();
        }
        



    }
   


    private void ChangeDate(object sender, RoutedEventArgs e)
    {
        //timer.Stop();
        //if (first is true)
        //{
           // s_bl.InitializeTime();
           // first = false;
       // }

        try
        {
            if (sender is Button button)
            {
                switch (button.Content)
                {
                    case "Add Day":
                       
                        s_bl.AddDay();
                        
                        break;

                    case "Add Month":
                        s_bl.AddMonth();
                        break;

                    case "Add Year":
                        s_bl.AddYear(); 
                        break;
                    case "Initialize Date":
                        s_bl.InitializeTime();
                        break;

                    default: break;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error", ex.Message,
          MessageBoxButton.OK, MessageBoxImage.Error); return;

        }
        SistemDate = s_bl.Clock;
        CurrentTime = s_bl.Clock.ToString();

    }
    


}
















