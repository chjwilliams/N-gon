using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using BasicEnemyStates;
using GameEvents;
using GameEventsManager;
using StateMachine;
using BehaviorTree;
using BehaviorNodes;

namespace Enemy 
{

	public class Hexagon : BasicEnemy 
	{

		//FiniteStateMachine<Hexagon> stateMachine;
		private Tree<Hexagon> _tree;
		
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
		
		// Use this for initialization
		override public void Start () 
		{
			enemySight = 2.0f;
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

			_tree = new Tree<Hexagon>(new Selector<Hexagon>(
				//	Flee
				new Sequence<Hexagon>(
					new Not<Hexagon>(new DoneAttackPrep()),
					new IsDamaged(),
					new FleeingBehavior()
				),
				//	Attack
				new Sequence<Hexagon>(
					new PlayerIsInRange(),
					new DoneAttackPrep(),
					new AttackingBehavior()
				),
				//	Attack Prep 
				new Sequence<Hexagon>(
					new PlayerIsInRange(),
					new Not<Hexagon>(new DoneAttackPrep()),
					new AttackPreperaionBehavior()
				),
				//	Seek
				new Sequence<Hexagon>(
					new Not<Hexagon>(new PlayerIsInRange()),
					new SeekingBehavior()
				)
			)

			);
			//stateMachine = new FiniteStateMachine<Hexagon>(this);
			//stateMachine.TransitionTo<SeekingState>();
		}

		void KillEnemy()
		{
			Destroy(gameObject);
		}

		void Flee(Vector3 initalFleeingPosition)
		{
			float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
			Vector3 fleeingDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
			fleeingPosition = player.position + (fleeingDirection * safetyDistance);

			fleeTimer = Vector3.Distance(initalFleeingPosition, fleeingPosition) / moveSpeed;
		}
		
		// Update is called once per frame
		override public void Update () 
		{
			//stateMachine.Update();
			_tree.Update(this);
		}

		override protected void OnCollisionEnter2D(Collision2D other)
		{

			if (isAttacking && other.gameObject.CompareTag("Player"))
			{
				KillEnemy();
			}

			if (other.gameObject.CompareTag("PlayerBullet"))
			{
				hasBeenDamaged = true;
				health--;
				hasBeenDamaged = false;
				if(preparingToAttack)
				{
					Flee(transform.position);
					//	Function to generate random position
					//	GameEventsManager.EventManager.Instance.Fire(new EnemyFleeEvent(this));
				}
			}
		}
	}
}
