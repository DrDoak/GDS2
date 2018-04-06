using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Property : MonoBehaviour
{

    protected abstract void OnCreation();
    protected abstract void OnHit();
    protected abstract void OnHitConfirm();
    protected abstract void OnDeath();
    protected abstract void OnUpdate();
    protected abstract void OnCollision();
    protected abstract void OnAttack();

}
