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

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }
    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
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