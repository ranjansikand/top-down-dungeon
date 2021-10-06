using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // list of premade hearts that can be enabled or disabled
    [SerializeField] GameObject[] hearts;

    int currentHealth;

    public void SetUpHearts(int maxHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth) hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
        }

        currentHealth = maxHealth;
    }

    public void DecreaseHealth()
    {
        if (currentHealth <= 0) {return;}

        // account for the zero-indexing by adjusting value first
        currentHealth--;
        hearts[currentHealth].GetComponent<Health_Heart>().Empty();
        
    }

    public void IncreaseHealth()
    {
        if (currentHealth >= hearts.Length + 1) {return;}

        // account for zero-indexing by adjusting value last
        hearts[currentHealth].GetComponent<Health_Heart>().Fill();
        currentHealth++;
    }
}
