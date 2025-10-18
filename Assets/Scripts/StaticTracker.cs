using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;

public static class StaticTracker
{
    public static int nextLevel;
    public static bool devMode = false;
    public static Dictionary<string, bool> achievements;
    public enum FailureType { Quit, TimeElapsed }
    public static GameOverParams gop;

    public class GameOverParams
    {
        public GameOverParams(int iLevel, FailureType iFailureType, float iFailureTime)
        {
            Level = iLevel;
            FailureType = iFailureType;
            FailureTime = iFailureTime;
        }
        public int Level { get; }
        public FailureType FailureType { get; }
        public float FailureTime { get; }
    }

    public static void SetGameOverParams(int level, FailureType failureType, float failureTime)
    {
        gop = new GameOverParams(level, failureType, failureTime);
    }

    public static GameOverParams GetGameOverParams() { return gop; }
    public static void ClearGameOverParams() { gop = null; }
}
