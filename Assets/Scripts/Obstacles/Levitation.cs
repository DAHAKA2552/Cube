using UnityEngine;

public class Levitation : MonoBehaviour
{
    [SerializeField] float levitationSpeed;
    [SerializeField] float levitationRange;

    Vector3 startPosition;
    float timer = 0.0f;


    void Start()
    {
        startPosition = transform.position;
    }


    void Update()
    {
        transform.position = startPosition + Vector3.up * Mathf.Sin(timer) * levitationRange;

        timer += Time.deltaTime * levitationSpeed;

    }
}
