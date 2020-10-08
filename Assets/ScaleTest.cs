using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTest : MonoBehaviour
{
    Vector3 temp;
    private BoxCollider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = playerCollider.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        temp = transform.localScale;
        temp.y = -0.5f;
        transform.localScale = temp;
    }
}
