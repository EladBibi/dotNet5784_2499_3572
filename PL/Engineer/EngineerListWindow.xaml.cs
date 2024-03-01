
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.EngineerExperience level { get; set; } = BO.EngineerExperience.None;

    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register(nameof(EngineerList), typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow));
    private void LevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (level == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == level)!;
    }

    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!;

    }
   
    private void Add_Click(object sender, RoutedEventArgs e)
    {

        new EngineerWindow().ShowDialog();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }


    private void Update_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.Engineer? Engineer = (sender as System.Windows.Controls.ListView)?.SelectedItem as BO.Engineer;
        if (Engineer is not null)
        {
            new EngineerWindow(Engineer.Id).ShowDialog();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }





        else
            System.Windows.MessageBox.Show("Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

    }
}
