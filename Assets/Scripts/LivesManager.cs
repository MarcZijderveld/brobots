using UnityEngine;
using System.Collections;

public class LivesManager : Photon.MonoBehaviour {
    private int lives = 6;
    [RPC]
    void modifyLives(int amount) 
    {
        lives += amount;
        if (lives == 0) 
        {
            //game over is called here

        }
    }
    [RPC]
    void setLives(int amount) {
        lives = amount;

    }
}
