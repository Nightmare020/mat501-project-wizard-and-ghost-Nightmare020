using System;

using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private int levelIdx = -1;
    [SerializeField] private int currentVersion;
    [SerializeField] private int currentArcadePoints = 10;

    private const int Version = 1;

    //general settings

    public SaveData()
    {
        levelIdx = -1;
        currentVersion = Version;
        currentArcadePoints = 10;
    }

    #region Methods

    public int GetCurrentLevel()
    {
        return levelIdx;
    }

    public void SetCurrentLevel(int idx)
    {
        levelIdx = idx;
    }

    public int GetCurrentArcadePoints()
    {
        return currentArcadePoints;
    }

    public void SetCurrentArcadePoints(int points)
    {
        currentArcadePoints = points;
    }

    public int GetVersion()
    {
        return currentVersion;
    }

    public static bool CheckVersionObsolet(int old)
    {
        return old < Version;
    }

    #endregion
}