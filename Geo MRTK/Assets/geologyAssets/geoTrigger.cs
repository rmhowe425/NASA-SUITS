using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geoTrigger : MonoBehaviour
{
    public geoInstructions instructions;

    public void triggerGeo()
    {
        FindObjectOfType<geologyManager>().startInstructions(instructions);
    }
}
