using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FishSchoolController : MonoBehaviour
{
    //Instances
    [SerializeField]
    SpriteRenderer arrow;
    [SerializeField]
    AudioClip[] launchSounds;
    [SerializeField]
    Transform healthBar;
    [SerializeField]
    GameObject barUI;
    [SerializeField]
    TextMeshPro nbrFishesText;
    [SerializeField]
    GameObject smallFish;

    //Values
    [SerializeField]
    float speed;
    [SerializeField]
    float distanceArrow;
    [SerializeField]
    float launchSpeed;
    [SerializeField]
    int damage;
    [SerializeField]
    bool consumeFood;
    [SerializeField]
    int foodMax;
    [SerializeField]
    float foodConsomeSpeed;

    //Components
    Rigidbody2D myRigidboy;
    AudioSource myAudioSource;

    //Variables
    public List<SmallFishController> fishes = new List<SmallFishController>();
    public Vector2 currentVelocity;
    float previousRightTriggerValue = 0f;
    int previousLaunchSoundIndex = -1;
    float currentFood;


    private void Awake()
    {
        myRigidboy = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();

        currentFood = foodMax;
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
        ConsumeFood();
        UpdateUI();
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
        Vector3 aimDirection = Vector3.zero;

        if (ControllerManager.instance.playWithPad)
        {
            aimDirection = new Vector2(Input.GetAxis("HorizontalRightStick"), Input.GetAxis("VerticalRightStick"));
        }
        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0f);
            aimDirection = (mousePosition - transform.position).normalized;
        }


        if (aimDirection.sqrMagnitude > 0.0f)
        {
            arrow.enabled = true;
            arrow.transform.position = transform.position + (aimDirection.normalized * distanceArrow);
            arrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, aimDirection);

            if (ControllerManager.instance.playWithPad)
            {
                if (previousRightTriggerValue <= 0f && Input.GetAxis("RightTrigger") > 0f)
                    LaunchFish(aimDirection);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                    LaunchFish(aimDirection);
            }
        }
        else
        {
            arrow.enabled = false;
        }

        previousRightTriggerValue = Input.GetAxis("RightTrigger");
    }

    void ConsumeFood()
    {
        if (consumeFood == false)
            return;

        currentFood -= Time.deltaTime * (foodConsomeSpeed + fishes.Count);

        if(currentFood <= 0)
        {
            fishes[0].Kill();
            currentFood = foodMax + currentFood;
        }
    }

    public void RemoveFish(SmallFishController fishToRemove)
    {
        fishes.Remove(fishToRemove);

        if(fishes.Count <= 0)
            SceneManager.LoadScene("End");
    }

    void UpdateUI()
    {
        nbrFishesText.text = fishes.Count.ToString();
        healthBar.localScale = new Vector3(currentFood / foodMax, healthBar.localScale.y, healthBar.localScale.z);
    }

    void LaunchFish(Vector3 direction)
    {
        if(fishes.Count > 1)
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

    public int GetDamage()
    {
        return damage;
    }

    public void GiveFood(int value)
    {
        currentFood += value;
        if(currentFood > foodMax)
        {
            currentFood = 25f;
            Instantiate(smallFish, transform.position, Quaternion.identity);
        }
    }

    public void ActivateFood()
    {
        barUI.SetActive(true);
        consumeFood = true;
    }
}
