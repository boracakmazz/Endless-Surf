using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt; //Our player 
    [SerializeField]
    public Vector3 offset = new Vector3(0, 5.0f, -10.0f);

    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = lookAt.position + offset;
    }

    
    void LateUpdate()   //It will start after our player moves
    {

        Vector3 desiredPosition = lookAt.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}
