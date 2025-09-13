using System;
using UnityEngine;

namespace MyPlatformer
{
    public abstract class Singleton<T> : MonoBehaviour
                   where  T : Singleton<T>
    {
        public static T instance => _instance;
        private static T _instance;

        protected virtual  void Awake()
        {
            _instance = this as T;
        }
    }
}