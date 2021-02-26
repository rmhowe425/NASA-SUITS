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
    public Text btext;
    public GameObject pan;
    public GameObject ban;
    public GameObject sla;
    public GameObject blk;
    public Material material1;
    public Text counttext ;

    void Start()
    {
       

    }

   
    // Update is called once per frame
    void Update()
    {
        //btext.text = "update";
        StartCoroutine(GetRequest("http://google.com"));
    }

    // Update is called once per frame
    public void Dostuff()
    {
        btext.text = "do stuff";
    }
    public void OpenS()
    {
        if (sla.activeSelf)
        {
            btext.text = "Stream";
            sla.SetActive(false);
        }
        else
        {
            btext.text = "Stream X";
            sla.SetActive(true);
        }
    }

    public void OpenB()
    {
        if (ban.activeSelf)
        {
            //btext.text = "Stream";
            
            ban.SetActive(false);
        }
        else
        {
            //btext.text = "Stream X";
            ban.SetActive(true);
        }
    }


    public void color_change()
    {
        // ren = sla.GetComponent.<Renderer>();
        //sla.GetComponent.< Renderer > () = null;
       blk.GetComponent<MeshRenderer>().material = material1;
       btext.text = "color changed";
    }

    public void voice_change()
    {
        // ren = sla.GetComponent.<Renderer>();
        //sla.GetComponent.< Renderer > () = null;
        blk.GetComponent<MeshRenderer>().material = material1;
        btext.text = "voice on Ccolor!!!!!";
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
                counttext.text = webRequest.error;
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
                        counttext.text = webRequest.downloadHandler.text;
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
