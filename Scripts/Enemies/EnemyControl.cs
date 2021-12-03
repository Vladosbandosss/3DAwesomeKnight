using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyState
{
    IDLE,
    WALK,
    RUN,
    PAUSE,
    GOBACK,
    ATTACK,
    DEATH
}
public class EnemyControl : MonoBehaviour
{
    private float attackDistance = 1.5f;
    private float alertAttackDistance = 8f;
    private float followDistance = 15f;

    private float enemyToPlayerDistance;

    [HideInInspector] public EnemyState enemyCurentStane = EnemyState.IDLE;
   private EnemyState enemyLastStane = EnemyState.IDLE;

   private Transform playerTarget;

   private Vector3 initialPosition;

   private float moveSpeed = 2f;
   private float walkSpeed = 1f;

   private CharacterController charcontroller;
   private  Vector3 whereToMove=Vector3.zero;

   private float currentAttackTime;
   private float waitAttackTime = 1f;

   private Animator _anim;
   private bool finishedAnimation = true;
   private bool finishedMovement = true;

   private NavMeshAgent _navAgent;
   private Vector3 wheroToNavigate;

   private void Awake()
   {
       playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
       _navAgent = GetComponent<NavMeshAgent>();
       charcontroller = GetComponent<CharacterController>();
       _anim = GetComponent<Animator>();

       initialPosition = transform.position;
       wheroToNavigate = transform.position;
   }

   // Update is called once per frame
    void Update()
    {
        if (enemyCurentStane != EnemyState.DEATH)
        {
            enemyCurentStane = SetEnemyState(enemyCurentStane,enemyLastStane,enemyToPlayerDistance);

            if (finishedMovement)
            {
                GetStateControl(enemyCurentStane);
            }
            else
            {
                if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    finishedMovement = true;
                }else if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsTag("Atk1")
                          || _anim.GetCurrentAnimatorStateInfo(0).IsTag("Atk2"))
                {
                   _anim.SetInteger("Atk2",0); 
                }
            }
        }
        else
        {
            _anim.SetBool("Death",true);
            charcontroller.enabled = false;
            _navAgent.enabled = false;
            if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Death")
            &&_anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.95f)
            {
                Destroy(gameObject,2f);
            }
        }
    }

    EnemyState SetEnemyState(EnemyState curState,EnemyState lastState,float enemyToPlayerDis)
    {
        enemyToPlayerDis = Vector3.Distance(transform.position, playerTarget.position);
        
        float initiolDistnace = Vector3.Distance(initialPosition, transform.position);

        if (initiolDistnace > followDistance)
        {
            lastState = curState;
            curState = EnemyState.GOBACK;
        }else if (enemyToPlayerDis <= attackDistance)
        {
            lastState = curState;
            curState = EnemyState.ATTACK;
        }else if (enemyToPlayerDis>=alertAttackDistance&&lastState==EnemyState.PAUSE||lastState==EnemyState.ATTACK)
        {
            lastState = curState;
            curState = EnemyState.PAUSE;
        }else if (enemyToPlayerDis <= alertAttackDistance && enemyToPlayerDis > attackDistance)
        {
            if (curState != EnemyState.GOBACK || lastState == EnemyState.WALK)
            {
                lastState = curState;
                curState = EnemyState.PAUSE;
            }
        }else if (enemyToPlayerDis > alertAttackDistance && lastState != EnemyState.GOBACK &&
                  lastState != EnemyState.PAUSE)
        {
            
            lastState = curState;
            curState = EnemyState.WALK;
        }

        return curState;
    }

    void GetStateControl(EnemyState curState)
    {
        if (curState == EnemyState.RUN || curState == EnemyState.PAUSE)
        {
            if (curState != EnemyState.ATTACK)
            {
                Vector3 targetPos = new Vector3(playerTarget.position.x, transform.position.y,playerTarget.position.z);
                if (Vector3.Distance(transform.position, targetPos) >= 2.1f)
                {
                    _anim.SetBool("Walk",false);
                    _anim.SetBool("Run",true);
                    _navAgent.SetDestination(targetPos);
                    Debug.Log("ХОЖУ БРОЖУ");
                }
            }
        }else if (curState == EnemyState.ATTACK)
        {
            _anim.SetBool("Run",false);
            whereToMove.Set(0f, 0f, 0f);
            _navAgent.SetDestination(transform.position);
            Debug.Log("ИДУ");
            transform.rotation=Quaternion.Slerp
                (transform.rotation,Quaternion.LookRotation(playerTarget.position-transform.position),5f*Time.deltaTime);
            if (currentAttackTime >=waitAttackTime)
            {
                int atkRange = Random.Range(1, 3);
                _anim.SetInteger("Atk",atkRange);
                finishedAnimation = false;
                currentAttackTime = 0f;
            }
            else
            {
                _anim.SetInteger("Atk",0);
                currentAttackTime += Time.deltaTime;
                
            }
        }else if (curState==EnemyState.GOBACK)
        {
            _anim.SetBool("Run",true);
            Vector3 targetPos = new Vector3(initialPosition.x, transform.position.y, initialPosition.z);

            _navAgent.SetDestination(targetPos);

            if (Vector3.Distance(targetPos, initialPosition) <= 3.5f)
            {
                enemyLastStane = curState;
                curState = EnemyState.WALK;
            }
        }else if (curState == EnemyState.WALK)
        {
            _anim.SetBool("Run",false);
            _anim.SetBool("Walk",true);

            if (Vector3.Distance(transform.position, wheroToNavigate) <= 2f)
            {
                wheroToNavigate.x = Random.Range(initialPosition.x - 5f, initialPosition.x + 5f);
                wheroToNavigate.z = Random.Range(initialPosition.z - 5f, initialPosition.z + 5f);
            }
            else
            {
                _navAgent.SetDestination(wheroToNavigate);
                Debug.Log("хз куда");
                Debug.Log(wheroToNavigate.x );
            }
        }
        else
        {
            _anim.SetBool("Run",false);
            _anim.SetBool("Walk",false);
            whereToMove.Set(0f,0f,0f);
            _navAgent.isStopped = true;
        }
        
    }
    
    
    
}//clas
