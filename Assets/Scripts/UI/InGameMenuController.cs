using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuController : BaseGameMenuController
{
    [SerializeField] private Button restart;
    [SerializeField] private Button backToMenu;

    protected override void Start()
    {
        base.Start();
        resume.onClick.AddListener(OnMenuClicked);
        restart.onClick.AddListener(ServiceManager.Instance.Restart);
        backToMenu.onClick.AddListener(OnMainMenuClicked);
    }

    protected override void OnDestroy()
    {
        resume.onClick.RemoveListener(OnMenuClicked);
        restart.onClick.RemoveListener(ServiceManager.Instance.Restart);
        backToMenu.onClick.RemoveListener(OnMainMenuClicked);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Escape))
            OnMenuClicked();
    }

    protected override void OnMenuClicked()
    {
        base.OnMenuClicked();
        Time.timeScale = menu.activeInHierarchy ? 0 : 1;
    }

    public void OnMainMenuClicked()
    {
        ServiceManager.Instance.ChangeLvL((int)Scenes.MainMenu);
    }
}
