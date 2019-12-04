using System;
using UnityEngine;


public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    #region Fields

    static readonly Type type = typeof(T);
    static readonly object lockObj = new object();

    static T instance;
    static bool isDestroyed = false;

    #endregion



    #region Properties

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                if (isDestroyed)
                {
                    Debug.LogWarning($"Singleton '{type.Name}' is already destroyed.");
                }
                else
                {
                    lock (lockObj)
                    {
                        CreateInstance();
                    }
                }
            }

            return instance;
        }
    }


    public static T InstanceIfExist => instance;

    #endregion



    #region Unity lifecycle

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Debug.LogWarning($"More than one Awake() of Singleton '{type.Name}'", gameObject);
        }
    }


    protected virtual void OnDestroy()
    {
        if (instance != null)
        {
            instance = null;
            isDestroyed = true;
        }
        else
        {
            Debug.LogWarning($"More than one OnDestroy() of Singleton '{type.Name}'", gameObject);
        }
    }

    #endregion



    #region Private methods

    static void CreateInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject obj = new GameObject(type.Name);
                instance = obj.AddComponent<T>();

                Debug.Log($"Created GameObject for Singleton '{type.Name}'", obj);
            }
            else
            {
                Debug.Log($"Singleton '{type.Name}' is using reference to found object", instance.gameObject);
            }
        }
    }

    #endregion
}

