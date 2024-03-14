using BlApi;
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
using BO;


namespace PL;

/// <summary>
/// Interaction logic for update_dependency.xaml
/// </summary>
public partial class update_dependency : Window
{

 

    int id;

    private readonly IBl bl = BlApi.Factory.Get();
    public update_dependency(int ID)
    {
         id = ID;
       
        InitializeComponent();
    }


    private void delete_(object sender, RoutedEventArgs e)
    {

        int id_d = 0;
        string? userInput = null;

        while (userInput != "")
        {
            userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter the id of the task for delete:", "Delete", "");

            if (userInput == "")
                break;
            if (int.TryParse(userInput, out id_d) is false)
            {
                MessageBox.Show("Error", "Invalid Id for the task",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                userInput = null;
            }
            else
                userInput = "";

            if (userInput is not null)
            {
                try
                {
                    bl.Task.DeleteDependency(id, id_d);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error", ex.Message,
                         MessageBoxButton.OK, MessageBoxImage.Error);
                    userInput = null;

                }



            }
            if (userInput == "")
                MessageBox.Show("The operation was successful");
        }

        this.Close();

    }



    private void  add_(object sender, RoutedEventArgs e)
    {
        int id_d = 0;
        string? userInput = null;

        while (userInput != "") { 
            userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter the id of the task you want to add:", "Add", "");

            if (userInput == "")
                break;
            if (int.TryParse(userInput, out id_d) is false)
            {
                MessageBox.Show("Error", "Invalid Id for the task",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                userInput = null;
            }
            else
                userInput = "";
               
            if (userInput is not null)
                {
                    try
                    {
                        bl.Task.AddDependency(id, id_d);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error", ex.Message,
                             MessageBoxButton.OK, MessageBoxImage.Error);
                    userInput = null;

                }
                


            }
               if (userInput == "")
                MessageBox.Show("The operation was successful");
        }

        this.Close();
    }
   
       


    






}
