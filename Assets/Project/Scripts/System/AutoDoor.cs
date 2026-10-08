using UnityEngine;
using System.Collections;

public class AutoDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorModel;
    [Tooltip("Góc muốn mở TƯƠNG ĐỐI so với lúc đóng ")]
    public Vector3 openRotationOffset = new Vector3(0, 90, 0);
    [Tooltip("Thời gian mở cửa (giây)")]
    public float openDuration = 0.5f;

    private Vector3 closedRotation;
    private Vector3 actualOpenRotation; 
    private bool isOpen = false;
    private Coroutine animationCoroutine;

    void Start()
    {
        if (doorModel != null)
        {
            closedRotation = doorModel.localEulerAngles;
            actualOpenRotation = closedRotation + openRotationOffset;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(AnimateDoor(actualOpenRotation));
            isOpen = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(AnimateDoor(closedRotation));
            isOpen = false;
        }
    }

    private IEnumerator AnimateDoor(Vector3 targetRot)
    {
        Quaternion startRot = doorModel.localRotation;
        Quaternion endRot = Quaternion.Euler(targetRot);
        float elapsed = 0;

        while (elapsed < openDuration)
        {
            doorModel.localRotation = Quaternion.Slerp(startRot, endRot, elapsed / openDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        doorModel.localRotation = endRot;
    }
}