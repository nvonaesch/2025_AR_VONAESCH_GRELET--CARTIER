using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitcher : MonoBehaviour
{
    [SerializeField] private Button targetButton; // Le bouton à activer

    public void SwitchButtons(Button clickedButton)
    {
        // Désactiver le bouton cliqué (visuellement et fonctionnellement)
        clickedButton.gameObject.SetActive(false);

        // Activer le bouton cible
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Aucun bouton cible assigné !");
        }
    }
}
