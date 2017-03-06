using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleManager;
using EnemyWaveSpawner;
using GameEventManager;
using GameEvents;

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
    private EnemyWaveDestroyedEvent.Handler onEnemyWaveDestroyed;
	private const string ENEMY_WAVE_DESTROYED = "EnemyWaveDestroyed";

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
        onEnemyWaveDestroyed = new EnemyWaveDestroyedEvent.Handler(OnEnemyWaveDestroyed);
		EventManager.Instance.Register<EnemyWaveDestroyedEvent>(onEnemyWaveDestroyed);
	}

    private void Start()
    {
        enemyWaveManager.Create(5);
    }
	
    public void OnEnemyWaveDestroyed(GameEvent e)
	{
        enemyWaveManager.Create(5);
	}

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Update: Called once per frame														*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    void Update ()
	{
		
	}
}
