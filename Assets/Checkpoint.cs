using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject checkpointGround;
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    
    
    public int CheckpointCount;
    public TextMeshProUGUI CountText;

    private void Awake()
    {
        CountText.text = $"0 / " + CheckpointCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CheckpointCountCoroutine());
            GetComponent<Collider>().enabled = false;
            other.transform.parent.GetComponent<PlayerController>().playerSettings.moveSpeed = 0f;
        }
    }
    
    IEnumerator CheckpointCountCoroutine()
    {
        yield return new WaitForSeconds(2f);

            StartCoroutine(OpenDoorCoroutine());
            checkpointGround.SetActive(true);
            while (checkpointGround.transform.localPosition.y != 0)
            {
                yield return new WaitForFixedUpdate();
                checkpointGround.transform.localPosition = Vector3.MoveTowards(checkpointGround.transform.localPosition, new Vector3(checkpointGround.transform.localPosition.x, 0f, checkpointGround.transform.localPosition.z), 5f * Time.fixedDeltaTime);
            }

            GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponent<PlayerController>().playerSettings.moveSpeed = 10f;
          //TODO: üstteki buttonda sector güncellemesi yapılacak
       
    }
    
    IEnumerator OpenDoorCoroutine()
    {
        while (leftDoor.rotation.x != -90f)
        {
            leftDoor.rotation = Quaternion.Lerp(leftDoor.rotation, Quaternion.Euler(-90f, 90f, 90f), Time.deltaTime * 2f);
            rightDoor.rotation = Quaternion.Lerp(rightDoor.rotation, Quaternion.Euler(90f, 90f, 90f), Time.deltaTime * 2f);

            yield return null;
        }
    }
}
