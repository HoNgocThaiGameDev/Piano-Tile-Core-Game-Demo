using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : BySingleton<GameController>
{
    public string SONG_NAME;
    public int SONG_ID;
    const string BEST_SCORE = "bestScore";
    public string TYPE_MUSIC;
    public int bestScore;
    
}
