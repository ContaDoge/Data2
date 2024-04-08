using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class DataManager : MonoBehaviour
{
    private string _dataPath;
    private string _xmlAccelerationData;
    private float _timer = 0.0f;
    private bool DoDataThingy = true;

    public List<DataSnapshot> data = new List<DataSnapshot>();

    private void Awake()
    {
        _dataPath = Application.persistentDataPath + "/Acceleration_Data/";
        _xmlAccelerationData = _dataPath + "AccelerationData.xml";
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_dataPath);
        NewDirectory();
        Debug.Log(Application.persistentDataPath);

    }
    public void NewDirectory()
    {
        if (File.Exists(_dataPath))
        {
            Debug.Log("Directory exists");
            return;
        }
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New Directory created");
    }

    private void FixedUpdate()
    {
        _timer += 1;

        if (DoDataThingy && _timer % 5 == 0)
        {
            Debug.Log($"{_timer} updates: Saving data points...");
            AddAccelerationData();
        }
        if (_timer > 500 && DoDataThingy)
        {
            DoDataThingy = false;
            StopAndSerialize();
        }
    }

    private void AddAccelerationData()
    {
        data.Add(new DataSnapshot());
    }

    public void StopAndSerialize()
    {
        Debug.Log($"Stop initiated at {_timer}, attempting to serialize...");

        var xmlSerializer = new XmlSerializer(typeof(List<DataSnapshot>));

        using(FileStream stream = File.Create(_xmlAccelerationData))
        {
            xmlSerializer.Serialize(stream, data);
        }
        Debug.Log("Serialization complete.");
    }
}
