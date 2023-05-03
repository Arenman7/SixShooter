using UnityEngine;

public class ModularGun : MonoBehaviour
{
    //Gun Stats
    public int damage;
    public float fireRate, spread, range, reloadTime, burstShotTime;
    public int magazineSize, bulletsPerTap;
    public bool allowHoldToFire;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //references
    public Camera mainCamera;
    public Transform attackPoint;
    public RaycastHit raycastHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;

    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;


    }

    private void Update()
    {
        HandleInput();

    }

    private void HandleInput()
    {
        if(allowHoldToFire) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) 
        Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }

    }

    private void Shoot()
    {
        readyToShoot = false;

        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculates direction with spread.
        Vector3 direction = mainCamera.transform.forward + new Vector3(x, y, 0);

        Debug.DrawRay(mainCamera.transform.position, direction, Color.cyan, 5f, false);
        
        if(Physics.Raycast(mainCamera.transform.position, direction, out raycastHit, range, whatIsEnemy))
        {
            Debug.Log(raycastHit.collider.name);
            
            if(raycastHit.collider.CompareTag("Enemy"))
            {
                raycastHit.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        //Camera Shake
        //camShake.Shake(camShakeDuration, camShakeMagnitude);
        
        //Graphics
        Instantiate(bulletHoleGraphic, raycastHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);


        bulletsLeft--;
        bulletsShot--;
        
        if(!IsInvoking("ResetShot") && !readyToShoot)
        {
            Invoke("ResetShot", fireRate);
        }

        if(bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", burstShotTime);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }


}
