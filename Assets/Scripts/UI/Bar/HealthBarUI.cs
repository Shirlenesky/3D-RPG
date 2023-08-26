using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{    
    public bool alwaysVisable;

    public float visiableTime;

    private float timeLeft;

    public GameObject healthBarUIPrefab;

    public Transform healthBarPos;

    private Image healthSlider;

    private Transform UIbarTrans;
    private Transform cam;

    private CharacterStats currStats;

    private void Awake()
    {
        currStats = GetComponent<CharacterStats>();
        currStats.UpdateHealthBarOnAttack += OnUpdateHealthBarOnAttack;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbarTrans = Instantiate(healthBarUIPrefab, canvas.transform).transform;
                healthSlider = UIbarTrans.GetChild(0).GetComponent<Image>();

                UIbarTrans.gameObject.SetActive(alwaysVisable);
            }
        }

        //EventHandler.UpdateHealthBarOnAttack += OnUpdateHealthBarOnAttack;
    }


    private void OnDisable()
    {
        //EventHandler.UpdateHealthBarOnAttack -= OnUpdateHealthBarOnAttack;
    }

    private void OnUpdateHealthBarOnAttack(int currHealth, int maxHealth)
    {
        if (currHealth <= 0 && UIbarTrans.gameObject)
        {
            Destroy(UIbarTrans.gameObject);
        }

        UIbarTrans.gameObject.SetActive(true);
        timeLeft = visiableTime;

        float sliderPercent = (float)currHealth / maxHealth;

        healthSlider.fillAmount = sliderPercent;
    }

    private void LateUpdate()
    {
        if(UIbarTrans != null)
        {
            UIbarTrans.position = healthBarPos.position;
            UIbarTrans.forward = -cam.forward;

            if(timeLeft <= 0 && !alwaysVisable)
            {
                UIbarTrans.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }

        }
    }
}
