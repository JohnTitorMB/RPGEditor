using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraNavigation : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    Vector3 distance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            transform.position = target.position + distance;
            transform.LookAt(target);
        }
    }


}
