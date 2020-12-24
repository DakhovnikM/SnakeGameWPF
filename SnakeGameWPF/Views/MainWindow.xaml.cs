using SnakeGameWPF.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace SnakeGameWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DialogWindow dialogWindow;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            dialogWindow = new DialogWindow(this);

            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dialogWindow.Owner = this;
            Btn_Exit.IsEnabled = false;
            ApplyBlur();
            dialogWindow.Show();
        }

        public void ApplyBlur()
        {
            var blurEffect = new BlurEffect() { Radius = 15 };
            Effect = blurEffect;
        }

        public void ClearBlur()
        {
            Effect = null;
        }

        private void MW_Loaded(object sender, RoutedEventArgs e)
        {
            //TextBox.Text = "СЧЁТ : ";
        }

        private void MW_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MW_B_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
