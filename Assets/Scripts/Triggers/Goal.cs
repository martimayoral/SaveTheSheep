using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Vector3 afterCollisionDirection;
    public float afterCollisionAliveTime;
    public string tagFilter;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(tagFilter) && GameState.Instance.stage == GameState.Stage.playing) 
        {
            Sheep sheep = other.gameObject.GetComponent<Sheep>();
            sheep.SetSheepInGoal();
            sheep.reduceVelocity = false;
            Destroy(other.gameObject, afterCollisionAliveTime); 
        }

    }
}
