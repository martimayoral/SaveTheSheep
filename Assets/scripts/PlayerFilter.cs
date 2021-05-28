using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFilter : MonoBehaviour
{

    private Vector3 lastPos;
    private Vector3 velocity;
    public float smoothTime;

    void Awake()
    {
        lastPos = transform.position;
    }

    private void Update()
    {
        transform.position += new Vector3(Random.Range(-0.2f, 0.2f), 0f, Random.Range(-0.2f, 0.2f));
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(lastPos, transform.position, ref velocity, smoothTime);
        lastPos = transform.position;
    }
}
