using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolController : MonoBehaviour
{
    //Instances
    [SerializeField]
    SpriteRenderer arrow;
    [SerializeField]
    AudioClip[] launchSounds;

    //Values
    [SerializeField]
    float speed;
    [SerializeField]
    float distanceArrow;
    [SerializeField]
    float launchSpeed;

    //Components
    Rigidbody2D myRigidboy;
    AudioSource myAudioSource;

    //Variables
    public List<SmallFishController> fishes = new List<SmallFishController>();
    public Vector2 currentVelocity;
    public float previousRightTriggerValue = 0f;
    int previousLaunchSoundIndex = -1;


    private void Awake()
    {
        myRigidboy = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Aim();
    }

    void Move()
    {
        Vector2 velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(velocity.magnitude > 1.0f)
            velocity.Normalize();

        myRigidboy.velocity = velocity * speed;
        currentVelocity = myRigidboy.velocity;
    }

    void Aim()
    {
        Vector3 aimDirection = new Vector2(Input.GetAxis("HorizontalRightStick"), Input.GetAxis("VerticalRightStick"));

        if (aimDirection.sqrMagnitude > 0.0f)
        {
            arrow.enabled = true;
            arrow.transform.position = transform.position + (aimDirection.normalized * distanceArrow);
            arrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimDirection);

            if (previousRightTriggerValue <= 0f && Input.GetAxis("RightTrigger") > 0f)
                LaunchFish(aimDirection);
        }
        else
        {
            arrow.enabled = false;
        }

        previousRightTriggerValue = Input.GetAxis("RightTrigger");
    }

    void LaunchFish(Vector3 direction)
    {
        if(fishes.Count > 0)
        {
            SmallFishController fish = fishes[fishes.Count - 1];
            fishes.RemoveAt(fishes.Count - 1);
            fish.Launch(direction.normalized * launchSpeed);

            int randomIndex = Random.Range(0, launchSounds.Length);

            while (randomIndex == previousLaunchSoundIndex)
                randomIndex = Random.Range(0, launchSounds.Length);

            myAudioSource.clip = launchSounds[randomIndex];
            myAudioSource.Play();

            previousLaunchSoundIndex = randomIndex;
        }
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
        if(fish.IsFishInSchool() == false)
        {
            fishes.Add(fish);
            fish.Capture(this);
        }
    }
}
