using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    FPSShooter shooter;
    public float movSpeed = 4;
    private float x;
    private float z;
    //private float gravity = -10f;
    //public float fallVelocity;
    CharacterController EnemyCC;
    public Transform EnemyBody;
    private Vector3 move;
    private Animator anim;

    //private bool isFiring;
    public bool enemyFollows = true;
    public GameObject enemyTarget;

    void Start()
    {
        EnemyCC = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        shooter = GetComponent<FPSShooter>();
        //isFiring = false;
    }

    /*
    void FixedUpdate()
    {
        //GRAVITY CHECK
        if (!EnemyCC.isGrounded)
        {
            fallVelocity += gravity * Time.deltaTime;
            EnemyCC.Move(new Vector3(0, fallVelocity * Time.deltaTime, 0));

            if (EnemyCC.isGrounded)
            {
                fallVelocity = 0;
            }
        }
    }
    */

    void ResetAnimAttackMelee()
    {
        anim.SetBool("Shoot_Melee", false);
        shooter.MeleeHit();
    }

    void Update()
    {
        if (enemyFollows)
        {
            if (Vector3.Distance(transform.position, enemyTarget.transform.position) >= 2)
            {
                move = transform.right * 1 + transform.forward * 1;
                EnemyCC.Move(move * movSpeed * Time.deltaTime);

                anim.SetFloat("VelX", 1);
                anim.SetFloat("VelZ", 1);
            }
            else
            {
                anim.SetFloat("VelX", 0);
                anim.SetFloat("VelZ", 0);

                //Fire
                if (!anim.GetBool("Shoot_Melee"))
                {
                    anim.SetBool("Shoot_Melee", true);
                    Invoke("ResetAnimAttackMelee", 3.2f);
                    print(this.gameObject.name + " attacked.");
                }
            }
        }

        this.transform.LookAt(enemyTarget.transform);
    }
}
