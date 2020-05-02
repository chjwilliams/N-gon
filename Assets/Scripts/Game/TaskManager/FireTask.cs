using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using GamePrefabs;

namespace GameTasks
{
	public class FireTask : Task 
	{

		private bool isFiring;

		private float cooldownTime = 1.0f;
		private float shotTimer = 0.0f;
		private List<BasicBullet> _BulletList;
		protected BasicEnemyBoss boss;

		private float rotationOffset = 270;
		public FireTask(BasicEnemyBoss b)
		{
			boss = b;
			isFiring = true;
		}

		protected override void Init()
		{
			Debug.Assert(boss != null);

			SetStatus(TaskStatus.Pending);
		}

		/*--------------------------------------------------------------------------------------*/
		/*																						*/
		/*	Shoot: Tells Basic Bullet class to get active										*/
		/*																						*/
		/*--------------------------------------------------------------------------------------*/
		private void Shoot ()
		{
			//	Shakes camera when 
			CameraShake.cameraShakeEffect.Shake (0.1f, 0.25f);
			Quaternion bulletRotation = Quaternion.Euler (0f, 0f, Random.rotation.z + rotationOffset);
			//	_Muzzle needs a new potion _Reticle need a new rotation
			GameObject bullet = (GameObject)MonoBehaviour.Instantiate (PrefabManager.Instance.bulletPrefab, boss.gameObject.transform.position, bulletRotation);
			bullet.tag = "EnemyBullet";
			//	Takes instantiated bullet and gets its BasicBullet script
			BasicBullet newBullet = bullet.GetComponent<BasicBullet> ();

			//	Random angle here
			//	Adjusts the trajectory of bullet shot to account for unity's coordinates system
			Transform adjustBullettrajectory = boss.gameObject.transform.transform;
			//	Rotates
			adjustBullettrajectory.Rotate (0.0f, 0.0f, 90.0f);
			float theta = adjustBullettrajectory.rotation.eulerAngles.z;

			//	Moves bullet in appropriate direction
			newBullet.dy = Mathf.Sin (theta.toRadians());
			newBullet.dx = Mathf.Cos (theta.toRadians());

		}

	


		// Update is called once per frame
		internal override void Update () 
		{
			// Fires Bullets
			if(isFiring)
			{
				shotTimer += Time.deltaTime;
				if (shotTimer > cooldownTime)
				{
					Shoot();
					shotTimer = 0.0f;
				}
			}
		}
	}
}
