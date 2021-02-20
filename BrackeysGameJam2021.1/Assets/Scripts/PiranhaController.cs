using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaController : MonoBehaviour
{
    //Values
    [SerializeField]
    int hitPoints;
    [SerializeField]
    float stunDuration;
    [SerializeField]
    float pushBackForce;
    [SerializeField]
    float minFoodToSpawn;
    [SerializeField]
    float maxFoodToSpawn;

    //Components
    Rigidbody2D myRigidBody;

    //Variables
    int currentHitPoints = 1;
    bool isStun = false;
    float stunTimeStamp = 0f;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        currentHitPoints = hitPoints;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStun)
        {
            if (Time.timeSinceLevelLoad >= stunTimeStamp)
                Unstun();
            else
                return;
        }
    }

    public void Damage(int amount, Vector2 pushBackDirection)
    {
        currentHitPoints -= amount;

        myRigidBody.velocity = Vector2.zero;
        myRigidBody.AddForce(pushBackDirection * pushBackForce);
        Stun();

        if (currentHitPoints <= 0)
            Kill();
    }

    void Stun()
    {
        isStun = true;
        stunTimeStamp = Time.timeSinceLevelLoad + stunDuration;
    }

    void Unstun()
    {
        isStun = false;
        Debug.Log("Yessir");
    }

    void Kill()
    {
        float randomNbrFood = Random.Range(minFoodToSpawn, maxFoodToSpawn);
        float randomForce = Random.Range(GameManager.instance.minForceFood, GameManager.instance.maxForceFood);

        for (int i = 0; i < randomNbrFood; ++i)
        {
            GameObject fishFood = Instantiate(GameManager.instance.fishFood, transform.position, Quaternion.identity) as GameObject;
            fishFood.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * randomForce);
        }

        Destroy(gameObject);
    }
}
