using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openGeology : MonoBehaviour
{
    public GameObject Geo_Panel;

    bool open = false;

    public void open_Geo_Panel(){
        if (open == false){
            Geo_Panel.SetActive(true);
            open = true;
        }
        else {
            Geo_Panel.SetActive(false);
            open = false;
        }
    }
}
