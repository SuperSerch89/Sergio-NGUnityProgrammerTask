using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace NicoUtilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        public static bool isInitialized {
            get { return Instance != null; }
        }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}