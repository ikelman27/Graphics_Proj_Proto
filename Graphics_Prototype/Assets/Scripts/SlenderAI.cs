using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// struct to hold values for each agro level
public struct AgroLevel
{
    public float minRange;
    public float maxRange;
    public float teleportTimer;

    public AgroLevel(float min, float max, float timer)
    {
        minRange = min;
        maxRange = max;
        teleportTimer = timer;
    }
}

public class SlenderAI : MonoBehaviour
{
    private Transform player;
    private AgroLevel[] level;
    private int agroLevel;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character").transform;

        agroLevel = 0;

        // create the agro level values
        level = new AgroLevel[4];
        level[0] = new AgroLevel(20f, 30f, 7f);
        level[1] = new AgroLevel(15f, 20f, 5f);
        level[2] = new AgroLevel(10f, 15f, 3f);
        level[3] = new AgroLevel(7f, 10f, 1.5f);

        // move him to a valid location
        Teleport();
    }

    // Update is called once per frame
    void Update()
    {
        // update the timer
        timer += Time.deltaTime;

        // if he should teleport, teleport and reset the timer
        if(timer >= level[agroLevel].teleportTimer)
        {
            Teleport();
            timer = 0;
        }
    }

    private void Teleport()
    {
        // get inverse of player forward
        Vector3 behind = -player.forward;

        // find random angle behind player
        float angle = Random.Range(-90f, 90f);

        // get point on unit circle
        Vector3 target = Quaternion.AngleAxis(angle, Vector3.up) * behind.normalized;

        // multiply by range value (random between min and max per level)
        float val = Random.Range(level[agroLevel].minRange, level[agroLevel].maxRange);
        target = target.normalized * val;

        // add to position of player
        transform.position = player.position + target;
    }

    public void IncreaseLevel()
    {
        // increases slender AI level
        agroLevel++;
    }
}
