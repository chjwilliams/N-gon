using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicManager;
using EnemyWaveSpawner;
using GameEventsManager;
using GameEvents;
using Enemy;

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

    private BasicEnemyBoss boss;
    private EnemyWaveDestroyedEvent.Handler onEnemyWaveDestroyed;
	private const string ENEMY_WAVE_DESTROYED = "EnemyWaveDestroyed";

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    private void Awake ()
	{
        
       
	}

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
     
        enemyWaveManager = new EnemyWaveManager();
        enemyWaveManager.PopulateSpawnPoints();
        enemyWaveManager.PopulateDictionary();

        onEnemyWaveDestroyed = new EnemyWaveDestroyedEvent.Handler(OnEnemyWaveDestroyed);
		EventManager.Instance.Register<EnemyWaveDestroyedEvent>(onEnemyWaveDestroyed);
        enemyWaveManager.Create(5);
    }
	
    public void OnEnemyWaveDestroyed(GameEvent e)
	{
        enemyWaveManager.Create(5);
	}

    public void SpawnAtEnemies(uint number, Transform t)
    {
        enemyWaveManager.CreateNAtLocation(number, t);
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
