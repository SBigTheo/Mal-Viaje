using UnityEngine;

public class JefeCaminando : StateMachineBehaviour
{
    private Jefe jefe;
    private Rigidbody2D rb2D;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distanciaAtaque = 3f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            jefe = animator.GetComponent<Jefe>();
        
        if (jefe == null)
        {
            Debug.LogError("No se encontró el componente Jefe");
            return;
        }
    
        rb2D = jefe.rb2D != null ? jefe.rb2D : animator.GetComponent<Rigidbody2D>();
        
        if (jugador == null)
        {
            GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
            if (jugadorObj != null) jugador = jugadorObj.transform;
        }
        
        jefe.MirarJugador();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rb2D == null || jugador == null) return;
        
        Vector2 direccion = (jugador.position - animator.transform.position).normalized;
        
        // Mover hacia el jugador
        rb2D.linearVelocity = new Vector2(direccion.x * velocidadMovimiento, rb2D.linearVelocity.y);
        
        // Verificar si está en rango de ataque
        float distancia = Vector2.Distance(animator.transform.position, jugador.position);
        if (distancia <= distanciaAtaque)
        {
            animator.SetTrigger("Atacar");
        } 
    }    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
