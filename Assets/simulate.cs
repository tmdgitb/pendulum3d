using UnityEngine;
using System.Collections;
using CsvHelper;
using System.IO;
using System.Collections.Generic;
using System;

public class simulate : MonoBehaviour {

    //private Dictionary<float, PendulumRes> records = new Dictionary<float, PendulumRes>();
    private List<PendulumRes> records = new List<PendulumRes>();
    private float? timeStep = null;
    private float maxTime;
    private int maxTimeInt;
    private const float speed = 10f;
    private Vector3 initialEuler;

    // Use this for initialization
    void Start () {
        initialEuler = new Vector3(270, -90, 90);
        using (var textReader = File.OpenText("Pendulum_res.csv"))
        {
            using (var csv = new CsvReader(textReader))
            {
                float? firstTime = null;
                while (csv.Read())
                {
                    //Debug.LogFormat("Reading: {0}", (object)csv.CurrentRecord);
                    var record = new PendulumRes {
                        time = float.Parse(csv.CurrentRecord[0]),
                        x1 = float.Parse(csv.CurrentRecord[1]),
                        x2 = float.Parse(csv.CurrentRecord[2]),
                        der_x1 = float.Parse(csv.CurrentRecord[3]),
                        der_x2 = float.Parse(csv.CurrentRecord[4]),
                        u = float.Parse(csv.CurrentRecord[5]),
                        y = float.Parse(csv.CurrentRecord[6]),
                    };
                    //if (!records.ContainsKey(record.time))
                    //{
                    //    records.Add(record.time, record);
                    //}
                    records.Add(record);
                    if (firstTime == null)
                    {
                        firstTime = record.time;
                    } else if (timeStep == null)
                    {
                        timeStep = record.time - firstTime;
                    }
                    maxTime = record.time;
                }
            }
        }
        maxTimeInt = records.Count - 1;
        Debug.LogFormat("Records {0}: {1}", records.Count, records);
	}
	
	// Update is called once per frame
	void Update () {
        //float simTime = Mathf.Round(Time.time * speed / timeStep.Value) * timeStep.Value;
        //if (simTime > maxTime)
        //{
        //    simTime = maxTime;
        //}
        int simTimeInt = Mathf.RoundToInt(Time.time * speed);
        if (simTimeInt > maxTimeInt)
        {
            simTimeInt = maxTimeInt;
        }
        try {
            var res = records[simTimeInt];
            //Debug.LogFormat("{0} => {1}", simTimeInt, res);
            transform.eulerAngles = initialEuler + new Vector3(res.x1 / Mathf.PI * 180, 0, 0);
            //transform.eulerAngles = new Vector3(270, 0, 0);
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("{0} => {1}", simTimeInt, "ERROR");
        }
    }
}
