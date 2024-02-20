using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMangement : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
}
