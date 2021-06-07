//Navigation script for Hololens 2


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if !UNITY_EDITOR
using System.Threading.Tasks;
#endif

public class MyTcpClient : MonoBehaviour
{

#if !UNITY_EDITOR
    private bool _useUWP = true;
    private Windows.Networking.Sockets.StreamSocket socket;
    private Task exchangeTask;
#endif

#if UNITY_EDITOR
    private bool _useUWP = false;
    System.Net.Sockets.TcpClient client;
    System.Net.Sockets.NetworkStream stream;
    private Thread exchangeThread;
#endif


    private Byte[] bytes = new Byte[256];
    private StreamWriter writer;
    private StreamReader reader;

    //UI elements
    public Text Test_text;
    string gps_time;
    public TextMeshPro gps_speed;
    public TextMeshPro gps_alt;
    public TextMeshPro gps_lat;
    public TextMeshPro gps_lon;
    public TextMeshPro gps_head;
    public TextMeshPro front_lux;
    //public TextMeshPro aft_lux;
    string[] gps_data = null;

    public GameObject NaviPanelUI;
    public GameObject ArrowUI;
    public GameObject StopUI;

    public TextMeshProUGUI DistanceToTargetUI;
    public TextMeshProUGUI GpsSpeedUI;
    public TextMeshProUGUI WalkBackTimerUI;

    // Camera Transformation
    public Transform playerTransform;
    public Transform compassTransform;
    Vector3 direct;
    float currentHeading = 0;

    // Holds the current location of the user
    double userCurrentLat = 0;
    double userCurrentLon = 0;
    TextMeshPro TextToUpdate;
    public TextMeshProUGUI currentDestination;

    // This is the active point being tracked by the compass and distance to target
    double targetLat = 35.932145;
    double targetLon = -78.924748;

    // Change this to reflect the geology test site at the location
    double geologySiteLat = 0;
    double geologySiteLon = 0;

    // Change this to reflect the lander site at the location
    double LanderLat = 0;
    double LanderLon = 0;

    // This represents the lunar rover location (unused in this years challenge)
    double RoverLat = 0;
    double RoverLon = 0;

    public void Start()
    {
        //Server ip address and port (set the ip to the PI address)
        Connect("192.168.0.23", "9010");
    }

    public void Connect(string host, string port)
    {
        if (_useUWP)
        {
            ConnectUWP(host, port);
        }
        else
        {
            ConnectUnity(host, port);
        }
    }



#if UNITY_EDITOR
    private void ConnectUWP(string host, string port)
#else
    private async void ConnectUWP(string host, string port)
#endif
    {
#if UNITY_EDITOR
        errorStatus = "UWP TCP client used in Unity!";
#else
        try
        {
            if (exchangeTask != null) StopExchange();
        
            socket = new Windows.Networking.Sockets.StreamSocket();
            Windows.Networking.HostName serverHost = new Windows.Networking.HostName(host);
            await socket.ConnectAsync(serverHost, port);
        
            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            writer = new StreamWriter(streamOut) { AutoFlush = true };
        
            Stream streamIn = socket.InputStream.AsStreamForRead();
            reader = new StreamReader(streamIn);

            RestartExchange();
            successStatus = "Connected!";
        }
        catch (Exception e)
        {
            errorStatus = e.ToString();
        }
#endif
    }

    private void ConnectUnity(string host, string port)
    {
#if !UNITY_EDITOR
        errorStatus = "Unity TCP client used in UWP!";
#else
        try
        {
            if (exchangeThread != null) StopExchange();

            client = new System.Net.Sockets.TcpClient(host, Int32.Parse(port));
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            RestartExchange();
            successStatus = "Connected!";
        }
        catch (Exception e)
        {
            errorStatus = e.ToString();
        }
#endif
    }

    private bool exchanging = false;
    private bool exchangeStopRequested = false;
    private string lastPacket = null;

    private string errorStatus = null;
    private string warningStatus = null;
    private string successStatus = null;
    private string unknownStatus = null;

    public void RestartExchange()
    {
#if UNITY_EDITOR
        if (exchangeThread != null) StopExchange();
        exchangeStopRequested = false;
        exchangeThread = new System.Threading.Thread(ExchangePackets);
        exchangeThread.Start();
#else
        if (exchangeTask != null) StopExchange();
        exchangeStopRequested = false;
        exchangeTask = Task.Run(() => ExchangePackets());
#endif
    }

