using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
        TODO: Audio: Each enemy type should play different sounds when they are created and destroyed.
                Add audio source to BasicEnemyControls.cs
                Import sounds 4 sounds.

/*

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GameManager: As of now, just spawns enemies        									*/
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
    public List <BasicEnemyControls> enemyList;        //    A List holding all active enemies
    public Transform spawnPointA;                      //    Transform for SpawnPoint A
    public Transform spawnPointB;                      //    Transform for SpawnPoint B

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    private void Start ()
	{
	    //    Enemy prefabs do not have the BasicEnemyControls Script attached

	    GameObject newEnemy = Instantiate (PrefabManager.Instance.trianglePrefab, spawnPointA.position, spawnPointA.rotation);
	    newEnemy.AddComponent <BasicEnemyControls.Triangle> ();

	    GameObject newEnemy2 = Instantiate (PrefabManager.Instance.squarePrefab, spawnPointB.position, spawnPointB.rotation);
	    newEnemy2.AddComponent <BasicEnemyControls.Square> ();

	    BasicEnemyControls enemy = newEnemy.GetComponent <BasicEnemyControls.Triangle> ();
	    BasicEnemyControls enemy2 = newEnemy2.GetComponent <BasicEnemyControls.Square> ();

	    enemy.sides = 3;
	    enemy2.sides = 4;

	    enemy.moveSpeed = 5.0f;
	    enemy2.moveSpeed = 5.0f;

	    enemyList.Add (enemy);
	    enemyList.Add (enemy2);
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
