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

namespace PL;

/// <summary>
/// Interaction logic for Admin.xaml
/// </summary>
public partial class Admin : Window
{
    public Admin()
    {
        InitializeComponent();
    }

    private void GanttClick(object sender, RoutedEventArgs e) => new GanttWindow().Show();

    private void btnEngineer_Click(object sender, RoutedEventArgs e)
    { new EngineerListWindow().Show(); }

    private void btTask_Click(object sender, RoutedEventArgs e)
    { new TaskListWindow().Show(); }

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

}
