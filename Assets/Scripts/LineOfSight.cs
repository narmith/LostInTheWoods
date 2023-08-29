using UnityEngine;
using UnityEngine.UIElements;

public class LineOfSight : MonoBehaviour
{
    private RaycastHit vision;
    public float hitRange;
    private Transform grabbedTransform;
    private Transform lastParent;
    private Rigidbody grabbedRigidbody;
    private Collider grabbedCollider;
    private GameObject _lastGrabbedGameObject;
    public bool isGrabbing;
    private bool hadCollider;
    private bool lastTriggerState;
    private bool hadRigidbody;
    private bool lastKinematicState;

    float mouseDelta = 0;



    public Color targetOriginalColor;

    private void Start()
    {
        hitRange = 4f;
        isGrabbing = false;
        hadCollider = false;
        lastTriggerState = false;
        hadRigidbody = false;
        lastKinematicState = false;
        grabbedTransform = null;
        grabbedCollider = null;
        lastParent = null;
    }

    private void Update()
    {
        if (isGrabbing)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                mouseDelta = Mathf.FloorToInt(Input.GetAxis("Mouse ScrollWheel") * 1000f * Time.deltaTime);
                if(Input.GetKey(KeyCode.LeftShift))
                    grabbedTransform.Rotate(new Vector3(1, 0, 1), mouseDelta * 10f);
                else grabbedTransform.Rotate(new Vector3(0, 1, 0), mouseDelta * 10f);
            }

            if (Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.E))
            {
                if (hadCollider)
                {
                    grabbedCollider.isTrigger = lastTriggerState;
                    grabbedCollider = null;
                }
                if (hadRigidbody)
                {
                    grabbedRigidbody.isKinematic = lastKinematicState;
                    grabbedRigidbody.transform.parent = lastParent;
                    grabbedRigidbody = null;
                }

                grabbedTransform.SetParent(lastParent);
                grabbedTransform = null;
                isGrabbing = false;
            }
        }
        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out vision, hitRange))
        {
            if (_lastGrabbedGameObject != null && _lastGrabbedGameObject.GetInstanceID() != vision.transform.gameObject.GetInstanceID())
            {
                ResetColor(_lastGrabbedGameObject);
                ChangeColor(vision.transform.gameObject, Color.grey);
                _lastGrabbedGameObject = vision.transform.gameObject;
            }
            else if (_lastGrabbedGameObject == null)
            {
                ChangeColor(vision.transform.gameObject, Color.grey);
                _lastGrabbedGameObject = vision.transform.gameObject;
            }

            if (vision.collider.CompareTag("Object"))
            {
                if (Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Grabbed: " + vision.transform.name);

                    grabbedTransform = vision.transform;
                    lastParent = grabbedTransform.parent;
                    grabbedTransform.SetParent(Camera.main.transform);
                    isGrabbing = true;

                    if (grabbedTransform.TryGetComponent<Collider>(out grabbedCollider))
                    {
                        hadCollider = true;
                        lastTriggerState = grabbedCollider.isTrigger;
                        grabbedCollider.isTrigger = true;
                    }
                    else
                    {
                        hadCollider = false;
                        grabbedCollider = null;
                    }

                    if (grabbedTransform.TryGetComponent<Rigidbody>(out grabbedRigidbody))
                    {
                        hadRigidbody = true;
                        lastKinematicState = grabbedRigidbody.isKinematic;
                        grabbedRigidbody.isKinematic = true;
                    }
                    else
                    {
                        hadRigidbody = false;
                        grabbedRigidbody = null;
                    }
                }
            }
        }
        else if (_lastGrabbedGameObject != null)
        {
            ResetColor(_lastGrabbedGameObject);
            _lastGrabbedGameObject = null;
        }
    }

    void ChangeColor(GameObject _target, Color _newColor)
    {
        if (_target.gameObject.GetComponent<Renderer>() != null)
        {
            if (_target.gameObject.CompareTag("Object"))
            {
                targetOriginalColor = _target.gameObject.GetComponent<Renderer>().material.color;
                _target.gameObject.GetComponent<Renderer>().material.color = _newColor;
            }
        }
    }
    void ResetColor(GameObject _target)
    {
        if (_target.gameObject.GetComponent<Renderer>() != null)
        {
            if (_target.gameObject.CompareTag("Object"))
            {
                _target.gameObject.GetComponent<Renderer>().material.color = targetOriginalColor;
            }
        }
    }
}
