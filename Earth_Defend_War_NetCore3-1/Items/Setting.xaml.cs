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

using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace Earth_Defend_War_NetCore3_1.Items
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        private Uri MusicPath { get; set; } = null;
        private MediaPlayer MediaPlayer { get; set; }
        public Setting()
        {
            InitializeComponent();
        }
        public void SetMusic(ref MediaPlayer mediaPlayer)
        {
            MediaPlayer = mediaPlayer;

            Volume.Value = mediaPlayer.Volume * 100;
        }

        private void SettingMusic()
        {
            if (MusicPath != null)
            {
                MediaPlayer.Open(MusicPath);
                MediaPlayer.Play();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? res = dialog.ShowDialog();
            if (res != null && res == true)
            {
                if (dialog.FileName != null)
                {
                    MusicPath = new Uri(dialog.FileName);
                }
            }
            SettingMusic();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Play();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MediaPlayer != null)
            {
                MediaPlayer.Volume = e.NewValue / 100;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Pause();
        }
    }
}
