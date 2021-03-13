using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    [SerializeField]
    Transform randomZoneStart;
    [SerializeField]
    float randomZoneL;
    [SerializeField]
    float randomZoneH;
    [SerializeField]
    float minRoamTime;
    [SerializeField]
    float maxRoamTime;
    [SerializeField]
    GameObject exclamationPoint;

    //Instances
    [SerializeField]
    DoorController door;
    [SerializeField]
    EndController end;

    //Components
    Rigidbody2D myRigidBody;
    SpriteRenderer mySpriteRenderer;
    IAstarAI myAiPath;

    //Variables
    int currentHitPoints = 1;
    bool isStun = false;
    bool isChassing = false;
    float stunTimeStamp = 0f;
    float roamTimeStamp = 0f;
    bool canFindFish = true;
    GameObject target;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAiPath = GetComponent<IAstarAI>();
        currentHitPoints = hitPoints;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStun)
        {
            if (Time.timeSinceLevelLoad >= stunTimeStamp)
                Unstun();
            else
                return;
        }
        else
        {
            if(isChassing)
            {
                if (target == null || target.gameObject.layer != 7)
                {
                    canFindFish = true;
                    isChassing = false;
                    exclamationPoint.SetActive(false);
                    target = null;
                }
                else
                    myAiPath.destination = target.transform.position;
            }
            else
            {
                if(Time.timeSinceLevelLoad >= roamTimeStamp)
                {
                    myAiPath.destination = new Vector3(Random.Range(randomZoneStart.position.x, randomZoneStart.position.x + randomZoneL), Random.Range(randomZoneStart.position.y, randomZoneStart.position.y + randomZoneH), 0f);
                    roamTimeStamp = Time.timeSinceLevelLoad + Random.Range(minRoamTime, maxRoamTime);
                }
            }
        }

        if (myAiPath.desiredVelocity.x > 0f)
            mySpriteRenderer.flipX = false;
        else if (myAiPath.desiredVelocity.x < 0f)
            mySpriteRenderer.flipX = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (canFindFish == false)
            return;

        if (other.gameObject.tag == "SmallFish")
        {
            canFindFish = false;
            isChassing = true;
            exclamationPoint.SetActive(true);
            target = other.gameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isChassing == false)
            return;

        if (collision.gameObject.tag == "SmallFish" && collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<SmallFishController>().Kill();
            Stun();
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
        myAiPath.isStopped = true;
        canFindFish = false;
        isChassing = false;
        exclamationPoint.SetActive(false);
    }

    void Unstun()
    {
        isStun = false;
        myAiPath.isStopped = false;
        canFindFish = true;
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

        if (door != null)
            door.IncrementObjective();

        if (end != null)
            end.IncrementObjective();

        Destroy(gameObject);
    }
}
