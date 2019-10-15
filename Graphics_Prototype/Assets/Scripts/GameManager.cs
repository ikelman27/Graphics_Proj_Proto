using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int notesCollected;
    public int totalNotes;
    public GameObject notePrefab;
    public GameObject slenderPrefab;
    private SlenderAI slenderRef;
    public Image staticFX;

    public static GameManager Instance;


    [SerializeField]
    private GameObject SpawnLocationParrent = null;

    public Vector2 Bounds = new Vector2(150, 150);

    private List<Transform> SpawnLocations;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        notesCollected = 0;
        SpawnNotes();
    }

    private void SpawnNotes()
    {
        if (SpawnLocationParrent != null)
        {
            SpawnLocations = new List<Transform>();
            foreach (Transform child in SpawnLocationParrent.transform)
            {
                SpawnLocations.Add(child);
            }
        }

        if (SpawnLocations.Count > totalNotes)
        {
            int maxValue = SpawnLocations.Count - 1;
            List<Transform> spawnedObj = new List<Transform>(SpawnLocations);

            for (int i = 0; i < totalNotes; i++)
            {
                int spawnIndex = Random.Range(0, maxValue);
                Instantiate(notePrefab, spawnedObj[spawnIndex].position, Quaternion.identity, spawnedObj[spawnIndex]);
                spawnedObj.RemoveAt(spawnIndex);
                maxValue--;
            }
        }
        else
        {
            // randomly place notes around the scene (for testing)
            for (int i = 0; i < totalNotes; i++)
            {
                Vector3 pos = new Vector3(Random.Range(0f, 50f), 1.5f, Random.Range(0f, 50f));
                Instantiate(notePrefab, pos, Quaternion.identity);
            }
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
        if (notesCollected == totalNotes)
        {
            StartGameOver(true);
        }
        // if this is the first note the player colected, spawn slender
        // otherwise increase his AI level
        if (notesCollected == 1)
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

    public void StartGameOver(bool win)
    {
          Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(win){
            SceneManager.LoadScene("Game Win", LoadSceneMode.Single);
            return;
        }
        else{
            SceneManager.LoadScene("Game Loss");
        }


    }

    public static void ExitGame(){
        Application.Quit();
    }

}
