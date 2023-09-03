using UnityEngine;
using UnityEngine.PlayerLoop;

abstract public class Creature : MonoBehaviour
{
    protected CharacterController creatureController;
    protected Animator creatureAnims;
    protected FPSShooter shooter;
    protected float actionCooldown = 0;
    protected bool canShoot = false;
    protected bool canMove = false;
    protected bool canAnimate = false;
    protected Vector3 direction = Vector3.zero;

    public GameObject creatureTarget;
    public bool isGrounded;
    public float movSpeed = 4f;
    public float jumpHeight = 10f;
    public AudioSource shootFx;
    public AudioSource hitFx;
    public AudioSource hitMissFx;
    public AudioSource getHitFx;
    public AudioSource runFx;

    // Loot when killed
    public int xpGiven = 10;
    public float moneyDropRatio = 0.2f;

    virtual public void Start()
    {
        if (TryGetComponent(out creatureController)) { canMove = true; }
        if (TryGetComponent(out creatureAnims)) { canAnimate = true; }
        if (TryGetComponent(out shooter)) { canShoot = true; }
    }

    virtual public void Update()
    {
        
    }

    virtual public void FixedUpdate()
    {
        isGrounded = creatureController.isGrounded;
        if (actionCooldown > 0) { actionCooldown -= Time.deltaTime; }
        else actionCooldown = 0;
        GetActions();
    }

    virtual protected void GetActions()
    {
        if (direction == Vector3.zero)
        {
            if (runFx.isPlaying) { runFx.Stop(); }
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
        if (!canAnimate) return;

        ResetAnim(); // Reset any previous animation
        creatureAnims.SetBool(animState, true); // Apply new animation
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
    }

    virtual public void AggroTarget(GameObject newTarget)
    {
        creatureTarget = newTarget;
    }

    void ReceiveDamage(Damage dmg)
    {
        if (getHitFx && !getHitFx.isPlaying) { getHitFx.Play(); }
    }

    protected virtual void Death()
    {
        if (getHitFx && !getHitFx.isPlaying) { getHitFx.Play(); }
        Invoke(nameof(Despawn), 10f);
    }

    protected virtual void Despawn()
    {
        Destroy(this.gameObject);
    }

}
