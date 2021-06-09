using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auxCameraActivator : MonoBehaviour
{
    public GameObject auxCam;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            auxCam.SetActive(!auxCam.activeSelf);
            print(auxCam.activeSelf ? "Aux cam activated!" : "Aux cam deactivated!");
        }
    }
}
