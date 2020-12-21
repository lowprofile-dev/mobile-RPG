using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _accessTargetRange;
    private Vector3 _dirVec;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isInteracting) // 상호작용 중일때
        {
            _rigidbody.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 3 * Time.deltaTime);

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                _dirVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            }
        }

        //상호작용 중이 아닐때
        InteractWithForwardObject(); 
    }

    public void InteractWithForwardObject()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.DrawRay(_rigidbody.position, _dirVec, Color.green, 5);
            RaycastHit rayHit;
            if (Physics.Raycast(_rigidbody.position, _dirVec, out rayHit, 3.0f, LayerMask.GetMask("Object")))
            {
                NonLivingEntity entity = rayHit.transform.gameObject.GetComponent<NonLivingEntity>();

                if (entity != null)
                {
                    entity.Interaction();
                }
            }
        }
    }
}
