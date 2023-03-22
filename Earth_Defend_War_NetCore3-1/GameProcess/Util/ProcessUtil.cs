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
using System.Windows;

namespace Earth_Defend_War_NetCore3_1.GameProcess.Util
{
    public static class ProcessUtil
    {
        /// <summary>
        /// 边界检测,判断一个物体是否碰触边界(目前只能检测方形)
        /// </summary>
        /// <param name="modelA">检测主体物A点</param>
        /// <param name="modelB">检测主体物B点</param>
        /// <param name="A">被检测物体A点</param>
        /// <param name="B">被检测物体B点</param>
        /// <returns></returns>
        public static bool BorderDetection(Point modelA, Point modelB, Point A, Point B)
        {
            if (Math.Max(modelA.X, modelB.X) < Math.Min(A.X, B.X) || Math.Min(modelA.X, modelB.X) > Math.Max(A.X, B.X))
            {
                return false;
            }
            else
            {
                return !(Math.Max(modelA.Y, modelB.Y) < Math.Min(A.Y, B.Y) || Math.Min(modelA.Y, modelB.Y) > Math.Max(A.Y, B.Y));
            }
        }

        /// <summary>
        /// 边界检测,判断一个物体是否碰触边界(目前只能检测方形)
        /// </summary>
        /// <param name="modelAX"></param>
        /// <param name="modelAY"></param>
        /// <param name="modelBX"></param>
        /// <param name="modelBY"></param>
        /// <param name="AX"></param>
        /// <param name="AY"></param>
        /// <param name="BX"></param>
        /// <param name="BY"></param>
        /// <returns></returns>
        public static bool BorderDetection(int modelAX, int modelAY, int modelBX, int modelBY, int AX, int AY, int BX, int BY)
        {
            if (Math.Max(modelAX, modelBX) < Math.Min(AX, BX) || Math.Min(modelAX, modelBX) > Math.Max(AX, BX))
            {
                return false;
            }
            else
            {
                return !(Math.Max(modelAY, modelBY) < Math.Min(AY, BY) || Math.Min(modelAY, modelBY) > Math.Max(AY, BY));
            }
        }

        /// <summary>
        /// 边界检测,判断一个物体是否碰触边界(目前只能检测方形)
        /// </summary>
        /// <param name="modelAX"></param>
        /// <param name="modelAY"></param>
        /// <param name="modelBX"></param>
        /// <param name="modelBY"></param>
        /// <param name="AX"></param>
        /// <param name="AY"></param>
        /// <param name="BX"></param>
        /// <param name="BY"></param>
        /// <returns></returns>
        public static bool BorderDetection(float modelAX, float modelAY, float modelBX, float modelBY, float AX, float AY, float BX, float BY)
        {
            if (Math.Max(modelAX, modelBX) < Math.Min(AX, BX) || Math.Min(modelAX, modelBX) > Math.Max(AX, BX))
            {
                return false;
            }
            else
            {
                return !(Math.Max(modelAY, modelBY) < Math.Min(AY, BY) || Math.Min(modelAY, modelBY) > Math.Max(AY, BY));
            }
        }
    }
}
