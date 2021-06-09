using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public GameObject player;
    public float interpolationFactior = 0.1f;
    public float minDistance;

    public GameObject particleClose;
    public GameObject particleFar;

    private GameObject wolf;

    void Start()
    {
        transform.SetPositionAndRotation(player.transform.position, Quaternion.identity);
        particleClose = transform.GetChild(1).gameObject;
        particleFar = transform.GetChild(2).gameObject;
        wolf = GameObject.Find("Wolf");
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

        particleFar.transform.LookAt(wolf.transform);
        particleClose.transform.LookAt(wolf.transform);
    }
}
