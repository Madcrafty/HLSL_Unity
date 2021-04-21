using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Scripts.Movement
{
    public class PlayerMovement : Entity
    {
        #region PRIVATE FIELDS
        // Debug Fields
        private bool m_playerIsJumping;
        private float m_currentSpeed;
        private float m_distanceFromPlayerToGround;
        private bool m_playerIsGrounded;
        private bool m_playerJumpStarted;
        private int m_currentNumberOfJumpsMade; //current number of jumps processed

        // Player Movement Variables
        private float m_xAxis;
        private float m_zAxis;
        private Rigidbody m_rb;
        private Collider m_playerCapsule;
        private RaycastHit m_hit;
        private Vector3 m_groundLocation;
        private bool m_leftShiftPressed;
        private int m_groundLayerMask;


        // Entity Variables
        private float healRate = 10.0f;
        private float elapsedHitTime;
        private Animator animator;

        #endregion

        #region PUBLIC FIELDS
        [Header(header: "Aim Settings")]
        public float lookSpeed = 2.0f;
        public Transform cam;
        public float turnSmoothTime = 0.1f;

        [Header(header: "Walk / Run Settings")]
        public float walkSpeed;
        public float runSpeed;

        [Header(header: "Jump Settings")]
        public float playerJumpForce;
        public int MaxAllowJump = 2; //maximum allowed jumps
        public ForceMode appliedForceMode;
        public LayerMask groundLayerMask;

        //[Header(header: "Panel")]
        //public Image Panel;

        #endregion

        #region MONODEVELOP ROUTINES
        private void Awake()
        {
            m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        protected override void Start()
        {
            #region initializing components

            base.Start();
            m_rb = GetComponent<Rigidbody>();
            animator = transform.GetChild(1).GetComponent<Animator>();
            m_playerCapsule = GetComponent<Collider>();

            #endregion
            #region jump press initiated

            m_playerJumpStarted = true;

            #endregion
        }

        private void Update()
        {
            #region controller Input [horizontal | vertical ] movement

            m_xAxis = Input.GetAxis("Horizontal");
            animator.SetFloat("Xpos", m_xAxis);
            m_zAxis = Input.GetAxis("Vertical");
            animator.SetFloat("Ypos", m_zAxis);

            #endregion
            #region adjust player move speed [walk | run]

            m_currentSpeed = m_leftShiftPressed ? runSpeed : walkSpeed;

            #endregion

            #region Camera Movement
            // Normally this would control the camera, but because I am using a
            // cinemachine's thrid person camera I just need to rotate to face
            // the front
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            #endregion

            #region Health Regeneration
            if (m_hp < maxHealth)
            {
                elapsedHitTime += Time.deltaTime;
                m_hp += healRate * elapsedHitTime * Time.deltaTime;
            }
            if (m_hp > maxHealth)
            {
                m_hp = maxHealth;
            }
            #endregion

            // Read inputs to be calculated in FixedUpdate
            #region play jump input

            m_playerIsJumping = Input.GetButton("Jump");

            #endregion
            #region Shift To Change Speed

            m_leftShiftPressed = Input.GetKey(KeyCode.LeftShift);

            #endregion

            #region compute player distance from ground
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raycastDistance, Color.blue); 
            // added layermask for those dealing with complex ground objects.
            // It casts from the centre of the player downward, radius equal to horizontal extents
            if (Physics.SphereCast(m_playerCapsule.bounds.center, m_playerCapsule.bounds.extents.x, transform.TransformDirection(Vector3.down), out m_hit, Mathf.Infinity,groundLayerMask))
            {
                m_groundLocation = m_hit.point;
                m_distanceFromPlayerToGround = m_playerCapsule.bounds.ClosestPoint(m_groundLocation).y - m_groundLocation.y;
            }
            #endregion
        }
        private void FixedUpdate()
        {
            #region move player
            // Move the position of the entity, direction normalised to prevent double speed;
            m_rb.MovePosition(transform.position + Time.deltaTime * m_currentSpeed * Vector3.Normalize(transform.TransformDirection(m_xAxis, 0f, m_zAxis)));
            // When player moves in the opposite direction to their current velocity
            // Add counter force to enable player to slow down
            if (m_xAxis * m_rb.velocity.x < 0)
            {
                m_rb.AddForce(Time.deltaTime * m_currentSpeed * transform.TransformDirection(m_xAxis, 0f, 0f), ForceMode.VelocityChange);
            }
            if (m_zAxis * m_rb.velocity.z < 0)
            {
                m_rb.AddForce(Time.deltaTime * m_currentSpeed * transform.TransformDirection(0f, 0f, m_zAxis), ForceMode.VelocityChange);
            }
            #endregion

            #region apply single / double jump
            // check if player is grounded
            m_playerIsGrounded = m_distanceFromPlayerToGround <= 0f;
            // The player needs to be pressing the jump key, the last jump input has gone through and the player has to be grounded or have more jumps left
            if (m_playerIsJumping && m_playerJumpStarted && (m_playerIsGrounded || MaxAllowJump > m_currentNumberOfJumpsMade))
                StartCoroutine(ApplyJump());

            if (m_playerIsGrounded)
                m_currentNumberOfJumpsMade = 0;

            #endregion
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Incarnate"))
            {
                SceneManager.LoadScene(0);
            }
        }

        #region Base Entity Overrides
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            elapsedHitTime = 0;
        }
        protected override void Die()
        {
            base.Die();
            m_gameManager.GameOver();
        }
        public override void RagdollState(bool toggle)
        {

        }
        #endregion

        #endregion

        #region HELPER ROUTINES

        /// <summary>
        /// applies force in the upward direction
        /// using jump force and supplied force mode
        /// </summary>
        /// <param name="jumpForce"></param>
        /// <param name="forceMode"></param>

        private void PlayerJumps(float jumpForce, ForceMode forceMode)
        {
            m_rb.AddForce(jumpForce * m_rb.mass * Time.deltaTime * Vector3.up, forceMode);
        }

        /// <summary>
        /// handles single and double jump
        /// waits until space bar pressed is terminated before
        /// next jump is initiated
        /// </summary>
        private IEnumerator ApplyJump()
        {
            PlayerJumps(playerJumpForce, appliedForceMode);
            m_playerIsGrounded = false;
            m_playerJumpStarted = false;
            yield return new WaitUntil(() => !m_playerIsJumping);
            ++m_currentNumberOfJumpsMade;
            m_playerJumpStarted = true;
        }
        #endregion
    }
}