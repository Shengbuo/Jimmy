using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CharacterController cc;
    private float changeFB = 0f, changeLR = 0f;
    private float curGravity = 0f;
    private float bodyFacing = 0f;

    public float gravity;
    public float maxGravity;
    public float jumpVel;
    public float moveSpeed;

    //Check if grounded by raycasting
    public bool isGrounded()
    {
        return Physics.Linecast(transform.position, transform.position + Vector3.down * 1.02f);
    }

    void rotateBody()
    {
        float mouseX = Input.GetAxis("Mouse X") * (OptIonsFunctions.MouseSens);
        bodyFacing -= mouseX;
        bodyFacing = Mathf.Repeat(bodyFacing, 360f);
        this.transform.rotation = Quaternion.Euler(0f, -bodyFacing, 0f);
    }

    private Vector3 applyGravity()
    {
        if (!isGrounded())
        {
            if (curGravity < maxGravity) {curGravity += gravity * Time.deltaTime;}
        }
        else {curGravity = gravity;}

        return Vector3.down * curGravity;
    }

    //Movement key for velocity
    private Vector3 movement()
    {
        changeFB = 0f; changeLR = 0f;
        
        if (Input.GetKey(KeyCode.W)) {changeFB +=  1f;}
        if (Input.GetKey(KeyCode.S)) {changeFB += -1f;}
        if (Input.GetKey(KeyCode.A)) {changeLR += -1f;}
        if (Input.GetKey(KeyCode.D)) {changeLR +=  1f;}
        
        
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space)) {jump();}

        Vector3 FBVel = transform.forward * changeFB;
        Vector3 LRVel = transform.right * changeLR;

        Vector3 moveVel = (FBVel + LRVel) * moveSpeed;

        return moveVel;
    }


    //Add jump velocity
    void jump()
    {
        cc.Move(Vector3.up * 0.1f);
        curGravity -= jumpVel;
    }

    //Apply the velocities
    void applyMovement()
    {
        cc.Move(movement() * Time.deltaTime);
        cc.Move(applyGravity() * Time.deltaTime);
    }
    
    // Start is called before the first frame update
     void Start()
    {
        //Assign variable and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cc = this.GetComponent<CharacterController>();
        bodyFacing = -this.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        applyMovement();
        rotateBody();
    }
}
