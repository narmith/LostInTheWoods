using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform playerCam;
    private CharacterController charController;
    private Animator charAnims;
    public AudioSource audioShoot;
    public AudioSource audioWalk;
    private FPSShooter shooter;

    private bool canMove = true;
    private float moveSpeed = 4f;
    private float jumpHeight = 10f;
    private bool isGrounded;
    public float distToGround = 1f;

    private float actionCooldown = 0;

    private float mouseX;
    private float mouseY;
    private float mouseSentitivity = 600f;
    private float xRotation = 0f;
    
    void Start()
    {
        charController = this.gameObject.GetComponent<CharacterController>();
        charAnims = this.gameObject.GetComponent<Animator>();
        shooter = this.gameObject.GetComponent<FPSShooter>();

        Cursor.lockState=CursorLockMode.Locked;
        playerCam = this.gameObject.GetComponentInChildren<Camera>().transform;
        playerCam.LookAt(this.gameObject.transform);

        // starting variables
        if (StaticGlobals.GodMode) { this.gameObject.GetComponent<Health>().godMode = true; }
    }

    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            CanMove = false;
            SceneManager.LoadScene("MainMenu");
        }
        */

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
        if (actionCooldown>=0) {actionCooldown -= Time.deltaTime; }
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
        charAnims.SetBool("Shoot_Arrow", false);
        charAnims.SetBool("Shoot_Melee", false);
        charAnims.SetBool("Shoot_Rock", false);
    }

    private bool OnGround(GameObject obj)
    {
        if (Physics.Raycast(obj.transform.position, Vector3.down, distToGround))
        {
            return true;
        }
        return false;
    }

    private void getMovement()
    {
        Vector3 direction = Vector3.zero;

        if (!hasActionCooldown())
        {
            // Movement keys
            if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    StartAnim("IsRunning");
                    if (isGrounded && !audioWalk.isPlaying) { audioWalk.Play(); } // Footsteps sound
                    direction += this.transform.forward;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    StartAnim("IsRunning");
                    if (isGrounded && !audioWalk.isPlaying) { audioWalk.Play(); } // Footsteps sound
                    direction -= this.transform.forward;
                }
            }
            if (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    StartAnim("LStrafe");
                    if (isGrounded && !audioWalk.isPlaying) { audioWalk.Play(); } // Footsteps sound
                    direction -= this.transform.right;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    StartAnim("RStrafe");
                    if (isGrounded && !audioWalk.isPlaying) { audioWalk.Play(); } // Footsteps sound
                    direction += this.transform.right;
                }
            }

            direction += hasJumped(direction);
            charController.Move(direction * moveSpeed * Time.deltaTime);
        }

        if (direction == Vector3.zero)
        {
            if (audioWalk.isPlaying) { audioWalk.Stop(); }
            ResetAnim();
        }

        // MOUSE VIEW
        mouseX = Input.GetAxis("Mouse X") * mouseSentitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSentitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(-mouseY, -90f, 90f);

        playerCam.transform.Rotate(Vector3.right * xRotation);
        this.transform.Rotate(Vector3.up * mouseX);

        isShooting();
    }

    private bool hasActionCooldown()
    {
        if (actionCooldown > 0)
        {
            return true;
        }
        return false;
    }

    private Vector3 hasJumped(Vector3 currentDir)
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            if (audioWalk.isPlaying) { audioWalk.Stop(); }
            actionCooldown = 2f;
            StartAnim("IsJumping");
            return currentDir += Physics.gravity * -jumpHeight;
        }
        else return Vector3.zero;
    }

    private bool isShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (shooter.ShootArrow())
            {
                actionCooldown = 4f;
                StartAnim("Shoot_Arrow");
                audioShoot.Play();
            }
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            actionCooldown = 2f;
            StartAnim("Shoot_Melee");
            if (shooter.MeleeHit())
            {
                audioShoot.Play();
            }
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (shooter.ShootRock())
            {
                actionCooldown = 2f;
                StartAnim("Shoot_Rock");
                audioShoot.Play();
            }
            return true;
        }
        return false;
    }

}
