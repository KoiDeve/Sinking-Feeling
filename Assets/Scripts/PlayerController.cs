using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private bool dead = false;
    private bool floating = true;

    public float playerSpeed = 3f;
    public float gravity_min = 0.05f, gravityMax = 1f, gravity_threshold = 1.4f;

    private Rigidbody2D myBody;
    private Animator animator;

    private Slider oxygenLevel;

    private GamePlayer gamePlayer;

    private AudioSource sfx_source;
    public AudioClip sfx_swim, sfx_breathe, sfx_drown;


    // Start is called before the first frame update
    void Start()
    {
        gamePlayer = FindObjectOfType<GamePlayer>();
        oxygenLevel = GameObject.Find("Oxygen Level").GetComponent<Slider>();
        animator = GetComponent<Animator>();
        myBody = FindObjectOfType<Rigidbody2D>();
        
        RotationUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) {
            return;
        }
        if (oxygenLevel.value <= 0) {
            if (sfx_source == null) {
                sfx_source = GameObject.Find("sfx_radio").GetComponent<AudioSource>();
            }
            sfx_source.Stop();
            sfx_source.clip = sfx_drown;
            sfx_source.Play();
            StartCoroutine(Death());
        }

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

        RotationUpdate();
        


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

    private void RotationUpdate() {
        Vector2 spritePos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(spritePos.y - mouseOnScreen.y, spritePos.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle+90));
    }


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

    IEnumerator Death() {
        dead = true;
        animator.SetBool("death", true);
        yield return new WaitForSeconds(4f);
        Debug.LogWarning("Lose Condition");
        FindObjectOfType<GamePlayer>().Lose();
    }

    

}
