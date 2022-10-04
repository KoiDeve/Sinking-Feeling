using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{

    private bool started = false;
    private Animator animator;

    public AudioClip sfx_click;
    private AudioSource radio;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (radio == null) {
            radio = GameObject.Find("sfx_radio").GetComponent<AudioSource>();
        }
    }

    private void Click() {
        radio.Stop();
        radio.clip = sfx_click;
        radio.Play();
    }

    public void SetRulesOpen(bool open) {
        Click();
        animator.SetBool("rulesOpen", open);
    }

    public void SetGameStart() {
        if (!started) {
            started = true;
            Click();
            animator.SetBool("gameStart", true);
            StartCoroutine(DelayGame());
        }
    }

    public void ExitGame() {
        Application.Quit();
    }

    IEnumerator DelayGame() { 
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("GameScene");
    }

    public void TitleScreen() { 
        SceneManager.LoadScene("TitleScreen");
    }

}
