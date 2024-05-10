



namespace PL.Engineer;


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
/// <summary>
/// using System.Text;

/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    int id;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


   


    public BO.Engineer Engineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
DependencyProperty.Register("Engineer", typeof(BO.Engineer),
    typeof(EngineerWindow), new PropertyMetadata(null));










    public EngineerWindow(int Id = 0)

 {
       
        InitializeComponent();
        
            Engineer = (Id == 0) ? new BO.Engineer() : s_bl.Engineer.Read(Id);
        
        if(Engineer is null)
            MessageBox.Show("Error", $"The ID:{Id}  entered does not exist in the system",
                MessageBoxButton.OK, MessageBoxImage.Error);



        id = Id;
    }
   

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        
        try
        {
            if (id==0)
                s_bl.Engineer.Create(Engineer);
            else
                s_bl.Engineer.Update(Engineer);
        }
        catch (Exception ex) { 
        
            MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
            return;
        }
        MessageBox.Show("The operation was successful");
            
        this.Close();

    }

    private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        TextBox? text = sender as TextBox;
        if (text == null) return;
        if (e == null) return;

        //allow get out of the text box
        if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
            return;

        //allow list of system keys (add other key here if you want to allow)
        if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
            e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
         || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
            return;

        char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

        //allow control system keys
        if (Char.IsControl(c)) return;

        //allow digits (without Shift or Alt)
        if (Char.IsDigit(c))
            if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                return; //let this key be written inside the textbox

        //forbid letters and signs (#,$, %, ...)
        e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
        return;
    }












}










