using UnityEngine;

public class FPSShooter : MonoBehaviour
{
    public GameObject prefabArrow;
    public GameObject prefabRock;
    private int counter_arrows;
    public GameObject shootPoint;
    public float hitRange;

    public float defaultCd, arrowCd, meleeCd;

    void Start()
    {
        counter_arrows = 0;
        defaultCd = 5f;
        arrowCd = 0f;
        meleeCd = 0f;
    }

    public void FixedUpdate()
    {
        float deltaT = 1 * Time.deltaTime;
        if (arrowCd > 0) { arrowCd -= deltaT; }
        if (meleeCd > 0) { meleeCd -= deltaT; }
    }

    public bool CanHit() {  return meleeCd <= 0; }
    public bool CanShoot() { return arrowCd <= 0; }

    public bool ShootArrow(Transform shootPointer)
    {
        if (prefabArrow && arrowCd <= 0)
        {
            arrowCd = 2f;
            counter_arrows++;
            GameObject _arrow = Instantiate(prefabArrow, shootPointer.position + shootPointer.forward, shootPointer.rotation);
            _arrow.gameObject.name = "(" + this.gameObject.name + ") Arrow " + counter_arrows;
            return true;
        }
        return false;
    }
    public bool ShootArrow()
    {
        if (prefabArrow && arrowCd <= 0)
        {
            arrowCd = 3f;
            counter_arrows++;
            GameObject _arrow = Instantiate(prefabArrow, shootPoint.transform.position + shootPoint.transform.forward, shootPoint.transform.rotation);
            _arrow.gameObject.name = "(" + this.gameObject.name + ") Arrow " + counter_arrows;
            return true;
        }
        return false;
    }

    public bool MeleeHit()
    {
        if (meleeCd <= 0)
        {
            RaycastHit _target;
            if (Physics.Raycast(shootPoint.transform.position, shootPoint.transform.TransformDirection(Vector3.forward), out _target, hitRange))
            {
                if (_target.transform.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
                    if (_target.transform.TryGetComponent(out HP _targetHP)) { _targetHP.DamageHP(8); }
                meleeCd = 2f;
                return true;
            }
            meleeCd = 2f;
        }
        return false;
    }

    public void ActionHit()
    {
        //Debug.Log("Action!");
    }
    public void BuildHit()
    {
        //Debug.Log("Build!");
    }

}
