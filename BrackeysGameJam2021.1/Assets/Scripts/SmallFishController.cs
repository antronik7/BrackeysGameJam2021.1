using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFishController : MonoBehaviour
{
    //Instances
    [SerializeField]
    DoorController door;

    //Components
    Rigidbody2D myRigidboy;
    SpriteRenderer mySprite;

    //Variables
    bool isInSchool = false;
    float roamTimeStamp = 0;
    Vector2 velocity;
    FishSchoolController currentSchool;
    float currentSpeed;
    Vector2 flockVelocity = Vector2.zero;
    int currentDamage = 0;

    private void Awake()
    {
        myRigidboy = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = GameManager.instance.smallFishSpeed;
        velocity = transform.forward * GameManager.instance.smallFishSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer != 9)
        {
            if (isInSchool)
            {
                if (Random.Range(0, GameManager.instance.flockChance) < 1)
                    Flock();

                myRigidboy.velocity = flockVelocity + currentSchool.currentVelocity;
            }
            else if (gameObject.layer == 8)
            {
                Roam();
            }

            if (myRigidboy.velocity.x >= 0)
                mySprite.flipX = false;
            else
                mySprite.flipX = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(gameObject.layer == 9)
        {
            if (collision.gameObject.tag == "Piranha")
                collision.gameObject.GetComponent<PiranhaController>().Damage(currentDamage, collision.transform.position - transform.position);//Debug.Log(collision.gameObject);//collision.gameObject.GetComponent<PiranhaController>().Damage(currentSchool.GetDamage(), collision.transform.position - transform.position);

            transform.rotation = Quaternion.identity;
            mySprite.flipX = false;
            mySprite.flipY = false;
            myRigidboy.velocity = Vector2.zero;
            gameObject.layer = 8;
        }
        else if (gameObject.layer == 7)
        {
            if (collision.gameObject.tag == "FishFood")
            {
                currentSchool.GiveFood(GameManager.instance.foodValue);
                collision.gameObject.GetComponent<FoodController>().Eat();
            }
        }
    }

    void Roam()
    {
        if(Time.timeSinceLevelLoad >= roamTimeStamp)
        {
            roamTimeStamp = Time.timeSinceLevelLoad + Random.Range(GameManager.instance.roamMinTime, GameManager.instance.roamMaxTime);
            velocity = GameManager.instance.RandomUnitVector() * currentSpeed;
        }

        myRigidboy.velocity = velocity;
    }

    void Flock()
    {
        Vector3 vCenter = Vector2.zero;
        Vector3 vAvoid = Vector2.zero;
        float groupSpeed = currentSpeed;

        Vector3 goalPos = currentSchool.transform.position;

        foreach (SmallFishController fish in currentSchool.fishes)
        {
            if(fish != this)
            {
                float distance = Vector2.Distance(fish.transform.position, this.transform.position);
                vCenter += fish.transform.position;

                if (distance < GameManager.instance.distanceAvoid)
                    vAvoid = vAvoid + (this.transform.position - fish.transform.position);

                groupSpeed += fish.currentSpeed;
            }
        }

        if(currentSchool.fishes.Count > 1)
            vCenter = goalPos;//vCenter = vCenter / currentSchool.fishes.Count + (goalPos - this.transform.position);
        else
            vCenter = goalPos;

        currentSpeed = groupSpeed / currentSchool.fishes.Count;

        Vector3 direction = (vCenter + vAvoid) - transform.position;
        if(direction != Vector3.zero)
        {
            float singleStep = GameManager.instance.smallFishRotationSpeed * Time.deltaTime;
            //myRigidboy.velocity = Vector3.RotateTowards(myRigidboy.velocity, direction, singleStep, 0.0f);
            float angleDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float angleVelocity = Mathf.Atan2(flockVelocity.y, flockVelocity.x) * Mathf.Rad2Deg;

            flockVelocity = Quaternion.Slerp(Quaternion.AngleAxis(angleVelocity, Vector3.forward), Quaternion.AngleAxis(angleDirection, Vector3.forward), singleStep) * Vector3.right * currentSpeed;
        }
    }

    public void Capture(FishSchoolController school)
    {
        isInSchool = true;
        currentSchool = school;
        gameObject.layer = 7;

        Flock();

        if (door != null)
            door.IncrementObjective();
    }

    public void Release()
    {
        isInSchool = false;
        currentSchool = null;
        gameObject.layer = 8;

        if (door != null)
            door.DecrementObjective();
    }

    public void Launch(Vector3 velocity)
    {
        //transform.position = currentSchool.transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, velocity);
        myRigidboy.velocity = velocity;
        currentDamage = currentSchool.GetDamage();
        mySprite.flipX = false;

        if (velocity.x < 0 && transform.rotation.eulerAngles.y != 180)
            mySprite.flipY = true;

        Release();
        gameObject.layer = 9;
    }

    public void Kill()
    {
        currentSchool.RemoveFish(this);
        Destroy(gameObject);
    }

    public bool IsFishInSchool()
    {
        return isInSchool;
    }
}
