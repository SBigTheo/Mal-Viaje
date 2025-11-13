using UnityEngine;

public class Patada : Ataque
{
    public float fuerzaRetroceso = 8f;
    public override void EjecutarAtaque()
    { }
    protected override void OnEnemyHit(GameObject enemigo)
    { }
}
