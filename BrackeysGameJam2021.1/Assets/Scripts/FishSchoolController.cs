using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolController : MonoBehaviour
{
    //Values
    [SerializeField]
    float speed;

    //Components
    Rigidbody2D myRigidboy;

    //Variables
    public List<SmallFishController> fishes = new List<SmallFishController>();

    private void Awake()
    {
        myRigidboy = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(velocity.magnitude > 1.0f)
            velocity.Normalize();

        myRigidboy.velocity = velocity * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "SmallFish")
        {
            CaptureFish(other.GetComponent<SmallFishController>());
        }
    }

    void CaptureFish(SmallFishController fish)
    {
        if(fish.isFishInSchool() == false)
        {
            fishes.Add(fish);
            fish.Capture();
        }
    }
}
