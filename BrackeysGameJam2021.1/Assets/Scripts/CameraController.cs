using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Instances
    [SerializeField]
    Transform fishSchool;

    //Values
    [SerializeField]
    float smoothTime = 0.3F;

    //Variables
    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, fishSchool.position, ref velocity, smoothTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }
}
