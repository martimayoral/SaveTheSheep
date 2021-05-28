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
        leaving
    }
    private State state;

    private float toInside;
    private float toHunt;

    private GameObject selectedSheep;

    public GameObject Sheeps;

    public float speed = 50.0f;

    private void reset()
    {
        toInside = Time.time + Random.Range(3,5) ;
        toHunt = toInside + Random.Range(10, 15);
    }

    // Start is called before the first frame update
    void Start()
    {
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

        transform.position = Vector3.Lerp(transform.position, nextPoint, Time.deltaTime);

        if (Time.time > toHunt) state = State.selecting;
    }

    void stateSelecting()
    {

        selectedSheep = Sheeps.transform.GetChild(Random.Range(0,Sheeps.transform.childCount-1)).gameObject;

        state = State.hunting;
    }

    void stateHunting()
    {
        if (!selectedSheep) state = State.selecting;

        float distance = Vector3.Distance(selectedSheep.transform.position, transform.position);
        transform.position = Vector3.Lerp(transform.position, selectedSheep.transform.position, (Time.deltaTime * speed) / distance);

        if (distance < 0.5f)
        {
            Sheep sheep = selectedSheep.GetComponent<Sheep>();
            sheep.changeHuntedState();

            state = State.leaving;
        }

    }

    void stateLeaving()
    {
        Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);
        float distance = Vector3.Distance(target, transform.position);
        transform.position = Vector3.Lerp(transform.position, target, (Time.deltaTime * speed) / distance);
        selectedSheep.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
