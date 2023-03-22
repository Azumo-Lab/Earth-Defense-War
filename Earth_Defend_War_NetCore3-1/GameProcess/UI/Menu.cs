//    < Earth - Defense - War >
//    Copyright(C) < 2022 >  < Azumo - Lab >
//    <https://github.com/Azumo-Lab/Earth-Defense-War>

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Earth_Defend_War_NetCore3_1.GameProcess.UI
{
    /// <summary>
    /// 这个是菜单界面
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 点击开始按钮时候的事件
        /// </summary>
        public delegate void StartClick();
        public event StartClick StartClickEven;

        /// <summary>
        /// 点击设置按钮时候的事件
        /// </summary>
        public delegate void SettingClick();
        public event SettingClick SettingClickEven;

        /// <summary>
        /// 点击结束按钮时候的事件
        /// </summary>
        public delegate void EndClick();
        public event EndClick EndClickEven;

        /// <summary>
        /// 点击DEBUG按钮时候的事件
        /// </summary>
        public delegate void DebugClick();
        public event DebugClick DebugClickEven;

        /// <summary>
        /// 
        /// </summary>
        private Timer Timer { get; set; }

        /// <summary>
        /// 执行间隔的事件
        /// </summary>
        private int interval;
        public int Interval { get { return interval; } set { interval = value; Timer.Interval = value; } }

        /// <summary>
        /// 初始化
        /// </summary>
        protected Menu()
        {
            Timer = new Timer();
        }

        /// <summary>
        /// 
        /// </summary>
        private static Menu This { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Menu GetMenu()
        {
            if (This == null)
            {
                This = new Menu();
            }
            return This;
        }

        /// <summary>
        /// 游戏开始时的界面
        /// </summary>
        public void GameStartMenu(Canvas canvas)
        {
            //清空界面
            canvas.Clear();
            canvas.Background = System.Windows.Media.Brushes.White;

            //界面闪死你
            var Elsp = new ElapsedEventHandler((obj, e) =>
            {
                canvas.Dispatcher.Invoke(() =>
                {
                    var now = DateTime.Now.Millisecond.ToString();
#if !DEBUG
                    switch (now.LastOrDefault())
                    {
                        case '0':
                            canvas.Background = System.Windows.Media.Brushes.Gold;
                            break;
                        case '1':
                            canvas.Background = System.Windows.Media.Brushes.LightPink;
                            break;
                        case '2':
                            canvas.Background = System.Windows.Media.Brushes.MediumVioletRed;
                            break;
                        case '3':
                            canvas.Background = System.Windows.Media.Brushes.Purple;
                            break;
                        case '4':
                            canvas.Background = System.Windows.Media.Brushes.Silver;
                            break;
                        case '5':
                            canvas.Background = System.Windows.Media.Brushes.Violet;
                            break;
                        case '6':
                            canvas.Background = System.Windows.Media.Brushes.White;
                            break;
                        case '7':
                            canvas.Background = System.Windows.Media.Brushes.RoyalBlue;
                            break;
                        case '8':
                            canvas.Background = System.Windows.Media.Brushes.DarkGray;
                            break;
                        case '9':
                            canvas.Background = System.Windows.Media.Brushes.Aqua;
                            break;
                        default:
                            break;
                    }
#endif
                });
            });
            Timer.Elapsed += Elsp;

            var Height = 30;
            var Width = 100;
            var Margin = new Thickness(5, 5, 5, 5);
            //添加按钮组
            var startbtn = new Button { Content = "开始", Height = Height, Width = Width, Margin = Margin };
            startbtn.Click += new RoutedEventHandler((obj, e) =>
            {
                StartClickEven?.Invoke();
            });
            var SettingBtn = new Button { Content = "设置", Height = Height, Width = Width, Margin = Margin };
            SettingBtn.Click += new RoutedEventHandler((obj, e) =>
            {
                SettingClickEven?.Invoke();
            });
            var EndGame = new Button { Content = "结束游戏", Height = Height, Width = Width, Margin = Margin };
            EndGame.Click += new RoutedEventHandler((obj, e) =>
            {
                GameStartMenuClose();
                EndClickEven?.Invoke();
            });

            StackPanel stackPanel = new StackPanel()
            {
                Width = 150,
                Height = 0,
            };
            stackPanel.Children.Add(startbtn);
            stackPanel.Children.Add(SettingBtn);
            stackPanel.Children.Add(EndGame);
            foreach (Button item in stackPanel.Children)
            {
                stackPanel.Height = item.Height + stackPanel.Height + item.Margin.Top + item.Margin.Bottom;
            }

#if DEBUG
            //如果是DEBUG，那么将会添加DEBUG按钮
            var debugbtn = new Button() { Content = "Debug" };
            debugbtn.Click += new RoutedEventHandler((obj, e) =>
            {
                DebugClickEven?.Invoke();
            });
            canvas.Children.Add(debugbtn);
#endif

            Canvas.SetLeft(stackPanel, (canvas.Width - stackPanel.Width) / 2);
            Canvas.SetTop(stackPanel, (canvas.Height - stackPanel.Height) / 2);

            _ = canvas.Children.Add(stackPanel);

            //开始闪
            Timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GameStartMenuClose()
        {
            Timer.Dispose();
            This = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GameMenuStop()
        {
            Timer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GameMenuStart()
        {
            Timer.Start();
        }
    }
}
