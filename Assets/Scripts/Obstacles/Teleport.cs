using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform endPoint;
    [SerializeField] PlayerController player;
    [SerializeField] float speed;
    Vector3 startPosition;
    bool isOnTeleport = false;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if(isOnTeleport)
        {
            if (player.transform.position != endPoint.position + Vector3.up)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, endPoint.position + Vector3.up, speed * Time.deltaTime);
            }
            else
            {
                isOnTeleport = false;
                player.gameObject.transform.localScale = new Vector3(1, 1, 1);
                player.gameObject.GetComponent<BoxCollider>().enabled = true;
                player.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!isOnTeleport)
            {
                player.enabled = false;
                player.gameObject.transform.localScale = new Vector3(0, 0, 0);
                player.gameObject.GetComponent<BoxCollider>().enabled = false;
                isOnTeleport = true;
            }
        }
    }
}
