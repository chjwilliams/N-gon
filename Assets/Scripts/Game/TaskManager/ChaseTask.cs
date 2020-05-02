using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using GameEvents;
using GameEventsManager;

namespace GameTasks
{
	public class ChaseTask : Task 
	{

		protected Transform target;
		protected BasicEnemyBoss boss;
		//private EnemyBossDamaged.Handler onEnemyBossDamaged;

		public ChaseTask(BasicEnemyBoss b, Transform t)
		{
			target = t;
			boss = b;
		}


		protected override void Init()
		{
			if (boss == null || target == null)
			{
				SetStatus(TaskStatus.Aborted);
			}

			//onEnemyBossDamaged = new EnemyBossDamaged.Handler(OnEnemyBossDamaged);
			//EventManager.Instance.Register<EnemyBossDamaged>(onEnemyBossDamaged);
			SetStatus(TaskStatus.Pending);
		}

		public void OnEnemyBossDamaged(GameEvent e)
		{
			
		}
	
		// Update is called once per frame
		internal override void Update () 
		{
			if (boss == null || target == null)
			{
				SetStatus(TaskStatus.Aborted);
			}

			boss.ChasePlayer(target);
		}
	}
}