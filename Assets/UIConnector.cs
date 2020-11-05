using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIConnector : MonoBehaviour
{
    public Dictionary<string, StringVar> StringVars = new Dictionary<string, StringVar>();

    public StringVar GetStringVar(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        if (!StringVars.ContainsKey(name)){
            var stringVar = new StringVar();
            StringVars.Add(name, stringVar);
            return stringVar;
        }
        else return StringVars[name];
    }


    public class StringVar
    {
        private string currentValue = "";

        public UnityEventString OnChange = new UnityEventString();

        public void Set(string value)
        {
            currentValue = value;
            OnChange.Invoke(value);
        }

        public string Get() => currentValue;
      
    }

    public class UnityEventString : UnityEvent<string> {}
}
