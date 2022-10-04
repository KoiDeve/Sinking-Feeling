using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A 'secret' script used to check if the player swam above the lifeguard during the game.
public class Lifeguard : MonoBehaviour
{

    // If the player is seen above the lifeguard. . .
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") {
            FindObjectOfType<GamePlayer>().SetOverBoat();
        }
    }

}
