using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// A background object that keeps track of the state of the game.
// Parameters such as oxygen, amount of souls left, as well as achievements are also saved here.
public class GamePlayer : MonoBehaviour
{
    // Oxygen level
    public int oxygenCounter = 0;

    // Achievement parameters
    private static bool speedrunner = false, wuss = false, overTheMoon = false, savedall = false;
    private bool overBoat = false;

    private bool finished = false;
    public Animator canvasAnimator;

    // Used for the canvas element, to display the amount of souls left + time played.
    private Slider oxygenLevel;
    public TMP_Text timerText, corpseText;
    int ms = 0, s = 0;

    // Soul tracker, as well as animators for achievements.
    public List<DrownedController> corpses;

    private Animator _wuss;
    private Animator _savedALL;
    private Animator _boat;
    private Animator _speedrunner;

    private static bool exists;

    public AudioClip sfx_newAchievement;


    // Ensures that there is only one object keeping track of the game.
    void Start()
    {
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
    }

    // Finds the parameters needed depending on the scene.
    // levels:
    //   0 - Title Screen
    //   1 - Gameplay
    //   2 - Achievements Screen
    private void OnLevelWasLoaded(int level)
    {
        if (canvasAnimator == null) {
            canvasAnimator = GameObject.Find("Canvas").GetComponent<Animator>();
        }
        if (level == 2)
        {
            Debug.LogWarning("update stats / achievenments");
            GameObject.Find("FinalTime").GetComponent<TMP_Text>().text = "" + s + "." + ms;
            _wuss = GameObject.Find("wuss").GetComponent<Animator>();
            _speedrunner = GameObject.Find("speedrunner").GetComponent<Animator>();
            _boat = GameObject.Find("boat").GetComponent<Animator>();
            _savedALL = GameObject.Find("saved").GetComponent<Animator>();
            StartCoroutine(Achievements());
        }
        else if (level == 1) {
            ms = 0;
            s = 0;
            oxygenCounter = 0;
            overBoat = false;
            oxygenLevel = FindObjectOfType<Slider>();
            timerText = GameObject.Find("Timer").GetComponent<TMP_Text>();
            timerText.text = "0000.0";
            corpses.Clear();
            corpses.AddRange(FindObjectsOfType<DrownedController>());
            corpseText = GameObject.Find("CorpseCounter").GetComponent<TMP_Text>();
            corpseText.text = corpses.Count.ToString();
            StopAllCoroutines();
            StartCoroutine(StartTimer());
        }
            
        
    }

    // Updates the souls remaining, and checks to see if the player meets the win condition.
    public void UpdateCorpse() {
        for (int i = 0; i < corpses.Count; i++) {
            if (corpses[i].returnSaved()) {
                corpses.RemoveAt(i);
                corpseText.text = corpses.Count.ToString();
            }
        }
        if (corpses.Count <= 0) {
            oxygenLevel.value = 499;
            Debug.LogWarning("Trigger win condition");
            canvasAnimator.SetBool("conditionalMet", true);
            StopAllCoroutines();
            StartCoroutine(EndGame());
        }
    }

    // Starts the countdown to load the next scene.
    IEnumerator EndGame() {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("GameFinished");
    }

    // A timer used for the amount of time played for the game.
    IEnumerator StartTimer() {
        ms++;
        if (ms >= 10) {
            ms = 0;
            s++;
        }
        timerText.text = "" + s + "." + ms;
        yield return new WaitForSeconds(0.1f);
        if (!finished) {
            StartCoroutine(StartTimer());
        }
    }

    // Checks to see which achievements have been accomplished by the player.
    IEnumerator Achievements() {

        bool newAch = false;

        float delayAch = 0.30f;
        if (speedrunner) {
            _speedrunner.SetInteger("ach", 2);
        }
        if (overTheMoon) {
            _boat.SetInteger("ach", 2);
        }
        if (wuss) {
            _wuss.SetInteger("ach", 2);
        }
        if (savedall) {
            _savedALL.SetInteger("ach", 2);
        }

        if (!savedall) {
            if (corpses.Count <= 0) {
                newAch = true;
                savedall = true;
                yield return new WaitForSeconds(delayAch);
                _savedALL.SetInteger("ach", 1);
            }
        }
        if (!speedrunner) {
            if (corpses.Count <= 0 && (s * 10) + ms < 600)
            {
                newAch = true;
                speedrunner = true;
                yield return new WaitForSeconds(delayAch);
                _speedrunner.SetInteger("ach", 1);
            }
        }
        if (!wuss) {
            if (oxygenCounter > 25) {
                newAch = true;
                wuss = true;
                yield return new WaitForSeconds(delayAch);
                _wuss.SetInteger("ach", 1);
            }
        }
        if (!overTheMoon) {
            if (overBoat) {
                newAch = true;
                overTheMoon = true;
                yield return new WaitForSeconds(delayAch);
                _boat.SetInteger("ach", 1);
            }
        }

        if (newAch) {
            AudioSource x = GameObject.Find("sfx_radio").GetComponent<AudioSource>();
            x.Stop();
            x.clip = sfx_newAchievement;
            x.Play();
        }

    }

    // The lose condition for the game.
    public void Lose() {
        canvasAnimator.SetBool("conditionalMet", true);
        StopAllCoroutines();
        StartCoroutine(EndGame());
    }

    // Checks to see if the player swam above the boat for an achievement.
    public void SetOverBoat() {
        overBoat = true;
    }

}
