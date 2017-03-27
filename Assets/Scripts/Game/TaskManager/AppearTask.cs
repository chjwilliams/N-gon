using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace GameTasks
{
	public class AppearTask : Task 
	{
		BasicEnemyBoss boss;
		private Vector3 MIN_SIZE = new Vector3(0.1f, 0.1f, 0.1f);
		private Vector3 currentSize;
		private float growthRate = 1.1f;
		private Vector3 MAX_SIZE = Vector3.one;
		
		public AppearTask(BasicEnemyBoss b)
		{
			boss = b;
			boss.transform.localScale = MIN_SIZE;
			currentSize = MIN_SIZE;
		}

		protected override void Init()
		{
			SetStatus(TaskStatus.Pending);
		}
	
		// Update is called once per frame
		internal override void Update () 
		{
			if (currentSize.x < MAX_SIZE.x)
			{
				currentSize = new Vector3(currentSize.x * growthRate, currentSize.y * growthRate, currentSize.z * growthRate);
				boss.transform.localScale = currentSize;
			}
			else
			{
				SetStatus(TaskStatus.Success);
			}

		}
	}
}