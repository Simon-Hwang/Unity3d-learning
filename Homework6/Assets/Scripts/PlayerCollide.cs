using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;
public class PlayerCollide : MonoBehaviour
{

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameEventManager.Instance.PlayerGameover();
        }
    }
}
