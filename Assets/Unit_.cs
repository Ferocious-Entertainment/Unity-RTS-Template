using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_ : MonoBehaviour
{
    public float Health = 10f;
    public string Name = "";
    public float Attack = 2f;

    public float MovementSpeed = 1f;

    public new MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
