using System.Collections;
using UnityEngine;
using System;

public class Pistol : MonoBehaviour
{
    [Header("Animation Hashes")]
    private static readonly int IdleHash = Animator.StringToHash("PistolIdle");
    private static readonly int FireHash = Animator.StringToHash("PistolFire");
    private static readonly int ReloadHash = Animator.StringToHash("PistolReload");
    private static readonly int PistolWalkHash = Animator.StringToHash("PistolWalkSway");

    [Header("References")]
    EnemyAwareness enemyAwareness;
    public Animator pistolRightAnimator;
    Camera mainCamera;
    UIManager uIManager;
    PlayerMove playerMove;

    [Header("LayerMasks")]
    public LayerMask whatIsEnemy;
    public LayerMask whatIsWall;

    [Header("Weapon Stats")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float fireRate = 1.7f;

    [Header("Magazine")]
    [SerializeField] private int maxMagCapacity=6;
    [SerializeField] private int maxAmmoCapacity=48;
    public int currentMag, currentAmmo; 

    [Header("Bools")]
    [SerializeField] private bool firing;
    [SerializeField] private bool reloading;
    [SerializeField] public bool allowedToFire=true;
    [SerializeField] private bool walking;
    [HideInInspector] public bool isPaused;
    
    [Header("Animations")]
    int _currentHash;
    float _lockedTill;


    public static event Action OnFire;
    public static event Action OnPickupAmmo;
    public static event Action OnReload;

    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        mainCamera = FindObjectOfType<Camera>();
        playerMove = FindObjectOfType<PlayerMove>();

        currentMag = maxMagCapacity;
        currentAmmo = maxAmmoCapacity / 2;
    }

    
    void Update()
    {
        if(!isPaused)
        {
            HandleFiring();

            HandleWalks();
            HandleAnims();

            HandleMagazine();
        }
    }

    void HandleWalks()
    {
        if(playerMove.characterController.velocity.magnitude > 1)
        {
            walking = true;
        }
        else walking = false;
    }

    

    void HandleMagazine()
    {
        if(currentAmmo > 0) 
        {
            if(currentMag == 0 || Input.GetKeyDown(KeyCode.R) && currentMag < maxMagCapacity && allowedToFire && !firing)
            {
                Reload();
            }
        }
    }

    

    void Reload()
    {
        OnReload?.Invoke();
        allowedToFire = false;
        reloading = true;
        FindObjectOfType<AudioManager>().PlayClip("PistolReload");
        
        int reloadAmount = maxMagCapacity - currentMag; //how many to refill clip
        reloadAmount = (currentAmmo - reloadAmount) >= 0 ? reloadAmount : currentAmmo;
        
        currentMag += reloadAmount;
        currentAmmo -= reloadAmount;

        StartCoroutine(DelayReload());
    }

    private IEnumerator DelayReload()
    {
        yield return new WaitForSeconds(1.2f);
        
        reloading = false;
        allowedToFire = true;
    }

    void HandleAnims()
    {
        var state = GetState();

        if(state == _currentHash) return;
        pistolRightAnimator.CrossFade(state, 0, 0);
        _currentHash = state;
        
    }

    private int GetState()
    {
        if(Time.time < _lockedTill) return _currentHash;
        
        if(reloading) return ReloadHash;

        if(firing) return FireHash;

        if(walking) return PistolWalkHash;


        return IdleHash;
    }

    private void HandleFiring()
    {
        if(Input.GetMouseButtonDown(0) && allowedToFire && currentMag > 0 && !reloading)
        {
            StartCoroutine(Fire());
        }

        if(Input.GetMouseButtonDown(0) && allowedToFire && currentAmmo == 0 && !reloading)
        { //gun is empty
            StartCoroutine(EmptyGunClick());
        }
    }

    private IEnumerator Fire()
    {
        allowedToFire = false;
        currentMag--;
        firing = true;
        
        OnFire?.Invoke();

        FindObjectOfType<AudioManager>().PlayClip("PistolShot");

        Vector3 halfBoxSize = new Vector3(.7f, .75f, 20f);
        float playerHeightOffset = .8f;
        Collider[] colliderArray = Physics.OverlapBox(transform.position + transform.up * playerHeightOffset + transform.forward * halfBoxSize.z, halfBoxSize, transform.rotation);
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        foreach (Collider collider in colliderArray) {
        Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Physics.Raycast(ray, out RaycastHit hit, whatIsWall);
                if(hit.collider.tag != "Wall")
                {
                    if(enemy.enemyHealth > 0)
                        enemy.TakeDamage(damage);
                }
            }
        }

        yield return new WaitForSeconds(fireRate-.1f);
        firing = false;
        yield return new WaitForSeconds(.1f);
        allowedToFire = true;
        
    }

    public void GiveAmmo(int amount, GameObject pickup)
    {
        if(currentAmmo < maxAmmoCapacity)
        {
            OnPickupAmmo?.Invoke();
            currentAmmo += amount;
            FindObjectOfType<AudioManager>().PlayClip("PickupAmmo");
            Destroy(pickup);
        }

        if(currentAmmo > maxAmmoCapacity)
            currentAmmo = maxAmmoCapacity;
    }

    private IEnumerator EmptyGunClick()
    {
        allowedToFire = false;
        FindObjectOfType<AudioManager>().PlayClip("PistolEmptyClick");
        yield return new WaitForSeconds(.5f);
        allowedToFire = true;
    }
}
