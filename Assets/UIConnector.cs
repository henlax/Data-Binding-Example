using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class UIConnector : MonoBehaviour
{
    public Dictionary<string, StringVar> StringVars = new Dictionary<string, StringVar>();
    public Dictionary<string, GenericList> ListVars = new Dictionary<string, GenericList>();

    public ListVar<T> GetListVar<T>(string listName) {
        if (string.IsNullOrEmpty(listName)) return null;

        var listvar = GetListVar(listName);
        if (listvar.GetType() == typeof(EmptyListVar))
        {
            var emptylistVar = (EmptyListVar)listvar;
            var newListvar = new ListVar<T>();

            if (emptylistVar.registered) newListvar.SetUpdateList(emptylistVar.listObject, emptylistVar.parent);
            ListVars.Remove(listName);
            ListVars.Add(listName, newListvar);
            return newListvar;
        }
        if (listvar.GetType() == typeof(ListVar<T>)) return (ListVar<T>)listvar;
        return null;

    }
    public GenericList GetListVar(string listName)
    {
        if (string.IsNullOrEmpty(listName)) return null;
        if (!ListVars.ContainsKey(listName))
        {
            var listvar = new EmptyListVar();
            ListVars.Add(listName, listvar);
            return listvar;
        }
        return ListVars[listName];
    }
    
    public StringVar GetStringVar(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        if (!StringVars.ContainsKey(name)){
            var stringVar = new StringVar();
            StringVars.Add(name, stringVar);
            return stringVar;
        }

        return StringVars[name];
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

    public class ListVar<T> : GenericList
    {
        public List<T> Objects = new List<T>();
        public UnityEventList<T> OnChange = new UnityEventList<T>();
        public List<UIConnector> connectors = new List<UIConnector>();

        public GameObject listObject;
        public Transform parent;

        public void Add(T newObject) {
            Objects.Add(newObject);
            OnChange.Invoke(Objects);
        }
        
        public T Get(int index)
        {
            if (Objects.Count <= index) return default;
            return Objects[index];
        }

        public void RemoveAt(int index)
        {
            Objects.RemoveAt(index);
            OnChange.Invoke(Objects);
        }

        public int Count() => Objects.Count;

        public override void SetUpdateList(GameObject listObject, Transform parent)
        {
            this.listObject = listObject;
            this.parent = parent;
            UpdateList(Objects);
            OnChange.AddListener(UpdateList);
        }

        private void UpdateList(List<T> list)
        {
            while (list.Count > connectors.Count)
            {
                var instantiatedListObject = Instantiate(listObject, parent);
                var uIConnector = instantiatedListObject.GetComponent<UIConnector>();
                connectors.Add(uIConnector);
            }

            for (int i = 0; i < list.Count; i++)
            {
                Type objType = list[i].GetType();
                FieldInfo[] info = objType.GetFields();
                for (int propertyIndex = 0; propertyIndex < info.Length; propertyIndex++)
                {
                    if (info[propertyIndex].FieldType == typeof(string))
                    {
                        connectors[i].GetStringVar(info[propertyIndex].Name).Set((string)info[propertyIndex].GetValue(list[i]));
                    }
                }
            }

            if (list.Count >= connectors.Count) return;
            for (int i = list.Count; i < connectors.Count; i++)
            {
                var uiConnector = connectors[i];
                connectors.Remove(connectors[i]);
                Destroy(uiConnector.gameObject);
            }
        }
    }

    public class EmptyListVar : GenericList
    {
        public bool registered;
        public GameObject listObject;
        public Transform parent;
        public override void SetUpdateList(GameObject listObject, Transform parent)
        {
            registered = true;
            this.listObject = listObject;
            this.parent = parent;
        }
    }

    public abstract class GenericList
    {
        public abstract void SetUpdateList(GameObject listObject, Transform parent);
    }
    public class UnityEventString : UnityEvent<string> {}
    public class UnityEventList<T> : UnityEvent<List<T>> {}

    public static bool Validate(UIConnector UIConnector, string varName)
    {
        if (UIConnector == null)
        {
            Debug.LogError("Need a UIConnector");
            return false;
        }

        if (string.IsNullOrEmpty(varName))
        {
            Debug.LogError("Needvar name can't be empty");
            return false;
        }
        return true;
    }
}

