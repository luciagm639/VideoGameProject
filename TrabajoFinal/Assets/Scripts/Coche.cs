using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Coche : MonoBehaviour
{
    public GameObject objetoConNavMesh;
    public GameObject rayos;

    private Rigidbody rb;
    private float velRotacion = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AlcanzarPosicion(objetoConNavMesh.transform.position, 1, 1);
    }

    private void AlcanzarPosicion(Vector3 posObjetivo, float rapidezHorizontal, float propulsionFrontal)
    {
        Vector3 vectorHaciaObjetivo = posObjetivo - this.transform.position;
        float velocidadRelativa = objetoConNavMesh.GetComponent<NavMeshAgent>().velocity.magnitude - rb.velocity.magnitude;
        float angulo = Vector3.Angle(vectorHaciaObjetivo, rb.velocity);

        if (velocidadRelativa > 0 || angulo < 70)
        {
            float factor = vectorHaciaObjetivo.magnitude * rapidezHorizontal;
            rb.AddForce(vectorHaciaObjetivo * propulsionFrontal * factor);
        } else
        {
            rb.velocity *= 0.95f;
        }

        RotateToNavMeshObject(posObjetivo);
    }

    private void RotateToNavMeshObject(Vector3 posObjetivo)
    {
        var qTo = Quaternion.LookRotation(posObjetivo - transform.position);
        qTo = Quaternion.Slerp(transform.rotation, qTo, velRotacion * Time.deltaTime);
        rb.MoveRotation(qTo);
    }

    public void NotificarRayos()
    {
        Raycast[] rayosHijos = rayos.GetComponentsInChildren<Raycast>();

        foreach (Raycast r in rayosHijos)
        {
            r.SetPuedoRegistrarPunto(true);
        }
    }
}
