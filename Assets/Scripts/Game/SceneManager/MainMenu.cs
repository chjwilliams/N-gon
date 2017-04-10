using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneManager;
using GameEvents;
using GameScenes;
public class MainMenu : Scene<TransitionData> 
{


	private SpaceButtonDownEvent.Handler onSpaceDown;
	internal override void OnEnter(TransitionData data)
	{
		onSpaceDown = new SpaceButtonDownEvent.Handler(OnSpaceDown);
		Services.Events.Register<SpaceButtonDownEvent>(onSpaceDown);
	}

	internal override void OnExit()
	{
		Services.Events.Unregister<SpaceButtonDownEvent>(onSpaceDown);
	}

	private void OnSpaceDown(GameEvent e)
	{
		Services.Scenes.Swap<GameScene>();
	}
}
