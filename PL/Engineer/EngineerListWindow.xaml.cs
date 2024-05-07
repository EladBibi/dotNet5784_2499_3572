
namespace PL.Engineer;

using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{

    public bool FINISH
    {
        get { return (bool)GetValue(FINISHProperty); }
        set { SetValue(FINISHProperty, value); }
    }

    public static readonly DependencyProperty FINISHProperty =
      DependencyProperty.Register(nameof(FINISH), typeof(bool), typeof(EngineerListWindow));











    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.EngineerExperience_List level { get; set; } = BO.EngineerExperience_List.None;

    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register(nameof(EngineerList), typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow));



    private void LevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (level == BO.EngineerExperience_List.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == (BO.EngineerExperience)level)!;
    }

    public EngineerListWindow()
    {
        FINISH = s_bl.GetDate("FinishDate") == DateTime.MinValue;
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!;

    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {

        new EngineerWindow().ShowDialog();
        EngineerList = s_bl?.Engineer.ReadAll()!;
        this.Close();
    }

    private void engineer_Click(object sender, MouseButtonEventArgs e)
    {
        BO.Engineer? Engineer = (sender as System.Windows.Controls.ListView)?.SelectedItem as BO.Engineer;
        if (Engineer is not null)
        
          new Engineers_Window(Engineer.Id,false).ShowDialog();

        
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
    private void delete_engineer(object sender, RoutedEventArgs e)
    {
        BO.Engineer? engineer = (sender as System.Windows.Controls.ListView)?.SelectedItem as BO.Engineer;
        if (engineer is not null)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show($"Are you sure you want to delete the task: " +
                $"{engineer.Id}", "Delete",
               MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {


                try
                {
                    s_bl.Engineer.Delete(engineer.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error", ex.Message,
                 MessageBoxButton.OK, MessageBoxImage.Error);

                }
                EngineerList = s_bl.Engineer.ReadAll();
            }

        }
    }
    

}