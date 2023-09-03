using System.Collections;
using UnityEngine;

public class Player : Creature
{
    public ActionBars actionsBarUI;
    private Camera playerCam;
    private Transform shootPointer;
    private GrabObjects grabObjects;
    private float camPitch = 0f;
    private float camYaw = 0f;
    private float camSpeed = 300f;
    private int actionSelected = 0;
    private bool pauseState = false;
    private bool isFPSActive = false;

    override public void Start()
    {
        base.Start();

        if (GameManager.instance.player == null)
        {
            GameManager.instance.player = this;
            DontDestroyOnLoad(gameObject);
        }

        if (StaticGlobals.GodMode) { GetComponent<HP>().godMode = true; }
        if (!TryGetComponent(out grabObjects)) { Debug.Log("ERROR!"); }
        //if (!TryGetComponent(out shootPointer)) { Debug.Log("ERROR!"); }
        shootPointer = shooter.shootPoint.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void PauseGame()
    {
        pauseState = !pauseState;

        if (pauseState)
        {
            Time.timeScale = 0.1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1.0f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    override public void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
            //CanMove = false;
            //SceneManager.LoadScene("MainMenu");
        }
    }

    override public void FixedUpdate()
    {
        base.FixedUpdate();

        if (actionCooldown > 0)
        {
            actionsBarUI.action1Timer.fillAmount += (Time.deltaTime / actionCooldown);
            actionsBarUI.action2Timer.fillAmount += (Time.deltaTime / actionCooldown);
        }
        else
        {
            actionsBarUI.action1Timer.fillAmount = 0;
            actionsBarUI.action2Timer.fillAmount = 0;
        }
        
    }

    override protected void ResetAnim()
    {
        base.ResetAnim();
        creatureAnims.SetBool("Shoot_Arrow", false);
    }

    override protected void GetActions()
    {
        base.GetActions();

        if (!HasActionCooldown() && canMove)
        {
            // Movement keys
            if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    StartAnim("IsRunning");
                    if (isGrounded && !runFx.isPlaying) { runFx.Play(); } // Footsteps sound
                    direction += this.transform.forward;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    StartAnim("IsRunning");
                    if (isGrounded && !runFx.isPlaying) { runFx.Play(); } // Footsteps sound
                    direction -= this.transform.forward;
                }
            }
            if (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    StartAnim("LStrafe");
                    if (isGrounded && !runFx.isPlaying) { runFx.Play(); } // Footsteps sound
                    direction -= this.transform.right;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    StartAnim("RStrafe");
                    if (isGrounded && !runFx.isPlaying) { runFx.Play(); } // Footsteps sound
                    direction += this.transform.right;
                }
            }
            direction += HasJumped(direction);
        }

        // Mouse transfromations
        camPitch += Input.GetAxis("Mouse Y") * camSpeed * Time.deltaTime;
        camYaw = Input.GetAxis("Mouse X") * camSpeed * Time.deltaTime;
        CameraUpdate(direction != Vector3.zero);
        BodyRotation();

        // Player interactions
        EvaluateActions();
    }

    protected void CameraUpdate(bool isMoving)
    {
        float hitRange = 20f;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hitRange, Color.yellow);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hitRange, out RaycastHit _target))
        {
            //Debug.Log(_target.transform.name);
            if (!_target.transform.CompareTag(this.gameObject.tag))
            {
                if (_target.transform.gameObject.tag == "Object" || _target.transform.gameObject.tag == "Enemy")
                {
                    shootPointer.LookAt(_target.transform);
                    creatureTarget = _target.transform.gameObject;
                }
            }
            else
            {
                shootPointer.position.Set(0, 0.5f, 0.5f);
                shootPointer.rotation.Set(0, 0, 0, 1);
                creatureTarget = null;
            }
        }
        else
        {
            shootPointer.position.Set(0, 0.5f, 0.5f);
            shootPointer.rotation.Set(0, 0, 0, 1);
            creatureTarget = null;
        }

        if (playerCam)
        {
            // Camera vertical rotation 
            camPitch = Mathf.Clamp(camPitch, -80, 40);
            if (isMoving) { camPitch = Mathf.LerpAngle(camPitch, playerCam.transform.rotation.eulerAngles.x, Time.deltaTime); }
            playerCam.transform.eulerAngles = new Vector3(-camPitch, playerCam.transform.rotation.eulerAngles.y, playerCam.transform.rotation.eulerAngles.z);
        }
        else
        {
            if (Camera.main) playerCam = Camera.main;
            playerCam.transform.LookAt(this.gameObject.transform);
        }
    }

    protected void BodyRotation()
    {
        // Body horizontal rotation
        this.transform.Rotate(Vector3.up * camYaw);
    }

    private bool IsFPSActive()
    {
        return isFPSActive;
    }

    private void IsFPSActive(bool fps)
    {
        if (fps)
        {
            isFPSActive = true;
            playerCam.transform.localPosition = shootPointer.transform.localPosition;
        }
        else
        {
            isFPSActive = false;
            playerCam.transform.localPosition = new Vector3(0, 1f, -2f);
        }
    }

    protected Vector3 HasJumped(Vector3 currentDir)
    {
        // memo: use for jump effect!!
        //public float _y = 0f;
        //_y += Time.deltaTime * 5f;
        //_y = Mathf.Clamp(_y, 9.81f, 0.1f);

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            if (runFx.isPlaying) { runFx.Stop(); }
            actionCooldown = 2f;
            StartAnim("IsJumping");
            return currentDir += Physics.gravity * -jumpHeight;
        }
        else return Vector3.zero;
    }

    private void EvaluateActions()
    {
        /* 0 = Idle mode
         * 1-9 = Attack mode (Attack with Melee or Ranged weapons)
         * 10-19 = Special Attack mode (Attack with the Shift key ON)
         * 20 = Grab mode (Grab/interact with world objects)
         * 32 = Build Mode
         * 42 = Inventory mode (UI/Inventory)
         * 99 = Terrain mode (Change the terrain with a tool, similar to Attack mode)
        */

        if (actionSelected > 0 && actionSelected < 20) IsAttacking();
        if (actionSelected == 20) { GrabMode(); }
        if (actionSelected == 32) { BuildMode(); }
        if (actionSelected == 42) { InventoryMode(); }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Should be hotkeys 1-9 for combat items
        {
            if (actionSelected != 1)
                actionSelected = 1;
            else
                actionSelected = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) // Should be hotkey 0 for the grab mode
        {
            if (actionSelected != 20)
                actionSelected = 20;
            else
                actionSelected = 0;
        }
        else if (Input.GetKeyDown(KeyCode.I)) // Should bring UI/Inventory 
        {
            if (actionSelected != 42)
                actionSelected = 42;
            else
                actionSelected = 0;
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            if (actionSelected != 32)
                actionSelected = 32;
            else actionSelected = 0;
        }

        IsFPSActive(Input.GetKey(KeyCode.Mouse1));
    }

    void PlaySound(AudioSource audioclip, float delayTime)
    {
        StartCoroutine(DoDelayed(audioclip, delayTime));
    }
    void PlaySound(AudioSource audioclip)
    {
        audioclip.Play();
    }

    IEnumerator DoDelayed(AudioSource audioclip, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        PlaySound(audioclip);
    }

    protected void IsAttacking()
    {
        if (IsFPSActive() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (shooter.ShootArrow(shootPointer))
            {
                actionCooldown = 3f;
                StartAnim("Shoot_Arrow");
                PlaySound(shootFx, 1.5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            actionCooldown = 2f;
            StartAnim("Shoot_Melee");
            if (shooter.CanHit())
            {
                if (shooter.MeleeHit())
                    PlaySound(hitFx, 0.8f);
                else PlaySound(hitMissFx, 0.8f);
            }
        }
    }

    private void GrabMode()
    {
        if (IsFPSActive() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            grabObjects.Interact();
        }
    }

    private void InventoryMode()
    {
        // Open/Close Inventory Selection UI
        // Select/use items to consume or drop
    }

    private void BuildMode()
    {
        // Open/Close Build Selection UI
        // Select item to build
        // Build to Inventory or drop into world to build with Shooting Mode (1)
    }

}
