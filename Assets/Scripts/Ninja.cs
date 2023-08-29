using UnityEngine;

public class Ninja : Creature
{
    override protected void GetActions()
    {
        base.GetActions();

        if (creatureTarget && !(creatureTarget == this.gameObject))
        {
            Vector3 lookatPos = creatureTarget.transform.position;
            lookatPos.y = this.transform.position.y;
            this.transform.LookAt(lookatPos);

            if (!HasActionCooldown())
            {
                if (canMove)
                {
                    if (Vector3.Distance(this.transform.position, creatureTarget.transform.position) >= 1.5f)
                    {
                        StartAnim("IsRunning");
                        direction += this.transform.forward;
                    }
                }
                if (Vector3.Distance(this.transform.position, creatureTarget.transform.position) < 1.5f)
                {
                    ShootMelee();
                }
            }
        }
    }

    protected bool ShootMelee()
    {
        actionCooldown = 3f;
        StartAnim("Shoot_Melee");
        if (shooter.MeleeHit())
        {
            shootFx.Play();
        }
        return true;
    }

    protected override void Death()
    {
        if (xpGiven > 0)
        {
            //GameManager.instance.GiveExp(xpGiven);
            //GameManager.instance.ShowText("+" + xpGiven + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.5f);
        }

        base.Death();
    }
}
