using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SerializeDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> m_keys = new List<TKey>();
    [SerializeField] private List<TValue> m_values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        m_keys.Clear();
        m_values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            m_keys.Add(pair.Key);
            m_values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if(m_keys.Count != m_values.Count)
            throw new Exception($"There are {m_keys.Count} keys and {m_values.Count} values after deserialization. Make sure that both key and value types are serializable.");
        
        for(int i = 0; i < m_keys.Count; i++)
        {
            this.Add(m_keys[i], m_values[i]);
        }
}

}
