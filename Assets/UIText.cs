using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public UIConnector UIConnector;
    public Text Text;
    public string varName;
    
    private void Awake()
    {
        if (!UIConnector.Validate(UIConnector, varName)) return;

        var stringVar= UIConnector.GetStringVar(varName);
        Text.text = stringVar.Get();
        stringVar.OnChange.AddListener(value => Text.text = value);
    }
}
