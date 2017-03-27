using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicManager;
using Enemy;
using GameEventsManager;
using GameEvents;

namespace EnemyWaveSpawner 
{
 public class EnemyWaveManager : Manager<BasicEnemy>
{
        public const string TRIANGLE_PREFAB = "Triangle";
		public const string SQUARE_PREFAB = "Square";
        public const string PENTAGON_PREFAB = "Pentagon";
        public const string HEXAGON_PREFAB = "Hexagon";
        public const string BASIC_BOSS_PREFAB = "BasicBoss";

        public GameObject[] spawnPoints;
		
        public Dictionary<string, GameObject> enemyPrefabs;
        public bool bossHasSpawned;
        
        private GameObject thisEnemy;
        private static readonly Array EnemyTypes = Enum.GetValues(typeof(EnemyType));
        private readonly System.Random _rng = new System.Random();

        private const int MAX_NUMBER_OF_WAVES = 2;
        public int enemyWavesCreated;

        void Start()
        {
            enemyWavesCreated = 0;
            bossHasSpawned = false;
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        }

		public void PopulateDictionary()
		{
            enemyPrefabs = new Dictionary<string, GameObject> ();
			thisEnemy = (GameObject)Resources.Load<GameObject>("Prefabs/" + TRIANGLE_PREFAB);
			enemyPrefabs.Add(TRIANGLE_PREFAB,thisEnemy);
            thisEnemy =  null;
			thisEnemy = (GameObject)Resources.Load<GameObject>("Prefabs/" + SQUARE_PREFAB);
			enemyPrefabs.Add(SQUARE_PREFAB, thisEnemy);
            thisEnemy =  null;
			thisEnemy = (GameObject)Resources.Load<GameObject>("Prefabs/" + PENTAGON_PREFAB);
			enemyPrefabs.Add(PENTAGON_PREFAB, thisEnemy);
            thisEnemy = null;
            thisEnemy = (GameObject)Resources.Load<GameObject>("Prefabs/" + HEXAGON_PREFAB);
			enemyPrefabs.Add(HEXAGON_PREFAB, thisEnemy);
            thisEnemy = null;
            thisEnemy = (GameObject)Resources.Load<GameObject>("Prefabs/" + BASIC_BOSS_PREFAB);
			enemyPrefabs.Add(BASIC_BOSS_PREFAB, thisEnemy);
		}

        public static bool ListIsEmpty()
        {
            return ManagedObjects.Count == 0? true: false;
        }

        public void PopulateSpawnPoints()
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint"); 
        }

        public override BasicEnemy Create()
        {
            BasicEnemy enemy = Init(GetRandomEnemyType());
            
            ManagedObjects.Add(enemy);
            enemy.OnCreated();
            return enemy;
        }

        public BasicEnemy CreateAtLocation(Transform t)
        {
            BasicEnemy enemy = Init(GetRandomEnemyType(), t);
            
            ManagedObjects.Add(enemy);
            enemy.OnCreated();
            return enemy;
        }

		public BasicEnemy Init(EnemyType enemyType)
		{
            BasicEnemy enemy = null;
            if (enemyType != EnemyType.BasicBoss)
            {
			    GameObject newEnemy = MonoBehaviour.Instantiate(enemyPrefabs[enemyType.ToString()], spawnPoints[_rng.Next() % spawnPoints.Length].transform.position, Quaternion.identity) as GameObject;
            
			    enemy = new BasicEnemy();
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
                    case PENTAGON_PREFAB:
				    	newEnemy.AddComponent <Pentagon> ();
				    	enemy = newEnemy.GetComponent <Pentagon> ();
				    	break;
                    case HEXAGON_PREFAB:
				    	newEnemy.AddComponent <Hexagon> ();
				    	enemy = newEnemy.GetComponent <Hexagon> ();
				    	break;
			    }
            }

			return enemy;
		}

        public BasicEnemy Init(EnemyType enemyType, Transform t)
		{
            BasicEnemy enemy = null;
            if (enemyType != EnemyType.BasicBoss)
            {
			    GameObject newEnemy = MonoBehaviour.Instantiate(enemyPrefabs[enemyType.ToString()], t.position, Quaternion.identity) as GameObject;
            
			    enemy = new BasicEnemy();
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
                    case PENTAGON_PREFAB:
				    	newEnemy.AddComponent <Pentagon> ();
				    	enemy = newEnemy.GetComponent <Pentagon> ();
				    	break;
                    case HEXAGON_PREFAB:
				    	newEnemy.AddComponent <Hexagon> ();
				    	enemy = newEnemy.GetComponent <Hexagon> ();
				    	break;
			    }
            }

			return enemy;
		}

        public BasicEnemyBoss InitBoss()
		{
            BasicEnemyBoss enemy = new BasicEnemyBoss();
			GameObject newEnemy = MonoBehaviour.Instantiate(enemyPrefabs[BASIC_BOSS_PREFAB], spawnPoints[_rng.Next() % spawnPoints.Length].transform.position, Quaternion.identity) as GameObject;
            
            newEnemy.AddComponent<BasicEnemyBoss>();
            enemy = newEnemy.GetComponent<BasicEnemyBoss>();
            EventManager.Instance.Fire(new SpawnEnemyBossEvent(enemy));
            bossHasSpawned = true;
			return enemy;
		}

        public override void Destroy(BasicEnemy enemy)
        {
            ManagedObjects.Remove(enemy);

            if(ManagedObjects.Count < 1)
            {
                EventManager.Instance.Fire(new EnemyWaveDestroyedEvent(this));
            }

            enemy.GetComponent<SpriteRenderer>().color = new Color (0, 0, 0, 0);
            enemy.OnDestroyed();
        }

        public List<BasicEnemy> Create(uint n)
        {
            
            List<BasicEnemy> enemy = null;
            if (enemyWavesCreated < MAX_NUMBER_OF_WAVES)
            {
                enemy = new List<BasicEnemy>();
                for (var i = 0; i < n; i++)
                {
                    enemy.Add(Create());
                }
            }
            else
            {
                 if (!bossHasSpawned)
                {
                    InitBoss();
                }
            }
            enemyWavesCreated++;
            return enemy;
        }

         public List<BasicEnemy> CreateNAtLocation(uint n, Transform t)
        {
            List<BasicEnemy> enemy = null;
  
            enemy = new List<BasicEnemy>();
            for (var i = 0; i < n; i++)
            {
               enemy.Add(CreateAtLocation(t));
            }
            
            return enemy;
        }

        

        private EnemyType GetRandomEnemyType()
        {   
            EnemyType thisType = (EnemyType)(_rng.Next() % enemyPrefabs.Count);
            while (thisType.ToString() == BASIC_BOSS_PREFAB)
            {
                thisType = (EnemyType)(_rng.Next() % enemyPrefabs.Count);
            }
            return thisType;
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