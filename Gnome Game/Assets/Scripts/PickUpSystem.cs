using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] float throwingPower = 10f;
    [SerializeField] float pickUpCooldown = 1.0f;
    private PlayerMovement _playerMovement;
    private CollectibleSpawner _collectibleSpawner;
    private InputActionAsset inputActionAsset;
    private InputAction _throwAction;
    private Transform _itemLocation;
    private GameObject _item;
    private GameObject _lastThrownItem;

    private bool _Throwing;
    private float _amountOfItemsHolding = 0;


    private void Start()
    {
        _collectibleSpawner = FindAnyObjectByType<CollectibleSpawner>();

        _playerMovement = GetComponent<PlayerMovement>();
        inputActionAsset = _playerMovement.inputActionAsset;

        _throwAction = inputActionAsset.FindActionMap("PlayerMovement").FindAction("Throw Item");
        _throwAction.Enable();

        _itemLocation = transform.Find("ItemLocation");
        if (_itemLocation == null)
        {
            Debug.LogError("ItemLocation not found");
        }
    }

    void Update()
    {
        _Throwing = _throwAction.ReadValue<float>() > 0;

        ThrowItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _lastThrownItem)
        {
            PickUpItem(other);
        }
    }

    private void PickUpItem(Collider other)
    {
        foreach (var item in _collectibleSpawner.collectibles)
        {
            if (item.objectTag == other.tag && _amountOfItemsHolding < 1)
            {
                _item = other.gameObject;
                _item.GetComponent<Collider>().enabled = false;
                _item.GetComponent<Rigidbody>().isKinematic = true;
                _item.transform.SetParent(_itemLocation);
                _item.transform.position = _itemLocation.position;
                _amountOfItemsHolding++;
            }
        }
    }
    private void ThrowItem()
    {
        if (_Throwing && _item != null)
        {
            _item.GetComponent<Collider>().enabled = true;
            _item.GetComponent<Rigidbody>().isKinematic = false;
            _item.transform.SetParent(null);
            Vector3 throwDirection = (transform.forward * throwingPower) + (transform.up * (throwingPower * 0.5f));
            _item.GetComponent<Rigidbody>().AddForce(throwDirection, ForceMode.Impulse);

            _lastThrownItem = _item;
            StartCoroutine(PickUpCooldown());

            _item = null;
            _amountOfItemsHolding--;
        }
    }

    private IEnumerator PickUpCooldown()
    {
        yield return new WaitForSeconds(pickUpCooldown);

        _lastThrownItem = null;
    }

    void OnDestroy()
    {
        _throwAction.Disable();
    }
}
