using Unity.VisualScripting;
using Unity.Netcode;
using UnityEngine;
using System;
using Unity.Collections;
using Random = UnityEngine.Random;

public class PlayerMovementScript : NetworkBehaviour
{
    private NetworkVariable<int> myValue = new NetworkVariable<int>();
    private NetworkVariable<MyCustomVar> myNewVar = new NetworkVariable<MyCustomVar>();

    private struct MyCustomVar: INetworkSerializable
    {
        private FixedString128Bytes chatString;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref chatString);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        HandleMovement(); 
    }

    //when the player spawns in
    public override void OnNetworkSpawn()
    {
       myValue.Value = Random.Range(1, 200);
        Debug.Log("My Test Int: " + myValue.ToString());
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
