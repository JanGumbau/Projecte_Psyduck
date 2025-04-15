using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimits : MonoBehaviour
{

    [SerializeField]
    private GameObject cameraLeftLimit;

    [SerializeField]
    private GameObject cameraRightLimit;

    [SerializeField]
    private GameObject cameraTarget;

    private void Awake()
    {
       if(transform.position.x < cameraLeftLimit.transform.position.x || transform.position.x > cameraRightLimit.transform.position.x)
        {
            transform.position = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y, transform.position.z);
        }
    }
}
