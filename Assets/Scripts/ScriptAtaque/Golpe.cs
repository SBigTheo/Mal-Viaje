using UnityEngine;

public class Golpe : Ataque
{
    public float fuerzaEmpuje = 5f;
    public override void EjecutarAtaque()
    { }
    protected override void OnEnemyHit(GameObject enemigo)
    { }
}
