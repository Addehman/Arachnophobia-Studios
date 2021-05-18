using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Slider
using System.Collections;

public class StaminaBar : MonoBehaviour
{
    public Image staminaBar; // Reference to the slider
    public static StaminaBar staminaBarInstance; // Create a instance of the class - easier to be called. Can be used from every script

    private float maxStamina = 1;
    public float currentStamina = 1;
    private WaitForSeconds regenerationForSeconds = new WaitForSeconds(0.1f); // Creates a waitforseconds given that while will creat a new one everytime otherwise
    private Coroutine regen; // Coroutine variable

    private void Awake()
    {
        staminaBarInstance = this; // Creates a instance
    }

    void Start()
    {
        //currentStamina = currentStamina; // Create so that when started - give 100 P
        staminaBar.fillAmount = maxStamina; // The gameobject - give it maxvalue and that is equal to maxstamina == 100
        //staminaBar.value = currentStamina; // The staminabars current value equal to max.
    }
    private void Update()
    {
        currentStamina = staminaBar.fillAmount;
    }

    public void UseStamina(float amount) // The amount of stamina we want to use for this function.
    {
        if (currentStamina - amount >= 0) // Current stamina - the amount (X) we want to use greator or equal to, we have enough to perform that action.
        {
            // currentStamina -= amount;
            staminaBar.fillAmount -= amount; // Current - = the amount.
            //staminaBar.fillAmount = currentStamina; // The value after we want to use it

            if (regen != null) // If the coroutine not equal to null = We are generating stamina.
                StopCoroutine(regen); // Stop the corotouine, regen.

            regen = StartCoroutine(Regenerate()); // else start it.
        }
        else
        {
            Debug.Log("Not enough Stamina");
        }
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(0.5f); // Hold and wait for sec.

        while (staminaBar.fillAmount < 1f) // Regenerate loop. Maxstamina higher then current
        {
            staminaBar.fillAmount += maxStamina / (maxStamina / 0.02f); // Increment current with max. / 100 will give us the same rate. Maybe change?
            //staminaBar.fillAmount = currentStamina; // The value of the bar
            yield return regenerationForSeconds;
        }
        regen = null; // After process is completed. This stops the regen.
    }
}