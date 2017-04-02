using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Enemy;

namespace BehaviorNodes
{
	public class PlayerIsInRange : Node<Hexagon>
	{
		public override bool Update(Hexagon enemy)
		{
			 if(enemy.GetDistanceFromPoint(enemy.player.position) < enemy.enemySight)
			 {
				return true;
			 }
			 else
			 {
				 return false;
			 }
		}
	}

	public class DoneAttackPrep : Node<Hexagon>
	{
		public override bool Update(Hexagon enemy)
		{
			if(enemy.numberOfPulses >= enemy.MAX_NUMBER_OF_PULSES)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public class IsDamaged : Node<Hexagon>
	{
		public override bool Update(Hexagon enemy)
		{
			return enemy.hasBeenDamaged;
		}
	}

	public class SeekingBehavior :  Node<Hexagon>
	{
		public override bool Update(Hexagon enemy)
		{
			enemy.preparingToAttack = false;
			enemy.isAttacking = false;
			enemy.FollowTarget(enemy.player);
			return true;
		}
	}	

	public class AttackPreperaionBehavior :  Node<Hexagon>
	{
		private float timeSincePulseStart = 0;
		public override bool Update(Hexagon enemy)
		{
			enemy.preparingToAttack = true;
			enemy.isAttacking = false;
			if (timeSincePulseStart <= enemy.pulseTimer)
            {
				enemy.PulseEnemy(Vector3.one, enemy.maxSize, timeSincePulseStart,enemy.pulseTimer);
				timeSincePulseStart += Time.deltaTime;
			}
			else
			{
				enemy.numberOfPulses++;
				timeSincePulseStart = 0.0f;
			}
			return true;
		}
	}

	public class AttackingBehavior :  Node<Hexagon>
	{
		public override bool Update(Hexagon enemy)
		{
			enemy.preparingToAttack = false;
			enemy.isAttacking = true;
			enemy.FollowTarget(enemy.player);
			return true;
		}
	}

	public class FleeingBehavior :  Node<Hexagon>
	{
		public override bool Update(Hexagon enemy)
		{
			enemy.preparingToAttack = false;
			enemy.isAttacking = false;

			Debug.Log("Flee");
			if (Vector3.Distance(enemy.transform.position, enemy.fleeingPosition) > enemy.safetyDistance)
			{
				enemy.transform.position = Vector3.Lerp(enemy.transform.position, enemy.fleeingPosition, enemy.fleeTimer);
			}
			return true;
		}
	}			
}