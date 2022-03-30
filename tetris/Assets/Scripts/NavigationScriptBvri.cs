using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationScriptBvri : MonoBehaviour
{
    public void PlayAgainBvri()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlayGameBvri()
    {
        SceneManager.LoadScene("Difficulty");
    }

    public void ControlsBvri()
    {
        SceneManager.LoadScene("Controls");
    }

    public void BackToMenuBvri()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void ExitGameBvri()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void EasyDifficultyBvri()
    {
        GameBvri.startingLevelBvri = 0;
        GameBvri.startingAtLevelZeroBvri = true;
        SceneManager.LoadScene("Game");
    }

    public void MediumDifficultyBvri()
    {
        GameBvri.startingLevelBvri = 5;
        GameBvri.startingAtLevelZeroBvri = false;
        SceneManager.LoadScene("Game");
    }

    public void HardDifficultyBvri()
    {
        GameBvri.startingLevelBvri = 9;
        GameBvri.startingAtLevelZeroBvri = false;
        SceneManager.LoadScene("Game");
    }
}
