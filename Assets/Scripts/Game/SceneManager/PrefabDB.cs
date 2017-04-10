using UnityEngine;

namespace PrefabDataBase
{
    [CreateAssetMenu (menuName = "Prefab DB")]
    public class PrefabDB : ScriptableObject
    {

        [SerializeField] private GameObject _player;
        public GameObject Player
        {
            get { return _player; }
        }

        [SerializeField] private GameObject[] _levels;
        public GameObject[] Levels
        {
            get { return _levels; }
        }
    }
}