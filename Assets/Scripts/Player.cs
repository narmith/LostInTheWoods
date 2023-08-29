﻿using UnityEngine;

public class Player : Creature
{
    private Camera playerCam;
    private float camPitch = 0f;
    private float camYaw = 0f;
    public float camSpeed = 300f;
    public Transform shootPointer;
    private bool buildMode = false;
    private bool pauseState = false;

    override protected void Start()
    {
        base.Start();

        if (GameManager.instance.player == null)
        {
            GameManager.instance.player = this;
            return;
        }
        DontDestroyOnLoad(gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (StaticGlobals.GodMode) { this.gameObject.GetComponent<Health>().godMode = true; }
    }

    void PauseGame()
    {
        pauseState = !pauseState;

        if (pauseState)
        {
            Time.timeScale = 0.005f;
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1.0f;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
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

    override protected void ResetAnim()
    {
        base.ResetAnim();

        creatureAnims.SetBool("Shoot_Arrow", false);
    }

    override protected void GetActions()
    {
        base.GetActions();

        // memo: use for jump effect!!
        //public float _y = 0f;
        //_y += Time.deltaTime * 5f;
        //_y = Mathf.Clamp(_y, 9.81f, 0.1f);

        if (!HasActionCooldown() && canMove)
        {
            // Movement keys
            if (!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    StartAnim("IsRunning");
                    if (isGrounded && !walkFx.isPlaying) { walkFx.Play(); } // Footsteps sound
                    direction += this.transform.forward;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    StartAnim("IsRunning");
                    if (isGrounded && !walkFx.isPlaying) { walkFx.Play(); } // Footsteps sound
                    direction -= this.transform.forward;
                }
            }
            if (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    StartAnim("LStrafe");
                    if (isGrounded && !walkFx.isPlaying) { walkFx.Play(); } // Footsteps sound
                    direction -= this.transform.right;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    StartAnim("RStrafe");
                    if (isGrounded && !walkFx.isPlaying) { walkFx.Play(); } // Footsteps sound
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

        IsShooting();
        IsBuildModeActive();
    }

    protected void CameraUpdate(bool isMoving)
    {
        float hitRange = 20f;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hitRange, Color.yellow);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hitRange, out RaycastHit _target))
        {
            Debug.Log(_target.transform.name);
            if (!_target.transform.CompareTag(this.gameObject.tag))
            {
                if (_target.transform.gameObject.tag == "Object" || _target.transform.gameObject.tag == "Enemy")
                {
                    shootPointer.LookAt(_target.transform);
                    creatureTarget = _target.transform.gameObject;
                    //Debug.Log(_target.transform.gameObject.name);
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

    protected Vector3 HasJumped(Vector3 currentDir)
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            if (walkFx.isPlaying) { walkFx.Stop(); }
            actionCooldown = 2f;
            StartAnim("IsJumping");
            return currentDir += Physics.gravity * -jumpHeight;
        }
        else return Vector3.zero;
    }

    protected bool IsShooting()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (shooter.ShootArrow(shootPointer))
                {
                    actionCooldown = 4f;
                    StartAnim("Shoot_Arrow");
                    shootFx.Play();
                }
            }
            IsFirstPersonActive(true);
            return true;
        } else IsFirstPersonActive(false);

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            actionCooldown = 2f;
            StartAnim("Shoot_Melee");
            if (shooter.MeleeHit())
            {
                shootFx.Play();
            }
            return true;
        }

        return false;
    }

    private void IsFirstPersonActive(bool fps)
    {
        if (fps)
        {
            playerCam.transform.localPosition = shootPointer.transform.localPosition;
        }
        else playerCam.transform.localPosition = new Vector3(0, 1f, -2f);
    }

    private void IsBuildModeActive()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            buildMode = !buildMode;
        }
    }

}