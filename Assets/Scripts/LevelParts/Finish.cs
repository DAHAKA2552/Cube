using System;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public static event Action OnFinish;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnFinish?.Invoke();
        }
    }
}