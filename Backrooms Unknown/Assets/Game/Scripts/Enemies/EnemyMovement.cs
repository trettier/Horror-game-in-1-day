using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : EnemyOldScript
{


    void FixedUpdate()
    {
        if (!takeDamage)
        {
            // Логика поведения в зависимости от состояния
            switch (currentState)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.Patrolling:
                    LookForPlayer();
                    Patrol();
                    Move();
                    break;
                case EnemyState.Investigating:
                    LookForPlayer();
                    Investigate();
                    StopMove();
                    break;
                case EnemyState.Chasing:
                    LookForPlayer();
                    ChasePlayer();
                    Move();
                    break;
                case EnemyState.Spawning:
                    PlaySpawnAnimation();
                    break;
                case EnemyState.Dying:                    
                    break;
            }
        }

    }

    void Move()
    {
        rb.AddForce(direction * speed);

        if (InPool && speed != 0)
        {
            Instantiate(particleSystem, transform.position, Quaternion.identity);
        }

        UpdateAnimation();

    }

    void StopMove()
    {
        if (_isFootstepSoundPlaying) // Если только остановились
        {
            _isFootstepSoundPlaying = false;
            footstepSound.Stop(); // Останавливаем звук
        }
    }

    private void Patrol()
    {
        Vector3 targetPoint = new Vector3(0, 0, 0);
        if (!patrolObject)
        {
            targetPoint = patrolPoints[currentPoint];
        }
        else
        {
            targetPoint = currentObject.GetPosition();
        }
        direction = (targetPoint - transform.position).normalized;
        animator.SetBool("Move", true);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            if (!patrolObject)
            {
                currentPoint = (currentPoint + 1) % patrolPoints.Count;
            }
            else
            {
                currentObject = currentObject.GetRandomPoint();
            }
            flagDirection = directions.IndexOf(GetDirection(direction));
            currentState = EnemyState.Investigating;
            lastFrameTime = Time.time;
            animator.SetBool("Move", false);
        }

        if (Time.time - lastFrameTime > 10)
        {
            foreach (var i in directions)
            {
                int layerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Player"));
                RaycastHit2D hit2D = Physics2D.Raycast(transform.position + Vector3.down, i, viewDistance / 2, layerMask);

                Debug.DrawRay(transform.position, i * viewDistance / 2, Color.red);

                if (hit2D.collider == null)
                {
                    patrolPoints = new List<Vector3> { transform.position, transform.position + i * viewDistance / 2 };
                    patrolObject = false;
                    currentPoint = 1;
                    flagDirection = directions.IndexOf(GetDirection(i));
                    rb.linearVelocity = Vector2.zero;
                    currentState = EnemyState.Investigating;
                    lastFrameTime = Time.time;
                    animator.SetBool("Move", false);
                    break;
                }
            }
        }
    }

    private void Investigate()
    {
        if (Time.time - lastFrameTime > rotationTime)
        {
            int i = (directions.IndexOf(GetDirection(direction)) + 1) % 4;
            direction = directions[i];

            UpdateAnimation();

            if (i == flagDirection)
            {
                currentState = EnemyState.Patrolling;
                animator.SetBool("Move", true);
            }

            lastFrameTime = Time.time;
        }
    }

    private void ChasePlayer()
    {
        if (Vector3.Distance(chasePoint, transform.position) < 0.1f)
        {
            playerInVision = false;
            currentState = EnemyState.Investigating;
            flagDirection = directions.IndexOf(GetDirection(direction));
            lastFrameTime = Time.time;
            animator.SetBool("Move", false);
        }
        else if (Time.time - lastFrameTime < 2.5f)
        {
            animator.SetBool("Move", true);
            direction = (chasePoint - transform.position).normalized;
        }
        else
        {
            flagDirection = directions.IndexOf(GetDirection(direction));
            currentState = EnemyState.Investigating;
            lastFrameTime = Time.time;
            animator.SetBool("Move", false);
        }

    }

    private void PlaySpawnAnimation()
    {
        if (Time.time - lastFrameTime > 1)
        {
            int i = directions.IndexOf(GetDirection(direction));
            flagDirection = i;
            if (patrolObject)
            {
                currentObject = patrolObjects[0];
            }
            currentState = EnemyState.Patrolling;
            lastFrameTime = Time.time;
        }
    }

    Vector3 GetDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            return direction.y > 0 ? Vector3.up : Vector3.down;
        }
    }

    bool IsPlayerInFieldOfView()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetween = Vector3.Angle(direction, directionToPlayer);

        if (angleBetween < viewAngle / 2 && Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            return true;
        }
        return false;
    }
    //bool IsEnemyInFieldOfView()
    //{
    //    if (player == null) return false;

    //    Vector3 directionToPlayer = (player.position - transform.position).normalized;
    //    float angleBetween = Vector3.Angle(direction, directionToPlayer);

    //    if (angleBetween < viewAngle / 2 && Vector3.Distance(transform.position, player.position) < viewDistance)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    void LookForPlayer()
    {
        if (IsPlayerInFieldOfView())
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            int layerMask = LayerMask.GetMask("Default");
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, layerMask);

            Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
            //Debug.Log(hit2D.collider.tag);
            if (hit2D.collider != null && hit2D.collider.CompareTag("Player"))
            {
                playerInVision = true;
                chasePoint = player.position;
                currentState = EnemyState.Chasing;
                lastFrameTime = Time.time;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            currentState = EnemyState.Dying;
            GetComponent<CapsuleCollider2D>().enabled = false;
            animator.SetBool("IsDead", true);
            animator.SetBool("Move", false);
        }
        else
        {
            chasePoint = player.position;
            currentState = EnemyState.Chasing;
        }
        playerInVision = true;
        takeDamageSound.Play();
        StartCoroutine(Camera.main.GetComponent<ScreenShake>().Shake(0.3f, 0.05f));
        StartCoroutine(Flash());
    }

    public void IncreaseSpeed()
    {
        if (!InPool)
        {
            rb.AddForce(direction * speed / 2);
        }
    }

    //void OnTriggerEnter2D(Collider2D collider)
    //{

    //    if (collider.CompareTag("Floor"))
    //    {
    //        InPool = false;
    //        speed *= 3;
    //        animator.SetBool("InPool", false);
    //    }
    //}

    //void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (collider.CompareTag("Floor"))
    //    {
    //        InPool = true;
    //        speed /= 3;
    //        animator.SetBool("InPool", true);
    //    }
    //}

    private void UpdateAnimation()
    {
        _isMoving = (direction.x != 0 || direction.y != 0);
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Анимация движения

        // Управление звуком шагов
        if (_isMoving && distanceToPlayer < maxDistance)
        {

            float normalizedDist = Mathf.Clamp01(1 - (distanceToPlayer / maxDistance));
            footstepSound.volume = normalizedDist * normalizedDist; // Или Mathf.Pow(normalizedDist, 2)

            if (!_isFootstepSoundPlaying) // Если только начали движение
            {
                _isFootstepSoundPlaying = true;
                footstepSound.Play(); // Запускаем звук
            }

        }

        if (distanceToPlayer < 20 && !playerIsNear)
        {
            playerIsNear = true;
            Laughter.Play();
        }
        else if (distanceToPlayer > 20 && playerIsNear)
        {
            playerIsNear = false;
        }


        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private IEnumerator Flash()
    {

        float currentFlash = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlash = Mathf.Lerp(1f, 0f, elapsedTime / flashTime);
            material.SetFloat("Flash", currentFlash);
            yield return null;
        }
    }

    public void HearSound(bool coinInVision, Vector3 coinPosition)
    {
        if (coinInVision)
        {
            chasePoint = coinPosition;
            currentState = EnemyState.Chasing;
            lastFrameTime = Time.time;
        }
        else
        {
            flagDirection = directions.IndexOf(GetDirection(direction));
            animator.SetBool("Move", false);
            currentState = EnemyState.Investigating;
        }
    }
}
