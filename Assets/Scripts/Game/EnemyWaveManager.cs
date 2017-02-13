using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	EnemyWaveManager: Spawns waves of Enemies			   								*/
/*			Functions:																	*/
/*					public:																*/
/*						void EnemyToDestroy (BasicEnemyControls enemy)     				*/
/*					proteceted:															*/
/*                                                                                      */
/*					private:															*/
/*						void Start ()													*/
/*						IEnumerator SpawnWave(bool waveAIsActive)						*/
/*						void DestroyEnemies(List<BasicEnemyControls> deadEnemies)	    */
/*						void Update ()													*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class EnemyWaveManager : MonoBehaviour 
{
	//	Static Variables
	public static EnemyWaveManager instance;						//	Instance of the EnemyWaveManager

	//	Public Variables
	public int maxNumberOfEnemiesInWave = 10;						//	Max number of enemies in a wave
	public List<BasicEnemyControls> currentWave;					//	Reference to current wave

	//	Private Variables
	[SerializeField]
	private bool _WaveAIsActive;									//	Toggles which wave is active
	private List<BasicEnemyControls> _EnemiesToDelete;				//	List for enemies to delete
	private List<BasicEnemyControls> _EnemyWaveA;					//	Enemies in wave A
	private List<BasicEnemyControls> _EnemyWaveB;					//	Enemies in wave B

	/*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Start: Runs once at the begining of the game. Initalizes variables.					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
	private void Start () 
	{
		if (instance == null)
		{
			instance = GetComponent<EnemyWaveManager>();
		}
		else
		{
			Destroy(this.gameObject);
		}

		_EnemiesToDelete = new List<BasicEnemyControls>();
		_EnemyWaveA = new List<BasicEnemyControls>();
		_EnemyWaveB = new List<BasicEnemyControls>();
		currentWave = new List<BasicEnemyControls>();

		_WaveAIsActive = true;
		
		currentWave = _EnemyWaveA;
		StartCoroutine(SpawnWave());
	}

	private IEnumerator SpawnWave()
	{
		
		yield return new WaitForSeconds(4.0f);
		if (_WaveAIsActive)
		{
			currentWave = _EnemyWaveA;
			for (int i = 0; i < maxNumberOfEnemiesInWave; i++)
			{
				if (_EnemyWaveA.Count < maxNumberOfEnemiesInWave)
				{
					
					GameObject newEnemy = Instantiate (PrefabManager.Instance.trianglePrefab, new Vector3 (i - 2.0f , i , 0), Quaternion.identity);
	    			newEnemy.AddComponent <Triangle> ();

					BasicEnemyControls enemy = newEnemy.GetComponent <Triangle> ();

	    			enemy.sides = 3;
	    			enemy.moveSpeed = 5.0f;
				
					_EnemyWaveA.Add(enemy);
				}
			}
			
		}
		else
		{
			currentWave = _EnemyWaveB;
			for (int i = 0; i < maxNumberOfEnemiesInWave; i++)
			{
				if (_EnemyWaveB.Count < maxNumberOfEnemiesInWave)
				{
					GameObject newEnemy = Instantiate (PrefabManager.Instance.squarePrefab, new Vector3 (i - 2.0f , i , 0), Quaternion.identity);
	    			newEnemy.AddComponent <Square> ();

					BasicEnemyControls enemy = newEnemy.GetComponent <Square> ();

	    			enemy.sides = 4;
	    			enemy.moveSpeed = 5.0f;
				
					_EnemyWaveB.Add(enemy);
				}
			}

			
		}

	}

	public void EnemyToDestroy (BasicEnemyControls enemy)
	{
		enemy.GetComponent<Collider2D>().enabled = false;
		enemy.GetComponent<SpriteRenderer>().color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		_EnemiesToDelete.Add(enemy);

		currentWave.Remove(enemy);
	}

	private void DestroyEnemies(List<BasicEnemyControls> deadEnemies)
	{
		for (int i = 0; i < deadEnemies.Count; i++)
		{
			Destroy(deadEnemies[i].gameObject);
			_EnemiesToDelete.Remove(deadEnemies[i]);
		}
	}
	
	/*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	Update: Called once per frame														*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if (currentWave.Count == 0)
		{
			_WaveAIsActive = !_WaveAIsActive;
			StartCoroutine(SpawnWave());
		}

		if (_EnemiesToDelete.Count > 0)
		{
			DestroyEnemies (_EnemiesToDelete);
		}
	}
}
