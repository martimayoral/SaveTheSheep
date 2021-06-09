using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public int totalSheep;
    public int killedSheep;
    public int savedSheep;

    public int minSheep = 10;
    public int startingSheep = 10;
    public int addedSheepPerWin = 4;
    public int removeSheepPerLose = 8;
    public float spawnDifference = 0.2f;

    public GameObject sheep;
    public Transform spawn;

    public Transform sheepParent;
    public Wolf wolf;

    public enum Stage
    {
        starting,
        playing,
        win,
        lose
    }

    [HideInInspector]
    public Stage stage;

    private float InstantiationTimer = 0;

    void Start()
    {
        Instance = this;

        Restart();
        killedSheep = 0;
    }

    private void nextWave()
    {
        Restart();
        killedSheep = Mathf.Min(0, killedSheep - 1);
    }

    private void Restart()
    {
        stage = Stage.starting;
        savedSheep = 0;
        totalSheep = 0;
    }

    void StageStarting()
    {
        if (totalSheep >= startingSheep)
        {
            stage = Stage.playing;
            wolf.ResetCycle();
        }

        InstantiationTimer -= Time.deltaTime;
        if (InstantiationTimer <= 0)
        {
            Instantiate(sheep, spawn.position, Quaternion.identity, sheepParent);
            InstantiationTimer = spawnDifference;
            totalSheep++;
        }
    }

    void StagePlaying()
    {
        if (killedSheep >= 3)
        {
            Debug.LogWarning("GAME LOSE");
        }

        if (savedSheep == (totalSheep - killedSheep))
        {
            Debug.LogWarning("GAME WIN");
        }
    }

    void Update()
    {
        switch (stage)
        {
            case Stage.starting:
                StageStarting();
                break;
            case Stage.playing:
                StagePlaying();
                break;
            case Stage.lose:

                break;
            case Stage.win:

                break;
        }
    }
}
