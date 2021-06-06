using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Vector3 afterCollisionDirection;
    public float afterCollisionAliveTime;
    public string tagFilter;

    private void OnTriggerEnter(Collider other) // 1
    {
        if (other.CompareTag(tagFilter)) // 2
        {
            Sheep sheep = other.gameObject.GetComponent<Sheep>();
            sheep.setSheepInGoal();
            sheep.reduceVelocity = false;
            other.gameObject.GetComponent<Rigidbody>().AddForce(afterCollisionDirection);
            Destroy(other.gameObject, afterCollisionAliveTime); // 3
        }

    }
}
