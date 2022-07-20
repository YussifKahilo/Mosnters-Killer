using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider[] healthBars;
    [SerializeField] float maxHealth;
    [SerializeField] float actualHealth;
    float currentHealth;
    float targetHealth;

    [SerializeField] float timeElapsed;
    [SerializeField] float lerpSpeed = 0.2f;
    [SerializeField] float secondsToWait = 3f;
    [SerializeField]float hitTime;

    bool canChange = false;
    bool increase = false;

    // Start is called before the first frame update
    void Start()
    {
        actualHealth = maxHealth;
        currentHealth = maxHealth;
        targetHealth = maxHealth;
        SetHealthBars();
        SetHealthValue();
    }

    void SetHealthBars()
    {
        float amount = maxHealth / 4;
        for (int i = 0; i < healthBars.Length; i ++)
        {
            healthBars[i].minValue = i * amount;
            healthBars[i].maxValue = (i + 1) * amount;
        }
    }

    void SetHealthValue()
    {
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].value = currentHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth < maxHealth && !canChange)
        {
            hitTime += Time.deltaTime;
            if (hitTime >= secondsToWait)
            {
                IncreseHealth();
            }
        }

        if (canChange)
        {
            if (timeElapsed < lerpSpeed * (increase ? 20 : 1) && (int) targetHealth != (int) currentHealth)
            {
                currentHealth = Mathf.Lerp(currentHealth, targetHealth, timeElapsed / (lerpSpeed * (increase ? 20 : 1) ));
                timeElapsed += Time.deltaTime;
            }
            else
            {
                currentHealth = targetHealth;
                increase = false;
                canChange = false;
                timeElapsed = 0;
                hitTime = 0;
            }
        }

        healthBars[0].GetComponent<Animator>().SetBool("Change Alpha" , !(currentHealth >= maxHealth / 4));

        SetHealthValue();

    }


    public void IncreseHealth()
    {
        targetHealth = maxHealth;
        increase = true;
        canChange = true;
    }

    public void TakeDamage(int amount)
    {
        hitTime = 0;
        canChange = true;
        targetHealth = currentHealth - amount;
        if (targetHealth < 0)
            targetHealth = 0;
    }
}
