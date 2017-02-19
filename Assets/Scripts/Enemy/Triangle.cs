using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------------------*/
/*  [SUBCLASS] Extends BasicEnemyControls.cs											*/
/*	Triangle: Triangle enemy logic			                    	    				*/
/*			Functions:																	*/
/*					public:																*/
/*						override void Update ()											*/
/*						override void Start ()					            			*/
/*					private:															*/
/*						IEnumerator ChangeDirection ()									*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
namespace Enemy 
{
public class Triangle : BasicEnemyControls
{
    public PlayerControls player;        //   Reference to player for Square subclass

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    override public void Start ()
    {
        //    Runs the base class's Start() function
        moveSpeed = 5.0f;
        base.Start ();
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControls> ();
    }

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Update: Called once per frame														*/
    /*		This Update function is used instead of the base class's						*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    override public void Update ()
    {
      SinWaveMovement ();
      FollowTarget(player.transform);
    }
}
}