using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public GameObject Sheeps;

    public GameObject bluePlayer;
    public GameObject redPlayer;

    public float speed;
    public float minPlayerDistance;
    public float grabDistance;

    public GameObject[] exits;

    private enum State
    {
        WaitOutside,
        EnterAndMoveInCircle,
        SelectSheep,
        HuntSelectedSheep,
        LeaveSceneWithHuntedSheep,
        GrabbedByPlayers,
        RestaringCicle
    }
    private State state;
    private State lastState;

    private float toInside;
    private float toHunt;

    private GameObject selectedSheep;

    private Vector3 selectedExit;

    private Vector3 startingPosition;

    private void reset()
    {
        state = State.WaitOutside;
        toInside = Time.time + Random.Range(10, 15) ;
        toHunt = toInside + Random.Range(10, 15);
        transform.position = startingPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        reset();
    }

    void stateOutside()
    {
        if (Time.time > toInside)
        {
            SoundManager.Instance.PlayWolfClip();
            state = State.EnterAndMoveInCircle;
        }
    }

    void move(Vector3 target)
    {
        transform.position += (target - transform.position).normalized * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    void stateInside()
    {
        float x = 50 + Mathf.Cos(Time.time * 0.5f) * 35;
        float y = 0;
        float z = 37 + Mathf.Sin(Time.time * 0.5f) * 20;

        Vector3 nextPoint = new Vector3(x, y, z);

        transform.LookAt(nextPoint);

        transform.position = Vector3.Lerp(transform.position, nextPoint, Time.deltaTime * 0.5f);

        if (Time.time > toHunt) state = State.SelectSheep;
    }

    void stateSelecting()
    {

        selectedSheep = Sheeps.transform.GetChild(Random.Range(0,Sheeps.transform.childCount-1)).gameObject;
        selectedExit = exits[Random.Range(0, exits.Length - 1)].transform.position;

        selectedSheep.GetComponent<Sheep>().mira.SetActive(true);
        state = State.HuntSelectedSheep;
    }

    // perseguint a la ovella
    void stateHunting()
    {

        Sheep sheep = selectedSheep.GetComponent<Sheep>();
        if (sheep.IsInGoal() == true)
        {
            state = State.SelectSheep;
            return;
        }

        move(selectedSheep.transform.position);

        float distance = Vector3.Distance(selectedSheep.transform.position, transform.position);
        if (distance < 0.5f)
        {
            SoundManager.Instance.PlaySheepClip();
            sheep.ChangeHuntedState();

            state = State.LeaveSceneWithHuntedSheep;
        }
    }

    // ha agafat a la ovella
    void stateLeaving()
    {
        if (Vector3.Distance(transform.position, selectedExit) < 2)
        {
            reset();
            selectedSheep.GetComponent<Sheep>().KillSheep();
            Destroy(selectedSheep.gameObject);
        }

        move(selectedExit);

    }

    // agafat pel jugadors
    void stateGrabed()
    {
        transform.position = redPlayer.transform.position + (bluePlayer.transform.position - redPlayer.transform.position) / 2;

    }

    void checkPlayerInteraction(){
        float playerDistance = Vector3.Distance(redPlayer.transform.position, bluePlayer.transform.position);

        float blueWolfDistance = Vector3.Distance(transform.position, bluePlayer.transform.position);

        float redWolfDistance = Vector3.Distance(redPlayer.transform.position, transform.position);

        if(playerDistance < minPlayerDistance && blueWolfDistance < grabDistance && redWolfDistance < grabDistance)
        {
            if (state != State.GrabbedByPlayers && state != State.RestaringCicle)
            {
                setGrabState();
                if (selectedSheep)
                {
                    Sheep sheep = selectedSheep.GetComponent<Sheep>();
                    if (sheep.GetHuntedState())
                    {
                        sheep.ChangeHuntedState();
                    }
                }
            }
        } else
        {
            if(state == State.GrabbedByPlayers)
            {
                setNonGrabState();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal") && state == State.GrabbedByPlayers) // 2
        {
            state = State.RestaringCicle;
            if(selectedSheep)
                selectedSheep.GetComponent<Sheep>().mira.SetActive(false);
        }
    }

    void stateReseting()
    {
        if (Vector3.Distance(startingPosition, transform.position) < 3)
            reset();
        move(startingPosition);
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerInteraction();
            
        switch (state)
        {
            case State.WaitOutside:
                stateOutside();
                break;
            case State.EnterAndMoveInCircle:
                stateInside();
                break;
            case State.SelectSheep:
                stateSelecting();
                break;
            case State.HuntSelectedSheep:
                stateHunting();
                break;
            case State.LeaveSceneWithHuntedSheep:
                stateLeaving();
                break;
            case State.GrabbedByPlayers:
                stateGrabed();
                break;
            case State.RestaringCicle:
                stateReseting();
                break;
        }
    }

    void setGrabState()
    {
        lastState = state;
        state = State.GrabbedByPlayers;
    }

    void setNonGrabState()
    {
        state = (lastState == State.LeaveSceneWithHuntedSheep) ? State.HuntSelectedSheep : lastState;
    }
}
