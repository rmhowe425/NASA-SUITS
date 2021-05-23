using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    public GameObject DestinationMenu;
    public GameObject MarkerMenu;
    public GameObject LightingMenu;
    public GameObject Geo_Panel;
    //public GameObject SpareMenu;

    int caseSwitch = 0;


    public void SwitchMenu(int caseSwitch)
    {

        switch (caseSwitch)
        {
            case 1:
                DestinationMenu.SetActive(true);
                MarkerMenu.SetActive(false);
                LightingMenu.SetActive(false);
                Geo_Panel.SetActive(false);
                break;
            case 2:
                DestinationMenu.SetActive(false);
                MarkerMenu.SetActive(true);
                LightingMenu.SetActive(false);
                Geo_Panel.SetActive(false);
                break;
            case 3:
                DestinationMenu.SetActive(false);
                MarkerMenu.SetActive(false);
                LightingMenu.SetActive(true);
                Geo_Panel.SetActive(false);
                break;
            case 4:
                DestinationMenu.SetActive(false);
                MarkerMenu.SetActive(false);
                LightingMenu.SetActive(false);
                Geo_Panel.SetActive(true);
                break;
            case 5:
                DestinationMenu.SetActive(false);
                MarkerMenu.SetActive(false);
                LightingMenu.SetActive(false);
                Geo_Panel.SetActive(false);
                break;
            default:
                break;

        }
    }
}
