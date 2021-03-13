using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    //Instances
    [SerializeField]
    DoorController door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Eat()
    {
        if (door != null)
            door.IncrementObjective();

        Destroy(gameObject);
    }
}
