using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using UnityEngine;

public class Obstacle :MonoBehaviour
{
    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //kill player
        if(collision.gameObject.name == "Player")
        {
            playerMovement.Die();
        }
    }
}
