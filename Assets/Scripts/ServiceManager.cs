using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceManager : MonoBehaviour
{
    #region Singleton
    public static ServiceManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    private void Start()
    {
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayerPrefs.SetInt(GamePrefs.LastpPlayedLvl.ToString(), SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt(GamePrefs.LvlPlayed.ToString() + SceneManager.GetActiveScene().buildIndex, 1);
        }
    }

    public void Restart()
    {
        ChangeLvL(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndLevel()
    {
        if((SceneManager.GetActiveScene().buildIndex + 1) == 4)
            ChangeLvL(0);
        else
            ChangeLvL(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeLvL(int lvl)
    {
        SceneManager.LoadScene(lvl);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}

public enum Scenes
{
    MainMenu,
    First,
    Second,
    Third,
}

public enum GamePrefs
{
    LastpPlayedLvl,
    LvlPlayed,
}
