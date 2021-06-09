using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheepState : MonoBehaviour
{
    [HideInInspector]
    public int totalSheep;
    [HideInInspector]
    public int killedSheep;
    [HideInInspector]
    public int savedSheep;

    void Start()
    {
        totalSheep = transform.childCount;
        killedSheep = 0;
        savedSheep = 0;
    }

    void Update()
    {
        if (killedSheep >= 3)
        {
            Debug.LogWarning("GAME LOSE");
        }

        if(savedSheep == (totalSheep - killedSheep))
        {
            Debug.LogWarning("GAME WIN");
        }
    }
}
