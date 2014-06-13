using UnityEngine;
using System.Collections;

public class Highscore
{
    public int rank;
    public string user;
    public float score;
    public bool ownHighscore;

    public Highscore(int rank, string user, float score, bool ownHighscore = false)
    {
        this.rank = rank;
        this.user = user;
        this.score = score;
        this.ownHighscore = ownHighscore;
    }
}