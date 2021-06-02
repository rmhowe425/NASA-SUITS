/**
 * 2/25/2010
 * Suit Vital
 * Guanwen C
 * Richard H
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;


public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    //public Text btext;
    public GameObject Vpan;
    public GameObject Upan;
    public GameObject Dpan;
    //public GameObject ban;
    //public GameObject sla;
    //public GameObject emergency_sla;
    //public GameObject blk;
    //public GameObject emergency_s;
    //green
    //public Material material0;
    //red
    //public Material material1;
    //yellow
    //public Material material2;

    //public Text counttext ;

    void Start()
    {

        //blk.GetComponent<MeshRenderer>().material = material0;
        //emergency_s.GetComponent<MeshRenderer>().material = material1;

    }

   
    // Update is called once per frame
    void Update()
    {
        //btext.text = "update";
        
        //StartCoroutine(GetRequest("http://45.37.165.34:3000/api/simulation/state"));
    }

    // Update is called once per frame
    public void Dostuff()
    {
        //btext.text = "do stuff";
    }
    public void OpenS()
    {
        /**if (sla.activeSelf)
        {
            btext.text = "Stream";
            sla.SetActive(false);
        }
        else
        {
            btext.text = "Stream X";
            sla.SetActive(true);
        }**/
    }

    public void OpenVPan()
    {
        if (Vpan.activeSelf)
        {
            //btext.text = "Stream";
            
            Vpan.SetActive(false);
        }
        else
        {
            //btext.text = "Stream X";
            Vpan.SetActive(true);
        }
    }

    public void OpenUPan()
    {
        if (Upan.activeSelf)
        {
            //btext.text = "Stream";

            Upan.SetActive(false);
        }
        else
        {
            //btext.text = "Stream X";
            Upan.SetActive(true);
        }
    }

    public void OpenDPan()
    {
        if (Dpan.activeSelf)
        {
            //btext.text = "Stream";

            Dpan.SetActive(false);
        }
        else
        {
            //btext.text = "Stream X";
            Dpan.SetActive(true);
        }
    }


    public void color_change()
    {
        // ren = sla.GetComponent.<Renderer>();
        //sla.GetComponent.< Renderer > () = null;
       //blk.GetComponent<MeshRenderer>().material = material1;
       //btext.text = "color changed";
    }

    public void voice_change()
    {
        // ren = sla.GetComponent.<Renderer>();
        //sla.GetComponent.< Renderer > () = null;
        //blk.GetComponent<MeshRenderer>().material = material2;
        //btext.text = "voice on Ccolor!!!!!";
    }

    public void warning()
    {

        //emergency_sla.GetComponent<MeshRenderer>().material = material2;
        //btext.text = "warning!!!!!";
    }

    public void emergency()
    {

        //emergency_sla.GetComponent<MeshRenderer>().material = material1;
        //btext.text = "emergency!!!!!";
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
                //counttext.text = webRequest.error;
            }
            else
            {

                
                if (webRequest.downloadHandler.text == null)
                {

                    Debug.Log("No connection");
                }
                //info = JsonUtility.FromJson<Teleinfo>(webRequest.downloadHandler.text);

                else if (webRequest.downloadHandler != null)
                {

                    try
                    {
                        //counttext.text = webRequest.downloadHandler.text;
                        string response = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    }
                    catch 
                    {
                        //  Block of code to handle errors
                    }


                    
                }




            }
        }
    }


}
