using System.Collections;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    [SerializeField] float speed = 1f;
    [SerializeField] float nextTargetDelay = 0f;

    Vector3 targetWaypoint;
    int countPoint = 1;
    bool isWaiting = false;


    void Start()
    {
        transform.position = new Vector3(
            waypoints[0].transform.position.x, 
            waypoints[0].transform.position.y + waypoints[0].transform.localScale.y, 
            waypoints[0].transform.position.z);

        targetWaypoint = new Vector3(
            waypoints[countPoint].transform.position.x, 
            waypoints[countPoint].transform.position.y + waypoints[countPoint].transform.localScale.y, 
            waypoints[countPoint].transform.position.z);
    }


    void Update()
    {
        if (transform.position == targetWaypoint)
        {
            if (!isWaiting)
            {
                StartCoroutine(ChangeWaypoint());
                isWaiting = !isWaiting;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
        }
    }


    IEnumerator ChangeWaypoint()
    {
        yield return new WaitForSeconds(nextTargetDelay);

        if (countPoint < waypoints.Length - 1)
        {
            countPoint++;
        }
        else
        {
            countPoint = 0;
        }

        targetWaypoint = new Vector3(
                waypoints[countPoint].transform.position.x,
                waypoints[countPoint].transform.position.y + waypoints[countPoint].transform.localScale.y,
                waypoints[countPoint].transform.position.z);

        isWaiting = !isWaiting;
    }
}
