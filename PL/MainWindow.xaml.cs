



namespace PL
{
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


    /// <summary>


    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(MainWindow));



        public MainWindow()
        {
            InitializeComponent();

            Date = DateTime.Now;
        }

        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        { new EngineerListWindow().Show(); }

        private void Init_Data(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to initialize the data?", "Init",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Factory.Get().InitializeDB();
        }

        private void TaskClick(object sender, RoutedEventArgs e) => new TaskListWindow().Show();

        private void Reset_Data(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to rest the data?", "Reset",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Factory.Get().ResetDB();
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save the data?", "Exit",
               MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                Factory.Get().ResetDB();
        }

        private void GanttClick(object sender, RoutedEventArgs e) => new GanttWindow().Show();

        private void ChangeDate(object sender, RoutedEventArgs e)
        {
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
        }
    }

}
















