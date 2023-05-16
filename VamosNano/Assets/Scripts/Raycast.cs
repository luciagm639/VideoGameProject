using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public int identificador;
    private NavMesh scriptNavMesh;

    // Start is called before the first frame update
    void Start()
    {
        scriptNavMesh = this.gameObject.GetComponentInParent<NavMesh>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            print("ESTOY DETECTANDO PUNTOS");
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            string tag = hit.collider.gameObject.tag;
            Vector3[] puntosDetectados = scriptNavMesh.GetPuntosDetectados();

            if ((tag.Equals("carreteraExterior") || tag.Equals("bordeCarretera") || tag.Equals("carreteraInterior")) 
                && puntosDetectados[identificador].Equals(Vector3.negativeInfinity)) {
                Vector3 puntoDetectado = hit.collider.transform.position;
                puntosDetectados[identificador] = puntoDetectado;

            } else if (puntosDetectados[identificador].Equals(Vector3.negativeInfinity))
            {
                puntosDetectados[identificador] = Vector3.positiveInfinity;
            }
        }
    }
}
