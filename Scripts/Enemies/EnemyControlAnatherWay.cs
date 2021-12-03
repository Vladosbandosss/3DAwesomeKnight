using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyControlAnatherWay : MonoBehaviour
{
    public Transform[] walkPoints;
    private int _walkIndex = 0;

    private Transform _playerTarget;

    private Animator _animator;
    
    private NavMeshAgent _navMeshAgent;

    private float _walkDistance = 8f;
    private float _attackDistance = 2f;

    private float _currentAttackTime;
    private float _waitAttackTime = 1f;

    private Vector3 _nextDistination;
    private void Awake()
    {
        _playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        float distance = Vector3.Distance(transform.position, _playerTarget.position);

        if (distance > _walkDistance)
        {
            if (_navMeshAgent.remainingDistance <= 0.5f)
            {
                _navMeshAgent.isStopped = false;
                
               _animator.SetBool("Walk",true);
               _animator.SetBool("Run",false);
               _animator.SetInteger("Atk",0);

               _nextDistination = walkPoints[_walkIndex].position;
               _navMeshAgent.SetDestination(_nextDistination);

               if (_walkIndex == walkPoints.Length - 1)
               {
                   _walkIndex = 0;
               }
               else
               {
                   _walkIndex++;
               }
            }
        }
        
        
        
        else
        {
            if (distance > _attackDistance)
            {
                _navMeshAgent.isStopped = false;
                
                _animator.SetBool("Walk",false);
                _animator.SetBool("Run",true);
                _animator.SetInteger("Atk",0);
                
                _navMeshAgent.SetDestination(_playerTarget.position);
            }

            else
            {
                _navMeshAgent.isStopped = true;
                
                _animator.SetBool("Run",false);

                Vector3 targetPosition = new Vector3(_playerTarget.position.x, transform.position.y,
                    _playerTarget.position.z);
                
                transform.rotation=Quaternion.Slerp
                    (transform.rotation,Quaternion.LookRotation(targetPosition-transform.position),5f*Time.deltaTime);

                if (_currentAttackTime > -_waitAttackTime)
                {
                    int atkRange = Random.Range(1, 3);
                    
                    _animator.SetInteger("Atk",atkRange);
                    _currentAttackTime = 0f;
                    
                }
                else
                {
                    _animator.SetInteger("Atk",0);
                    _currentAttackTime += Time.deltaTime;
                }
            }

        }
    }
}//class
