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
        public static void AddToCanvas(this Canvas canvas, UIElement element)
        {
            canvas.Children.Add(element);
        }
    }
}
