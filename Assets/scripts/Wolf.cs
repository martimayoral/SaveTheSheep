using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 4.0f)] float wolfSpeed;
    [SerializeField] Vector3[] wolfPositions;

    private int indexPos = 0;

    private int numberOfPositions;

    private float time = 0.0f;
    Rigidbody rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        numberOfPositions = wolfPositions.Length;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toLook = wolfPositions[indexPos];
        transform.position = Vector3.Lerp(transform.position, wolfPositions[indexPos], wolfSpeed * Time.deltaTime);

        transform.LookAt(new Vector3(toLook.x, 0, toLook.z));

        time = Mathf.Lerp(time, 1.0f, wolfSpeed * Time.deltaTime);
        if (time > .9f)
        {
            time = 0.0f;
            indexPos = (indexPos + 1) % numberOfPositions;
        }
    }
}
