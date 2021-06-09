using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float playerRepeelDistance;
    private GameObject sheepsStartPositions;

    [HideInInspector]
    public bool reduceVelocity = true;

    public GameObject mira;
    private Vector3 nextPos;

    private List<GameObject> players = new List<GameObject>(); 
    private GameObject wolf; 

    private bool sheepHunted;
    private bool sheepInGoal;
    public Inits sheepInit;
    public enum Inits
    {
        commun,
        chosing,
        personal,
        play
    }
    
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
        sheepInit = Inits.commun;

        goalPos = GameObject.Find("Goal").transform.position;


        mira = transform.GetChild(1).gameObject;
        sheepsStartPositions = GameObject.Find("SheepStartPositions").gameObject;
    }

    void Move(Vector3 desiredPosition)
    {
        transform.position += (desiredPosition - transform.position).normalized * movementSpeed * 1.8f * Time.deltaTime;
        transform.LookAt(desiredPosition);
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
            toLook += movementSpeed * direction * Time.deltaTime * dist * 1;
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
        Move(goalPos);
    }

    void InitBehaviour()
    {
        Inits next;
        switch (sheepInit)
        {
            case Inits.commun:
                next = Inits.chosing;
                nextPos = new Vector3(50, 0, 50);
                break;
            case Inits.chosing:
                next = Inits.personal;
                sheepInit = Inits.personal;
                nextPos = sheepsStartPositions.transform.GetChild(Random.Range(0, sheepsStartPositions.transform.childCount)).position;
                nextPos += new Vector3(Random.Range(-3,3),0, Random.Range(-3, 3));
                break;
            case Inits.personal:
                next = Inits.play;
                break;
            default:
                next = Inits.commun;
                break;
        }

        if (Vector3.Distance(transform.position, nextPos) < 5)
            sheepInit = next;

        Move(nextPos);
    }

    void FixedUpdate()
    {
        if (sheepInit != Inits.play) InitBehaviour();
        else if (sheepHunted) HuntedSheepBehaviour();
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
        GameState.Instance.killedSheep++;
    }

    public void SetSheepInGoal()
    {
        sheepInGoal = true;

        GameState.Instance.savedSheep++;

        SoundManager.Instance.PlaySheepClip();
    }

    public bool IsInGoal()
    {
        return sheepInGoal;
    }

}
