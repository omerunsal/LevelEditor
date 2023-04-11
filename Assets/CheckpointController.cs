using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private GameObject checkpointGround;
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private GameObject floor;

    public int CheckpointCount;
    public TextMeshProUGUI CountText;

    private void Awake()
    {
    }

    private void Update()
    {
        CountText.text = $"{floor.GetComponent<BasketCollectableCount>().CollectedCount} / " +
                         CheckpointCount.ToString();
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
        yield return new WaitForSeconds(1f);

        if (floor.GetComponent<BasketCollectableCount>().CollectedCount >= CheckpointCount)
        {
            GameManager.instance.CompletedLevelSectorCount++;
            StartCoroutine(OpenDoorCoroutine());
            checkpointGround.SetActive(true);

            float duration = 2.5f;
            Vector3 targetPos = new Vector3(checkpointGround.transform.localPosition.x, 0f,
                checkpointGround.transform.localPosition.z);
            yield return checkpointGround.transform.DOLocalMove(targetPos, duration).WaitForCompletion();

            GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponent<PlayerController>().playerSettings
                .moveSpeed = 15f;
            yield return null;

        }
        else
        {
            GameManager.instance.LevelFail();
        }
    }

    IEnumerator OpenDoorCoroutine()
    {
        if (floor.GetComponent<BasketCollectableCount>().CollectedCount >= CheckpointCount)
        {
            leftDoor.transform.DORotate(new Vector3(-60f, 90f, 90f), 1f);
            rightDoor.transform.DORotate(new Vector3(60f, 90f, 90f), 1f);

            yield return null;
        }
        else
        {
            GameManager.instance.LevelFail();
        }
    }
}