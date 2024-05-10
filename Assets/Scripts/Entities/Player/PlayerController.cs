using Assets.Scripts;
using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

// NOTE: The movement for this script uses the new InputSystem. The player needs to have a PlayerInput
// component added and the Behaviour should be set to Send Messages so that the OnMove and OnFire methods
// actually trigger

public class PlayerController : DamagableCharacter
{
    public float moveSpeed = 600f;
    public float maxSpeed = 10f;
    public float collisionOffset = 0.05f;
    public float cooldown = 1;
    public ContactFilter2D contactFilter;
    public float attackRange = 0.5f;
    public float idleFriction = 0.9f;
    public GameObject attackPoint;

    private bool isShielding;
    private Vector2 input;
    private float lastHit;

    private bool isMoving = false;
    private bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }
    [SerializeField]
    private PlayerCharacteristics characteristics;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private SkinChanger skinChanger;

    [SerializeField]
    private UIController uiController;

    public static Transform transform;

    [field: SerializeField]
    public static bool IsCutScene { get; set; }

    private delegate void CleanupDelegate();
    private CleanupDelegate cleanupDelegate;

    public void Start()
    {
        Initialize();
        PlayerEvents.GetInstance().OnShieldUse += UseShield;
        PlayerEvents.GetInstance().OnTeleported += Teleport;
        PlayerEvents.GetInstance().OnSave += SavePlayer;

        transform = GetComponent<Transform>();

        Debug.Log("Player info Loaded");
    }

    public void LateUpdate()
    {
        animator.SetBool("IsShielding", false);
        skinChanger.SkinChoice();
    }

    private void Awake()
    {
        //start the animation by get the component animator from the player
        animator = GetComponent<Animator>();

        LoadPlayer();

        // TO-DO: it' could be broken
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void FixedUpdate()
    {
        MouseLook();
        if (input != Vector2.zero)
        {
            Move(input);
        } else
        {
            Stop();
        }
    }

    private void OnDestroy()
    {
        PlayerEvents.GetInstance().OnSave -= SavePlayer;
    }

    private void Teleport(Vector2 coordinates)
    {
        rb.position = coordinates;
    }

    // Tries to move the player in a direction by casting in that direction by the amount
    // moved plus an offset. If no collisions are found, it moves the players
    // Returns true or false depending on if a move was executed
    public void Move(Vector2 direction)
    {
        //rb.velocity = Vector2.ClampMagnitude(rb.velocity + (direction * moveSpeed * Time.deltaTime), maxSpeed);
        rb.AddForce(input * GetPlayerSpeed() * Time.deltaTime);
        if(rb.velocity.magnitude > maxSpeed)
        {
            float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
            rb.velocity = rb.velocity.normalized * limitedSpeed;
        }
        IsMoving = true;
    }

    public void Stop()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
        IsMoving = false;
    }

    public void OnMove(InputValue value)
    {
        if (!uiController.IsInputBlocked())
            input = value.Get<Vector2>();
    }

    private void OnFire()
    {
        if (!uiController.IsInputBlocked())
            SwordAttack();
    }

    private void SwordAttack()
    {
        if(Time.time - lastHit < cooldown)
        {
            return;
        }
        lastHit = Time.time;
        animator.SetTrigger("SwordAttack");
        audioManager.PlayEffect(audioManager.hitting);
    }

    private void UseShield(bool trigger)
    {
        animator.SetBool("IsShielding", trigger);
    }

    private void MouseLook()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        checkHitboxDirection(direction.x, direction.y);


        FindAnyObjectByType<DamagableCharacter>();
    }

    private void checkHitboxDirection(float x, float y)
    {
        bool right = x > 0 && (y < 1.5 && y > -1.5);
        bool left = x < 0 && (y < 1.5 && y > -1.5);
        bool up = y > 0.5;
        bool down = y < -0.5;

        if (right)
        {
            gameObject.BroadcastMessage("TurnRight", right);
        }
        else if (left)
        {
            gameObject.BroadcastMessage("TurnLeft", left);
        }
        if (up)
        {
            gameObject.BroadcastMessage("TurnUp", up);
        }
        else if (down)
        {
            gameObject.BroadcastMessage("TurnDown", down);
        }
    }

    // Functions integrating Inventory and PlayerCharacteristics

    public SpritesContainer GetSpriteOnPlayer(int index)
    {
        if (index < 0 || index > 6) throw new Exception("Index Out of range [0, 6]");
        Inventory.InventorySlot slot = inventory.GetSlotAt(index);
        if (!slot.IsEmpty)
            return new SpritesContainer(slot.item.GetSprite());
        else
            return null;
    }

    public float GetDefence()
    {
        float defence = 0f;

        for (int i = 0; i < 4; i++)
        {
            Inventory.InventorySlot slot = inventory.GetSlotAt(i);
            if (!slot.IsEmpty)
                defence += slot.item.GetDefenceAmount();
        }

        defence += characteristics.Endurance;

        Debug.Log("Total defence amount of a player: " + defence);
        return defence;
    }

    // TODO: Nerf Endurance Perk
    public override float CalculateReceivedDamage(float damage) 
    { 
        float totalDamage = damage;
        totalDamage -= GetDefence();
        if (totalDamage < 0) { totalDamage = 0f; }

        Debug.Log("Total damage received by a player: " + totalDamage);
        return totalDamage; 
    }

    public Weapon.Damage GetDamage() {
        return inventory.GetDamage().NewMultiplied(1 + 0.1f * characteristics.Strength, DamageTypes.Physical).NewAdded(1 + 0.1f * characteristics.Intelligence, DamageTypes.Magical);
    }

    public float GetPlayerSpeed() 
    { 
        return moveSpeed * (1 + 0.1f * characteristics.Agility); 
    }

    public SpritesContainer[] GetWearableSprites()
    {
        SpritesContainer[] containers = new SpritesContainer[7];

        for (int i = 0; i < 7; i++)
        {
            containers[i] = new SpritesContainer(inventory.GetSlotAt(i).item?.GetSprite());
        }

        return containers;
    }

    public void SavePlayer()
    {
        Vector2 playerPosition = transform.position;
        PlayerSave playerSave = new PlayerSave()
        {
            inventory = inventory,
            characteristics = characteristics,
            posX = playerPosition.x,
            posY = playerPosition.y,
            health = Health
        };
        PlayerEvents.dataService.SaveData("/playerSave", playerSave);
    }

    public void LoadPlayer()
    {
        try
        {
            PlayerSave playerSave = PlayerEvents.dataService.LoadData<PlayerSave>("/playerSave");
            inventory.Clone(playerSave.inventory);
            characteristics.Clone(playerSave.characteristics);
            Health = playerSave.health;
        } 
        catch 
        {
            Debug.Log("LoadDefault()");
            Health = 5;
        }
  
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlayerSave
    {
        [JsonProperty]
        public Inventory inventory;
        [JsonProperty]
        public PlayerCharacteristics characteristics;
        [JsonProperty]
        public float posX; 
        [JsonProperty]
        public float posY;
        [JsonProperty]
        public float health;
    }
}