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
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{


    public string Username
    {
        get { return (string)GetValue(UsernameProperty); }
        set { SetValue(UsernameProperty, value); }
    }

    public static readonly DependencyProperty UsernameProperty =
      DependencyProperty.Register(nameof(Username), typeof(string), typeof(LoginWindow));


    public string Password
    {
        get { return (string)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    public static readonly DependencyProperty PasswordProperty =
      DependencyProperty.Register(nameof(Password), typeof(string), typeof(LoginWindow));



    public LoginWindow()
    {
        Username = "";
        Password = "";
        InitializeComponent();
    }

    private void PasswordBox_(object sender, RoutedEventArgs e)
    {
        PasswordBox passwordBox = (PasswordBox)sender;
        Password = passwordBox.Password;
    }







    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {

       if(Authenticate(Username,Password) is true)
      
        {
            MessageBox.Show("The operation was successful");
            new Admin().Show();
             MainWindow.first_login = false;
            this.Close();
        }
        else
        {
            MessageBox.Show("Invalid username or password.");
        }
    }

    // פונקציה זו מבצעת אימות של שם משתמש וסיסמה
    private bool Authenticate(string username, string password)
    {
        // כאן אתה יכול להשתמש בכל מיני דרכים לאימות המשתמש, כמו לדוגמה בבדיקה על בסיס מידע חיצוני כמו מסד נתונים
        // לדוגמה, הנה בדיקה קשתונית לצורך הדגמה:
        if (username == "admin" && password == "1234")
        {
            return true;
        }
        else
        {
            return false;
        }
    }


   

        
    

    
}

