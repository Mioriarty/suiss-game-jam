using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;

public static class StaticTracker
{
    public static int nextLevel;
    public static bool devMode = false;
    public static Dictionary<string, bool> achievements;
    public enum GameEndType { Success, Quit, TimeElapsed }
    public static GameResult gr;

    public class GameResult
    {
        public GameResult(int iLevel, GameEndType iGameEndType, float iEndTime)
        {
            Level = iLevel;
            GameEnd = iGameEndType;
            EndTime = iEndTime;
        }
        public int Level { get; }
        public GameEndType GameEnd { get; }
        public float EndTime { get; }
    }

    public static void SetGameResult(int level, GameEndType gameEnd, float failureTime)
    {
        gr = new GameResult(level, gameEnd, failureTime);
    }

    public static GameResult GetGameResult() { return gr; }
    public static void ClearGameResult() { gr = null; }
}
