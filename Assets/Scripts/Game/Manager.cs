using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleManager
{
    
    // BASE CLASSES
  
    public interface IManaged
    {
        void OnCreated();
        void OnDestroyed();
    }

    public abstract class Manager<T> : MonoBehaviour where T : IManaged
    {
        protected static readonly List<T> ManagedObjects = new List<T>();

        public abstract T Create();

        public abstract void Destroy(T o);

        public T Find(Predicate<T> predicate)
        {
            return ManagedObjects.Find(predicate);
        }

        public List<T> FindAll(Predicate<T> predicate)
        {
            return ManagedObjects.FindAll(predicate);
        }
    }

}