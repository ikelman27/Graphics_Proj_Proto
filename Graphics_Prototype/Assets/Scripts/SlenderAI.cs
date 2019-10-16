using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float staticAlpha;
    private float distance;
    private bool proximityCheck;
    private Vector3 lastPos;
    private float standingTimer;
    public bool stopTeleport;
    public Image staticFX;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character").transform;
        gm = GameManager.Instance;
        agroLevel = 0;
        staticAlpha = 0;
        distance = 0;
        proximityCheck = false;
        lastPos = Vector3.zero;
        standingTimer = 0;

        // create the agro level values
        level = new AgroLevel[4];
        level[0] = new AgroLevel(40f, 50f, 7f);
        level[1] = new AgroLevel(30f, 40f, 5f);
        level[2] = new AgroLevel(20f, 30f, 3f);
        level[3] = new AgroLevel(10f, 20f, 1.5f);

        // move him to a valid location
        Teleport();
    }

    // Update is called once per frame
    void Update()
    {
        // update the timer
        timer += Time.deltaTime;

        GetDistance();
        CheckForProximity();
        CheckForStatic();
        CheckStandingStill();

        staticFX.color = new Color(255f,255f,255f, staticAlpha);

        // if he should teleport, teleport and reset the timer
        if(timer >= level[agroLevel].teleportTimer && !proximityCheck && !stopTeleport)
        {
            if(CheckLineOfSight())
            {
                Teleport();
                timer = 0;
            }
        }

        if(staticAlpha >= 1.0f){
            gm.StartGameOver(false);
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
        Vector3 newPos = player.position + target;

        //Make sure the creature is spawned withing 10 feet of the edge of the area
        float distanceFromEdge = 10;
        newPos.x = Mathf.Clamp(newPos.x, -distanceFromEdge, gm.Bounds.x + distanceFromEdge);
        newPos.y = Mathf.Clamp(newPos.y, -distanceFromEdge, gm.Bounds.y + distanceFromEdge);
        transform.position = newPos;
    }

    // returns true if slenderman is not visible
    // returns false if slenderman is visible
    private bool CheckLineOfSight()
    {
        // don't teleport if the player is looking at slender
        if (GetComponent<Renderer>().isVisible)
        {
            // raycast to see if anything is in the way
            RaycastHit hit;
            Physics.Raycast(player.position, transform.position - player.position, out hit);

            // check if we hit slender or a random object
            if(hit.transform.name != "Slenderman(Clone)")
            {
                return true;
            }
            else
            {
                // clear line of sight
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    private void CheckForStatic()
    {
        // if outside field of view, don't do anything
        if (CheckLineOfSight())
        {
            // reduce the static
            staticAlpha = Mathf.Clamp(staticAlpha - Time.deltaTime, 0f, 1f);
            return;
        }
        // get the dot product of the player forward to the angle between slender and player
        // the closer you look at slender, the faster the static appears
        float dotVal = Mathf.Abs(Vector3.Dot(player.forward.normalized, (transform.position - player.position).normalized));

        // map to a useable value
        float val = (20f / distance) / 200f * dotVal;
        
        // add to the static value
        staticAlpha = Mathf.Clamp(staticAlpha + val, 0f, 1f);
    }

    private void CheckForProximity()
    {
        if (distance < 8f)
        {
            proximityCheck = true;

            float val = (1f / distance);
            val *= Time.deltaTime;
            // add to the static value
            staticAlpha = Mathf.Clamp(staticAlpha + val, 0f, 1f);
        }
        else
            proximityCheck = false;
    }

    private void CheckStandingStill()
    {
        if(lastPos == player.position)
        {
            standingTimer += Time.deltaTime;

            if (standingTimer >= 5f)
                staticAlpha += .5f * Time.deltaTime;
        }
        else
        {
            standingTimer = 0;
        }

        lastPos = player.position;
    }

    private void GetDistance()
    {
        distance = Vector3.Distance(player.position, transform.position);
    }

    public void IncreaseLevel()
    {
        // increases slender AI level
        agroLevel++;
    }
}
