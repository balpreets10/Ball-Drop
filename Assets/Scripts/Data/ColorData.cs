using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public enum GameColors
    {
        red,
        blue,
        green,
        purple,
        yellow
    }

    public class ColorCombo
    {
        public GameColors color1;
        public GameColors color2;

        public ColorCombo(GameColors color1, GameColors color2)
        {
            this.color1 = color1;
            this.color2 = color2;
        }
    }

    public class ColorData : SingletonMonoBehaviour<ColorData>
    {
        [SerializeField]
        private Color PrimaryColor;

        [SerializeField]
        private Color SecondaryColor;

        [Header("Set colors in same order as GameColors enum")]
        [SerializeField]
        private Color[] Colors;

        private List<ColorCombo> ColorCombos = new List<ColorCombo>();

        private void Start()
        {
            ColorCombos.Add(new ColorCombo(GameColors.blue, GameColors.red));
            ColorCombos.Add(new ColorCombo(GameColors.blue, GameColors.green));
            ColorCombos.Add(new ColorCombo(GameColors.blue, GameColors.yellow));
            ColorCombos.Add(new ColorCombo(GameColors.red, GameColors.green));
            ColorCombos.Add(new ColorCombo(GameColors.red, GameColors.yellow));
            ColorCombos.Add(new ColorCombo(GameColors.green, GameColors.purple));
            ColorCombos.Add(new ColorCombo(GameColors.yellow, GameColors.purple));
        }

        public Color GetPrimaryColor()
        {
            return PrimaryColor;
        }

        public Color GetSecondaryColor()
        {
            return SecondaryColor;
        }

        public void SetColors()
        {
            if (Colors != null)
            {
                int position = Random.Range(0, ColorCombos.Count);

                int invert = Random.Range(0, 2);
                if (invert == 0)
                {
                    PrimaryColor = Colors[(int)ColorCombos[position].color2];
                    SecondaryColor = Colors[(int)ColorCombos[position].color1];
                }
                else
                {
                    PrimaryColor = Colors[(int)ColorCombos[position].color1];
                    SecondaryColor = Colors[(int)ColorCombos[position].color2];
                }
            }
        }
    }
}