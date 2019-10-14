using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int notesCollected;
    public int totalNotes;
    public GameObject notePrefab;
    public GameObject slenderPrefab;
    private SlenderAI slenderRef;
    public Image staticFX;
    // Start is called before the first frame update
    void Start()
    {
        notesCollected = 0;

        SpawnNotes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnNotes()
    {
        // randomly place notes around the scene (for testing)
        for(int i = 0; i < totalNotes; i++)
        {
            Vector3 pos = new Vector3(Random.Range(0f, 50f), 1.5f, Random.Range(0f, 50f));
            Instantiate(notePrefab, pos, Quaternion.identity);
        }
    }

    private void OnGUI()
    {
        // gui to display how many notes are collected
        GUI.Box(new Rect(0, 0, 200, 35), "Notes Collected: " + notesCollected + " / " + totalNotes);
    }

    public void CollectNote()
    {
        // increment number of notes collected
        notesCollected++;

        // if this is the first note the player colected, spawn slender
        // otherwise increase his AI level
        if(notesCollected == 1)
        {
            SpawnSlenderman();
        }
        else
        {
            slenderRef.IncreaseLevel();
        }
    }

    private void SpawnSlenderman()
    {
        // spawn him in and save his reference
        GameObject temp = Instantiate(slenderPrefab, Vector3.zero, Quaternion.identity);
        slenderRef = temp.GetComponent<SlenderAI>();
        slenderRef.staticFX = staticFX;
    }
}
