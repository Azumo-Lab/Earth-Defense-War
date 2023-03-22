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

namespace Earth_Defend_War_NetCore3_1.Items
{
    /// <summary>
    /// 游戏的对话框
    /// </summary>
    public partial class GameTextBox : UserControl
    {
        
        /// <summary>
        /// 停止屏幕的刷新
        /// </summary>
        /// <param name="ScreenStop"></param>
        public delegate void ScreenStop(bool ScreenStop);
        public event ScreenStop StopScreen;

        /// <summary>
        /// 点击画布时触发的事件
        /// </summary>
        public delegate void CanvasClick();
        public event CanvasClick ClickCanvas;

        /// <summary>
        /// 用以添加对话框上面按钮的点击事件
        /// 可以让按钮和其他画面联动起来
        /// </summary>
        /// <param name="sender"></param>
        public delegate void GameTextBoxButtonClick(Button sender);
        public event GameTextBoxButtonClick GameTextBoxButtonClickEvent;

        /// <summary>
        /// 创建一个新的对话框
        /// </summary>
        /// <returns></returns>
        public static GameTextBox Create(Canvas canvas)
        {
            if (!Maked)
            {
                GameTextBox gameTextBox = new GameTextBox()
                {
                    Opacity = 0.5,
                    Width = canvas.Width - 100,
                    Height = 150,
                };
                gameTextBox.ClickCanvas += gameTextBox.GetClickCanvas(canvas);
                gameTextBox.SetXY(gameTextBox.GetCenter(canvas.Width), canvas.Height - 200);
                gameTextBox.SetZIndex(StaticUtils.UIZindex.UITop);
                Maked = true;
                return gameTextBox;
            }
            return null;
        }

        /// <summary>
        /// 用于指示对话框是否是唯一
        /// </summary>
        public static bool Maked { get; private set; }

        /// <summary>
        /// 对话框显示的台词
        /// </summary>
        private List<string> strs;
        public List<string> Str 
        { 
            get 
            {
                return strs;
            }
            set 
            {
                if (value.Count > 0)
                {
                    TextB.Text = value[0];
                }
                else
                {
                    TextB.Text = string.Empty;
                }
                strs = value;
            } 
        }
        private int StrIndex = 0;
        /// <summary>
        /// 设置透明度
        /// </summary>
        public new double Opacity
        {
            get
            {
                return base.Opacity;
            }
            set
            {
                BackGroundGrid.Opacity = value;
            }
        }

        /// <summary>
        /// 是否显示对话框
        /// </summary>
        public bool Show
        {
            get
            {
                return IsEnabled;
            }
            set
            {
                IsEnabled = value;
                if (value)
                {
                    Visibility = Visibility.Visible;
                }
                else
                {
                    Visibility = Visibility.Hidden;
                }
                StopScreen?.Invoke(value);
            }
        }

        public bool Lock { get; set; }

        /// <summary>
        /// 初始化对话框
        /// </summary>
        protected GameTextBox()
        {
            InitializeComponent();
            IsEnabled = false;
            Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 显示设置按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            GameTextBoxButtonClickEvent?.Invoke((Button)sender);
        }

        /// <summary>
        /// 显示菜单按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            GameTextBoxButtonClickEvent?.Invoke((Button)sender);
        }

        /// <summary>
        /// 隐藏按钮点击事件,隐藏对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HiddenButton_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            ClickCanvas?.Invoke();
        }

        /// <summary>
        /// 鼠标点击事件,用于推进对话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownEnven(object sender, MouseButtonEventArgs e)
        {
            if (Str.Count <= StrIndex)
            {
                StrIndex = 0;
                if (!Lock)
                {
                    Show = false;
                }
                return;
            }
            TextB.Text = Str[StrIndex];
            StrIndex++;
        }

        /// <summary>
        /// 默认的点击画布的实现
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public CanvasClick GetClickCanvas(Canvas canvas)
        {
            ///如果对话框隐藏的话
            ///就显示对话框的一个事件
            return new CanvasClick(() =>
            {
                MouseButtonEventHandler canvasmousedown = null;
                canvasmousedown = new MouseButtonEventHandler((obj, e) =>
                {
                    Visibility = Visibility.Visible;
                    canvas.RemoveHandler(e.RoutedEvent, canvasmousedown);
                });
                canvas.MouseDown += canvasmousedown;
            });
        }
    }
}
