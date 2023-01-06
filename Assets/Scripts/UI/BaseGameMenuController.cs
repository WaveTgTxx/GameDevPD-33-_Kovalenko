using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameMenuController : MonoBehaviour
{
    protected ServiceManager serviceManager;

    [SerializeField] protected GameObject menu;

    [Header("MainButtons")]
    [SerializeField] protected Button resume;
    [SerializeField] protected Button quit;

    protected virtual void Start()
    {
        serviceManager = ServiceManager.Instance;
        quit.onClick.AddListener(serviceManager.Quit);
    }

    protected virtual void OnDestroy()
    {
        quit.onClick.RemoveListener(serviceManager.Quit);
    }

    protected virtual void Update() { }

    protected virtual void OnMenuClicked()
    {
        menu.SetActive(!menu.activeInHierarchy);
    }
}