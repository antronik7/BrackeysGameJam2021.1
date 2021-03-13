using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager instance = null;

    public bool playWithPad = true;
    bool gameStarted = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
            return;

        if (Input.GetButtonDown("Start"))
        {
            playWithPad = true;
            gameStarted = true;
            SceneManager.LoadScene("Main");

        }
        else if(Input.GetKey(KeyCode.Return))
        {
            playWithPad = false;
            gameStarted = true;
            SceneManager.LoadScene("Main");
        }
    }
}
