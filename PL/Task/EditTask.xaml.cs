using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BlApi;
using BO;
using PL.Engineer;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        private readonly IBl bl = BlApi.Factory.Get();

        public bool AddMode
        {
            get { return (bool)GetValue(AddModeProperty); }
            set { SetValue(AddModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddModeProperty =
            DependencyProperty.Register(nameof(AddMode), typeof(bool), typeof(EditTask));


        public bool ShowDialog
        {
            get { return (bool)GetValue(ShowDialogProperty); }
            set { SetValue(ShowDialogProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowDialogProperty =
            DependencyProperty.Register(nameof(ShowDialog), typeof(bool), typeof(EditTask));

        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register(nameof(Task), typeof(BO.Task), typeof(EditTask));

        public IEnumerable<BO.TaskInList> TasksList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }

        public static readonly DependencyProperty TasksListProperty =
            DependencyProperty.Register(nameof(TasksList), typeof(IEnumerable<BO.TaskInList>), typeof(EditTask));

        public EditTask(int id = 0)
        {
            ShowDialog = false;
            AddMode = id == 0;
            if (!AddMode)
                Task = bl.Task.Read(id) ?? new();

            if (Task.Dependencies is null)
                Task.Dependencies = new List<BO.TaskInList>();

            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AddMode)
                    bl.Task.Create(Task);
                else
                    bl.Task.Update(Task);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            MessageBox.Show("succeful update");
            this.Close();
        }

        private void OpenDialoge(object sender, RoutedEventArgs e)
        {
            TasksList = bl.Task.ReadAll();
            ShowDialog = true;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox? checkBox = sender as CheckBox;
            if (checkBox!.IsChecked == true)
            {
                Task.Dependencies!.Add((checkBox.Tag as TaskInList)!);
                //TasksList.Add((checkBox.Tag as TaskInList)!);
            }
            else
            {
                Task.Dependencies!.Remove((checkBox.Tag as TaskInList)!);
                //TasksList.Remove((checkBox.Tag as TaskInList)!);
            }
        }

        private void AddDependency(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            try
            {
                if (sender is ListView listView)
                {
                    TaskInList selected = (TaskInList)listView.SelectedItem;
                    Task.Dependencies!.Add(selected);
                    BO.Task tmp = Task;
                    Task = null;
                    Task = tmp;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}
