using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This controls the lost souls in the game. This script is attached to each lost soul.
public class DrownedController : MonoBehaviour
{

    // An animator to save the lost soul, as well as parameters for oxygen levels and the status of saving the lost soul.
    private Animator animator;
    private bool saved = false;
    private Slider oxygen;
    public AudioClip sfx_ressurect;
    private bool available = false;

    // Gets the souls' animator component, as well as the player's oxygen levels.
    void Start()
    {
        animator = GetComponent<Animator>();
        oxygen = GameObject.Find("Oxygen Level").GetComponent<Slider>();
    }

    // When ressurecting the soul, a sfx will be queued as well as an animation corresponding to it. 
    // Oxygen levels are also decreased when a soul is saved.
    void Update()
    {
        if (available) {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (!saved)
                {
                    AudioSource x = GameObject.Find("sfx_resurrect").GetComponent<AudioSource>();
                    x.Stop();
                    x.clip = sfx_ressurect;
                    x.Play();
                    saved = true;
                    FindObjectOfType<GamePlayer>().UpdateCorpse();
                    StartCoroutine(Ressurect());
                }
            }
            if (!animator.GetBool("bounds"))
            {
                animator.SetBool("bounds", true);
            }
        }
    }

    // Lowers the player's oxygen levels, and removes the soul from the scene.
    IEnumerator Ressurect() {
        oxygen.value -= 90;
        animator.SetBool("ressurect", true);
        yield return new WaitForSeconds(3f);
        Destroy(this);
    }

    // Checks to make sure the player is available to save the soul.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        available = true;
    }

    // Disables the player's ability to save the soul.
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        available = false;
        if (saved) {
            return;
        }
        animator.SetBool("bounds", false);
    }

    // Checks to make sure that the soul is saved, ensuring it doesn't switch to another animation state.
    public bool returnSaved() {
        return saved;
    }

}
