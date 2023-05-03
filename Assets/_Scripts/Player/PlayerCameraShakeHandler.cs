using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerCameraShakeHandler : MonoBehaviour
{
    private void OnEnable()
    {
        Pistol.OnFire += PistolFireShake;
        PlayerHealth.PlayerHit += HurtPlayerShake;
        Pistol.OnReload += ReloadPistolShake;
        CameraLean.OnStep += FootStep;
    }

    private void OnDisable()
    {
        Pistol.OnFire -= PistolFireShake;
        PlayerHealth.PlayerHit -= HurtPlayerShake;
        Pistol.OnReload -= ReloadPistolShake;
    }

    [Header("Pistol Fire")]
    public float magnitudePistol;
    public float roughnessPistol;
    public float fadeinPistol;
    public float fadeoutPistol;

    [Header("Pistol Reload")]
    public float magnitudePistolR;
    public float roughnessPistolR;
    public float fadeinPistolR;
    public float fadeoutPistolR;

    [Header("Hurt Player Shake")]
    public float magnitudeHP;
    public float roughnessHP;
    public float fadeinHP;
    public float fadeoutHP;

    [Header("FootStep")]
    public float mFootstep;
    public float rFootstep;
    public float fiFootstep;
    public float foFootstep;

    void PistolFireShake()
    {
        CameraShaker.Instance.ShakeOnce(magnitudePistol, roughnessPistol, fadeinPistol, fadeoutPistol);
    }

    void ReloadPistolShake()
    {
        CameraShaker.Instance.ShakeOnce(magnitudePistolR, roughnessPistolR, fadeinPistolR, fadeoutPistolR);
    }

    public void FootStep()
    {
        CameraShaker.Instance.ShakeOnce(mFootstep, rFootstep, fiFootstep, foFootstep);
    }

    void HurtPlayerShake()
    {
        CameraShaker.Instance.ShakeOnce(magnitudeHP, roughnessHP, fadeinHP, fadeoutHP);
    }


}
