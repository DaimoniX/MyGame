using System;
using UnityEngine;

namespace Player
{
    public static class PlayerData
    {
        private const string ShipKey = "ship";
        private const string TotalKey = "totalscore";
        private const string HighScoreKey = "highscore";
        public static int SelectedShip => Data[0];
        public static int TotalScore => Data[1];
        public static int HighScore => Data[2];
        private const int TL = 3;
        private static readonly string[] Keys = new string[TL] { ShipKey, TotalKey, HighScoreKey };
        private static readonly int[] Data = new int[TL] { 0, 0, 0 };
        private static bool _loaded = false;

        public static void Load()
        {
            Check();
            Update();
            PlayerPrefs.Save();
            _loaded = true;
        }

        private static void Check()
        {
            for (int i = 0; i < TL; i++)
            {
                if (!PlayerPrefs.HasKey(Keys[i]))
                    PlayerPrefs.SetInt(Keys[i], 0);
            }
        }

        private static void Update()
        {
            for (int i = 0; i < TL; i++)
            {
                Data[i] = PlayerPrefs.GetInt(Keys[i]);
            }
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteAll();
            if (!_loaded) return;
            Check();
            Update();
        }

        public static void IncreaseScore()
        {
            if (!_loaded)
                throw new InvalidOperationException("Cannot modify value before initialization");
            PlayerPrefs.SetInt(TotalKey, TotalScore + 1);
            PlayerPrefs.Save();
            Update();
        }

        public static void SelectShip(int shipID)
        {
            if (!_loaded)
                throw new InvalidOperationException("Cannot modify value before initialization");
            PlayerPrefs.SetInt(ShipKey, shipID);
            PlayerPrefs.Save();
            Update();
        }

        public static void SetHighScore(int score)
        {
            if (!_loaded)
                throw new InvalidOperationException("Cannot modify value before initialization");
            if (score > HighScore)
            {
                PlayerPrefs.SetInt(HighScoreKey, score);
                PlayerPrefs.Save();
            }

            Update();
        }
    }
}