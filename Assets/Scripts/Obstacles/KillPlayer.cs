using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            InputManager.Instance.IsEnabled = false;
            UIManager.Instance.ShowDeathScreen();
        }
    }
}
