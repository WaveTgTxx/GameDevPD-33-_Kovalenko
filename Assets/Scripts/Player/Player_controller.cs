using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_controller : MonoBehaviour
{
    private ServiceManager serviceManager;
    [SerializeField] private int maxHp;
    private int currHp;
    [SerializeField] private int maxMp;
    private int currMp;

    [SerializeField] private TMP_Text text;

    [SerializeField] Slider hpSlider;
    [SerializeField] Slider mpSlider;

    Movement_Controller playerMovement;
    private bool _canBeDamaged = true;
    Vector2 startPosition;
    public int amountOfCoins = 0;

    void Start()
    {
        playerMovement = GetComponent<Movement_Controller>();
        playerMovement.OnGetHurt += OnGetHurt;
        currHp = maxHp;
        currMp = maxMp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        mpSlider.maxValue = maxMp;
        mpSlider.value = maxMp;
        startPosition = transform.position;
        serviceManager = ServiceManager.Instance;
    }

    public void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform enemy = null)
    {
        if (!_canBeDamaged)
            return;

        currHp -= damage;
        if (currHp <= 0)
            OnDeath();

        switch (type)
        {
            case DamageType.Casual:
                playerMovement.GetHurt(enemy.position);
                break;
            case DamageType.Traps:
                break;
        }

        hpSlider.value = currHp;
    }

    public void TakeDamageFromTraps(int value)
    {
        currHp += value;
        if (currHp > maxHp)
            currHp = maxHp;
        else if (currHp <= 0)
            OnDeath();

        hpSlider.value = currHp;
    }

    private void OnGetHurt(bool canBeDamaged)
    {
        _canBeDamaged = canBeDamaged;
    }

    #region UseItems
    public void RestoreHP(int hp)
    {
        currHp += hp;
        if (currHp > maxHp)
            currHp = maxHp;

        hpSlider.value = currHp;
    }

    public void TakeCoins(int value)
    {
        amountOfCoins += value;

        text.text = amountOfCoins.ToString();

    }

    public bool ChangeMp(int value)
    {
        if (value < 0 && currMp < Mathf.Abs(value))
            return false;

        currMp += value;
        if (currMp > maxMp)
            currMp = maxMp;
        mpSlider.value = currMp;

        return true;
    }
    #endregion

    public void OnDeath()
    {
        serviceManager.Restart();
    }

}
