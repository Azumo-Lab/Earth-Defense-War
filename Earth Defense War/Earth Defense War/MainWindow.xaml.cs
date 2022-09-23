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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Earth_Defense_War
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        #region 游戏界面的初始化
        
        public void InitGame()
        {
            Button ExitButton = new Button()
            {
                Content = "退出",
                Height = 50,
                Width = 100,
            };
            ExitButton.Click += new RoutedEventHandler((obj, e) =>
            {
                MessageBox.Show("退出");
                Close();
            });
            GameCanvas.AddToCanvas(ExitButton);
            Canvas.SetLeft(ExitButton, 100);
            Canvas.SetTop(ExitButton, 100);
        }
        #endregion
    }
}
