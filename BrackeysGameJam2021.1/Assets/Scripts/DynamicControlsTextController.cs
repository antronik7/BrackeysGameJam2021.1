using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicControlsTextController : MonoBehaviour
{
    [SerializeField]
    TextMeshPro textAim;
    [SerializeField]
    TextMeshPro textShoot;

    // Start is called before the first frame update
    void Start()
    {
        if (ControllerManager.instance.playWithPad)
        {
            textAim.text = "WITH THE RIGHT STICK";
            textShoot.text = "AND PRESSING THE RIGHT TRIGGER";
        }
        else
        {
            textAim.text = "WITH THE MOUSE";
            textShoot.text = "AND PRESSING THE LEFT BUTTON";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
