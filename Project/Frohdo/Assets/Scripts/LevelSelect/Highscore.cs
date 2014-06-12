using UnityEngine;
using System.Collections;

class Highscore
{
    public int rank;
    public string user;
    public float score;

    public Highscore(int rank, string user, float score)
    {
        this.rank = rank;
        this.user = user;
        this.score = score;
    }
}