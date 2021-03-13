using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    //Instances
    [SerializeField]
    TextMeshPro nbrFishesText;

    //Values
    [SerializeField]
    int objective;
    [SerializeField]
    float openSpeed;

    //Components
    BoxCollider2D myBoxCollider;

    //Variables
    int currentObjective = 0;
    Vector2 startPosition;

    void Awake()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
        startPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentObjective >= objective)
            transform.position = Vector3.MoveTowards(transform.position, startPosition + Vector2.down, openSpeed * Time.deltaTime);
        else
            nbrFishesText.text = currentObjective + " / " + objective;
    }

    public void IncrementObjective()
    {
        ++currentObjective;

        if (currentObjective >= objective)
        {
            myBoxCollider.enabled = false;
            nbrFishesText.enabled = false;
        }
    }

    public void DecrementObjective()
    {
        --currentObjective;
    }
}
