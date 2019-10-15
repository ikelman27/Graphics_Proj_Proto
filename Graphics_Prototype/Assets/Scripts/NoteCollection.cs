using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollection : MonoBehaviour
{
    private bool checkForInput;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        checkForInput = false;
        gm = GameManager.Instance;
    }
        
    
    // Update is called once per frame
    void Update()
    {
        if(checkForInput)
        {
            // player clicked while inside radius
            if(Input.GetMouseButtonDown(0))
            {
                // increment notes collected and delete this note
                gm.CollectNote();
                Destroy(gameObject);
            }
        }
    }

    // if the player enters the radius, check for mouse input
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Character")
            checkForInput = true;
    }

    // stop checking for input when the player leaves
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Character")
            checkForInput = false;
    }
}
