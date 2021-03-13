using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
    //Values
    [SerializeField]
    int objective;

    //Variables
    int currentObjective = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementObjective()
    {
        ++currentObjective;

        if (currentObjective >= objective)
            SceneManager.LoadScene("End");
    }
}
