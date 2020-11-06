using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class someScript : MonoBehaviour
{
    public UIConnector UIConnector;
    public string ListName;

    private void Update()
    {
        var list = UIConnector.GetListVar<Name>(ListName);
        if (Input.GetKeyDown(KeyCode.A)) list.Add(GetRandomName());
        if (Input.GetKeyDown(KeyCode.R) && list.Count()>0) list.RemoveAt(UnityEngine.Random.Range(0, list.Count()-1));
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < list.Count(); i++)
            {
                Debug.Log(list.Get(i).first+" "+ list.Get(i).last);
            }
        }

        UIConnector.GetStringVar("timer").Set(Time.time.ToString());
    }

    private Name GetRandomName()
    {
        return new Name() { first = Path.GetRandomFileName().Replace(".", "").Substring(0, 8),
            last = Path.GetRandomFileName().Replace(".", "").Substring(0, 8)};
    }

    [System.Serializable]
    public class Name
    {
        public string first;
        public string last;
    }
}
