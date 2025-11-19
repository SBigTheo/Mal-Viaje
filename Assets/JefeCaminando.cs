using UnityEngine;

public class JefeCaminando : StateMachineBehaviour
{
    private Jefe jefe;
    private Rigidbody2D rb2D;
    [SerializeField] private float velocidadMovimiento;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            jefe = animator.GetComponent<Jefe>();
    // Obtener Rigidbody2D directamente si jefe.rb2D es null
            rb2D = jefe.rb2D != null ? jefe.rb2D : animator.GetComponent<Rigidbody2D>();
            jefe.MirarJugador();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float direccion = animator.transform.right.x > 0 ? 1 : -1;
        rb2D.linearVelocity = new Vector2(velocidadMovimiento * direccion, rb2D.linearVelocity.y);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
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
