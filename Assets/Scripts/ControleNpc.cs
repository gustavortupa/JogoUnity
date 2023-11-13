using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleNpc : MonoBehaviour
{
    //ponto de destino
    private Vector3 target;
    //velocidade
    private float speed = 1.0F;

    // Start is called before the first frame update
    void Start()
    {
        //inicia para nova direcao
        novaDirecao();
    }

    void novaDirecao() 
    {
        target = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        transform.rotation = Quaternion.LookRotation(target);
    }


    // Update is called once per frame
    void Update()
    {
        //move NPC
        float step =  speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    void OnCollisionEnter(Collision collision) {
        //vai para nova direção caso haja colisão
        //importante que o objeto tenha um RigidBody 
        //desabilite a rotação XYZ
        novaDirecao();
    }


}
