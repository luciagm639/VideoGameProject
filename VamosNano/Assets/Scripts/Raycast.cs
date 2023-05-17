using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public int identificador;

    public GameObject objetoConNavMesh;
    private NavMeshObjectScript navMeshScript;

    private bool puedoRegistrarPunto;

    // Start is called before the first frame update
    void Start()
    {
        puedoRegistrarPunto = true;
        navMeshScript = objetoConNavMesh.GetComponent<NavMeshObjectScript>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            string tag = hit.collider.gameObject.tag;

            Vector3 puntoEnArray = navMeshScript.GetPosicionArray(identificador);

            if (puntoEnArray.Equals(Vector3.negativeInfinity) && puedoRegistrarPunto)
            {
                if (tag.Equals("carreteraExterior") || tag.Equals("bordeCarretera") || tag.Equals("carreteraInterior"))
                {
                    Vector3 puntoDetectado = hit.collider.transform.position;
                    navMeshScript.SetPosicionArray(puntoDetectado, identificador);
                } else
                {
                    navMeshScript.SetPosicionArray(Vector3.positiveInfinity, identificador);
                }
                puedoRegistrarPunto = false;
            }
        }
    }

    public void SetPuedoRegistrarPunto(bool puedo)
    {
        puedoRegistrarPunto = puedo;
    }
}
