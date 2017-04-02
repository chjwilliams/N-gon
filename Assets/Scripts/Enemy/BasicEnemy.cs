using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.Networking;
using BasicManager;
using EnemyWaveSpawner;
using GameEventsManager;
using GameEvents;
using GC;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	BasicEnemy: Handles baisc enemy state in N-gon								        */
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

namespace Enemy 
{
 public enum EnemyType
{
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        BasicBoss
}
public class BasicEnemy : MonoBehaviour, IManaged
{

    public EnemyType EnemyTypes {get; private set;}

    //    Public Variables
    public int sides;                            //    Refernece to number of sides enemy has
    public float moveSpeed;                      //    Movement speed of enemy
    public int MAX_NUMBER_OF_PULSES = 5;
	public bool hasBeenDamaged;
	public bool isAttacking;
	public bool preparingToAttack;		
	public float enemySight;
	public float pulseTimer;
	public float fleeTimer;
	public float maxSize;
	public float attackRange;
	public float safetyDistance;
	public int numberOfPulses;
	public Vector3 fleeingPosition; 
    public LayerMask collisionMask;              //
    public AudioClip spawn;                      //    Audio clip played with spawned
    public AudioClip hit;                        //    Audio clip played when hit with plauer bullet
    public Transform player;

    //    Protected Variables
    protected float _Width;						 //	   Reference to player's width
    protected float _Height;						 //	   Refernece to player's height
    protected Vector3 _Position;                   //    Reference to position
    protected AudioSource _AudioSource;            //    Refernce to gameobject's audio source
    protected Animator _Animator;                  //    Reference to Enemy's animator
    protected Rigidbody2D _Rigidbody2D;            //    Reference to Rigidbody2D
    private EnemyWaveManager _MyManager;

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	BasicEnemyControls: Empty Constructor                            					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    public BasicEnemy () { }

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    public virtual void Start ()
    {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();

        _Width = spriteRenderer.bounds.extents.x;
        _Height = spriteRenderer.bounds.extents.y;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        _Position = transform.position;
        _AudioSource = GetComponent <AudioSource> ();
        _Animator = GetComponent <Animator> ();
        _Rigidbody2D = GetComponent <Rigidbody2D> ();
        _MyManager = GameObject.Find("EnemyManager").GetComponent<EnemyWaveManager>();
    }

    public virtual void Init(EnemyType enemy) 
    {   
    }

    public virtual void OnCreated() {}

    public virtual void OnDestroyed()
    {
        EventManager.Instance.Fire(new EnemyDiedEvent(this));
        
        Destroy(gameObject, 1.0f);
    }

    public float GetDistanceFromPoint(Vector3 point)
    {
        return Vector3.Distance(transform.position, point);
    }

    public void FollowTarget (Transform target)
    {
        _Position = transform.localPosition;
        _Position = Vector3.Lerp (_Position, target.localPosition, Time.deltaTime * moveSpeed);
        transform.localPosition = _Position;
    }

    public void PulseEnemy(Vector3 initalScale, float maxSize, float t, float pulseTimer)
    {
        transform.localScale = Vector3.Lerp(initalScale, maxSize * initalScale, Easing.QuadEaseOut(t / pulseTimer));
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
        if (other.gameObject.CompareTag ("PlayerBullet"))
        {
            moveSpeed = 20.0f;
            _MyManager.Destroy(this);
        }
    }
}
}
