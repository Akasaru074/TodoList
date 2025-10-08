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

namespace TodoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddTask_Click(object sender, RoutedEventArgs e) {

            if (string.IsNullOrWhiteSpace(todoInp.Text)) return;

            CheckBox newTodo = new CheckBox();
            newTodo.Content = todoInp.Text;
            newTodo.FontSize = 18;
            newTodo.VerticalContentAlignment = VerticalAlignment.Center;
            todosList.Children.Insert(0, newTodo);

            todoInp.Text = "";

        }
    }
}