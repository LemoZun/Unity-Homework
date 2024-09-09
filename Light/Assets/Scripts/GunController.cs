using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] AudioClip shotSound;
    [SerializeField] ParticleSystem muzzleFlash;
    private void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //�� �߻�
            SoundManager.Instance.PlayGunFireSound(shotSound);
            PlayFlash();
        }
    }

    private void PlayFlash()
    {
        if(muzzleFlash != null)
        {
            
            Debug.Log("����Ʈ �غ�");
            
            muzzleFlash.Play();
            Debug.Log("����Ʈ �߻�");
            muzzleFlash.Stop(); 
        }
    }
}
