using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image heartPrefab;         // Drag your heart sprite (Image) prefab here.
    [SerializeField] private Transform heartContainer;  // Drag your container (with GridLayoutGroup) here.
    [SerializeField] private int maxHearts;

    private Image[] heartImages;

    private HealthManager healthManager;

    private void Start()
    {
        healthManager = GetComponent<HealthManager>();

        heartImages = new Image[maxHearts];

        for (var i = 0; i < maxHearts; i++)
        {
            heartImages[i] = Instantiate(heartPrefab, heartContainer);
        }

        UpdateHealthBar();

        healthManager.Damaged += (sender, args) =>
        {
            UpdateHealthBar();
        };
    }

    private void UpdateHealthBar()
    {
        float normalizedHealth = healthManager.GetHealthNormalized();
        int fullHearts = (int)(normalizedHealth * maxHearts);
        float partialFill = maxHearts * normalizedHealth - fullHearts;

        for (var i = 0; i < maxHearts; i++)
        {
            if (i < fullHearts)
            {
                heartImages[i].fillAmount = 1;
            }
            else if (i == fullHearts)
            {
                heartImages[i].fillAmount = partialFill;
            }
            else
            {
                heartImages[i].fillAmount = 0;
            }
        }

    }
}
