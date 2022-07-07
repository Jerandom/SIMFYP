using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaBar : MonoBehaviour
{
    [SerializeField] private Slider staminaBar;

    private float maxStamina = 100f;
    private float currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);

    private Coroutine regen;

    public static PlayerStaminaBar instance;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void UseStamina(float amount)
    {
        if(currentStamina >= 0)
        {

            currentStamina -= amount * Time.deltaTime;
            staminaBar.value = currentStamina;

            if (regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(regenStamina());
        }
    }

    private IEnumerator regenStamina()
    {
        yield return new WaitForSeconds(2);

        while(currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }

    public float getCurrentStamina()
    {
        return currentStamina;
    }
}
