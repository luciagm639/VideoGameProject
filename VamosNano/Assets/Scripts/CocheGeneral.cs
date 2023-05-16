using java.lang;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CocheGeneral : MonoBehaviour
{
    public GameObject objetoConNavMesh;
    private NavMeshAgent agentDelObjeto;
    private Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        agentDelObjeto = objetoConNavMesh.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AlcanzarPosicion(objetoConNavMesh.transform.position, 1f, 1);
    }

    

    private void AlcanzarPosicion(Vector3 posObjetivo, float rapidezHorizontal, float propulsionFrontal)
    {
        Vector3 vectorHaciaObjetivo = posObjetivo - transform.position;
        float velocidadRelativa = agentDelObjeto.speed - rb.velocity.magnitude;
        float angulo = Vector3.Angle(vectorHaciaObjetivo, rb.velocity);

        if (velocidadRelativa > 0 || angulo < 70)
        {
            float factor = vectorHaciaObjetivo.magnitude * rapidezHorizontal;
            rb.AddForce(vectorHaciaObjetivo * propulsionFrontal * factor);
        } else
        {
            rb.velocity *= 0.95f;
        }

        rb.transform.LookAt(new Vector3(posObjetivo.x, rb.transform.localPosition.y, posObjetivo.z));
    }
}
