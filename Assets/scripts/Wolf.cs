using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    private enum State
    {
        outside,
        inside,
        selecting,
        hunting,
        leaving,
        grabed,
        reseting
    }
    private State state;
    private State lastState;

    private float toInside;
    private float toHunt;

    private GameObject selectedSheep;

    public GameObject Sheeps;

    public GameObject bluePlayer;
    public GameObject redPlayer;

    public float speed = .8f;
    public float minPlayerDistance;
    public float grabDistance;

    public GameObject[] exits;
    private Vector3 selectedExit;

    private Vector3 startingPosition;

    private void reset()
    {
        state = State.outside;
        toInside = Time.time + Random.Range(3,5) ;
        toHunt = toInside + Random.Range(10, 15);
        transform.position = startingPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.outside;
        startingPosition = transform.position;

        reset();
    }

    void stateOutside()
    {
        if (Time.time > toInside)
        {
            SoundManager.Instance.PlayWolfClip();
            state = State.inside;
        }
    }

    void move(Vector3 target)
    {
        transform.position += (target - transform.position).normalized * speed;
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

        if (Time.time > toHunt) state = State.selecting;
    }

    void stateSelecting()
    {

        selectedSheep = Sheeps.transform.GetChild(Random.Range(0,Sheeps.transform.childCount-1)).gameObject;
        selectedExit = exits[Random.Range(0, exits.Length)].transform.position;

        selectedSheep.GetComponent<Sheep>().mira.SetActive(true);
        state = State.hunting;
    }

    // perseguint a la ovella
    void stateHunting()
    {

        Sheep sheep = selectedSheep.GetComponent<Sheep>();
        if (sheep.isInGoal() == true)
        {
            state = State.selecting;
            return;
        }

        move(selectedSheep.transform.position);

        float distance = Vector3.Distance(selectedSheep.transform.position, transform.position);
        if (distance < 0.5f)
        {
            SoundManager.Instance.PlaySheepClip();
            sheep.changeHuntedState();

            state = State.leaving;
        }
    }

    // ha agafat a la ovella
    void stateLeaving()
    {
        if (Vector3.Distance(transform.position, selectedExit) < 2)
        {
            reset();
            selectedSheep.GetComponent<Sheep>().killSheep();
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
            if (state != State.grabed && state != State.reseting)
            {
                setGrabState();
                if (selectedSheep)
                {
                    Sheep sheep = selectedSheep.GetComponent<Sheep>();
                    if (sheep.getHuntedState())
                    {
                        sheep.changeHuntedState();
                    }
                }
            }
        } else
        {
            if(state == State.grabed)
            {
                setNonGrabState();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal") && state == State.grabed) // 2
        {
            state = State.reseting;
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
            case State.outside:
                stateOutside();
                break;
            case State.inside:
                stateInside();
                break;
            case State.selecting:
                stateSelecting();
                break;
            case State.hunting:
                stateHunting();
                break;
            case State.leaving:
                stateLeaving();
                break;
            case State.grabed:
                stateGrabed();
                break;
            case State.reseting:
                stateReseting();
                break;
        }
    }

    void setGrabState()
    {
        lastState = state;
        state = State.grabed;
    }

    void setNonGrabState()
    {
        state = (lastState == State.leaving) ? State.hunting : lastState;
    }
}
