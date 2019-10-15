using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = true;
    }
    public void Press()
    {
        GameManager.ExitGame();
    }
}