    public void Update()
    {
        // Rotates the compass arrow so that it appears 2D
        direct.y = playerTransform.eulerAngles.y;
        direct.x = playerTransform.eulerAngles.x;

        if (lastPacket != null)
        {
            gps_data = lastPacket.Split(',');

            if (gps_data[0] != gps_time)
            {

                gps_speed.text = gps_data[0]; // kilometers per hour
                gps_alt.text = gps_data[1];
                gps_lat.text = gps_data[2];
                gps_lon.text = gps_data[3];
                gps_head.text = gps_data[4];
                front_lux.text = gps_data[5];
                //aft_lux.text = gps_data[6];
                //Test_text.text = "Lux value: " + gps_data[5];

                // For the user tab on destinations
                userCurrentLat = Convert.ToDouble(gps_data[2]);
                userCurrentLon = Convert.ToDouble(gps_data[3]);

                // last value needs to be changed to adjust for heading
                currentHeading = float.Parse(gps_data[4]);
                compassTransform.transform.rotation = Quaternion.Euler(direct.x, direct.y, currentHeading);

                // Buffer to toggle UI when user reaches destination
                int bufferDistance = (int)DistanceToTarget(targetLat, targetLon, Convert.ToDouble(gps_lat.text), Convert.ToDouble(gps_lon.text));

                if (bufferDistance < 3)
                {
                    toggleNaviPanel(1);
                }
                else if (bufferDistance > 5)
                {
                    toggleNaviPanel(2);
                }

                // Head Mounted Display
                currentDestination.text = TextToUpdate.text;
                DistanceToTargetUI.text = Convert.ToString(bufferDistance);
                GpsSpeedUI.text = gps_speed.text;
                WalkBackTimerUI.text = Convert.ToString(WalkBackTimer_Calculation(Convert.ToDouble(gps_speed.text), Convert.ToDouble(DistanceToTargetUI.text)));

            }

            gps_data = null;
            
        }

        if (errorStatus != null)
        {
            Debug.Log(errorStatus);
            errorStatus = null;
        }
        if (warningStatus != null)
        {
            Debug.Log(warningStatus);
            warningStatus = null;
        }
        if (successStatus != null)
        {
            Debug.Log(successStatus);
            successStatus = null;
        }
        if (unknownStatus != null)
        {
            Debug.Log(unknownStatus);
            unknownStatus = null;
        }
    }

    public void ExchangePackets()
    {
        while (!exchangeStopRequested)
        {
            if (writer == null || reader == null) continue;
            exchanging = true;

            writer.Write("Start\n");
            //Debug.Log("Sent data!");

            string received = null;

#if UNITY_EDITOR
            byte[] bytes = new byte[client.SendBufferSize];
            int recv = 0;
            while (true)
            {
                recv = stream.Read(bytes, 0, client.SendBufferSize);
                received += Encoding.UTF8.GetString(bytes, 0, recv);
                if (received.EndsWith("\n")) break;
            }
#else
            received = reader.ReadLine();

#endif

            lastPacket = received;

            exchanging = false;
        }
    }

    private void ReportDataToTrackingManager(string data)
    {
        if (data == null)
        {
            Debug.Log("Received a frame but data was null");
            return;
        }

        var parts = data.Split(',');
        /*
        foreach (var part in parts)
        {

            ReportStringToTrackingManager(part);
        }
        */
    }

    private void ReportStringToTrackingManager(string rigidBodyString)
    {
        var parts = rigidBodyString.Split(':');
        var positionData = parts[1].Split(',');
        var rotationData = parts[2].Split(',');

        int id = Int32.Parse(parts[0]);
        float x = float.Parse(positionData[0]);
        float y = float.Parse(positionData[1]);
        float z = float.Parse(positionData[2]);
        float qx = float.Parse(rotationData[0]);
        float qy = float.Parse(rotationData[1]);
        float qz = float.Parse(rotationData[2]);
        float qw = float.Parse(rotationData[3]);

        Vector3 position = new Vector3(x, y, z);
        Quaternion rotation = new Quaternion(qx, qy, qz, qw);


    }

    public void StopExchange()
    {
        exchangeStopRequested = true;

#if UNITY_EDITOR
        if (exchangeThread != null)
        {
            exchangeThread.Abort();
            stream.Close();
            client.Close();
            writer.Close();
            reader.Close();

            stream = null;
            exchangeThread = null;
        }
#else
        if (exchangeTask != null) {
            exchangeTask.Wait();
            socket.Dispose();
            writer.Dispose();
            reader.Dispose();

            socket = null;
            exchangeTask = null;
        }
#endif
        writer = null;
        reader = null;
    }

    public void OnDestroy()
    {
        StopExchange();
    }

    public double DistanceToTarget(double TargetLat, double TargetLong, double UserLat, double UserLong)
    {
        var lat = (TargetLat - UserLat) * (Math.PI / 180);
        var lng = (TargetLong - UserLong) * (Math.PI / 180);

        var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                      Math.Cos(UserLat * (Math.PI / 180)) * Math.Cos(TargetLat * (Math.PI / 180)) *
                      Math.Sin(lng / 2) * Math.Sin(lng / 2);

        //var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
        var h2 = 2 * Math.Atan2(Math.Sqrt(h1), Math.Sqrt(1 - h1));
        return (6373 * h2) * 1000; // in meters
    }

    public double WalkBackTimer_Calculation(double speed, double distance)
    {
        double WB_Timer = speed / distance;
        return WB_Timer;
    }

    public void SwitchDestination(int DestinationChoice)
    {
        switch (DestinationChoice)
        {
            case 1:
                TextToUpdate.text = "Geology Site";
                targetLat = geologySiteLat;
                targetLon = geologySiteLon;
                break;
            case 2:
                TextToUpdate.text = "Rover Site";
                targetLat = RoverLat;
                targetLon = RoverLon;
                break;
            case 3:
                TextToUpdate.text = "Lander Site";
                targetLat = LanderLat;
                targetLon = LanderLon;
                break;
            case 4:
                TextToUpdate.text = "Current Site";
                targetLat = userCurrentLat;
                targetLon = userCurrentLon;
                break;
            default:
                break;
        }
    }

    public void toggleNaviPanel(int option)
    {

        switch (option)
        {
            case 1:
                NaviPanelUI.SetActive(false);
                ArrowUI.SetActive(false);
                StopUI.SetActive(true);
                break;
            case 2:
                NaviPanelUI.SetActive(true);
                ArrowUI.SetActive(true);
                StopUI.SetActive(false);
                break;
        }
    }
}
