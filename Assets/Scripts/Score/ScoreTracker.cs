using System;
using UnityEngine;

public class ScoreTracker : MonoBehaviour , IScoreAdder
{
    [SerializeField] private int PlayerScore = 0;

    public static event Action<int> ScoreChanged;

    public void InvokeScoreEvent(int score)
    {
        ScoreChanged?.Invoke(score);
    }

    //Interface method
    public void AddScore(int x)
    {
        int score = ScoreIdentifier(x);
        PlayerScore += score;
        InvokeScoreEvent(PlayerScore);
    }

    public int ReturnScore()
    {
        return PlayerScore;
    }

    int ScoreIdentifier(int x)
    {
        if (x == 3)
        {
            return 100;
        }
        else if (x==4)
        {
            return 200;
        }
        else if(x==5)
        {
            return 300;
        }
        else if(x>5)
        {
            return 400;
        }

        return 0;
    }
}
