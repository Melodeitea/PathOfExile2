using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    Vector3 InitPos = new Vector3(0, 15, -10f);
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + InitPos;
        this.transform.LookAt(player.transform.position);
    }
}
