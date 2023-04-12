using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    
    [Header("Limits")]
    [SerializeField] protected Vector2 xLimits;
    [SerializeField] protected Vector2 zLimits;

    public Vector3 direction;
    private Rigidbody rigidbody;


    public Player playerSettings;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Movement()
    {
        if (GameManager.Instance.isLevelStarted)
        {
            rigidbody.velocity = new Vector3(direction.x,direction.y,1f) * playerSettings.moveSpeed;
            
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    protected void RotateToDirection()
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), playerSettings.rotateSpeed * Time.deltaTime);

        }
    }

    public Vector3 LimitPosition(Vector3 pos)
    {
        float x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        

        Vector3 limitedPos = new Vector3(x, pos.y, pos.z);
        return limitedPos;
    }
}