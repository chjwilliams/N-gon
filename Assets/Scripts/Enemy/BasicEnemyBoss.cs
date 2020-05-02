using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using GameEvents;
using GameEventsManager;
using GameTaskManager;
using GameTasks;

namespace Enemy 
{


	public class BasicEnemyBoss : BasicEnemy 
	{
		public const string PLAYER = "Player";

		public const int MAX_HEALTH = 20;
		public const int MIN_HEALTH = 0;
		public const int FIFTY_PERCENT_HEALTH = (int)(BasicEnemyBoss.MAX_HEALTH * 0.5f);
        public const int FIFTEEN_PERCENT_HEALTH = (int)(BasicEnemyBoss.MAX_HEALTH * 0.15f);
		public GameObject target;
	
		private AppearTask appearTask;
		private SpawnMinionsTask spawnTask;
		private FireTask fireTask;
		private ChaseTask chaseTask;
		[SerializeField]
		private int health;
		public int Health
		{
			get 
			{
				return health;
			}
			private set
			{
				health = value;
				if (health < MIN_HEALTH)
				{
					health = MIN_HEALTH;
					spawnTask.SetStatus(TaskStatus.Aborted);
					fireTask.SetStatus(TaskStatus.Aborted);
					chaseTask.SetStatus(TaskStatus.Aborted);
					Destroy(gameObject);
				}

				if (health > MAX_HEALTH)
				{
					health = MAX_HEALTH;
				}
			}
		}

		public SpriteRenderer spriteRenderer;

		public TaskManager tm = new TaskManager();
 		private EnemyBossDamagedEvent.Handler onEnemyBossDamaged;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			onEnemyBossDamaged = new EnemyBossDamagedEvent.Handler(OnEnemyBossDamaged);
			EventManager.Instance.Register<EnemyBossDamagedEvent>(onEnemyBossDamaged);
			target = GameObject.FindWithTag(PLAYER);
			appearTask = new AppearTask(this);
			spawnTask = new SpawnMinionsTask(this);
			appearTask.Then(spawnTask);
			tm.AddTask(appearTask);
		}

		override public void Start()
		{
			
			health = MAX_HEALTH;
			spriteRenderer = GetComponent<SpriteRenderer> ();

        	_Width = spriteRenderer.bounds.extents.x;
        	_Height = spriteRenderer.bounds.extents.y;

        	_Position = transform.position;
        	_AudioSource = GetComponent <AudioSource> ();
        	_Animator = GetComponent <Animator> ();
        	_Rigidbody2D = GetComponent <Rigidbody2D> ();			
		}

		void OnEnemyBossDamaged(GameEvent e)
		{
			int currentHP = ((EnemyBossDamagedEvent) e).boss.Health;
			switch(currentHP)
            {
                case FIFTY_PERCENT_HEALTH:
                        fireTask = new FireTask(this);
						tm.AddTask(fireTask);
                    break;
                case FIFTEEN_PERCENT_HEALTH:
                        chaseTask = new ChaseTask(this, target.transform);
						tm.AddTask(chaseTask);
                    break;
                default:
                    break;
            }
		}

		public void Create()
		{
			
		}

		public void ChasePlayer(Transform target)
		{
			FollowTarget(target);
		}

		protected override void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.CompareTag ("PlayerBullet"))
        	{
				Health--;
				GameEventsManager.EventManager.Instance.Fire(new EnemyBossDamagedEvent(this));
        	}
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		override public void Update()
		{
			tm.Update();
		}
	}
}