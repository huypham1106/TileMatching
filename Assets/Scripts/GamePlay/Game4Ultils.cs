using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Game4
{
    public class Game4Ultils : MonoBehaviour
    {
        public static float GetXByCol(int col, int maxCol)
        {
            float pos = 0;
            pos = (col - maxCol / 2) * getRange(maxCol) + getOffsetByCount(maxCol);
            return pos;
        }
        public static float GetYByRow(int row, int maxRow)
        {
            float pos = 0;
            pos = (row - maxRow / 2) * getRange(maxRow) + getOffsetByCount(maxRow);
            return pos;
        }
        private static float getRange(int max)
        {
            if (max == 4) return 250;
            if (max > 4) return 200;
            return 300;
        }
        private static float getOffsetByCount(int max)
        {
            if (max > 3) return 100;
            if (max == 3) return 200;
            return 300;
        }


        public static float GetXOfSlot(int col, int maxCol)
        {
            float pos = 0;
            float offSet = 0;
            if (maxCol % 2 == 0) offSet = (Game4Constants.SIZE_ITEM_SLOT + Game4Constants.RANGE_ITEM_SLOT) / 2;
            pos = (col - maxCol / 2) * (Game4Constants.SIZE_ITEM_SLOT + Game4Constants.RANGE_ITEM_SLOT) + offSet;
            return pos;
        }
    }
}
