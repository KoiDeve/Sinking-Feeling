using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The script that controls the player. Attached to the player.
public class PlayerController : MonoBehaviour {

    // Parameters to see where the player is in relation to the game.
    private bool dead = false;
    private bool floating = true;

    // Parameters for the speed and gravity of the player.
    public float playerSpeed = 3f;
    public float gravity_min = 0.05f, gravityMax = 1f, gravity_threshold = 1.4f;

    // Physics elements for the player.
    private Rigidbody2D myBody;
    private Animator animator;

    // Other canvas/external components.
    private Slider oxygenLevel;
    private GamePlayer gamePlayer;

    // Audio for the swimming sounds.
    private AudioSource sfx_source;
    public AudioClip sfx_swim, sfx_breathe, sfx_drown;


    // Finds the GamePlayer object which keeps track of the game, as well as animators and physics components.
    void Start()
    {
        gamePlayer = FindObjectOfType<GamePlayer>();
        oxygenLevel = GameObject.Find("Oxygen Level").GetComponent<Slider>();
        animator = GetComponent<Animator>();
        myBody = FindObjectOfType<Rigidbody2D>();
        
        RotationUpdate();
    }

    // Updates actions the player can take, such as turning, swimming, gravity, etc.
    void Update()
    {
        if (dead) {
            return;
        }

        // Used to measure the oxygen levels of the player.
        if (oxygenLevel.value <= 0) {
            if (sfx_source == null) {
                sfx_source = GameObject.Find("sfx_radio").GetComponent<AudioSource>();
            }
            sfx_source.Stop();
            sfx_source.clip = sfx_drown;
            sfx_source.Play();
            StartCoroutine(Death());
        }

        // Used to update the gravity position of the player in relation to the position on the map.
        // Updates oxygen leves depending on the height of the player.
        if (transform.position.y > gravity_threshold) {
            myBody.gravityScale = gravityMax * transform.position.y;
            if (!floating) {
                gamePlayer.oxygenCounter++;
                if (sfx_source == null)
                {
                    sfx_source = GameObject.Find("sfx_radio").GetComponent<AudioSource>();
                }
                sfx_source.Stop();
                sfx_source.clip = sfx_breathe;
                sfx_source.Play();
            }
            floating = true;
            if (oxygenLevel.value < 500) {
                oxygenLevel.value = 500;
            }
        }
        else {
            myBody.gravityScale = gravity_min;
            if (floating)
            {
                floating = false;
                StartCoroutine(StartToDrown());
            }
        }

        // Updates the rotation in relation to the position of the mouse.
        RotationUpdate();

        // Used for swimming the player around.
        if (Input.GetKeyUp(KeyCode.Space)) {
            Debug.Log("Space pressed");
            oxygenLevel.value -= 5;
            if (sfx_source == null) {
                sfx_source = GameObject.Find("sfx_radio").GetComponent<AudioSource>();
            }
            sfx_source.Stop();
            sfx_source.clip = sfx_swim;
            sfx_source.Play();

            animator.SetTrigger("swim");
            float currentDegree = gameObject.transform.rotation.eulerAngles.z;
            Debug.Log(currentDegree);
            currentDegree = 360 - currentDegree;
            currentDegree *= Mathf.PI / 180;
            myBody.velocity = new Vector2(Mathf.Sin(currentDegree) * playerSpeed, Mathf.Cos(currentDegree) * playerSpeed);
        }
    }

    // Updates the rotational position fo the player.
    private void RotationUpdate() {
        Vector2 spritePos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(spritePos.y - mouseOnScreen.y, spritePos.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle+90));
    }

    // A coroutine used for the oxygen levels (essentially decreases the oxygen levels for the player).
    IEnumerator StartToDrown() {
        int cnt = 0;
        for (int i = 0; i < 1; i++) {
            if (!floating) {
                i--;
                cnt++;
                yield return new WaitForSeconds(0.1f);
                if (cnt % 10 == 0) {
                    oxygenLevel.value--;
                }
            }
        }
    }

    // A coroutine used for when the player dies.
    IEnumerator Death() {
        dead = true;
        animator.SetBool("death", true);
        yield return new WaitForSeconds(4f);
        Debug.LogWarning("Lose Condition");
        FindObjectOfType<GamePlayer>().Lose();
    }
}
