using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //Instances
    [SerializeField]
    public GameObject fishFood;

    //Values
    public float smallFishSpeed;
    public float smallFishRotationSpeed;
    public float flockChance;
    public float distanceAvoid;
    public float roamMinTime;
    public float roamMaxTime;
    public float minForceFood;
    public float maxForceFood;
    public int foodValue;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 RandomUnitVector()
    {
        float random = Random.Range(0f, 2 * Mathf.PI);
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }
}
