using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    private enum State
    {
        outside,
        inside,
        hunting,
        leaving
    }
    private State state;

    private float toInside;
    private float toHunt;

    public GameObject Sheeps;

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

        if (Time.time > toHunt) state = State.hunting;
    }

    void stateHunting()
    {
        GameObject sheep;
        sheep = Sheeps.transform.GetChild(Random.Range(0,Sheeps.transform.childCount-1)).gameObject;

        sheep.transform.position = new Vector3(50, 50, 50);

        state = State.leaving;
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
            case State.hunting:
                stateHunting();
                break;
            case State.leaving:
                break;
        }
    }
}
