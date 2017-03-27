using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using GameEventsManager;
using GC;
using Enemy;

namespace BasicEnemyStates
{
	public class DefaultEnemyState : FiniteStateMachine<Hexagon>.State
	{

	}

	public class SeekingState : FiniteStateMachine<Hexagon>.State
	{


		 private Transform target;
		 private float inAttackRange = 4.0f;

		 public override void Init()
		 {
			target = GameObject.FindGameObjectWithTag("Player").transform;
			Context.moveSpeed = 0.2f;
		 }

		 public override void OnEnter()
		 {
			 Context.moveSpeed = 0.2f;
		 }

		 public override void Update()
		 {
			 Context.FollowTarget(target);
			 if(Vector3.Distance(Context.transform.position, target.position) < inAttackRange)
			 {
				TransitionTo<AttackPrepState>();
			 }
		 }
	}

	public class AttackPrepState : FiniteStateMachine<Hexagon>.State
	{
		/*
			Attack Preparation: Enemy stops moving and pulses 5 times. If it is 
						hit while pulsing it will enter the fleeing state. 
						If not, it will enter the attack state. 
		*/

		private Vector3 initalScale;
		private float timeSincePulseStart;
		private int maxNumberOfPulses = 5;
		private EnemyFleeEvent.Handler onEnemyShouldFlee;
		public override void OnEnter()
		{
			timeSincePulseStart = 0;
			onEnemyShouldFlee = new EnemyBossDamagedEvent.Handler(OnEnemyShouldFlee);
			EventManager.Instance.Register<EnemyBossDamagedEvent>(onEnemyShouldFlee);
			initalScale = Context.transform.localScale;
			Context.preparingToAttack = true;
		}

		void OnEnemyShouldFlee(GameEvent e)
		{
			TransitionTo<FleeingState>();
		}

		public override void Update()
		{

			if (timeSincePulseStart <= Context.pulseTimer)
            {
                Context.gameObject.transform.localScale = Vector3.Lerp(initalScale, Context.maxSize * initalScale, Easing.QuadEaseOut(timeSincePulseStart / Context.pulseTimer));
                timeSincePulseStart += Time.deltaTime;
            }
			else
			{
				Context.numberOfPulses++;
				timeSincePulseStart = 0.0f;
			}


			if(Context.numberOfPulses >= maxNumberOfPulses)
			{
				TransitionTo<AttackState>();
			}
		}

		public override void OnExit()
		{
			timeSincePulseStart = 0.0f;
			Context.numberOfPulses = 0;
			Context.preparingToAttack = false;
			Context.transform.localScale = initalScale;
			EventManager.Instance.Unregister<EnemyFleeEvent>(onEnemyShouldFlee);
		}

	}

	public class AttackState : FiniteStateMachine<Hexagon>.State
	{
		/*
			Attack: Enemy moves quickly in a straight line towards the player for 
			X distance. If it hits the player it will explode for Y damage. 
			Otherwise it will enter the seeking state.		
		 */
		Transform target;
		Vector3 initalAttackPosition;
		private float attackTimer;
		public override void OnEnter()
		{
			target = GameObject.FindGameObjectWithTag("Player").transform;
			initalAttackPosition = Context.transform.position;
			Context.isAttacking = true;
			Context.moveSpeed = 2.0f;

			attackTimer = Vector3.Distance(initalAttackPosition, target.position) / (Context.moveSpeed * 0.5f);

		}

		public override void Update()
		{
			if(Context.transform == target)
			{
				TransitionTo<SeekingState>();
			}
			else
			{
				if(Vector3.Distance(initalAttackPosition, target.position) < Context.attackRange)
				{
					Context.transform.position = Vector3.Lerp(Context.transform.position, target.position, attackTimer);
				}
				else
				{
					TransitionTo<SeekingState>();
				}
			}
		}

		public override void OnExit()
		{
			Context.isAttacking = false;
			attackTimer = 0.0f;
		}
	}

	public class FleeingState : FiniteStateMachine<Hexagon>.State
	{
		/*

			Fleeing: 	Enemy picks a random position X distance away from player and 
						moves quickly towards it. When it reaches the position it 
						enters the seeking state
 		*/

		 private float fleeTimer;
		 private Vector3 fleeingPosition;
		 private Vector3 initalFleeingPosition;
		 private Transform dangerPosition;

		public override void OnEnter()
		{
			dangerPosition = GameObject.FindGameObjectWithTag("Player").transform;
			float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
			Vector3 fleeingDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
			fleeingPosition = dangerPosition.position + (fleeingDirection * Context.safetyDistance);

			initalFleeingPosition = Context.transform.position;

			fleeTimer = Vector3.Distance(initalFleeingPosition, fleeingPosition) / Context.moveSpeed;

		}

		public override void Update()
		{
			if (Vector3.Distance(initalFleeingPosition, fleeingPosition) < Context.safetyDistance)
			{
				Context.transform.position = Vector3.Lerp(Context.transform.position, fleeingPosition, fleeTimer);
			}
			else
			{
				TransitionTo<SeekingState>();
			}
		}
	}
}