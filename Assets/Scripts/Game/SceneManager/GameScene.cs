using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScenes;
using GameEvents;

public class GameScene : Scene<TransitionData> 
{

	private EnemyBossDiedEvent.Handler onEnemyBossDied;
	internal override void OnEnter(TransitionData data)
	{
		onEnemyBossDied = new EnemyBossDiedEvent.Handler(OnEnemyBossDied);
		Services.Events.Register<EnemyBossDiedEvent>(onEnemyBossDied);
	}

	internal override void OnExit()
	{
		Services.Events.Unregister<EnemyBossDiedEvent>(onEnemyBossDied);
	}

	private void OnEnemyBossDied(GameEvent e)
	{
		Services.Scenes.Swap<GameOver>();
	}
}
