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
using System.Timers;

namespace Earth_Defend_War_NetCore3_1.GameProcess
{
    public static class Time
    {
        private const int Interval = 100;
        /// <summary>
        /// 大略计时,每秒执行
        /// </summary>
        private static Timer Timer { get; } = new Timer(Interval);
        private static int S { get; set; } = 0;
        static Time()
        {
            Timer.Elapsed += new ElapsedEventHandler((o, e) =>
            {
                S += Interval;
            });
            Timer.Start();
        }

        public static void Close()
        {
            Timer.Dispose();
        }

        public static int GetTime()
        {
            return S;
        }
    }
}
