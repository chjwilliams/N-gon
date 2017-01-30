using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	BasicBullet: Handles state of bullet												*/
/*			Functions:																	*/
/*					public:																*/
/*						void Fired (Vector 2 direction)									*/
/*																						*/
/*					private:															*/
/*						void Start ()													*/
/*						void Fired(Vector2 direction)									*/
/*						void OnCollisionEnter2D(Collider2D other)						*/
/*						void Update ()													*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class BasicBullet : MonoBehaviour 
{
	//	Public Variables
	public float dx;						//	Direction moving in x coordinates
	public float dy;						//	Direction moving in y coordinates
	public float moveSpeed = 0.01f;			//	How fast the bullet moves

	//	Private Variables
	private Rigidbody2D _Rigidbody2D;		//	Reference to Rigidbody2D
	private GameObject _Player;			//	Reference to player to ignore collision

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void Start () 
	{
		_Rigidbody2D = GetComponent<Rigidbody2D> ();
		_Player = GameObject.FindGameObjectWithTag ("Player");

	}

	/*--------------------------------TO BE DELETED-----------------------------------------*/
	/*																						*/
	/*	Fired: Spawns bullet 																*/
	/*		param: Vector2 direction - where the bullet was aimed							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void Fired(Vector2 direction)
	{
		
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	OnCollisionEnter2D: Has something collided with Bullet?								*/
	/*		param: Collider2D other - the object the bullet collided with					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Physics2D.IgnoreCollision (GetComponent<BoxCollider2D>(), _Player.GetComponent<CircleCollider2D> ());
		}

		if (other.gameObject.tag == "Level")
		{
			Destroy (this);
		}
	}

	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void Update () 
	{
		/*
		 * 		Move in direction of rotation
		 * 
		 * 
		 */ 

		_Rigidbody2D.velocity = new Vector2 (dx * moveSpeed, dy * moveSpeed);
	}
}
