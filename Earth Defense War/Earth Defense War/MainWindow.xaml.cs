using Earth_Defense_War.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

        private Timer GameTimer = new Timer();

        #region 游戏中的事件

        private RoutedEventHandler Btn_GameStart { get; set; }
        private RoutedEventHandler Btn_GameEnd { get; set; }
        private RoutedEventHandler Btn_GameSetting { get; set; }

        #endregion

        #region 游戏按钮
        private Button StartGame_Btn = new Button
        {
            Content = "开始游戏",
            Height = 50,
            Width = 100,
        };
        private Button CloseGame_Btn = new Button
        {
            Content = "关闭",
            Height = 50,
            Width = 100,
        };
        private Button SettingGame_Btn = new Button
        {
            Content = "设置",
            Height = 50,
            Width = 100,
        };
        #endregion

        #region 游戏中的基本信息

        private string GameTitle = string.Empty; //游戏标题

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Btn_GameStart = new RoutedEventHandler((obj, e) =>
            {
                GameCanvas.Clear();
            });
            Btn_GameEnd = new RoutedEventHandler((obj, e) =>
            {
                Close();
            });
            Btn_GameSetting = new RoutedEventHandler((obj, e) => 
            {
                GameSetting gameSetting = new GameSetting();
                gameSetting.ShowDialog();
            });

            InitGame();
        }

        #region 游戏界面的初始化

        /// <summary>
        /// 游戏菜单的初始化
        /// </summary>
        public void InitGame()
        {
            InitMenu();
        }

        #endregion

        #region 游戏中的各种逻辑

        #region 游戏开始菜单
        /// <summary>
        /// 初始化菜单
        /// </summary>
        public void InitMenu()
        {
            CloseGame_Btn.Click += Btn_GameEnd;
            StartGame_Btn.Click += Btn_GameStart;
            SettingGame_Btn.Click += Btn_GameSetting;

            GameCanvas.AddToCanvas(CloseGame_Btn, 100, 100);
            GameCanvas.AddToCanvas(StartGame_Btn, 100, 200);
            GameCanvas.AddToCanvas(SettingGame_Btn, 100, 300);
        }
        #endregion

        #region 游戏控制
        /// <summary>
        /// 游戏暂停
        /// </summary>
        public void GamePause()
        {

        }
        /// <summary>
        /// 游戏继续
        /// </summary>
        public void GameContinue()
        {

        }
        #endregion

        #region 开始游戏
        public void GameStart()
        {
            // 清除界面元素
            GameCanvas.Clear();

            // 添加游戏元素
        }
        #endregion

        #endregion
    }
}
