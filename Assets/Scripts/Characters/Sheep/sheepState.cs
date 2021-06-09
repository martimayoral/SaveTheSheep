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

    // Start is called before the first frame update
    void Start()
    {
        totalSheep = transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (killedSheep >= 3)
        {
            Debug.LogWarning("GAME LOSE");
        }

        if(savedSheep == totalSheep - killedSheep)
        {
            Debug.LogWarning("GAME WIN");
        }
    }
}
