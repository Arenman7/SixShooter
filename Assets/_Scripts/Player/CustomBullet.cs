using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemy;

    //stats
    [Range(0,1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    //lifetime
    public int maxCollision;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physicsMaterial;
    PlayerHealth playerHealth;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //when to explode
        if(collisions > maxCollision) Explode(null);

        //count down lifetime
        maxLifeTime -= Time.deltaTime;
        if(maxLifeTime <= 0) Explode(null);
    }

    private void Explode(Collision collision)
    {
        //instantiate explosion
        if(explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        if(collision != null)
        {
            playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.DamagePlayer(explosionDamage);
        }

        //add delay just incase
        Invoke("DelayDestroy", 0.02f);

    }

    private void DelayDestroy()
    {
        Destroy(gameObject);
    }

    private void Setup()
    {
        //creates new physic material
        physicsMaterial = new PhysicMaterial();
        physicsMaterial.bounciness = bounciness;
        physicsMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicsMaterial.bounceCombine
         = PhysicMaterialCombine.Maximum;
        //assign mat to collider
        GetComponent<SphereCollider>().material = physicsMaterial;

        //set gravity
        rb.useGravity = useGravity; 
    }

    private void OnCollisionEnter(Collision collision)
    {   
        //count up collision
        collisions++;
        

        //explode if bullet hits enemy && explodeOnTouch
        if(collision.collider.CompareTag("Player") && explodeOnTouch) Explode(collision);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
