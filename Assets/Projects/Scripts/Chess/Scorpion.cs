using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorpion : Chess
{

    [SerializeField]
    private ScorpionProjectile m_projectilePrefab;

    [SerializeField]
    private Direction m_attackDirection;

    [SerializeField]
    private List<int> m_attackAtWhichCycle;

    private bool done;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_attackAtWhichCycle.Contains(GameManager.instance.CurCycle))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (done)
            return;

        var projectile = Instantiate<ScorpionProjectile>(m_projectilePrefab, transform.position, transform.rotation, GameManager.instance.CurStage.transform);

        var bornPos = Coordinate;

        projectile.Setup(bornPos, m_attackDirection) ;

        projectile.Move();

        GameManager.instance.AddToProjectileList(projectile);

        done = true;
    }

    public override void Reset()
    {
        done = false;
    }
}
