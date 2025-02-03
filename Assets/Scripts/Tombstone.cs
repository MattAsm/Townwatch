using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Tombstone : MonoBehaviour
{
    public List<string> occupation = new List<string>();
    public List<string> yourName = new List<string>();
    public TMP_Text tombText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        occupation.Add("");

        tombText.text = ($"Here lies {yourName[Random.Range(0, yourName.Count - 1)]} the town {occupation[Random.Range(0, yourName.Count - 1)]}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
