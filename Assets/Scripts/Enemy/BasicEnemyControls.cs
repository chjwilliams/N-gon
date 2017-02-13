using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.Networking;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	BasicEnemyControls: Handles baisc enemy state in N-gon								*/
/*			Functions:																	*/
/*					public:																*/
/*						BasicEnemyControls ()											*/
/*						virtual void Start ()											*/
/*						virtual void Update ()											*/
/*					proteceted:															*/
/*                      virtual void OnCollisionEnter2D ()                              */
/*					private:															*/
/*																						*/
/*					Subclasses:															*/
/*						Triangle ()						    							*/
/*						Square ()									    				*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class BasicEnemyControls : MonoBehaviour
{
    //    Public Variables
    public int sides;                            //    Refernece to number of sides enemy has
    public float moveSpeed;                      //    Movement speed of enemy
    public BasicEnemyControls thisEnemy;         //    A Referecence to self
    public LayerMask collisionMask;              //
    public AudioClip spawn;                      //    Audio clip played with spawned
    public AudioClip hit;                        //    Audio clip played when hit with plauer bullet

    //    Private Variables
    private float _Width;						 //	   Reference to player's width
    private float _Height;						 //	   Refernece to player's height
    private Vector3 _Position;                   //    Reference to position
    private AudioSource _AudioSource;            //    Refernce to gameobject's audio source
    private Animator _Animator;                  //    Reference to Enemy's animator
    private Rigidbody2D _Rigidbody2D;            //    Reference to Rigidbody2D

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	BasicEnemyControls: Empty Constructor                            					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    public BasicEnemyControls () { }

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    public virtual void Start ()
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetComponent <BasicEnemyControls> ();
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();

        _Width = spriteRenderer.bounds.extents.x;
        _Height = spriteRenderer.bounds.extents.y;

        _Position = transform.position;
        _AudioSource = GetComponent <AudioSource> ();
        _Animator = GetComponent <Animator> ();
        _Rigidbody2D = GetComponent <Rigidbody2D> ();
    }

    protected void FollowTarget (Transform target)
    {
        _Position = transform.position;
        _Position = Vector3.Lerp (_Position, target.position, Time.deltaTime);
        transform.position = _Position;
    }

    protected void SurroundTarget (Transform target)
    {
        _Position = transform.position;
        transform.RotateAround(target.position,Vector3.back, moveSpeed * 20 * Time.deltaTime);
        //transform.position = _Position;
    }

    protected void SinWaveMovement ()
    {
        bool turn = false;            //
        float changeX = 1.0f;         //
        float changeY = 1.0f;         //

        //    Raycasting
        Vector2 lineCastPosition = transform.position.toVector2 ();
        int collisionMask = 11;
        collisionMask = ~collisionMask;
        bool leftIsBlocked = Physics2D.Linecast (lineCastPosition, lineCastPosition + Vector2.up);
        bool rightIsBlocked = Physics2D.Linecast (lineCastPosition, lineCastPosition + Vector2.down);
        bool frontIsBlocked = Physics2D.Linecast (lineCastPosition, lineCastPosition + transform.right.toVector2 ());
        Debug.DrawLine (lineCastPosition, lineCastPosition + Vector2.up, Color.green);
        Debug.DrawLine (lineCastPosition, lineCastPosition + Vector2.down, Color.red);
        Debug.DrawLine (lineCastPosition, lineCastPosition + transform.right.toVector2 (), Color.blue);

        float frequency = 10.0f;      //    Speed of the Sine movement
        float magnitude = 0.1f;       //    Size of sine movement

        float phase =  Mathf.Sin (Time.time * frequency) * magnitude;
        Vector2 newVelocity = _Rigidbody2D.velocity;
        newVelocity.y = phase;
        newVelocity.x = transform.up.x * Time.deltaTime * moveSpeed;

        transform.position = new Vector3( transform.position.x + moveSpeed * changeX * Time.deltaTime,
                                          phase + transform.position.y,
                                          0);

        _Rigidbody2D.velocity = newVelocity;
    }

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Update: Called once per frame														*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    virtual public void Update () { }

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	OnCollisionEnter2D: Called once on collision										*/
    /*			param: Collision2D other - the collider of the other object					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    protected virtual void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag ("Bullet"))
        {
            moveSpeed = 40.0f;
            EnemyWaveManager.instance.EnemyToDestroy(this);
        }
    }
}
