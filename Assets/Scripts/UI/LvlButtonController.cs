using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvlButtonController : MonoBehaviour
{
    private Button button;
    [SerializeField] private Scenes scene;

    void Start()
    {
        button = GetComponent<Button>();
        if (!PlayerPrefs.HasKey(GamePrefs.LvlPlayed.ToString() + ((int)scene).ToString()))
        {
            button.interactable = false;
            return;
        }

        button.onClick.AddListener(OnChangeLvlClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    void OnChangeLvlClicked()
    {
        ServiceManager.Instance.ChangeLvL((int) scene);
    }
}
