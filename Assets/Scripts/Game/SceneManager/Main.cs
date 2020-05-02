using UnityEngine;
using UnityEngine.Assertions;
using GameEvents;
using GameEventsManager;
using PrefabDataBase;
using SceneManager;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        Assert.raiseExceptions = true;

        Services.Prefabs = Resources.Load<PrefabDB>("Prefabs/PrefabDB");
        Services.Events = new EventManager();
        Services.Scenes = new SceneManager<TransitionData>(gameObject, Services.Prefabs.Levels);

        Services.Scenes.PushScene<MainMenu>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Services.Events.Fire(new SpaceButtonDownEvent(KeyCode.Space));
        }
    }
}

public class SpaceButtonDownEvent : GameEvent
{
    public readonly KeyCode space;

    public SpaceButtonDownEvent(KeyCode key)
    {
        space = key;
    }
}
