﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class eyeToggle : MonoBehaviour
{
    [SerializeField] GameObject RenderPlaneRight;
    [SerializeField] GameObject RenderPlaneLeft;
    [SerializeField] GameObject BothEyes;
    [SerializeField] GameObject LeftEye;
    [SerializeField] GameObject RightEye;
    public GameObject SwitchEye;
    [Header("Combined Scene Settings Only")]
    [SerializeField] bool isCombinedScene;
    [SerializeField] CombinedSceneHandler combinedScript;
    private controllerRemap rightRemap;
    private controllerRemap leftRemap;
    public GameObject srFramework;

    private int state;

    // Start is called before the first frame update
    void Start()
    {
        state = 0;
        print("in start");
    }

    void Awake(){
        rightRemap = RenderPlaneRight.GetComponent<controllerRemap>();
        leftRemap = RenderPlaneLeft.GetComponent<controllerRemap>();
        srFramework = GameObject.Find("[SRwork_FrameWork]");
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any)){
            switch(state){
                // remap only the left eye
                case 0:
                    rightRemap.enabled = false;
                    BothEyes.SetActive(false);
                    LeftEye.SetActive(true);
                    state = 1;
                    break;
                // remap only the right eye
                case 1:
                    rightRemap.enabled = true;
                    leftRemap.enabled = false;
                    LeftEye.SetActive(false);
                    RightEye.SetActive(true);
                    state = 2;
                    break;
                // remap both eyes
                case 2:
                    rightRemap.enabled = true;
                    leftRemap.enabled = true;
                    RightEye.SetActive(false);
                    BothEyes.SetActive(true);
                    state = 0;
                    break;
            }
        }
        
        // animate UI to respond to input
        if (SteamVR_Actions._default.GrabPinch.GetState(SteamVR_Input_Sources.Any)){
            SwitchEye.SetActive(false);
        }
        else{
            SwitchEye.SetActive(true);
        }

        // Handle returning to the menu/quitting
        if (SteamVR_Actions._default.Menu.GetStateDown(SteamVR_Input_Sources.Any)){
            // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            // SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
            if (isCombinedScene){
                rightRemap.resetTransform();
                leftRemap.resetTransform();
                combinedScript.leaveRemap();
            }
            else{
                Application.Quit();
            }
        }
        // if (Input.GetKeyDown(KeyCode.L)){
        //     // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //     // srFramework.SetActive(true);
        //     // srFramework.transform.localScale = new Vector3 (1,1,1);
        // }
    }
}
