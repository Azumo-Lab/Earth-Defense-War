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
using System.Text;

namespace Earth_Defend_War_NetCore3_1
{
    public static class Global
    {
        public static void Init()
        {
            ImageRes.Init();
            BGMRes.Init();
        }
        public static class BGMRes
        {
            public static void Init()
            {

            }
        }

        public static class ImageRes
        {
            public static void Init()
            {
                Hero = Properties.Resources.Hero.GetImage();
                Boom = Properties.Resources.Boom.GetImage();
                IShi1 = Properties.Resources.IShi1.GetImage();
                IShi2 = Properties.Resources.IShi2.GetImage();
                IShi3 = Properties.Resources.IShi3.GetImage();
                IShi4 = Properties.Resources.IShi4.GetImage();
                KillPoints = Properties.Resources.KillPoints.GetImage();
                TekiBoss = Properties.Resources.TekiBoss.GetImage();
                TekiLV1 = Properties.Resources.TekiLV1.GetImage();
                TekiLV2 = Properties.Resources.TekiLV2.GetImage();
                TekiKillPoints = Properties.Resources.TekiKillPoints.GetImage();
                BackPic1 = Properties.Resources.StarBack1.GetImage();
                BackPic2 = Properties.Resources.StarBack2.GetImage();
            }
            /// <summary>
            /// 主角的资源文件
            /// </summary>
            public static System.Windows.Controls.Image Hero { get; private set; }
            /// <summary>
            /// 爆炸
            /// </summary>
            public static System.Windows.Controls.Image Boom { get; private set; }
            /// <summary>
            /// 石头1
            /// </summary>
            public static System.Windows.Controls.Image IShi1 { get; private set; }
            /// <summary>
            /// 石头2
            /// </summary>
            public static System.Windows.Controls.Image IShi2 { get; private set; }
            /// <summary>
            /// 石头3
            /// </summary>
            public static System.Windows.Controls.Image IShi3 { get; private set; }
            /// <summary>
            /// 石头4
            /// </summary>
            public static System.Windows.Controls.Image IShi4 { get; private set; }
            /// <summary>
            /// 我方子弹
            /// </summary>
            public static System.Windows.Controls.Image KillPoints { get; private set; }
            /// <summary>
            /// 敌方Boss
            /// </summary>
            public static System.Windows.Controls.Image TekiBoss { get; private set; }
            /// <summary>
            /// 敌方LV1
            /// </summary>
            public static System.Windows.Controls.Image TekiLV1 { get; private set; }
            /// <summary>
            /// 敌方LV2
            /// </summary>
            public static System.Windows.Controls.Image TekiLV2 { get; private set; }
            /// <summary>
            /// 敌方子弹，
            /// </summary>
            public static System.Windows.Controls.Image TekiKillPoints { get; private set; }
            /// <summary>
            /// 星空背景1
            /// </summary>
            public static System.Windows.Controls.Image BackPic1 { get; private set; }
            /// <summary>
            /// 星空背景2
            /// </summary>
            public static System.Windows.Controls.Image BackPic2 { get; private set; }
        }
    }
}
