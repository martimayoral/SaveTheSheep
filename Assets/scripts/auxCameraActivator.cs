using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auxCameraActivator : MonoBehaviour
{
    public GameObject auxCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            auxCam.SetActive(!auxCam.activeSelf);
            print(auxCam.activeSelf ? "Aux cam activated!" : "Aux cam deactivated!");
        }
    }
}
