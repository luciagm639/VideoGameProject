using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    private Vector3[] puntosDetectados;
    private NavMeshAgent agentDelObjeto;

    // Start is called before the first frame update
    void Start()
    {
        agentDelObjeto = this.gameObject.GetComponent<NavMeshAgent>();

        puntosDetectados = new Vector3[7];

        for (int i = 0; i < puntosDetectados.Length; i++)
        {
            puntosDetectados[i] = Vector3.negativeInfinity;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if (ArrayLleno())
        {
            Vector3 puntoMedio = CalcularPuntoMedio();
            agentDelObjeto.SetDestination(puntoMedio);
            SetPuntosANull();
        }
    }

    public Vector3[] GetPuntosDetectados()
    {
        return puntosDetectados;
    }

    private bool ArrayLleno()
    {
        int i = 0;
        while (i < puntosDetectados.Length && !puntosDetectados[i].Equals(Vector3.negativeInfinity)) i++;
        return !(i < puntosDetectados.Length);
    }

    private Vector3 CalcularPuntoMedio()
    {
        float x = 0.0f, y = 0.0f, z = 0.0f;
        float numPuntos = 0;

        foreach (Vector3 v in puntosDetectados)
        {
            if (!v.Equals(Vector3.positiveInfinity))
            {
                numPuntos++;
                x += v.x;
                y += v.y;
                z += v.z;
            }
        }

        return new Vector3(x / numPuntos, y / numPuntos, z / numPuntos);
    }

    private void SetPuntosANull()
    {
        for (int i = 0; i < puntosDetectados.Length; i++)
        {
            puntosDetectados[i] = Vector3.negativeInfinity;
        }
    }

}
