using System;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] List<String> Collectibles;

    private Transform _itemLocation;
    private GameObject _item;


    private void Start()
    {
        _itemLocation = transform.Find("ItemLocation");
        if (_itemLocation == null)
        {
            Debug.LogError("ItemLocation not found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Collectibles.Contains(other.tag))
        {
            Debug.Log("Picked up " + other.tag);
            _item = other.gameObject;
            _item.GetComponent<Collider>().enabled = false;
            _item.GetComponent<Rigidbody>().isKinematic = true;
            _item.transform.SetParent(_itemLocation);
            _item.transform.position = _itemLocation.position;
        }
    }
}
