using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.Networking;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	EnemyControls: Handles baisc enemy state in N-gon									*/
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
    public AudioClip spawn;                      //    Audio clip played with spawned
    public AudioClip hit;                        //    Audio clip played when hit with plauer bullet

    //    Private Variables
    private Vector3 _Axis;                       //    Reference to axis
    private Vector3 _Position;                   //    Reference to position
    private Quaternion _Rotation;                //    Reference to rotation
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
        _Axis = transform.right;
        _Position = transform.position;
        _Rotation = transform.rotation;
        _AudioSource = GetComponent <AudioSource> ();
        _Animator = GetComponent <Animator> ();
        _Rigidbody2D = GetComponent <Rigidbody2D> ();
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
        if (other.gameObject.CompareTag ("Boundary"))
        {

        }
    }

    /*--------------------------------------------------------------------------------------*/
    /*  [SUBCLASS] Extends BasicEnemyControls.cs											*/
    /*	Triangle: Triangle enemy logic			                    	    				*/
    /*			Functions:																	*/
    /*					public:																*/
    /*						override void Update ()											*/
    /*					private:															*/
    /*						void Start ()								        			*/
    /*						IEnumerator ChangeDirection ()									*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    public class Triangle : BasicEnemyControls
    {
        public bool turn = false;            //
        public float changeX = 1.0f;         //
        public float changeY = 1.0f;         //
        public float frequency = 10.0f;      //    Speed of the Sine movement
        public float magnitude = 0.1f;       //    Size of sine movement

        /*--------------------------------------------------------------------------------------*/
        /*																						*/
        /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
        /*																						*/
        /*--------------------------------------------------------------------------------------*/
        private void Start ()
        {
            //    Runs the base class's Start() function
            moveSpeed = 5.0f;
            base.Start ();
            StartCoroutine (ChangeDirection ());
        }

        private IEnumerator ChangeDirection ()
        {
            changeX = 1;
            changeY = 1;
            turn = false;

            yield return new WaitForSeconds (5.0f);

            turn = true;

            yield return new WaitForSeconds (3.0f);

            changeY = -1;
            changeX = -1;
            turn = false;

            yield return new WaitForSeconds (5.0f);

            turn = true;
            changeY = -1;
            changeX = 1;

            yield return new WaitForSeconds (3.0f);

            StartCoroutine (ChangeDirection ());
        }

        /*--------------------------------------------------------------------------------------*/
        /*																						*/
        /*	Update: Called once per frame														*/
        /*		This Update function is used instead of the base class's						*/
        /*																						*/
        /*--------------------------------------------------------------------------------------*/
        override public void Update ()
        {
            float phase;
            if (turn)
            {
                phase = Mathf.Sin (Time.time * frequency) * magnitude;
                Vector2 newVelocity = _Rigidbody2D.velocity;
                newVelocity.x = phase;
                newVelocity.y = transform.up.y * Time.deltaTime * moveSpeed;
                _Rigidbody2D.velocity = newVelocity;

                transform.position = new Vector3 (phase + transform.position.x,
                                                  transform.position.y + moveSpeed * changeY * Time.deltaTime,
                                                  0);
            }
            else
            {

                phase =  Mathf.Sin (Time.time * frequency) * magnitude;
                Vector2 newVelocity = _Rigidbody2D.velocity;
                newVelocity.x = phase;
                newVelocity.y = transform.up.y * Time.deltaTime * moveSpeed;

                transform.position = new Vector3( transform.position.x + moveSpeed * changeX * Time.deltaTime,
                                                  phase + transform.position.y,
                                                  0);
            }

            if (changeY < 0)
            {
                if (!turn)
                {
                    transform.localRotation = Quaternion.Euler (new Vector3 (transform.rotation.x,
                                                                             180.0f,
                                                                             phase * 30 - 90));
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(new Vector3( 180,
                                                                            transform.rotation.y,
                                                                            phase * 30));
                }
            }
            else
            {
                if (turn)
                {
                    transform.localRotation = Quaternion.Euler (new Vector3 (transform.rotation.x,
                                                                             0,
                                                                             phase * 30));
                }
                else
                {
                    transform.localRotation = Quaternion.Euler (new Vector3 (transform.rotation.x,
                                                                             0,
                                                                             phase * 30 + 270));
                }
            }
        }
    }

    /*--------------------------------------------------------------------------------------*/
    /*  [SUBCLASS] Extends BasicEnemyControls.cs											*/
    /*	Square: Square enemy logic			                    	    			    	*/
    /*			Functions:																	*/
    /*					public:																*/
    /*						override void Update ()											*/
    /*					private:															*/
    /*						void Start ()								        			*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    public class Square : BasicEnemyControls
    {
        public PlayerControls player;        //   Reference to player for Square subclass

        /*--------------------------------------------------------------------------------------*/
        /*																						*/
        /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
        /*																						*/
        /*--------------------------------------------------------------------------------------*/
        private void Start ()
        {
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
            _Position = transform.position;
            _Position = Vector3.Lerp (_Position, player.transform.position, Time.deltaTime);
            transform.position = _Position;
        }
    }









}
