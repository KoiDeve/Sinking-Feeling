using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrownedController : MonoBehaviour
{

    private Animator animator;
    private bool saved = false;
    private Slider oxygen;
    public AudioClip sfx_ressurect;

    private bool available = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        oxygen = GameObject.Find("Oxygen Level").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (available) {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (!saved)
                {
                    AudioSource x = GameObject.Find("sfx_radio").GetComponent<AudioSource>();
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
           // Debug.Log("able to save victim");
        }
    }

    IEnumerator Ressurect() {
        oxygen.value -= 90;
        animator.SetBool("ressurect", true);
        yield return new WaitForSeconds(3f);
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        available = true;
    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {

        available = false;
        Debug.Log("leaving victim");
        if (saved) {
            return;
        }
        animator.SetBool("bounds", false);
    }


    public bool returnSaved() {
        return saved;
    }

}
