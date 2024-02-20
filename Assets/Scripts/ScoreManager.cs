using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int lives;
    bool gotAllKeys = false;
    bool playerLose = false;
    public GameObject portal;
    public GameObject player;
    public GameObject UnlockUI;
    public TextMeshProUGUI livesUI;

    [SerializeField]
    private GameObject water;

    void Start()
    {
        score = 0;
        lives = 3;

        Physics.IgnoreCollision(portal.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
    void Update()
    {
        if (score == 3 && gotAllKeys == false)
        {
            unlockDoor();
            gotAllKeys = true;
        }
        if (lives <= 0 && playerLose == false)
        {
            StartCoroutine(EndGame());
            playerLose = true;
        }

        livesUI.SetText("" + lives);
    }

    IEnumerator EndGame()
    {
        player.GetComponent<Collider>().enabled = false;
        water.GetComponent<Animator>().SetTrigger("Move");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("LoseScreen");
    }

    public void LoseLife()
    {
        lives--;
    }

    public void AddScore()
    {
        score++;
    }

    //Player is able to collide with the main portal in order to escape
    void unlockDoor()
    {
        Physics.IgnoreCollision(portal.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        StartCoroutine(ShowText());
    }
    
    //Display text for a short period of time
    private IEnumerator ShowText()
    {
        UnlockUI.SetActive(true);
        yield return new WaitForSeconds(3);
        UnlockUI.SetActive(false);
    }
}
