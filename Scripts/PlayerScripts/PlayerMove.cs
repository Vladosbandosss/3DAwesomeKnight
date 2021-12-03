using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//РАБОЧИЙ СКРИПТ МНОГО НЕПОНЯТНОГО НО РАБОТАЕТ ПЕРС БЕГАЕТ В СТОРОНУ НАЖАТИЯ!!!
public class PlayerMove : MonoBehaviour
{
    private Animator _anim;
    private CharacterController _characterController;
    private CollisionFlags _collisionFlags = CollisionFlags.None;//если просто типо по коллизиям где вверх низ лево право

    private float _moveSpeed = 6f;
    private bool _canMove;
    private bool _finishedMovement = true;//при старте мы не двиг поэтому тру

    private  Vector3 targetPos = Vector3.zero;
    private  Vector3 playerMove=Vector3.zero;
    
    private float playerToPointDistance;

    private float gravity = 9.8f;
    private float height;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
      CalculateHeight();
      CheckIfFinishedMovement();
    }

    bool isGrounded()
    {
        return _collisionFlags == CollisionFlags.CollidedBelow ? true : false;
    }

    void CalculateHeight()
    {
        if (isGrounded())
        {
            height = 0f;
        }
        else
        {
            height -= gravity * Time.deltaTime;
        }
    }

    void CheckIfFinishedMovement()
    {
        if (!_finishedMovement)
        {
            if (!_anim.IsInTransition(0) && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Stan" +
                "d") && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                _finishedMovement = true;
            }
        }
        else
        {
            MovePlayer();
            playerMove.y = height * Time.deltaTime;
            _collisionFlags = _characterController.Move(playerMove);
        }
    }

    void MovePlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //луч с камеры до того куда нажали
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) //если луч столкнулся с коллайдерои
            {
                if (hit.collider is TerrainCollider) //ударились об еурайн колайдер(висит натерайне)
                {
                    playerToPointDistance = Vector3.Distance(transform.position, hit.point);

                    if (playerToPointDistance >= 1.0f)
                    {
                        _canMove = true;
                        targetPos = hit.point;
                    }
                }
            }
        }//mousebutton

        if (_canMove)
        {
                _anim.SetFloat("Walk",1.0f);
                Vector3 tartetTemp = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                
                transform.rotation=Quaternion.Slerp
                    (transform.rotation,Quaternion.LookRotation(tartetTemp-transform.position), 15f*Time.deltaTime);

                playerMove = transform.forward * _moveSpeed * Time.deltaTime;

                if (Vector3.Distance(transform.position, targetPos) <= 0.5f)
                {
                    _canMove = false;
                }
        }
        else
        {
                playerMove.Set(0f,0f,0f);
                _anim.SetFloat("Walk",0f);
        }
        
    }//moveplayer

    public bool FinishedMovement
    {
        get
        {
            return _finishedMovement;
        }
        set
        {
            _finishedMovement = value;
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            return targetPos;
        }
        set
        {
            targetPos = value;
        }
    }
}//class

    
    
    
    
    

