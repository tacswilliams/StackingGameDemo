using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    // This is known as the Singleton pattern
    public static ScoreTracker INSTANCE;
    public int bestScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        // I only want 1 ScoreTracker for my game.
        // If the Instacne does nto exist, then this is
        // the first instance of the ScoreTracker
        if (INSTANCE == null)
        {
            INSTANCE = this;
            bestScore = 0;
            // Do not destroy the first instance of ScoreTracker
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance exists, then I do not need this
            // new instance of ScoreTracker
            Destroy(gameObject);
        }
    }
}
