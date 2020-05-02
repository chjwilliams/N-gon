using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using EnemyWaveSpawner;
using GameEvents;
using GameEventsManager;

namespace GameTasks
{
	public class SpawnMinionsTask : Task 
	{
		private BasicEnemyBoss boss;
		public EnemyWaveManager enemyWaveManager;
    	private EnemyWaveDestroyedEvent.Handler onEnemyWaveDestroyed;
		public SpawnMinionsTask(BasicEnemyBoss b)
		{
			boss = b;
			onEnemyWaveDestroyed = new EnemyWaveDestroyedEvent.Handler(OnEnemyWaveDestroyed);
			EventManager.Instance.Register<EnemyWaveDestroyedEvent>(onEnemyWaveDestroyed);
		}

		protected override void Init()
		{
			Debug.Assert(boss != null);
			enemyWaveManager = GameManager.instance.enemyWaveManager;
			SetStatus(TaskStatus.Pending);
			enemyWaveManager.CreateNAtLocation(5, boss.transform);
			SetStatus(TaskStatus.Success);
		}

		protected override void OnAbort()
		{
			EventManager.Instance.Unregister<EnemyDiedEvent>(onEnemyWaveDestroyed);
		}

		public void OnEnemyWaveDestroyed(GameEvent e)
		{
        	enemyWaveManager.CreateNAtLocation(5, boss.transform);
		}
	}
}
