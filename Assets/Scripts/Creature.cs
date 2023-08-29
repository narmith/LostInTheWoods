using UnityEngine;

abstract public class Creature : MonoBehaviour
{
    public GameObject creatureTarget;
    protected CharacterController creatureController;
    protected Animator creatureAnims;
    protected FPSShooter shooter;

    protected float actionCooldown = 0;
    protected bool canShoot = false;
    protected bool canMove = false;
    protected Vector3 direction = Vector3.zero;

    public bool isGrounded;
    public float movSpeed = 4f;
    public float jumpHeight = 10f;
    public AudioSource shootFx;
    public AudioSource walkFx;

    // Loot when killed
    public int xpGiven = 10;
    public float moneyDropRatio = 0.2f;

    protected virtual void Start()
    {
        if (creatureController = this.gameObject.GetComponent<CharacterController>()) { canMove = true; }
        creatureAnims = this.gameObject.GetComponent<Animator>();
        if (shooter = this.gameObject.GetComponent<FPSShooter>()) { canShoot = true; }
    }

    virtual public void Update()
    {
        isGrounded = creatureController.isGrounded;
        if (actionCooldown >= 0) { actionCooldown -= Time.deltaTime; }
        GetActions();
    }

    virtual protected void GetActions()
    {
        if (direction == Vector3.zero)
        {
            if (walkFx.isPlaying) { walkFx.Stop(); }
            ResetAnim();
        }

        // Creature applied movement + gravity
        direction += Physics.gravity;
        creatureController.Move(direction * movSpeed * Time.deltaTime);
        direction = Vector3.zero;

        isGrounded = IsGrounded();
    }

    virtual protected bool HasActionCooldown()
    {
        if (actionCooldown > 0)
        {
            return true;
        }
        return false;
    }

    virtual protected void StartAnim(string animState)
    {
        ResetAnim();
        creatureAnims.SetBool(animState, true); // Apply animation
    }

    virtual protected void ResetAnim()
    {
        if (!isGrounded && !creatureAnims.GetBool("IsFalling")) { creatureAnims.SetBool("IsFalling", true); }
        else { creatureAnims.SetBool("IsFalling", false); }

        creatureAnims.SetBool("IsJumping", false);
        creatureAnims.SetBool("IsRunning", false);
        creatureAnims.SetBool("RStrafe", false);
        creatureAnims.SetBool("LStrafe", false);
        creatureAnims.SetBool("Shoot_Melee", false);
    }

    virtual protected bool IsGrounded()
    {
        return creatureController.isGrounded;
        //return Physics.Raycast(transform.position, Vector3.down, 1f, 1 << LayerMask.NameToLayer("Ground"));
    }

    virtual public void AggroTarget(GameObject newTarget)
    {
        creatureTarget = newTarget;
    }

    void ReceiveDamage(Damage dmg)
    {
        
    }

    protected virtual void Death()
    {
        Destroy(this.gameObject);
    }
}
