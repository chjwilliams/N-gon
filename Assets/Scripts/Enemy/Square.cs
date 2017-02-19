using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------------------*/
/*  [SUBCLASS] Extends BasicEnemyControls.cs											*/
/*	Square: Square enemy logic			                    	    			    	*/
/*			Functions:																	*/
/*					public:																*/
/*						override void Update ()											*/
/*						override void Start ()						        			*/
/*					private:															*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
namespace Enemy 
{
public class Square : BasicEnemyControls
{
    public PlayerControls player;        //   Reference to player for Square subclass

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    override public void Start ()
    {
         moveSpeed = 7.0f;
        //    Runs base class's Start function
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
        SurroundTarget (player.transform);        
        FollowTarget (player.transform);
        SinWaveMovement ();
    }
}
}