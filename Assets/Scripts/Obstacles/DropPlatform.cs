using System.Collections;
using UnityEngine;

public class DropPlatform : MonoBehaviour
{
    [SerializeField] Rigidbody platformRigitbody;
    [SerializeField] BoxCollider platformCollider;
    [SerializeField] float destructionDelay = 2f;
    [SerializeField] float fallDelay = 3f;

    Rigidbody player;
    bool isPlayerOnPlatform = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerOnPlatform = true;

            player = other.GetComponent<Rigidbody>();

            StartCoroutine(FallOnTimer());
        }
    }


    void OnTriggerExit(Collider other)
    {
        StartFalling();
    }


    IEnumerator DestroyOnTimer()
    {
        yield return new WaitForSeconds(destructionDelay);
        Destroy(gameObject);
    }


    IEnumerator FallOnTimer()
    {
        yield return new WaitForSeconds(fallDelay);

        if (isPlayerOnPlatform)
        {
            StartFalling();
            player.useGravity = true;
            player.isKinematic = false;

            InputManager.Instance.IsEnabled = false;
        }
    }


    void StartFalling()
    {
        if (isPlayerOnPlatform)
        {
            isPlayerOnPlatform = false;

            platformCollider.enabled = false;
            platformRigitbody.useGravity = true;
            StartCoroutine(DestroyOnTimer());
        }
    }
}
