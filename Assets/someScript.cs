using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class someScript : MonoBehaviour
{
    public UIConnector UIConnector;

    private void Update()
    {
        UIConnector.GetStringVar("timer").Set(Time.time.ToString());
    }
}
