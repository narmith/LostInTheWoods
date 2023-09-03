using UnityEngine;
using TMPro;

public class UIMinimapButtons : MonoBehaviour
{
    TextMeshProUGUI _TimeLabel;
    Transform _player;
    float zoomLevel = 10f;
    Camera _Minimap;
    public bool usePlusMinusKeys = true;
    public float zoomSensitivity = 10f;
    public float furthestZoom = 40f;

    void Start()
    {
        if (!(_player = GameObject.Find("Player").transform))
        {
            Debug.Log("ERROR: There is no Player GameObject!");
        }
        if (!(_TimeLabel = GameObject.Find("TimeLabel").GetComponent<TextMeshProUGUI>()))
        {
            Debug.Log("ERROR: There is no TimeLabel GameObject!");
        }
        if (!(_Minimap = GameObject.Find("MinimapCam").GetComponent<Camera>()))
        {
            Debug.Log("ERROR: There is no Minimap GameObject!");
        }
    }

    public void MaxButton() //go near
    {
        if (_Minimap.orthographicSize > zoomSensitivity)
        { _Minimap.orthographicSize -= zoomSensitivity; }
        
        if (_Minimap.orthographicSize <= zoomSensitivity)
        { _Minimap.orthographicSize = zoomSensitivity; }
    }

    public void MinButton() //go afar
    {
        if (_Minimap.orthographicSize < furthestZoom)
        { _Minimap.orthographicSize += zoomSensitivity; }

        if (_Minimap.orthographicSize >= furthestZoom)
        { _Minimap.orthographicSize = furthestZoom; }
    }

    void FixedUpdate()
    {
        // Position the Minimap camera above the player constantly
        if (_player && _Minimap)
        {
            _Minimap.transform.position = new(_player.position.x, _player.position.y + zoomLevel, _player.position.z);
        }
    }
    void Update()
    {
        if (usePlusMinusKeys)
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus)) { MaxButton(); }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus)) { MinButton(); }
        }
    }
    void LateUpdate()
    {
        // Get the current time and display it on the Minimap
        if (_TimeLabel)
            _TimeLabel.text = (System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString()); // XX:xx
    }
}
