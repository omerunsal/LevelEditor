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
        // direction.z = 1f;
        if (GameManager.Instance.isLevelStarted)
        {
            rigidbody.velocity = new Vector3(direction.x,direction.y,1f) * playerSettings.moveSpeed;
            LimitPosition(transform.position); 
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }
        
        
        // Vector3 forwardMove = Vector3.forward * playerSettings.moveSpeed;
        // rigidbody.velocity = forwardMove + new Vector3(direction.x, direction.y, 0f);
        // rigidbody.MovePosition(rigidbody.position+forwardMove+ new Vector3(direction.x,direction.y,0f));
       
        // rigidbody.velocity = Vector3.forward * playerSettings.moveSpeed;
        
    }

    protected void RotateToDirection()
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), playerSettings.rotateSpeed * Time.deltaTime);

        }
    }

    private Vector3 LimitPosition(Vector3 pos)
    {
        float x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        float z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);

        Vector3 limitedPos = new Vector3(x, pos.y, z);
        return limitedPos;
    }
}