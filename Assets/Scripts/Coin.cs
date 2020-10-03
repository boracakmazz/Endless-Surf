using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.collectCoin();
            Destroy(gameObject,0.09f);   //Change the second parameter if you want to make disappear the coin quickly or slower.
        }
    }

    
}
