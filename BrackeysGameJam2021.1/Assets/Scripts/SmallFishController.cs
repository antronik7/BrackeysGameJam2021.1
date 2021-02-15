using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFishController : MonoBehaviour
{
    //Variables
    bool IsInSchool = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Capture()
    {
        IsInSchool = true;
        Debug.Log("hell yeah");
    }

    public bool isFishInSchool()
    {
        return IsInSchool;
    }
}
