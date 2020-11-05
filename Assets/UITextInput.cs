using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextInput : MonoBehaviour
{

    public UIConnector UIConnector;
    public InputField TextInput;
    public string varName;

    void Start()
    {
        TextInput.onValueChanged.AddListener(value =>
        {
            UIConnector.GetStringVar(varName).Set(value);
        });
    }

}
