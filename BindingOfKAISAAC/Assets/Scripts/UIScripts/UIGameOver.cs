using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    public void RetryButton() {
        SceneManager.LoadScene("MainScene");
        GameManager.instance.enabled = true;
    }
}
