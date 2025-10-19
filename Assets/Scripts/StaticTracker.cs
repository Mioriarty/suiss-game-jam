using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;

public static class StaticTracker
{
    public static int nextLevel;
    public static bool devMode = false;

    public static Dictionary<int, float> highscores = new()
    {
        { 0, float.MaxValue },
        { 1, float.MaxValue },
        { 2, float.MaxValue },
        { 3, float.MaxValue }
    };
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

    public static void UpdateHighscore(int level, float score)
    {
        highscores[level] = Math.Min(highscores[level], score);
    }
    public static float GetHighscore(int level)
    {
        return highscores[level];
    }
    
    public static string floatToDisplayable(float t)
    {
        if (Mathf.Approximately(t, float.MaxValue)) { return "--:--"; }
        int secs = (int) t % 60;
        int mins = (int)t / 60;
        string secsBufferZero;
        string minsBufferZero;
        if (secs.ToString().Length == 1) { secsBufferZero = "0"; } else { secsBufferZero = ""; }
        if (mins.ToString().Length == 1) { minsBufferZero = "0"; } else { minsBufferZero = ""; }
        return minsBufferZero + mins + ":" + secsBufferZero + secs;
    }

}
