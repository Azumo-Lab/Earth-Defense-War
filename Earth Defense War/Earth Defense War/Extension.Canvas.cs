using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Earth_Defense_War
{
    internal static partial class Extension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="element"></param>
        public static void AddToCanvas(this Canvas canvas, UIElement element)
        {
            canvas.Children.Add(element);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="element"></param>
        public static void AddToCanvas(this Canvas canvas, UIElement element, int X, int Y)
        {
            canvas.AddToCanvas(element);
            canvas.SetCanvasXY(element, X, Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        public static void Clear(this Canvas canvas)
        {
            canvas.Children.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="uIElement"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public static void SetCanvasXY(this Canvas canvas, UIElement uIElement, int X, int Y)
        {
            Canvas.SetLeft(uIElement, X);
            Canvas.SetTop(uIElement, Y);
        }
    }
}
