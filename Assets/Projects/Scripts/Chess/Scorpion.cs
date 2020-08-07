using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorpion : Chess
{

    [SerializeField]
    private ScorpionProjectile m_projectilePrefab;

    private Direction m_attackDirection;

    private int m_lastAttackCycle;

    private Animator m_animator;

    private WarriorController m_warrior;

    [SerializeField]
    private int m_attackCoolDown;

    private bool isFirstTime;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_warrior = WarriorController.instance;
        isFirstTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstTime || GameManager.instance.CurCycle - m_lastAttackCycle > m_attackCoolDown)
        {
            if (IsOnTheSameLine(m_warrior.Coordinate, Coordinate))
            {
                if (m_warrior.Coordinate.x == Coordinate.x)
                    m_attackDirection = (m_warrior.Coordinate.y > Coordinate.y) ? Direction.Up : Direction.Down;
                else
                    m_attackDirection = (m_warrior.Coordinate.x > Coordinate.x) ? Direction.Right : Direction.Left;

                Attack();

                m_lastAttackCycle = GameManager.instance.CurCycle;
                isFirstTime = false;
            }
        }
 


    }

    bool IsOnTheSameLine(Vector2Int c1, Vector2Int c2)
    {
        return (c1.x == c2.x) || (c1.y == c2.y);
    }

    public void Attack()
    {
        var projectile = Instantiate<ScorpionProjectile>(m_projectilePrefab, transform.position, transform.rotation, GameManager.instance.CurStage.transform);

        var bornPos = Coordinate;

        projectile.Setup(bornPos, m_attackDirection, GameManager.instance.CurCycle);

        m_animator.SetTrigger("Attack");

        GameManager.instance.AddToProjectileList(projectile);
        SoundManager.instance.PlaySound(SoundType.ScorpionAttack);
    }

    public override void Reset()
    {
        m_lastAttackCycle = -1;
        isFirstTime = true;
    }
}
