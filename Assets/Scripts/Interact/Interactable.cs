using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public interface IInteractable{
    abstract void OnInteractBegin(InputAction.CallbackContext context);
    abstract void OnInteract();
    abstract void OnInteractEnd();
}
public abstract class Interactable : MonoBehaviour,IInteractable
{
    private Interactor Interactor;
    public float distanceForInteract; 
    private bool isInteractable=false;
    public bool IsInteractable{
        set{
            if(value&&!isInteractable){
                BindingChange.Instance.inputControl.FindAction("ChangeItem").started+=OnInteractBegin;
            }
            else if(!value&&isInteractable){
                BindingChange.Instance.inputControl.FindAction("ChangeItem").started-=OnInteractBegin;
            }
            isInteractable = value;
        }
        get{ return isInteractable;}
    }

    public virtual void OnInteract()
    {
        throw new NotImplementedException();
    }

    public virtual void OnInteractBegin(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public virtual void OnInteractEnd()
    {
        throw new NotImplementedException();
    }

    protected void Start() {
        BindingChange.Instance.inputControl.FindAction("ChangeItem").started+=OnInteractBegin;
        Interactor=Interactor.Instance;
    }
    protected void Update(){
        if((Interactor?.transform.position-transform.position).Value.magnitude<distanceForInteract){
            IsInteractable=true;
        }
        else{
            IsInteractable=false;
        }
    }
    
}
