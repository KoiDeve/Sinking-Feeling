using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifeguard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.LogError("Lifeguard status obtained!" + collision.gameObject.name);
        if (collision.gameObject.name == "Player") {
            FindObjectOfType<GamePlayer>().SetOverBoat();
        }
    }

}
