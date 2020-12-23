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
            DataContext = new GameEngine(this);
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

        //private void W1_Btn_Ok_Click(object sender, EventArgs e)
        //{
        //    Btn_Exit.IsEnabled = true;
        //    ClearBlur();
        //    dialogWindow.Close();
        //}

        public void ApplyBlur()
        {
            var blurEffect = new BlurEffect() { Radius = 15 };
            Effect = blurEffect;
        }

        public void ClearBlur()
        {
            Effect = null;
        }

        public void DesableBtnExit()
        {
            Btn_Exit.IsEnabled = false;
        }

        public void EnableBtnExit()
        {
            Btn_Exit.IsEnabled = true;
        }

        private void MW_Loaded(object sender, RoutedEventArgs e)
        {
            //TextBox.Text = "СЧЁТ : ";
        }

        private void MW_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Space) ApplyBlur(); 
        }

        private void MW_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MW_B_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void canvas_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Btn_Restart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
