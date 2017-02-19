using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleManager;
using EnemyWaveManager_;

/*
        TODO: Audio: Each enemy type should play different sounds when they are created and destroyed.
                Add audio source to BasicEnemyControls.cs
                Import sounds 4 sounds.

/*

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GameManager: TBD                                  									*/
/*			Functions:																	*/
/*					public:																*/
/*						                        										*/
/*					proteceted:															*/
/*                                                                                      */
/*					private:															*/
/*						void Start ()													*/
/*						void Update ()													*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class GameManager : MonoBehaviour
{
    //  Static variables
    public static GameManager instance;                //   The current Game Manager  
    public EnemyWaveManager enemyWaveManager;

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    private void Awake ()
	{
        enemyWaveManager = new EnemyWaveManager();
        enemyWaveManager.PopulateSpawnPoints();
        enemyWaveManager.PopulateDictionary();
	}

    private void Start()
    {
        enemyWaveManager.Create(5);
    }
	
    IEnumerator SpawnNewWave()
    {
        yield return new WaitForSeconds(3.0f);
        
    }

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Update: Called once per frame														*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    void Update ()
	{
		if (EnemyWaveManager.ListIsEmpty())
        {
            enemyWaveManager.Create(5);
        }
	}
}
