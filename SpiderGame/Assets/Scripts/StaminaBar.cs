using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Slider
using System.Collections;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar; // Reference to the slider
    public static StaminaBar staminaBarInstance; // Create a instance of the class - easier to be called. Can be used from every script

    private int maxStamina = 1000;
    public int currentStamina;
    private WaitForSeconds regenerationForSeconds = new WaitForSeconds(0.1f); // Creates a waitforseconds given that while will creat a new one everytime otherwise
    private Coroutine regen; // Coroutine variable

    private void Awake()
    {
        staminaBarInstance = this; // Creates a instance
    }

    void Start()
    {
        currentStamina = maxStamina; // Create so that when started - give 100 P
        staminaBar.maxValue = maxStamina; // The gameobject - give it maxvalue and that is equal to maxstamina == 100
        staminaBar.value = maxStamina; // The staminabars current value equal to max.
    }

    public void UseStamina(int amount) // The amount of stamina we want to use for this function.
    {
        if (currentStamina - amount >= 0) // Current stamina - the amount (15) we want to use greator or equal to, we have enough to perform that action.
        {
            currentStamina -= amount; // Current - = the amount.
            staminaBar.value = currentStamina; // The value after we want to use it

            if (regen != null) // If the coroutine not equal to null = We are generating stamina.
                StopCoroutine(regen); // Stop the corotouine, regen.

            regen = StartCoroutine(RegenStamina()); // Esle start it.
        }
        else
        {
            Debug.Log("Not enough Stamina");
        }
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(0.5f); // Hold and wait for sec.

        while (currentStamina < maxStamina) // Regenerate loop. Maxstamina higher then current
        {
            currentStamina += maxStamina / (maxStamina/20); // Increment current with max. / 100 will give us the same rate. Maybe change?
            staminaBar.value = currentStamina; // The value of the bar
            yield return regenerationForSeconds; // 
        }
        regen = null; // After process is completed. This stops the regen.
    }
}