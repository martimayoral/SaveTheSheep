using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 4.0f)] float wolfLerpTime;
    [SerializeField] Vector3[] wolfPositions;

    private int indexPos = 0;

    private int numberOfPositions;

    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        numberOfPositions = wolfPositions.Length;


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, wolfPositions[indexPos], wolfLerpTime * Time.deltaTime);

        time = Mathf.Lerp(time, 1.0f, wolfLerpTime * Time.deltaTime);
        if (time > .9f)
        {
            time = 0.0f;
            indexPos = (indexPos + 1) % numberOfPositions;
        }
    }
}
