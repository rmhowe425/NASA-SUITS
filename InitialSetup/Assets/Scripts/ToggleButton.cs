using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    // Toggle function
    public GameObject TheButton;

    public void togglePanel()
    {
        if (TheButton != null)
        {
            bool isActive = TheButton.activeSelf;

            TheButton.SetActive(!isActive);
        }
    }
}
