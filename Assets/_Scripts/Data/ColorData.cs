using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Data
{
    public static class ColorData
    {
        private static List<Color> colors = null;


        public static void NextColor()
        {
            if (colors != null)
            {
                int index = colors.IndexOf(GameData.Instance().Color);
                if (index == 7) index = -1;
                GameData.Instance().Color = colors[index + 1];
            }
            else
                CreateColors();
        }

        private static void CreateColors()
        {
            colors = new List<Color>()
            {
                new Color32(247, 149, 30, 255),
                new Color32(32, 213, 210, 255),
                new Color32(255, 86, 7, 255),
                new Color32(248, 0, 255, 255),
                new Color32(255, 242, 0, 255),
                new Color32(0, 242, 0, 255),
                new Color32(65, 140, 255, 255),
                new Color32(255, 0, 19, 255),
            };
        }

        public static Color InitialColor()
        {
            if (colors == null)
                CreateColors();
            return colors[0];
        }
    }
}