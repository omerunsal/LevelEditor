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
        CountText.text = $"{floor.GetComponent<BasketCollectableCounter>().CollectedCount} / " +
                         CheckpointCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CheckpointCountCoroutine());
            GetComponent<Collider>().enabled = false;
            GameManager.Instance.isLevelStarted = false;
        }
    }

    IEnumerator CheckpointCountCoroutine()
    {
        yield return new WaitForSeconds(2f);

        if (floor.GetComponent<BasketCollectableCounter>().CollectedCount >= CheckpointCount)
        {
            GameManager.Instance.CompletedLevelSectorCount++;
            StartCoroutine(OpenDoorCoroutine());
            checkpointGround.SetActive(true);

            float duration = 2.5f;
            Vector3 targetPos = new Vector3(checkpointGround.transform.localPosition.x, 0f,
                checkpointGround.transform.localPosition.z);
            yield return checkpointGround.transform.DOLocalMove(targetPos, duration).WaitForCompletion();

            GameManager.Instance.isLevelStarted = true;
            yield return null;

        }
        else
        {
            GameManager.Instance.LevelFail();
        }
    }

    IEnumerator OpenDoorCoroutine()
    {
        if (floor.GetComponent<BasketCollectableCounter>().CollectedCount >= CheckpointCount)
        {
            leftDoor.transform.DORotate(new Vector3(-60f, 90f, 90f), 1f);
            rightDoor.transform.DORotate(new Vector3(60f, 90f, 90f), 1f);

            yield return null;
        }
        else
        {
            GameManager.Instance.LevelFail();
        }
    }
}