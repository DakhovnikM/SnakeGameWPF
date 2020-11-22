using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnakeGameWPF.Views
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        readonly MainWindow _mainWindow;
        public string DialogContent { get; set; }
        public DialogWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            DialogContent = "Старт/Стоп - клавиша Пробел.\nДвижение стрелками.";
            Label.Content = DialogContent;
            Button.Content = "OK";
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            _mainWindow.ClearBlur();
            _mainWindow.Btn_Exit.IsEnabled = true;
        }

    }
}
