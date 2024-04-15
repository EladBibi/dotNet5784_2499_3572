using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BlApi;
using BO;
using PL.Engineer;

namespace PL.Task;

/// <summary>
/// Interaction logic for EditTask.xaml
/// </summary>
public partial class EditTask : Window
{






    private readonly IBl bl = BlApi.Factory.Get();


    public static readonly DependencyProperty startdateProperty =
DependencyProperty.Register(nameof(startdate), typeof(DateTime?), typeof(EditTask));



    public DateTime? startdate
    {
        get { return (DateTime?)GetValue(startdateProperty); }
        set { SetValue(startdateProperty, value); }
    }












    public static readonly DependencyProperty dependency_idProperty =
DependencyProperty.Register(nameof(dependency_id), typeof(int?), typeof(EditTask));



    public int? dependency_id
    {
        get { return (int?)GetValue(dependency_idProperty); }
        set { SetValue(dependency_idProperty, value); }
    }






    public bool DependencyEnabled
    {
        get { return (bool)GetValue(DependencyEnabledProperty); }
        set { SetValue(DependencyEnabledProperty, value); }
    }

    public static readonly DependencyProperty DependencyEnabledProperty =
      DependencyProperty.Register(nameof(DependencyEnabled), typeof(bool), typeof(EditTask));

    public bool AddMode
    {
        get { return (bool)GetValue(AddModeProperty); }
        set { SetValue(AddModeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AddMode.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AddModeProperty =
        DependencyProperty.Register(nameof(AddMode), typeof(bool), typeof(EditTask));





    public bool Showdialog
    {
        get { return (bool)GetValue(ShowDialogProperty); }
        set { SetValue(ShowDialogProperty, value); }
    }

    // Using a DependencyProperty as the backing store for AddMode.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowDialogProperty =
        DependencyProperty.Register(nameof(ShowDialog), typeof(bool), typeof(EditTask));

    public BO.Task? Task
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


    public IEnumerable<BO.EngineerInTask?> EngineerList
    {
        get { return (IEnumerable<BO.EngineerInTask?>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register(nameof(EngineerList), typeof(IEnumerable<BO.EngineerInTask?>), typeof(EditTask));







    int ID;
    public EditTask(int id = 0)
    {
        ID = id;
        startdate = bl.GetDate("StartDate");
        Showdialog = false;
        DependencyEnabled = false;

        AddMode = (id == 0);
        Task = (AddMode is true) ? new BO.Task() : bl.Task.Read(id);


        if (Task is null)
            MessageBox.Show("Error", $"The ID:{id}  entered does not exist in the system",
                MessageBoxButton.OK, MessageBoxImage.Error);

        if (Task!.Dependencies is null)
            Task.Dependencies = new List<BO.TaskInList>();

        if (Task.Engineer is null)
            Task.Engineer = new BO.EngineerInTask();

        EngineerList = bl.Engineer.Read_Engineer_In_Task();

        InitializeComponent();
    }



    private void AddUpdate_Click(object sender, RoutedEventArgs e)
    {

        try
        {
            if (AddMode)
                bl.Task.Create(Task!);
            else
                bl.Task.Update(Task!);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error", ex.Message,
          MessageBoxButton.OK, MessageBoxImage.Error); this.Close(); return;

        }

        DependencyEnabled = true;

        (sender as Button)!.IsEnabled = false;
        if (Task!.Id != 0)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to update the dependencies?", "Update",
                  MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                new update_dependency(Task!.Id).Show();

            this.Close();

        }
    }


    private void Close_Window(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("The operation was successful");
        this.Close();
    }














    private void OpenDialoge(object sender, RoutedEventArgs e)
    {
        TasksList = bl.Task.ReadAll();
        Showdialog = true;
        DependencyEnabled = true;
    }




    private void CheckBox_Click(object sender, RoutedEventArgs e)
    {
        CheckBox? checkBox = sender as CheckBox;
        if (checkBox!.IsChecked == true)
        {
            Task!.Dependencies!.Add((checkBox.Tag as TaskInList)!);
            //TasksList.Add((checkBox.Tag as TaskInList)!);
        }
        else
        {
            Task!.Dependencies!.Remove((checkBox.Tag as TaskInList)!);
            //TasksList.Remove((checkBox.Tag as TaskInList)!);
        }
    }







    private void Add_Dependency_check(object sender, DataGridCellEditEndingEventArgs e)
    {
        int id = 0;
        // הבדיקה אם האירוע הופעל על ידי סיום עריכה של תא ב- DataGrid
        if (e.EditAction == DataGridEditAction.Commit)

            // אם זה מתאים לשדה המתאים שאתה רוצה לבדוק את הקלט שלו
            if (e.Column.Header.Equals("Id"))
            { // לשים לב שאתה צריך להשוות לכותרת המתאימה

                var editedTextBox = e.EditingElement as TextBox;
                if (editedTextBox != null)
                {
                    // בדיקת תקינות הקלט לפני שהוא משתמש ב- Binding
                    if (!string.IsNullOrEmpty(editedTextBox.Text))
                    {
                        if (!int.TryParse(editedTextBox.Text, out id) || id < 0)
                        {


                            MessageBox.Show("Error", "Invalid Id. Please enter a positive integer",
                         MessageBoxButton.OK, MessageBoxImage.Error);

                            // ביטול העריכה אם הקלט לא תקין
                            editedTextBox.Undo();
                            e.Cancel = true;
                            return;
                        }

                        if (id != 0)
                        {
                            try
                            {
                                int task_id = Task!.Id;
                                if (task_id == 0)
                                    task_id = bl.get_task_id();


                                bl.Task.AddDependency(task_id, id);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error", ex.Message,
                              MessageBoxButton.OK, MessageBoxImage.Error);
                                editedTextBox.Undo();
                                e.Cancel = true;
                            }
                        }

                    }

                }

            }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
    {

    }

    private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
    {

    }
}





















