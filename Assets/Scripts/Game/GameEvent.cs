using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using EnemyWaveSpawner;

namespace GameEvents
{
    public abstract class GameEvent 
    {
        public delegate void Handler(GameEvent e);
    }

    public class PlayerDiedEvent : GameEvent
    {
        public readonly PlayerControls player;

        public PlayerDiedEvent(PlayerControls player) 
	    {
            this.player = player;
        }
    }

    public class EnemyDiedEvent : GameEvent
    {
	    public readonly BasicEnemy enemy;
	    public EnemyDiedEvent(BasicEnemy enemy) 
	    {
            this.enemy = enemy;
        }
    }

    public class EnemyWaveDestroyedEvent : GameEvent
    {
    	public readonly EnemyWaveManager enemyWaveManager;
    	public EnemyWaveDestroyedEvent(EnemyWaveManager enemyWaveManager) 
    	{
            this.enemyWaveManager = enemyWaveManager;
        }
    }
}