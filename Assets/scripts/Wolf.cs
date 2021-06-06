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
        grabed
    }
    private State state;
    private State lastState;

    private float toInside;
    private float toHunt;

    private GameObject selectedSheep;

    public GameObject Sheeps;

    private GameObject bluePlayer;
    private GameObject redPlayer;

    public float speed = .8f;
    public float minPlayerDistance;
    public float grabDistance;

    private void reset()
    {
        toInside = Time.time + Random.Range(3,5) ;
        toHunt = toInside + Random.Range(10, 15);
    }

    // Start is called before the first frame update
    void Start()
    {
        bluePlayer = GameObject.Find("Red Player");
        redPlayer = GameObject.Find("Blue Player");

        state = State.outside;
        reset();
    }

    void stateOutside()
    {
        if (Time.time > toInside) state = State.inside;
    }

    void stateInside()
    {
        float x = 50 + Mathf.Cos(Time.time * 0.5f) * 35;
        float y = 0;
        float z = 37 + Mathf.Sin(Time.time * 0.5f) * 20;

        Vector3 nextPoint = new Vector3(x, y, z);

        transform.LookAt(nextPoint);

        transform.position = Vector3.Lerp(transform.position, nextPoint, Time.deltaTime * speed);

        if (Time.time > toHunt) state = State.selecting;
    }

    void stateSelecting()
    {

        selectedSheep = Sheeps.transform.GetChild(Random.Range(0,Sheeps.transform.childCount-1)).gameObject;

        state = State.hunting;
    }

    void stateHunting()
    {

        Sheep sheep = selectedSheep.GetComponent<Sheep>();
        if (sheep.isInGoal() == true)
        {
            state = State.selecting;
            return;
        }

        transform.position = Vector3.Lerp(transform.position, selectedSheep.transform.position, Time.deltaTime);

        float distance = Vector3.Distance(selectedSheep.transform.position, transform.position);
        if (distance < 0.5f)
        {
            sheep.changeHuntedState();

            state = State.leaving;
        }

    }

    void stateLeaving()
    {
        Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);

        Sheep sheep = selectedSheep.GetComponent<Sheep>();

        sheep.transform.position = transform.position;
    }

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
            if (state != State.grabed)
            {
                setGrabState();
                Sheep sheep = selectedSheep.GetComponent<Sheep>();
                if (sheep.getHuntedState())
                {
                    sheep.changeHuntedState();
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
