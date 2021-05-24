﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    private List<GameObject> players = new List<GameObject>(); // 5
    public float playerRepeelDistance;
    Rigidbody rigidbody;

    [HideInInspector]
    public bool reduceVelocity = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        players.Add(GameObject.Find("Red Player"));
        players.Add(GameObject.Find("Blue Player"));
    }

    void move(Vector3 desiredPosition)
    {
        Vector3 direction = (desiredPosition - this.transform.position);
        Ray ray = new Ray(this.transform.position, direction);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, direction.magnitude))
            rigidbody.MovePosition(desiredPosition);
        else
            rigidbody.MovePosition(hit.point);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if nothing happens, look forward
        Vector3 toLook =  this.transform.forward * 10;

        foreach (GameObject player in players ){
            // vector from player and sheep
            Vector3 vec = player.transform.position - this.transform.position;

            // direction: vector normalized
            Vector3 direction = -vec.normalized;

            // distance to move
            float dist = playerRepeelDistance - vec.magnitude;

            // if the distance to move is away from the player
            if (dist > 0)
            {
                // translation to aply
                toLook += movementSpeed * direction * Time.deltaTime * dist * -1;
                rigidbody.AddForce(movementSpeed * direction * dist);
            }
        }

        // update the rotation. Slerp will go to the desired location smoothly
        rigidbody.MoveRotation(
            Quaternion.Slerp(
                transform.rotation, 
                Quaternion.LookRotation(new Vector3(toLook.x, 0, toLook.z)), 
                rotationSpeed * Time.deltaTime
                )
            );

        // we reduce the velocity each update
        if(reduceVelocity)
            rigidbody.velocity = rigidbody.velocity * 0.9f;
    }

}
