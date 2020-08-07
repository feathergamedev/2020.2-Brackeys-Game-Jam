using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorpion : Chess
{

    [SerializeField]
    private ScorpionProjectile m_projectilePrefab;

    [SerializeField]
    private Direction m_attackDirection;

    [SerializeField]
    private List<int> m_attackAtWhichCycle;

    private int m_lastAttackCycle;

    [SerializeField]
    private Sprite m_normalSprite, m_attackSprite;

    private Animator m_animator;



    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_lastAttackCycle = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_attackAtWhichCycle.Contains(GameManager.instance.CurCycle) && m_lastAttackCycle != GameManager.instance.CurCycle)
        {
            Attack();
            m_lastAttackCycle = GameManager.instance.CurCycle;
        }
    }

    public void Attack()
    {
        var projectile = Instantiate<ScorpionProjectile>(m_projectilePrefab, transform.position, transform.rotation, GameManager.instance.CurStage.transform);

        var bornPos = Coordinate;

        projectile.Setup(bornPos, m_attackDirection) ;

        projectile.Move();

        m_animator.SetTrigger("Attack");

        GameManager.instance.AddToProjectileList(projectile);
        SoundManager.instance.PlaySound(SoundType.ScorpionAttack);
    }

    public override void Reset()
    {
        m_lastAttackCycle = -1;
    }
}
