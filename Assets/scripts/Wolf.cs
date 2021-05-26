using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{

    private float timerCounter = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timerCounter += Time.deltaTime * 0.5f;

        float x = 50 + Mathf.Cos(timerCounter) * 35;
        float y = 0;
        float z = 37 + Mathf.Sin(timerCounter) * 20;

        transform.position = new Vector3(x, y, z);
    }
}
