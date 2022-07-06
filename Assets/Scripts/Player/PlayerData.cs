﻿using System;
using UnityEngine;

namespace Player
{
    public static class PlayerData
    {
        private const string ShipKey = "ship";
        private const string TotalKey = "totalscore";
        private const string HighScoreKey = "highscore";
        private const string VolumeKey = "volume";
        public static int SelectedShip => Data[0];
        public static int TotalScore => Data[1];
        public static int HighScore => Data[2];
        public static float AudioVolume => Data[3] / 100f;
        private const int TL = 4;
        private static readonly string[] Keys = new string[TL] { ShipKey, TotalKey, HighScoreKey, VolumeKey };
        private static readonly int[] DefaultValues = new int[TL] { 0, 0, 0, 100 };
        private static readonly int[] Data = DefaultValues;
        private static bool _loaded = false;

        public static void Load()
        {
            if(_loaded)
                return;
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
                    PlayerPrefs.SetInt(Keys[i], DefaultValues[i]);
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
            _loaded = false;
            Load();
        }

        public static void IncreaseScore(int value = 1)
        {
            if (!_loaded)
                Load();
            PlayerPrefs.SetInt(TotalKey, TotalScore + value);
            PlayerPrefs.Save();
            Update();
        }

        public static void SelectShip(int shipID)
        {
            if (!_loaded)
                Load();
            PlayerPrefs.SetInt(ShipKey, shipID);
            PlayerPrefs.Save();
            Update();
        }

        public static void SetHighScore(int score)
        {
            if (!_loaded)
                Load();
            if (score > HighScore)
            {
                PlayerPrefs.SetInt(HighScoreKey, score);
                PlayerPrefs.Save();
            }

            Update();
        }

        public static void SetAudionVolume(float value)
        {
            if (!_loaded)
                Load();
            PlayerPrefs.SetInt(VolumeKey, Math.Clamp((int)(value * 100), 0, 100));
            PlayerPrefs.Save();
            Update();
        }
    }
}