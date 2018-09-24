using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Season
{
    summer,
    autumn,
    winter,
    spring,
}

public class GameController : MonoBehaviour
{
    public Season currentSeason;

    public float seasonTimer = 60.0f;
    private float seasonTimerIn = 0.0f;

    void Start ()
    {
        seasonTimerIn = seasonTimer;
	}

    public static GameController _instance;

    public static GameController Instance()
    {
        if(_instance == null)
        {
            _instance = GameObject.FindObjectOfType<GameController>();

            if(_instance == null)
            {
                GameObject container = new GameObject("GameController");
                _instance = container.AddComponent<GameController>();
            }
        }
        return _instance;
    }
	
	void Update ()
    {
        seasonTimer -= Time.deltaTime;

        if(seasonTimer <= 0)
        {
            if (currentSeason == Season.spring)
            {
                currentSeason = 0;
            }
            else
            {
                currentSeason++;
                seasonTimer = seasonTimerIn;
            }
        }
	}
}
