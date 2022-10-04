using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script controls the canvas elements to the game, for example, various scene transitions and buttons.
public class CanvasController : MonoBehaviour {

    // Checks to make sure an animation is ready to use and gets the Animator component.
    private bool started = false;
    private Animator animator;

    // A sfx mechanism for the click of the button.
    public AudioClip sfx_click;
    private AudioSource radio;

    // Start is called before the first frame update.
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame.
    void Update() {
        if (radio == null) {
            radio = GameObject.Find("sfx_radio").GetComponent<AudioSource>();
        }
    }

    // Plays the button clicking noise.
    private void Click() {
        radio.Stop();
        radio.clip = sfx_click;
        radio.Play();
    }

    // Opens the rules to the game.
    public void SetRulesOpen(bool open) {
        Click();
        animator.SetBool("rulesOpen", open);
    }

    // Starts the transition, and starts the game once the transition is finished.
    public void SetGameStart() {
        if (!started) {
            started = true;
            Click();
            animator.SetBool("gameStart", true);
            StartCoroutine(DelayGame());
        }
    }

    // Quits the game, and closes the application.
    public void ExitGame() {
        Application.Quit();
    }

    // Waits for the transition to finish before loading the game.
    IEnumerator DelayGame() { 
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("GameScene");
    }

    // Loads the title scene
    public void TitleScreen() { 
        SceneManager.LoadScene("TitleScreen");
    }

}
