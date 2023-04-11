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
        rigidbody.velocity = direction * playerSettings.moveSpeed;
        // rigidbody.velocity = Vector3.forward * playerSettings.moveSpeed;
        LimitPosition(transform.position); 
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