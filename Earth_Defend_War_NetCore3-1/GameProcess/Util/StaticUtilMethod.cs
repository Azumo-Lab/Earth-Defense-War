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
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Earth_Defend_War_NetCore3_1.GameProcess.Util
{
    public static class StaticUtilMethod
    {
        public static T RedomNumber<T>(this List<T> ts)
        {
            var seed = Guid.NewGuid().GetHashCode();
            Random r = new Random(seed);
            int i = r.Next(0, 1000000);
            double val = (double)i / 1000000;
            int Value = (int)Math.Round(ts.Count * val);
            if (Value >= ts.Count)
            {
                Value--;
            }
            return ts[Value];
        }
        public static System.Windows.Controls.Image GetImage(this Bitmap bitmap)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, bitmap.RawFormat);
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            return new System.Windows.Controls.Image
            {
                Source = bitmapImage
            };
        }

        public static void SetXY(this UIElement uIElement, double Left, double Top)
        {
            Canvas.SetLeft(uIElement, Left);
            Canvas.SetTop(uIElement, Top);
        }

        public static double GetCenter(this UIElement uIElement, double CanvasWeight)
        {
            var UIEL = uIElement as FrameworkElement;
            return (CanvasWeight - UIEL.Width) / 2;
        }

        public static void Add(this Canvas canvas, UIElement uIElement)
        {
            canvas.Children.Add(uIElement);
        }

        public static void Add(this Canvas canvas, Func<UIElement> action)
        {
            canvas.Children.Add(action.Invoke());
        }

        public static T Add<T>(this Canvas canvas, Func<T> action) where T : UIElement
        {
            var returnvalue = action.Invoke();
            canvas.Children.Add(returnvalue);
            return returnvalue;
        }

        public static T Add<T>(this Canvas canvas, List<T> AddtoList, Func<T> action) where T : Base
        {
            var returnvalue = action.Invoke();
            if (returnvalue.Image != null)
            {
                canvas.Children.Add(returnvalue.Image);
            }
            if (returnvalue.FrameworkElement != null)
            {
                canvas.Children.Add(returnvalue.FrameworkElement);
            }
            AddtoList.Add(returnvalue);
            return returnvalue;
        }

        public static void Remove(this Canvas canvas, UIElement uIElement)
        {
            canvas.Children.Remove(uIElement);
        }

        public static void Clear(this System.Collections.IList list, params System.Collections.IList[] ClaerList)
        {
            list.Clear();
            foreach (var item in ClaerList)
            {
                item.Clear();
            }
        }

        public static void Clear(this Canvas canvas)
        {
            canvas.Children.Clear();
        }

        public static void SetZIndex(this UIElement uIElement, UIZindex iZindex)
        {
            switch (iZindex)
            {
                case UIZindex.UITop:
                case UIZindex.UIElementTop:
                case UIZindex.BACKTop:
                case UIZindex.BACK:
                    Panel.SetZIndex(uIElement, (int)iZindex);
                    break;
                case UIZindex.NONE:
                    break;
                default:
                    break;
            }
        }

        private static readonly Dictionary<RoutedEvent, Delegate> Delegates = new Dictionary<RoutedEvent, Delegate>();

        public static void AddHandlerAndList(this Canvas canvas, RoutedEvent routedEvent, Delegate handler)
        {
            Delegates[routedEvent] = handler;
            canvas.AddHandler(routedEvent, Delegates[routedEvent]);
        }

        public static void RemoveHandlerAndList(this Canvas canvas, RoutedEvent routedEvent)
        {
            if (Delegates.ContainsKey(routedEvent))
            {
                canvas.RemoveHandler(routedEvent, Delegates[routedEvent]);
                Delegates.Remove(routedEvent);
            }
        }

        public static int SecondsTo(this int Seconds)
        {
            return Seconds * 1000;
        }

        public static int SecondsTo(this double Seconds)
        {
            return (int)(Seconds * 1000);
        }

        /// <summary>
        /// UI的层级
        /// </summary>
        public enum UIZindex
        {
            /// <summary>
            /// UI级别的Top
            /// </summary>
            UITop = 999,
            /// <summary>
            /// 游戏所有元素的TOP
            /// </summary>
            UIElementTop = 200,
            /// <summary>
            /// 一般的元素添加顺序
            /// </summary>
            NONE = 0,
            /// <summary>
            /// 背景元素上面
            /// </summary>
            BACKTop = -500,
            /// <summary>
            /// 最下面的背景元素
            /// </summary>
            BACK = -999,
        }
    }
}
