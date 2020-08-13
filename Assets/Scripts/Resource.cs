using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
    public string testMessage;

    public void Load()
    {
        Debug.Log(testMessage);
    }
}
