using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public int totalSheep;
    public int positionedSheep;
    public int killedSheep;
    public int savedSheep;

    public int minSheep = 10;
    public int startingSheep = 10;
    public int addedSheepPerWin = 3;
    public int removeSheepPerLose = 10;
    public float spawnDifference = 0.2f;

    public GameObject sheep;
    public Transform spawn;

    public Transform sheepParent;
    public Wolf wolf;

    public GameObject win;
    public GameObject lose;
    public GameObject t1;
    public GameObject t2;
    public GameObject t3;

    public GameObject tumb1;
    public GameObject tumb2;
    public GameObject tumb3;

    public enum Stage
    {
        starting,
        playing,
        win,
        lose
    }

    private float text;
    private bool textActive;

    [HideInInspector]
    public Stage stage;

    private float InstantiationTimer = 0;

    void Start()
    {
        Instance = this;
        text = -1;
        textActive = false;

        Restart();
    }

    private void nextWave()
    {
        if (text > 0)
            return;

        Restart();
        startingSheep += addedSheepPerWin;
    }

    private void Restart()
    {
        killedSheep = 0;
        tumb1.SetActive(false);
        tumb2.SetActive(false);
        tumb3.SetActive(false);

        wolf.ResetCycle();
        stage = Stage.starting;
        savedSheep = 0;
        positionedSheep = 0;
        totalSheep = 0;
        foreach (Transform child in sheepParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void StageStarting()
    {
        if(positionedSheep == startingSheep)
        {
            stage = Stage.playing;
            wolf.ResetCycle();
        }
        if (totalSheep == startingSheep)
            return;


        InstantiationTimer -= Time.deltaTime;
        if (InstantiationTimer <= 0)
        {
            Instantiate(sheep, spawn.position, Quaternion.identity, sheepParent);
            InstantiationTimer = spawnDifference;
            totalSheep++;
        }
    }

    void gameLose()
    {
        if (text > 0)
            return;

        startingSheep = Mathf.Max(startingSheep - removeSheepPerLose, minSheep);
        Restart();
    }

    void activateText()
    {
        textActive = true;
        text = 5;
    }

    void StagePlaying()
    {
        if (savedSheep == (totalSheep - killedSheep))
        {
            stage = Stage.win;
            activateText();
        }
    }

    public void killSheep()
    {
        killedSheep++;
        switch (killedSheep)
        {
            case 1:
                tumb1.SetActive(true);
                break;
            case 2:
                tumb2.SetActive(true);
                break;
            case 3:
                tumb3.SetActive(true);
                stage = Stage.lose;
                activateText();
                break;
        }
    }

    void showText()
    {
        text -= Time.deltaTime;

        if (text < 0)
        {
            win.SetActive(false);
            lose.SetActive(false);
            t1.SetActive(false);
            t2.SetActive(false);
            t3.SetActive(false);
            textActive = false;
            return;
        }
        if (text < 1)
        {
            t1.SetActive(true);
            t2.SetActive(false);
            t3.SetActive(false);
            return;
        }
        if (text < 2)
        {
            t2.SetActive(true);
            t3.SetActive(false);
            return;
        }
        if (text < 3)
        {
            t3.SetActive(true);
            return;
        }


        switch (stage)
        {
            case Stage.win:
                win.SetActive(true);

                break;
            case Stage.lose:
                lose.SetActive(true);
                break;
        }
    }

    
    void Update()
    {
        if(textActive)
        showText();

        switch (stage)
        {
            case Stage.starting:
                StageStarting();
                break;
            case Stage.playing:
                StagePlaying();
                break;
            case Stage.lose:
                gameLose();
                break;
            case Stage.win:
                nextWave();
                break;
        }
    }
}
