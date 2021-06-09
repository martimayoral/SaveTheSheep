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
        RestaringCycle
    }
    private State state;
    private State lastState;

    private float toInside;
    private float toHunt;

    private GameObject selectedSheep;

    private Vector3 selectedExit;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
        ResetCycle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal") && state == State.GrabbedByPlayers) // 2
        {
            state = State.RestaringCycle;
            if (selectedSheep)
                selectedSheep.GetComponent<Sheep>().mira.SetActive(false);
        }
    }

    void Update()
    {
        TestIfPlayersGrabTheWolf();

        switch (state)
        {
            case State.WaitOutside:
                StateWaitOutside();
                break;
            case State.EnterAndMoveInCircle:
                StateEnterAndMoveInCircle();
                break;
            case State.SelectSheep:
                StateSelectSheep();
                break;
            case State.HuntSelectedSheep:
                StateHuntSelectedSheep();
                break;
            case State.LeaveSceneWithHuntedSheep:
                StateLeaveSceneWithHuntedSheep();
                break;
            case State.GrabbedByPlayers:
                StateGrabbedByPlayers();
                break;
            case State.RestaringCycle:
                StateRestartingCycle();
                break;
        }
    }

    private void ResetCycle()
    {
        state = State.WaitOutside;
        toInside = Time.time + Random.Range(10, 15) ;
        toHunt = toInside + Random.Range(10, 15);
        transform.position = startingPosition;
    }

    void Move(Vector3 target)
    {
        transform.position += (target - transform.position).normalized * speed * Time.deltaTime;
        transform.LookAt(target);
    }


    void StateWaitOutside()
    {
        if (Time.time > toInside)
        {
            SoundManager.Instance.PlayWolfClip();
            state = State.EnterAndMoveInCircle;
        }
    }

    void StateEnterAndMoveInCircle()
    {
        float x = 50 + Mathf.Cos(Time.time * 0.5f) * 35;
        float y = 0;
        float z = 37 + Mathf.Sin(Time.time * 0.5f) * 20;

        Vector3 nextPoint = new Vector3(x, y, z);

        transform.LookAt(nextPoint);

        transform.position = Vector3.Lerp(transform.position, nextPoint, Time.deltaTime * 0.5f);

        if (Time.time > toHunt) state = State.SelectSheep;
    }

    void StateSelectSheep()
    {

        selectedSheep = Sheeps.transform.GetChild(Random.Range(0,Sheeps.transform.childCount-1)).gameObject;
        selectedExit = exits[Random.Range(0, exits.Length - 1)].transform.position;

        Sheep sheep = selectedSheep.GetComponent<Sheep>();
        if (sheep.IsInGoal()) return;

        sheep.mira.SetActive(true);
        state = State.HuntSelectedSheep;
    }

    // perseguint a la ovella
    void StateHuntSelectedSheep()
    {

        Sheep sheep = selectedSheep.GetComponent<Sheep>();
        if (sheep.IsInGoal())
        {
            state = State.SelectSheep;
            return;
        }

        Move(selectedSheep.transform.position);

        float distance = Vector3.Distance(selectedSheep.transform.position, transform.position);
        if (distance < 0.5f)
        {
            SoundManager.Instance.PlaySheepClip();
            sheep.ChangeHuntedState();

            state = State.LeaveSceneWithHuntedSheep;
        }
    }

    // ha agafat a la ovella
    void StateLeaveSceneWithHuntedSheep()
    {
        if (Vector3.Distance(transform.position, selectedExit) < 2)
        {
            ResetCycle();
            selectedSheep.GetComponent<Sheep>().KillSheep();
            Destroy(selectedSheep.gameObject);
        }

        Move(selectedExit);
    }

    // agafat pel jugadors
    void StateGrabbedByPlayers()
    {
        transform.position = redPlayer.transform.position + (bluePlayer.transform.position - redPlayer.transform.position) / 2;
    }

    void StateRestartingCycle()
    {
        if (Vector3.Distance(startingPosition, transform.position) < 3)
            ResetCycle();
        Move(startingPosition);
    }

    void SetGrabbedByPlayersState()
    {
        lastState = state;
        state = State.GrabbedByPlayers;
    }

    void setNonGrabState()
    {
        state = (lastState == State.LeaveSceneWithHuntedSheep) ? State.HuntSelectedSheep : lastState;
    }

    void UnhuntSheep()
    {
        if (selectedSheep)
        {
            Sheep sheep = selectedSheep.GetComponent<Sheep>();
            if (sheep.GetHuntedState())
            {
                sheep.ChangeHuntedState();
            }
        }
    }

    void TestIfPlayersGrabTheWolf()
    {
        float distanceBetweenPlayers = Vector3.Distance(redPlayer.transform.position, bluePlayer.transform.position);

        float distanceBetweenBlueAndWolf = Vector3.Distance(transform.position, bluePlayer.transform.position);

        float distanceBetweenRedAndWolf = Vector3.Distance(redPlayer.transform.position, transform.position);

        if ((distanceBetweenPlayers < minPlayerDistance) &&
            (distanceBetweenBlueAndWolf < grabDistance) &&
            (distanceBetweenRedAndWolf < grabDistance))
        {
            if ((state != State.GrabbedByPlayers) &&
                (state != State.RestaringCycle))
            {
                SetGrabbedByPlayersState();
                UnhuntSheep();
            }
        }
        else
        {
            if (state == State.GrabbedByPlayers) setNonGrabState();
        }
    }
}
