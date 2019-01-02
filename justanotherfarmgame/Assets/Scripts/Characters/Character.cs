using UnityEngine;

public abstract class Character : MonoBehaviour {
    [SerializeField]
    protected float movementSpeed;
    protected Vector2 direction;

    protected Rigidbody2D myRigidbody;
    protected Animator myAnimator;

    protected bool isMoving {
        get {
            return direction.sqrMagnitude != 0;
        }
    }

    private void Start() {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    protected virtual void Update() {
        AnimateMovement();
    }

    protected virtual void FixedUpdate() {
        Move();
    }

    private void AnimateMovement() {
        if(isMoving) {
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            myAnimator.SetLayerWeight(1, 1f);
        } else {
            myAnimator.SetLayerWeight(1, 0f);
        }
    }

    protected void Move() {
        myRigidbody.velocity = direction.normalized * movementSpeed;
    }
}
