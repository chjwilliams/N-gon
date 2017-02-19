using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleManager;
using Enemy;

namespace EnemyWaveManager_ 
{
 public class EnemyWaveManager : Manager<BasicEnemyControls>
{
        public const string TRIANGLE_PREFAB = "Triangle";
		public const string SQUARE_PREFAB = "Square";

        public GameObject[] spawnPoints;
		
        public Dictionary<string, GameObject> enemyPrefabs;
        
        private GameObject thisEnemy;
        private static readonly Array EnemyTypes = Enum.GetValues(typeof(EnemyType));
        private readonly System.Random _rng = new System.Random();

		public void PopulateDictionary()
		{
            enemyPrefabs = new Dictionary<string, GameObject> ();
			thisEnemy = (GameObject)Resources.Load<GameObject>("Prefabs/" + "Triangle");
			enemyPrefabs.Add(TRIANGLE_PREFAB,thisEnemy);
            thisEnemy =  null;
			thisEnemy = (GameObject)Resources.Load<GameObject>("Prefabs/" + SQUARE_PREFAB);
			enemyPrefabs.Add(SQUARE_PREFAB, thisEnemy);
		}

        public static bool ListIsEmpty()
        {
            return ManagedObjects.Count == 0? true: false;
        }

        public void PopulateSpawnPoints()
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint"); 
        }

        public override BasicEnemyControls Create()
        {
            BasicEnemyControls enemy = Init(GetRandomEnemyType());
            
            ManagedObjects.Add(enemy);
            enemy.OnCreated();
            return enemy;
        }

		public BasicEnemyControls Init(EnemyType enemyType)
		{
			GameObject newEnemy = MonoBehaviour.Instantiate(enemyPrefabs[enemyType.ToString()], spawnPoints[_rng.Next() % spawnPoints.Length].transform.position, Quaternion.identity) as GameObject;
			BasicEnemyControls enemy = new BasicEnemyControls();
			switch (enemyType.ToString())
			{
				case TRIANGLE_PREFAB:
	    			newEnemy.AddComponent <Triangle> ();
					enemy = newEnemy.GetComponent <Triangle> ();
					break;
				case SQUARE_PREFAB:
					newEnemy.AddComponent <Square> ();
					enemy = newEnemy.GetComponent <Square> ();
					break;
			}

			return enemy;
		}

        public override void Destroy(BasicEnemyControls enemy)
        {
            Debug.Log("Before Removing: " + ManagedObjects.Count);
            ManagedObjects.Remove(enemy);
            Debug.Log("After Removing: " + ManagedObjects.Count);
            enemy.GetComponent<SpriteRenderer>().color = new Color (0, 0, 0, 0);
            enemy.OnDestroyed();
        }

        public List<BasicEnemyControls> Create(uint n)
        {
            List<BasicEnemyControls> enemy = new List<BasicEnemyControls>();
            for (var i = 0; i < n; i++)
            {
                enemy.Add(Create());
            }
            return enemy;
        }

        private EnemyType GetRandomEnemyType()
        {
            return (EnemyType)(_rng.Next() % enemyPrefabs.Count);
        }

        public void DestroyAllWithType(EnemyType et)
        {
            foreach (var e in FindAll(e => e.EnemyTypes == et))
            {
                Destroy(e);
            }
        }

        public int NumWithType(EnemyType et)
        {
            return FindAll(e => e.EnemyTypes == et).Count;
        }

        public void PrintCounts()
        {
            foreach (EnemyType et in EnemyTypes)
            {
                Console.WriteLine("There are {0} {1} gems", NumWithType(et), et);
            }
        }
   }
}