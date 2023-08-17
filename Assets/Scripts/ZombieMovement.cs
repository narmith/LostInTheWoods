using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    private GameObject enemyTarget;
    private CharacterController enemyCC;
    private Animator enemyAnim;
    public AudioSource audioShoot;
    public AudioSource audioWalk;
    private FPSShooter shooter;

    //public bool enemyFollows = true;
    public bool canMove = true;
    public float movSpeed = 4f;
    public float jumpHeight = 10f;
    public bool isGrounded;
    public float distToGround = 0.13f;

    void Start()
    {
        enemyCC = GetComponent<CharacterController>();
        enemyAnim = GetComponent<Animator>();
        shooter = GetComponent<FPSShooter>();
    }

    void FixedUpdate() { }

    void LateUpdate()
    {
        if (!OnGround(this.gameObject))
        {
            isGrounded = false;
            //enemyAnim.SetBool("Falling", true);
            enemyCC.Move(Physics.gravity * Time.deltaTime);
        }
        else isGrounded = true;

        if (enemyTarget && enemyTarget!=this) {
            this.transform.LookAt(enemyTarget.transform);
            if (canMove)
            {
                if (Vector3.Distance(this.transform.position, enemyTarget.transform.position) >= 1.5f)
                {
                    enemyCC.Move(this.transform.forward * movSpeed * Time.deltaTime);
                }
            }
            if (Vector3.Distance(this.transform.position, enemyTarget.transform.position) < 1.5f)
            {
                //StartAnim("Shoot_Melee");
                if (shooter.MeleeHit())
                {
                    audioShoot.Play();
                }
            }
        }
    }

    private bool OnGround(GameObject obj)
    {
        if (Physics.Raycast(obj.transform.position, Vector3.down, distToGround))
        {
            return true;
        }
        return false;
    }

    public void AggroTarget(GameObject newTarget)
    {
        if (newTarget)
        {
            enemyTarget = newTarget;
        }
    }
}
