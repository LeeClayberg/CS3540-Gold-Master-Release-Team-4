using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewCharacterController : MonoBehaviour
{
    public int currentPlayer = 0;
    public int currentMaxPlayer = 0;
    public Material[] materials = new Material[4];
    public Color[] colors = new Color[4];
    public Sprite[] powerSymbols = new Sprite[4];
    public string[] powerLabels = {"Standard", "Size Reduction", "Gravity Control", "Double Jump" };

    CharacterController controller;
    SoundHandler soundHandler;
    PlayerHealth health;
    public Image minimapSurround;
    public Image minimapCompass;
    public Image PowerBackground;
    public Image PowerSymbol;
    public Image PowerLabelRectangle;
    public Text PowerLabel;
    public Text info;
    public float movementSpeed = 5f;
    public float gravity = 9.8f;
    public float jumpHeight = 1f;
    public float airControl = 2f;
    public GameObject deathEffect;
    Vector3 moveDirection;
    Vector3 groundDirection;
    public float flippedNegator;
    private bool flipped;
    public bool shrunken;
    private float doubleJump = 2f;
    private bool isDead = false;


    //add life for player
    public int maxLifeCount;
    private int currentLife;
    public Text life;
    public GameController gameController;
    private float currentSpeed;

    //Shield
    public static bool shieldUp = false;
    public GameObject shieldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        soundHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SoundHandler>();
        health = GetComponent<PlayerHealth>();
        groundDirection = Vector3.down;
        flippedNegator = 1;
        flipped = false;
        shrunken = false;
        setColor(currentPlayer);

        currentLife = maxLifeCount;
        SetLifeAmount();
        currentSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceUnder = transform.GetChild(0).localScale.y + transform.GetChild(0).localScale.y / 100.0f;
        bool grounded = Physics.Raycast(transform.GetChild(0).position, groundDirection, distanceUnder);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        var input = transform.right * moveHorizontal * flippedNegator + transform.forward * moveVertical;
        var character = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0);

        character.GetComponent<Animator>().SetInteger("velocityY", (int)controller.velocity.y);
        character.GetComponent<Animator>().SetBool("isGrounded", grounded);
        var anim = character.GetComponent<Animator>();

        if (!GameController.isGameOver && !isDead && !PauseMenuBehavior.isGamePaused)
        {
            soundHandler.PlayBackground();

            if (grounded)
            {
                if (moveHorizontal > 0)
                {
                    soundHandler.PlayRunningSound();
                    anim.Play("walking left");
                }
                else if (moveHorizontal < 0)
                {
                    soundHandler.PlayRunningSound();
                    anim.Play("walking right");
                }
                else if (Mathf.Approximately(moveVertical, 0))
                {
                    soundHandler.StopSound();
                    anim.Play("idle");
                }
                else
                {
                    soundHandler.PlayRunningSound();
                    anim.Play("running");
                }
            }

            input *= currentSpeed;
            
            if (grounded)
            {
                moveDirection = input;
                doubleJump = 2;

                if (Input.GetButtonDown("Jump"))
                {
                    soundHandler.PlayJumpSound();
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(gravity)) * flippedNegator;
                    doubleJump--;
                }
                else
                {
                    moveDirection.y = 0f;
                }
            }
            else
            {
                if (doubleJump > 0 && Input.GetButtonDown("Jump") && currentPlayer == 3)
                {
                    soundHandler.PlayJumpSound();
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(gravity)) * flippedNegator;
                    doubleJump--;
                }
                else
                {
                    input.y = moveDirection.y;
                    moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
                }
            }

            moveDirection.y -= gravity * Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime);

            CharacterController cc = transform.GetComponent<CharacterController>();
            cc.stepOffset = cc.height / 4 + cc.height / 40;

            if (Input.GetKeyDown(KeyCode.F) && grounded && currentPlayer == 2)
            {
                gravity *= -1;
                groundDirection *= -1;
                flippedNegator *= -1;

                if (flipped)
                {
                    soundHandler.PlayGravitySwitchSound();
                    transform.GetChild(0).GetComponent<Animator>().SetTrigger("FlipRightSideUp");
                    transform.GetChild(0).GetComponent<Animator>().ResetTrigger("FlipUpsideDown");
                }
                else
                {
                    soundHandler.PlayGravitySwitchSound();
                    transform.GetChild(0).GetComponent<Animator>().SetTrigger("FlipUpsideDown");
                    transform.GetChild(0).GetComponent<Animator>().ResetTrigger("FlipRightSideUp");
                }
                flipped = !flipped;
            }

            if (Input.GetKeyDown(KeyCode.F) && grounded && currentPlayer == 1)
            {
                if (shrunken)
                {
                    soundHandler.PlayGrowSound();
                    transform.GetChild(0).GetComponent<Animator>().SetTrigger("Grow");
                    transform.GetChild(0).GetComponent<Animator>().ResetTrigger("Shrink");
                    currentSpeed = 5;
                    jumpHeight = 1;
                }
                else
                {
                    soundHandler.PlayShrinkSound();
                    transform.GetChild(0).GetComponent<Animator>().SetTrigger("Shrink");
                    transform.GetChild(0).GetComponent<Animator>().ResetTrigger("Grow");
                    currentSpeed = 2.5f;
                    jumpHeight = 0.5f;
                }
                shrunken = !shrunken;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currentPlayer < 3 && currentPlayer < currentMaxPlayer)
                {
                    currentPlayer++;
                }
                else
                {
                    currentPlayer = 0;
                }
                setColor(currentPlayer);
            }

            if (Input.GetKeyDown(KeyCode.Q) && !shieldUp)
            {
                Vector3 pos = transform.GetChild(0).transform.position;
                GameObject shield = Instantiate(shieldPrefab, pos, transform.rotation) as GameObject;
                shield.transform.SetParent(transform);
                shield.layer = 2;
                soundHandler.PlayShieldSound();
            }

            controller.center = transform.GetChild(0).transform.localPosition;
            controller.height = Mathf.Round(transform.GetChild(0).transform.localScale.y * 2 * 10.0f) / 10.0f;
            controller.radius = Mathf.Round(transform.GetChild(0).transform.localScale.x / 2 * 10.0f) / 10.0f;

            RaycastHit hitLava = new RaycastHit();
            if (Physics.Raycast(transform.GetChild(0).position, groundDirection, out hitLava, distanceUnder + 0.2f))
            {
                if (hitLava.collider.CompareTag("Lava"))
                {
                    isDead = true;
                    GameObject ps = Instantiate(deathEffect, hitLava.point, Quaternion.Euler(-groundDirection))
                        as GameObject;
                    Destroy(ps, 0.3f);
                    Invoke("PlayerDied", 0.25f);
                }
            }

            if (transform.position.y > 80 || transform.position.y < -30)
            {
                PlayerDied();
            }

            if (currentMaxPlayer >= 1)
            {
                info.enabled = true;
            }

            Quaternion rotationVector = new Quaternion(transform.rotation.x, transform.rotation.z, transform.rotation.y, transform.rotation.w);
            minimapCompass.transform.rotation = rotationVector;
        } else
        {
            soundHandler.StopSound();
            anim.Play("idle");
        }
    }

    void setColor(int index)
    {
        var character = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(1);
        character.GetComponent<Renderer>().material = materials[index];
        minimapSurround.GetComponent<Image>().color = colors[index];
        PowerBackground.GetComponent<Image>().color = colors[index];
        PowerLabelRectangle.GetComponent<Image>().color = colors[index];
        PowerSymbol.GetComponent<Image>().sprite = powerSymbols[index];
        PowerLabel.text = powerLabels[index];
    }

    public void PlayerDied()
    {
        if (currentLife <= 0)
        {
            LostGame();
            life.gameObject.SetActive(false);
        }
        transform.position = new Vector3(0, 1.1f, 0);
        gravity = 9.8f;
        groundDirection = Vector3.down;
        flippedNegator = 1;
        flipped = false;
        shrunken = false;
        currentSpeed = movementSpeed;
        jumpHeight = 1;
        isDead = false;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        controller.center = new Vector3(0, 0, 0);
        controller.radius = 0.5f;
        controller.height = 2;
        controller.stepOffset = 0.55f;
        transform.GetChild(0).GetComponent<Animator>().Play("Init State", 0);
        transform.GetChild(0).GetComponent<Animator>().Play("Init State", 1);
        transform.GetChild(0).GetComponent<Animator>().ResetTrigger("FlipRightSideUp");
        transform.GetChild(0).GetComponent<Animator>().ResetTrigger("FlipUpsideDown");
        transform.GetChild(0).GetComponent<Animator>().ResetTrigger("Grow");
        transform.GetChild(0).GetComponent<Animator>().ResetTrigger("Shrink");
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
            ReduceLife();
        health.currentHealth = health.startingHealth;
        SetLifeAmount();
    }

    //Set the text of amount of life
    private void SetLifeAmount()
    {
        life.text = "Lifes Left: " + currentLife;
    }

    //Reduce a life
    private void ReduceLife()
    {
        currentLife -= 1;
    }

    //Lost game
    private void LostGame()
    {
        gameController.LevelLost();
        DontMove();
    }

    //stop player when game over
    public void DontMove()
    {
        currentSpeed = 0f;
    }
}
