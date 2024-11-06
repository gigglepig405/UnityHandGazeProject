using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabNetworkInteractable : XRGrabInteractable
{
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(OnSelectEnter);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(OnSelectEnter);
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        photonView.RequestOwnership();
    }
}
