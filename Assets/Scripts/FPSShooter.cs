using UnityEngine;

public class FPSShooter : MonoBehaviour
{
    public GameObject prefabArrow;
    public GameObject prefabRock;
    private int counter_rocks, counter_arrows, counter_punches;
    public GameObject shootPoint;
    public float hitRange;

    public float defaultCd, arrowCd, rockCd, meleeCd;

    void Start()
    {
        counter_rocks = 0;
        counter_arrows = 0;
        counter_punches = 0;
        defaultCd = 5f;
        arrowCd = 0f;
        rockCd = 0f;
        meleeCd = 0f;
    }

    public void Update()
    {
        float deltaT = 1 * Time.deltaTime;
        if (arrowCd > 0) { arrowCd -= deltaT; }
        if (rockCd > 0) { rockCd -= deltaT; }
        if (meleeCd > 0) { meleeCd -= deltaT; }
    }

    public bool ShootArrow()
    {
        if (prefabArrow && arrowCd <= 0)
        {
            arrowCd = defaultCd - 1f;
            counter_arrows++;
            GameObject _arrow = Instantiate(prefabArrow, shootPoint.transform.position + transform.forward, shootPoint.transform.rotation);
            _arrow.gameObject.name = "(" + this.gameObject.name + ") Arrow " + counter_arrows;
            return true;
        }
        return false;
    }

    public bool ShootRock()
    {
        if (prefabRock && rockCd <= 0)
        {
            rockCd = defaultCd + 5f;
            counter_rocks++;
            GameObject _rock = Instantiate(prefabRock, shootPoint.transform.position + transform.forward, shootPoint.transform.rotation);
            _rock.gameObject.name = "(" + this.gameObject.name + ") Rock " + counter_rocks;
            return true;
        }
        return false;
    }

    public bool MeleeHit()
    {
        if (meleeCd <= 0)
        {
            RaycastHit _target;
            //Debug.DrawRay(shootPoint.transform.position, shootPoint.transform.TransformDirection(Vector3.forward) * hitRange, Color.yellow);
            if (Physics.Raycast(shootPoint.transform.position, shootPoint.transform.TransformDirection(Vector3.forward), out _target, hitRange))
            {
                if (!_target.transform.CompareTag(this.gameObject.tag))
                {
                    if (meleeCd <= 0)
                    {
                        if (_target.transform.TryGetComponent(out HP _targetHP))
                        {
                            counter_punches++;
                            int dmgDone = _targetHP.DamageHP(8);
                            //print(_target.transform.gameObject.name + " got hit by " + this.gameObject.name + ". (Melee -" + dmgDone + ").");
                        }
                    }
                    //print(_target.transform.gameObject.name + " got hit by " + this.gameObject.name);
                }
                meleeCd = defaultCd - 3f;
                return true;
            }
            meleeCd = defaultCd - 3f;
        }
        return false;
    }
}
