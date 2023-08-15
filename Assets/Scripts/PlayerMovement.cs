using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //private Transform playerBody;
    private Transform playerCam;
    private CharacterController playerCC;
    private Animator playerAnim;
    public AudioSource audioShoot;
    public AudioSource audioWalk;
    private FPSShooter shooter;

    public bool CanMove = true;
    public float MoveSpeed = 4f;
    public float jumpHeight = 10f;
    private float mouseX;
    private float mouseY;
    public float mouseSentitivity = 600f;
    private float xRotation = 0f;
    public bool isGrounded;
    public float distToGround = 0.13f;

    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
        playerCC = this.gameObject.GetComponent<CharacterController>();
        shooter = GetComponent<FPSShooter>();
        playerAnim = this.gameObject.GetComponent<Animator>();
        playerCam = this.gameObject.GetComponentInChildren<Camera>().transform;
        playerCam.LookAt(this.gameObject.transform);

        // starting variables
        if (StaticGlobals.GodMode) { this.gameObject.GetComponent<Health>().godMode = true; }
    }

    void LateUpdate()
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
            StartAnim("Falling");
            playerCC.Move(Physics.gravity * Time.deltaTime);
        }
        else isGrounded = true;

        if(CanMove) { getMovement(); }
    }

    void StartAnim(string animState)
    {
        ResetAnim();
        playerAnim.SetBool(animState, true); // Apply animation
    }

    void ResetAnim()
    {
        if (isGrounded) { playerAnim.SetBool("Jumping", false); }
        playerAnim.SetBool("Running", false);
        playerAnim.SetBool("RStrafe", false);
        playerAnim.SetBool("LStrafe", false);
        playerAnim.SetBool("Falling", false);
        playerAnim.SetBool("Shoot_Arrow", false);
        playerAnim.SetBool("Shoot_Melee", false);
        playerAnim.SetBool("Shoot_Rock", false);
    }

    bool OnGround(GameObject obj)
    {
        if (Physics.Raycast(obj.transform.position, Vector3.down, distToGround))
        {
            return true;
        }
        return false;
    }

    void getMovement()
    {
        Vector3 direction = Vector3.zero;

        // Movement keys
        if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartAnim("Running");
                if (isGrounded && !audioWalk.isPlaying) { audioWalk.Play(); } // Footsteps sound
                direction += this.transform.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                StartAnim("Running");
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
        playerCC.Move(direction * MoveSpeed * Time.deltaTime);

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

    Vector3 hasJumped(Vector3 currentDir)
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            if (audioWalk.isPlaying) { audioWalk.Stop(); }
            StartAnim("Jumping");
            return currentDir += Physics.gravity * -jumpHeight;
        }
        else return Vector3.zero;
    }

    bool isShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAnim("Shoot_Arrow");
            audioShoot.Play();
            shooter.ShootArrow();
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartAnim("Shoot_Melee");
            audioShoot.Play();
            shooter.MeleeHit();
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            StartAnim("Shoot_Rock");
            audioShoot.Play();
            shooter.ShootRock();
            return true;
        }
        return false;
    }

}
