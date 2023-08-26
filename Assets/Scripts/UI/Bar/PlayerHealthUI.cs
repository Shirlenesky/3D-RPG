using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private Text levetText;

    private Image healthSlider;

    private Image expSlider;

    private void Awake()
    {
        levetText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider =  transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        levetText.text = "Level" + GameManager.Instance.playerStats.characerData.currentLevel.ToString("00");

        UpdateHealth();
        UpdateExp();
    }

    private void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    private void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.characerData.currtExp / GameManager.Instance.playerStats.characerData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
