using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHelper: MonoBehaviour
{
    public TMP_InputField intext;
    private GameObject go;
    private Server server;
   
    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("keyboard");
        server = go.GetComponent<Server>();
    }

    // Update is called once per frame
    void Update()
    {
        intext.text = server.mytext;

    }
}