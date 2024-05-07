using BlApi;
using BO;
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
/// Interaction logic for GanttWindow.xaml
/// </summary>
public partial class GanttWindow : Window
{
    private readonly IBl bl = Factory.Get();
    public IEnumerable<TaskInGantt> GanttList
    {
        get { return (IEnumerable<TaskInGantt>)GetValue(MyPropertyProperty); }
        set { SetValue(MyPropertyProperty, value); }
    }

    // Using a DependencyProperty as the backing store for OpenDialoge.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyPropertyProperty =
        DependencyProperty.Register("GanttList", typeof(IEnumerable<TaskInGantt>), typeof(GanttWindow));

    public GanttWindow()
    {
        //bl.Task.ScheduleTasks(DateTime.Now);
        GanttList = bl.Task.GanttList(bl.Clock);
        InitializeComponent();
    }
}
