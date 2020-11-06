using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIList : MonoBehaviour
{
    public UIConnector UIConnector;
    public string listName;
    public GameObject ListObject;

    private void Awake()
    {
        if (!UIConnector.Validate(UIConnector, listName)) return;

        if (ListObject.GetComponent<UIConnector>() == null)
        {
            Debug.LogError("list object need a UIConnector");
            return;
        }

        UIConnector.GetListVar(listName).SetUpdateList(ListObject, transform);
    }

}
