using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using BasicEnemyStates;
using GameEvents;
using GameEventsManager;

namespace Enemy 
{

	public class Hexagon : BasicEnemy 
	{

		FiniteStateMachine<Hexagon> stateMachine;
		public int numberOfPulses;
		public float pulseTimer;
		public float maxSize;
		public float attackRange;
		public float safetyDistance;
		private int health;
		public int Health
		{
			get { return health;}
			set
			{
				health = value;
				if(health < 0)
				{
					KillEnemy();
				}
			}
		}
		public bool preparingToAttack;
		public bool isAttacking;

		// Use this for initialization
		override public void Start () 
		{
			moveSpeed = 1.0f;
			isAttacking = false;
			pulseTimer = 1.0f;
			maxSize = 1.5f;
			health = 5;
			attackRange = 2.0f;
			safetyDistance = 5.0f;
			preparingToAttack = false;
			numberOfPulses = 0;
			base.Start();
			stateMachine = new FiniteStateMachine<Hexagon>(this);
			stateMachine.TransitionTo<SeekingState>();
		}

		void KillEnemy()
		{
			Destroy(gameObject);
		}
		
		// Update is called once per frame
		override public void Update () 
		{
			stateMachine.Update();

		}

		override protected void OnCollisionEnter2D(Collision2D other)
		{

			if (isAttacking && other.gameObject.CompareTag("Player"))
			{
				KillEnemy();
			}

			if (other.gameObject.CompareTag("PlayerBullet"))
			{
				health--;
				if(preparingToAttack)
				{
					GameEventsManager.EventManager.Instance.Fire(new EnemyFleeEvent(this));
				}
			}
		}
	}
}
