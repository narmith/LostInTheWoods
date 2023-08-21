using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    private GameObject charTarget;
    private CharacterController charController;
    private Animator charAnims;
    public AudioSource audioShoot;
    public AudioSource audioWalk;
    private FPSShooter shooter;

    private bool canMove = true;
    private float movSpeed = 4f;
    //private float jumpHeight = 10f;
    private bool isGrounded;
    private float distToGround = 1.13f;

    private float actionCooldown = 0;

    void Start()
    {
        charController = this.gameObject.GetComponent<CharacterController>();
        charAnims = this.gameObject.GetComponent<Animator>();
        shooter = this.gameObject.GetComponent<FPSShooter>();
    }

    void LateUpdate()
    {
        if (!OnGround(this.gameObject))
        {
            isGrounded = false;
            StartAnim("IsFalling");
            charController.Move(Physics.gravity * Time.deltaTime);
            //canMove = false;
        }
        else
        {
            isGrounded = true;
            //canMove = true;
        }

        if (canMove) { getMovement(); }
        actionCooldown -= Time.deltaTime;
    }

    private void getMovement()
    {
        if (charTarget && !(charTarget == this.gameObject))
        {
            this.transform.LookAt(charTarget.transform);
            if (!hasActionCooldown())
            {
                if (canMove)
                {
                    if (Vector3.Distance(this.transform.position, charTarget.transform.position) >= 1.5f)
                    {
                        StartAnim("IsRunning");
                        charController.Move(this.transform.forward * movSpeed * Time.deltaTime);
                    }
                }
                if (Vector3.Distance(this.transform.position, charTarget.transform.position) < 1.5f)
                {
                    if (shooter.MeleeHit())
                    {
                        actionCooldown = 3f;
                        StartAnim("Shoot_Melee");
                        audioShoot.Play();
                    }
                }
            }
        }
    }

    private bool hasActionCooldown()
    {
        if(actionCooldown>0)
        {
            return true;
        }
        return false;
    }

    private void StartAnim(string animState)
    {
        ResetAnim();
        charAnims.SetBool(animState, true); // Apply animation
    }

    private void ResetAnim()
    {
        if (isGrounded) { charAnims.SetBool("IsJumping", false); }
        charAnims.SetBool("IsRunning", false);
        charAnims.SetBool("RStrafe", false);
        charAnims.SetBool("LStrafe", false);
        charAnims.SetBool("IsFalling", false);
        //charAnims.SetBool("Shoot_Arrow", false);
        charAnims.SetBool("Shoot_Melee", false);
        //charAnims.SetBool("Shoot_Rock", false);
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
            charTarget = newTarget;
        }
    }
}
