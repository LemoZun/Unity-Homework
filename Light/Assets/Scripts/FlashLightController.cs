using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] GameObject flashLight;
    [SerializeField] AudioClip switchSound;
    bool lightMode;
    void Start()
    {
        lightMode = false;
        flashLight.SetActive(false);
    }

    private void Update()
    {
        FlashControl();
    }

    private void FlashControl()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            lightMode = !lightMode;
            flashLight.SetActive(lightMode);
            SoundManager.Instance.PlaySwitchSound(switchSound);
        }
    }


}
//private void TurnOnLight()
//{
//    if (Input.GetKeyDown(KeyCode.F) && lightMode == false)
//    {
//        flashLight.SetActive(true);
//        lightMode = true;
//    }
//}

//private void TurnOffLight()
//{
//    if (Input.GetKeyDown(KeyCode.F) && lightMode == true)
//    {
//        flashLight.SetActive(false);
//        lightMode = false;
//    }
//}