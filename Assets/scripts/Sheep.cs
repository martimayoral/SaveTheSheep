using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float movementSpeed;
    private List<GameObject> players = new List<GameObject>(); // 5
    public float playerRepeelDistance;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        players.Add(GameObject.Find("Red Player"));
        players.Add(GameObject.Find("Blue Player"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 translation = this.transform.position;

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
                translation += movementSpeed * direction * Time.deltaTime * dist;
            }
        }
        
        rigidbody.MovePosition(translation);
    }
}
