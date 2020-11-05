using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public UIConnector UIConnector;
    public Text Text;
    public string varName;
    
    private void Start()
    {
        UIConnector.GetStringVar(varName).OnChange.AddListener(value => Text.text = value);
    }
}
