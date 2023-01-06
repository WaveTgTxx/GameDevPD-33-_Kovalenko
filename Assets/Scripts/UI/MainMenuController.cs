using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : BaseGameMenuController
{
    [SerializeField] private Button chooselvl;
    [SerializeField] private Button reset;

    [SerializeField] private GameObject lvlMenu;
    [SerializeField] private Button closeLvlMenu;

    private int lvl = 1;

    protected override void Start()
    {
        base.Start();
        chooselvl.onClick.AddListener(OnMenuLvlClicked);
        closeLvlMenu.onClick.AddListener(OnMenuLvlClicked);

        if (PlayerPrefs.HasKey(GamePrefs.LastpPlayedLvl.ToString()))
        {
            resume.GetComponentInChildren<TMP_Text>().text = "Resume";
            lvl = PlayerPrefs.GetInt(GamePrefs.LastpPlayedLvl.ToString());
        }

        resume.onClick.AddListener(OnPlayClicked);
        reset.onClick.AddListener(OnResetClicked);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        chooselvl.onClick.RemoveListener(OnMenuLvlClicked);
        closeLvlMenu.onClick.RemoveListener(OnMenuLvlClicked);
        resume.onClick.RemoveListener(OnPlayClicked);
        reset.onClick.RemoveListener(OnResetClicked);
    }

    private void OnMenuLvlClicked()
    {
        lvlMenu.SetActive(!lvlMenu.activeInHierarchy);
        OnMenuClicked();
    }

    private void OnPlayClicked()
    {
        serviceManager.ChangeLvL(lvl);
    }

    private void OnResetClicked()
    {
        resume.GetComponentInChildren<TMP_Text>().text = "Play";
        lvl = 1;
        serviceManager.ResetProgress();
    }
}
