using UnityEngine;
using TMPro;

public class ModularProjectileGun : MonoBehaviour
{
    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //gun stats
    public float fireRate, spread, reloadTime, burstShotTime;
    public int magazineSize, bulletsPerTap;
    public bool allowHoldToFire;
    int bulletsLeft, bulletsShot;



    //bools
    bool shooting, readyToShoot, reloading;

    //references
    public Camera mainCamera;
    public Transform attackPoint;

    //bug fixing
    public bool allowInvoke = true;
    
    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        //mag is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        HandleInput();

        if(ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }

    }

    private void HandleInput()
    {
        if(allowHoldToFire) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) 
        Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }


    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //ray to middle screen
        RaycastHit hit;

        //check if hit
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //far from player

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position; //dist from attackPoint to targetPoint

        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculates new direction with spread.
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, 0, 0);//adds spread to last dir

        //instantiate bullet/proj
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //rotates bullet to shoot forward
        currentBullet.transform.forward = directionWithSpread.normalized;

        //add forces to bullets
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(mainCamera.transform.up * upwardForce, ForceMode.Impulse); //upward force, for arcing bullets

        //muzzle flash
        if(muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;
        
        if(!IsInvoking("ResetShot") && !readyToShoot)
        {
            Invoke("ResetShot", fireRate);
        }

        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
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
