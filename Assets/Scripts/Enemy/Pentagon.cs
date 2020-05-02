using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEventsManager;
using GameEvents;

/*--------------------------------------------------------------------------------------*/
/*  [SUBCLASS] Extends BasicEnemy.cs						        					*/
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
public class Pentagon : BasicEnemy
{
    public PlayerControls player;        //   Reference to player for Square subclass
    private EnemyDiedEvent.Handler onEnemyDied;
	private const string ENEMY_DIED = "EnemyDied";

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    override public void Start ()
    {
        moveSpeed = 0.0f;
        //    Runs base class's Start function
        base.Start ();
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControls> ();

        onEnemyDied = new EnemyDiedEvent.Handler(OnEnemyDied);
		EventManager.Instance.Register<EnemyDiedEvent>(onEnemyDied);
    }
    override public void OnDestroyed()
    {
        EventManager.Instance.Unregister<EnemyDiedEvent>(onEnemyDied);
        EventManager.Instance.Fire(new EnemyDiedEvent(this));
        Destroy(gameObject, 1.0f);
    }

    public void OnEnemyDied(GameEvent e)
	{
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.2f, 0.3f);
        GetComponent<TrailRenderer>().startColor = new Color (1.0f, 0.2f, 0.3f);
        GetComponent<TrailRenderer>().endColor = new Color (1.0f, 0.2f, 0.3f);
        moveSpeed++;
	}

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Update: Called once per frame														*/
    /*		This Update function is used instead of the base class's						*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    override public void Update ()
    {       
        FollowTarget (player.transform);
    }
}
}