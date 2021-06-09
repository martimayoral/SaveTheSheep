using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float playerRepeelDistance;

    [HideInInspector]
    public bool reduceVelocity = true;

    public GameObject mira;

    private List<GameObject> players = new List<GameObject>(); 
    private GameObject wolf; 

    private bool sheepHunted;
    private bool sheepInGoal;
    
    private Vector3 goalPos;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        players.Add(GameObject.Find("Red Player"));
        players.Add(GameObject.Find("Blue Player"));

        wolf = GameObject.Find("Wolf");
        sheepHunted = false;
        sheepInGoal = false;

        goalPos = GameObject.Find("Goal").transform.position;


        mira = transform.GetChild(1).gameObject;
    }

    void Move(Vector3 desiredPosition)
    {
        Vector3 direction = (desiredPosition - transform.position);

        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, direction.magnitude))
            rigidBody.MovePosition(desiredPosition);
        else
            rigidBody.MovePosition(hit.point);
    }

    Vector3 Repeel(GameObject player, float distance)
    {

        Vector3 toLook = Vector3.zero;

        // vector from player and sheep
        Vector3 vec = player.transform.position - transform.position;

        // direction: vector normalized
        Vector3 direction = -vec.normalized;

        // distance to move
        float dist = playerRepeelDistance - vec.magnitude;

        // if the distance to move is away from the player
        if (dist > 0)
        {
            // translation to aply
            toLook += movementSpeed * direction * Time.deltaTime * dist * -1;
            rigidBody.AddForce(movementSpeed * direction * dist);
        }

        return toLook;
    }

    void FreeSheepBehaviour()
    {
        // if nothing happens, look forward
        Vector3 toLook = transform.forward * 10;

        foreach (GameObject player in players)
        {
            toLook += Repeel(player, playerRepeelDistance);
        }
        toLook += Repeel(wolf, playerRepeelDistance * 0.5f);

        // update the rotation. Slerp will go to the desired location smoothly
        rigidBody.MoveRotation(
            Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(new Vector3(toLook.x, 0, toLook.z)),
                rotationSpeed * Time.deltaTime
                )
            );

        // we reduce the velocity each update
        if (reduceVelocity)
            rigidBody.velocity = rigidBody.velocity * 0.9f;
    }

    void HuntedSheepBehaviour()
    {
        // Grabbed by wolf
        transform.position = wolf.transform.position + wolf.transform.forward * 10;
    }

    void GoalSheepBehaviour()
    {
        transform.position += (goalPos - transform.position).normalized;
        transform.LookAt(new Vector3(50, 0, -100));
    }

    void FixedUpdate()
    {
        if (sheepHunted) HuntedSheepBehaviour();
        else if (sheepInGoal) GoalSheepBehaviour();
        else if (mira.activeSelf) return;
        else FreeSheepBehaviour();
    }

    public void ChangeHuntedState()
    {
        sheepHunted = !sheepHunted;
    }

    public bool GetHuntedState()
    {
        return sheepHunted;
    }

    public void KillSheep()
    {
        transform.parent.GetComponent<sheepState>().killedSheep++;
    }

    public void SetSheepInGoal()
    {
        sheepInGoal = true;
        transform.parent.GetComponent<sheepState>().savedSheep++;
    }

    public bool IsInGoal()
    {
        return sheepInGoal;
    }

}
