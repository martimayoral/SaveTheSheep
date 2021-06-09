using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public GameObject player;
    public float interpolationFactior = 0.1f;
    public float minDistance;

    void Start()
    {
        transform.SetPositionAndRotation(player.transform.position, Quaternion.identity);
    }

    void Update()
    {
        Vector3 playerPos = player.transform.position;

        float distanceToPlayer = Mathf.Abs(Vector3.Distance(playerPos, transform.position));
        if (distanceToPlayer > minDistance)
        {
            transform.LookAt(playerPos);
            transform.position += (playerPos - transform.position) * interpolationFactior;
        }
    }
}
