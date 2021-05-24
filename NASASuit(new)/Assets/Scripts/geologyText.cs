using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class geologyText : MonoBehaviour
{
    // determines which instruction set is shown
    int caseSwitch = 0;
    int currentPage = 0;

    // holds the text for the instruction
    public TextMeshProUGUI geo_text;

    public void Start()
    {
        SwitchMenu(1);
    }

    public void SwitchMenu(int caseSwitch)
    {
        // Each case is a page of text from the geology instruction
        switch (caseSwitch)
        {
            case 1:
                //Add your text with the line below, Do this for every case needed.
                geo_text.text = "1. Review geology mission materials, quantities being sampled, and collect tools.\n" + "Sample Bags: 1 bag minimum per unique sample (location and type)\n" + "For regolith (soil) sampling: select: Surface Sampling Pad and Small Scoop\n" + "For pebble (diameter > 1 cm) collection: select Rake\n" + "For rock (diameter < 10cm) collection: select Tongs\n";
                currentPage = 1;
                break;
            case 2:
                geo_text.text = "2.Minimize Geology Sampling App and Navigate to Geology worksite\n";
                currentPage = 2;
                break;
            case 3:
                geo_text.text = "3.Start Audio Recording and Document Pre - Sampling Information:\n" + "Geology Mission Identifier, Mission Location, Date & Time\n" + "Team Members Involved & Roles\n" + "Geology Tools being utilized.\n" + "Say \"Start recording.\" to begin the video and \"Stop recording.\" to stop.\n";
                currentPage = 3;
                break;
            case 4:
                geo_text.text = "4.Continue Video Recording and Pre-Sampling Documentation.\n" + "Note Telemetry Stream Readings: Temperature, Atmosphere, Visibility, and Attitude\n" + "Describe distinguishing features of worksite and capture video.\n" + "Say \"Start recording.\" to begin the video and \"Stop recording.\" to stop.\n";
                currentPage = 4;
                break;
            case 5:
                geo_text.text = "5.For Pebble and Rock Geology Samples, video document:\n" + "Describe sample shape, approximate metric size, and environmental surface features.\n" + "Document sample color, texture, finish and mineral description.\n" + "Describe sample durability and density.\n" + "Stow sample in bag and load in rover.\n" + "Repeat this step for all pebble and rock collections.\n" + "Say \"Start recording.\" to begin the video and \"Stop recording.\" to stop.\n";
                currentPage = 5;
                break;
            case 6:
                geo_text.text = "6.For Regolith(Soil) Samples, video document:\n" + "Describe environmental surface features.\n" + "Document sample color, texture, finish and mineral description.\n" + "Open Surface Sampling Pad, ensure cloth sampler is installed, press cloth into soil, remove and\n" + "close Surface Sampling Pad.\n" + "Scoop and stow sample in bag.\n" + "Load sample bag and surface sampling pad in rover.\n" + "Repeat this step for all regolith (soil) collections.\n" + "Say \"Start recording.\" to begin the video and \"Stop recording.\" to stop.\n";
                currentPage = 6;
                break;
            case 7:
                geo_text.text = "7. Once Mission Complete: Video document completion time, clean and store tools, close app\n" + "and navigate back to lunar base.\n";
                currentPage = 7;
                break;
            default:
                break;

        }
    }

    public void NextPage()
    {
        currentPage += 1;
        SwitchMenu(currentPage);

    }

    public void PrevPage()
    {
        currentPage -= 1;
        SwitchMenu(currentPage);

    }
}
