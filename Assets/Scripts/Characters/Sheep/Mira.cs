using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mira : MonoBehaviour
{
    public Vector3 rotationSpeed;

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
        transform.localScale = Vector3.one + Vector3.one * Mathf.Sin(Time.time) * 0.3f;
    }
}
