using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float movementSpeed;
    public GameObject player;
    public float playerRepeelDistance;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = player.transform.position - this.transform.position;
        Vector3 direction = vec.normalized;
        float dist = playerRepeelDistance - vec.magnitude;
        Vector3 translation = - movementSpeed * direction * Time.deltaTime * dist;
        if (dist > 0)
        {
            transform.Translate(new Vector3(translation.x,0, translation.z));
        }
    }
}
