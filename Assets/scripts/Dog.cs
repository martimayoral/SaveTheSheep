using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public GameObject player;
    public float interpolationFactior = 0.1f;
    public float minDistance;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetPositionAndRotation(player.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) > minDistance)
        {
            //Vector3 toLook = Vector3.Slerp(player.transform.position, transform.position, interpolationFactior);
            //transform.LookAt(toLook);
            transform.position = Vector3.Lerp(player.transform.position, transform.position, interpolationFactior);
        }
    }
}
