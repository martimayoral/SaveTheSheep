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
    void FixedUpdate()
    {
        // vector from player and sheep
        Vector3 vec = player.transform.position - this.transform.position;

        // direction: vector normalized
        Vector3 direction = - vec.normalized;

        // distance to move
        float dist = playerRepeelDistance - vec.magnitude;

        // if the distance to move is away from the player
        if (dist > 0)
        {
            // translation to aply
            Vector3 translation = movementSpeed * direction * Time.deltaTime * dist;
            rigidbody.MovePosition(this.transform.position + translation);
        }
    }
}
