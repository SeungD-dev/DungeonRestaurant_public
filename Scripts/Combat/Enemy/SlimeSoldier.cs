using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class SlimeSoldier : EnemyCombatAI
{
    protected EnemyInfo SlimeData { get; set; } = null;
    private BaseCombatAI forcedTarget;
    private float forcedTargetEndTime;

    protected override void ApplyCharacterStat()
    {
        base.ApplyCharacterStat();
        //Enemy enemy = GetComponent<Enemy>();
        //SlimeData = enemy.info;
        MaxHP = SlimeData.HP;
        CurrentHP = MaxHP;
        AttackDamage = SlimeData.ATK;
        MovementSpeed = SlimeData.MoveSpeed;
        AttackSpeed = SlimeData.AttackSpeed;
        BaseAttackInterval = 1f / AttackSpeed;
        AttackRange = SlimeData.Range;
        Def = SlimeData.DEF;
    }

    public override void SetBaseInfo()
    {
        base.SetBaseInfo();
        Initialize();
    }

    public void Initialize()
    {
        stateMachine = new StateMachine();
        animator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<CapsuleCollider2D>();
        InitializeStates();
        UpdateFacingDirection();
        UpdateNearestTarget();
        colliderResults = new Collider2D[MAX_ENEMIES]; 
    }
 
    protected override void InitializeStates()
    {
        
        var idleState = new IdleState(this);
        var moveState = new MovingState(this);
        var attackState = new EnemyAttackingState(this);
        var stunState = new StunState(this);
        var dieState = new DieState(this);

        //전투 시작 시 상태 전환
        stateMachine.AddTransition(idleState, moveState, new FuncPredicate(() => GameManager.Instance.isCombatStart));
        //사거리 내 적이 없다면 가까운 적을 추적
        stateMachine.AddTransition(moveState, attackState, new FuncPredicate(() => IsTargetInAttackRange() && CheckTarget()));
        stateMachine.AddTransition(attackState, moveState,
        new FuncPredicate(() => !IsTargetInAttackRange() || !CheckTarget()));

        stateMachine.AddAnyTransition(dieState, new FuncPredicate(() => CurrentHP <= 0));

        //스턴 상태 후 상태 전환
        stateMachine.AddTransition(stunState, attackState,
            new FuncPredicate(() => !IsStunned && IsTargetInAttackRange() && CheckTarget()));
        stateMachine.AddTransition(stunState, moveState,
            new FuncPredicate(() => !IsStunned && (!IsTargetInAttackRange() || !CheckTarget())));
        //스턴 상태
        stateMachine.AddAnyTransition(stunState, new FuncPredicate(() => IsStunned));

        //기본 상태
        stateMachine.SetState(idleState);
        stateMachine.AddAnyTransition(idleState, new FuncPredicate(() => !GameManager.Instance.isCombatStart));
    }

    public override void SetForcedTarget(BaseCombatAI target, float duration)
    {
        forcedTarget = target;
        forcedTargetEndTime = Time.time + duration;
    }

    // UpdateNearestTarget 메서드 오버라이드
    public override void UpdateNearestTarget()
    {
        if (forcedTarget != null)
        {
            cachedNearestTarget = forcedTarget;
        }
        else
        {
            base.UpdateNearestTarget();
        }
    }

    public override void Attack()
    {
        if (CheckTarget())
        {
            PlayAnimation(AnimationData.Attack_NormalHash, true);
            cachedNearestTarget.TakeDamage(AttackDamage,DamageType.Physical,AttackType.Blunt ,true);
        }
    }
}
