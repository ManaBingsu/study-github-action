using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayVersion : MonoBehaviour
{
    Text textObj;
    // Start is called before the first frame update
    private void Awake()
    {
        textObj = GetComponent<Text>();
    }
    void Start()
    {
        textObj.text = Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
