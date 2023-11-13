using UnityEngine;
using UnityEngine.SceneManagement;

//adiciona o componente CharacterController automaticamente
[RequireComponent(typeof(CharacterController))]

public class ControleJogador : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public Animator animator;
    public AudioSource Music, Coin, Jump, Star;
    public bool star;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    void Start()
    {
        star = false;
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        Music = GetComponents<AudioSource>()[0];
        Jump = GetComponents<AudioSource>()[1];
        Coin = GetComponents<AudioSource>()[2];
        Star = GetComponents<AudioSource>()[3];
    }
    void Update()
    {
        if (characterController.isGrounded)
        {
            // Se o jogador estiver no chão, então pode se mover
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX =  speed * Input.GetAxis("Vertical");
            float curSpeedY = speed * Input.GetAxis("Horizontal");
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);


            int lat = (int)curSpeedY;
            animator.SetInteger("lateral", lat);
            int vel = (int)curSpeedX;
            animator.SetInteger("velocidade", vel);



            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                animator.SetBool("pulando",true);
                Jump.Play();
            } else{
                animator.SetBool("pulando", false);
            }

            if (Input.GetKey(KeyCode.LeftShift)){
                animator.SetBool("correr", true);
            }
            else{
                animator.SetBool("correr", false);
            }

        }

        // Aplica gravidade
        moveDirection.y -= gravity * Time.deltaTime;

        // Move o jogador
        characterController.Move(moveDirection * Time.deltaTime);

        //Gira a Câmera para os lados
            if (Input.GetButton("Fire1")) {
                rotation.y -= 1 * lookSpeed;
            } else if (Input.GetButton("Fire2")) { 
            rotation.y += 1 * lookSpeed;
            } else {
                rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            }

        // Gira a Câmera para cima
        rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
        playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, rotation.y);

    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Moeda")) {
            other.gameObject.SetActive(false);
            Coin.Play();
            star = false;
        } else if(other.gameObject.CompareTag("Star")){
            other.gameObject.SetActive(false);
            star = true;
            Music.Stop();
            Star.Play();
        } else if(other.gameObject.CompareTag("Npc")){
            if(star==true){
                other.gameObject.SetActive(false);
            } else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}