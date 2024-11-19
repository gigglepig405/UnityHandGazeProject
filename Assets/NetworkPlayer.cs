using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private PhotonView photonView;

    [SerializeField] private Transform headRig;
    [SerializeField] private Transform leftHandRig;
    [SerializeField] private Transform rightHandRig;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        XROrigin rig = FindObjectOfType<XROrigin>();

        if (rig != null)
        {
            headRig = rig.transform.Find("Camera Offset/Main Camera");
            leftHandRig = rig.transform.Find("Camera Offset/Left Controller");
            rightHandRig = rig.transform.Find("Camera Offset/Right Controller");

            if (photonView.IsMine)
            {
                foreach (var item in GetComponentsInChildren<Renderer>())
                {
                    item.enabled = false;
                }
            }

            Debug.Log("headRig: " + (headRig != null ? "Found" : "Not Found"));
            Debug.Log("leftHandRig: " + (leftHandRig != null ? "Found" : "Not Found"));
            Debug.Log("rightHandRig: " + (rightHandRig != null ? "Found" : "Not Found"));
        }
        else
        {
            Debug.LogError("XROrigin not found in the scene.");
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Ensure the head position is updated if headRig is found
            if (headRig != null)
            {
                MapPosition(head, headRig);
                Debug.Log("Head Position: " + head.position);
            }
            else
            {
                Debug.LogError("headRig is null, cannot update head position.");
            }

            InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            Debug.Log("Left Hand Device Found: " + leftHandDevice.isValid);
            Debug.Log("Right Hand Device Found: " + rightHandDevice.isValid);

            if (leftHandDevice.isValid && rightHandDevice.isValid)
            {
                MapPosition(leftHand, leftHandRig);
                MapPosition(rightHand, rightHandRig);

                UpdateHandAnimation(leftHandDevice, leftHandAnimator);
                UpdateHandAnimation(rightHandDevice, rightHandAnimator);
            }
            else
            {
                Debug.LogWarning("One or both hand devices are invalid.");
            }
        }
    }



    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            Debug.Log("Trigger value: " + triggerValue);
            handAnimator.SetFloat("Trigger", triggerValue);
            Debug.Log("Setting Trigger: " + triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
            Debug.Log("Setting Trigger: 0");
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            Debug.Log("Grip value: " + gripValue);
            handAnimator.SetFloat("Grip", gripValue);
            Debug.Log("Setting Grip: " + gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
            Debug.Log("Setting Grip: 0");
        }
    }


    void MapPosition(Transform target, Transform rigTransform)
    {
        if (target == null || rigTransform == null)
        {
            Debug.LogError("Target or RigTransform is null in MapPosition.");
            return;
        }

        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}