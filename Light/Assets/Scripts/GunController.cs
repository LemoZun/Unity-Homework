using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] AudioClip shotSound;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Transform muzzlePoint;
    private void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //√— πﬂªÁ
            SoundManager.Instance.PlayGunFireSound(shotSound);
            PlayFlash();
            Raycasting();
        }
    }

    private void Raycasting()
    {
        RaycastHit hit;
        if(Physics.Raycast(muzzlePoint.position,muzzlePoint.forward, out hit))
        {
            Debug.Log($"{hit.collider.name} ∏¬√„ " );

            PlayHitEffect(hit.point,hit.normal);
        }
        
        

    }

    private void PlayHitEffect(Vector3 hitPosition, Vector3 hitNormal)
    {
        ParticleSystem effect = Instantiate(muzzleFlash, hitPosition, Quaternion.LookRotation(hitNormal));
        //ParticleSystem effect =  Instantiate(muzzleFlash, hitPosition, Quaternion.identity);

        effect.Play();

        Destroy(effect.gameObject, effect.main.duration); 
    }


    private void PlayFlash()
    {
        if(muzzleFlash != null)
        {
            
            Debug.Log("¿Ã∆Â∆Æ ¡ÿ∫Ò");
            
            muzzleFlash.Play();
            Debug.Log("¿Ã∆Â∆Æ πﬂªÁ");
            muzzleFlash.Stop(); 
        }
    }




}
