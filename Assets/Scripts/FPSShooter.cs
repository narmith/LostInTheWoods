using UnityEngine;

public class FPSShooter : MonoBehaviour
{
    public GameObject prefabArrow;
    public GameObject prefabRock;
    private int counter_rocks,counter_arrows,counter_punches;
    public Transform shootPoint;
    public float hitRange;

    void Start()
    {
        counter_rocks = 0;
        counter_arrows = 0;
        counter_punches = 0;
    }

    void LateUpdate()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hitRange, Color.red, 0.1f);
    }
    
    public void ShootArrow()
    {
        counter_arrows++;
        GameObject _arrow = Instantiate(prefabArrow, shootPoint.transform.position + transform.forward, shootPoint.transform.rotation);
        _arrow.gameObject.name = "arrow " + counter_arrows;
    }

    public void ShootRock()
    {
        counter_rocks++;
        GameObject _rock = Instantiate(prefabRock, shootPoint.transform.position + transform.forward, shootPoint.transform.rotation);
        _rock.gameObject.name = "rock " + counter_rocks;
    }

    public void MeleeHit()
    {
        counter_punches++;
        RaycastHit _target;
        if(Physics.Raycast(shootPoint.transform.position, shootPoint.transform.forward, out _target, hitRange))
        {
            _target.transform.gameObject.TryGetComponent(out HP _targetHP);
            if (_targetHP != null)
            {
                //_targetHP = target.GetComponent<HP>();
                _targetHP.TakeDamage(10);
                print(_target.transform.gameObject.name + " got hit (Melee). -10 HP.");
            }
        }
        print(shootPoint.name + " missed a hit (Melee).");
    }
}
