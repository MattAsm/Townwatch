using UnityEngine;

public class PurchasableItem : MonoBehaviour
{
    public enum itemType{ Armour, Immediate, MoveUpgrade, NewMove, StatBoost, Weapon};
    public itemType itemsType;

    [HideInInspector] public string itemName;
    [HideInInspector] public string description;
    [HideInInspector] public int price;
    [HideInInspector] public Rarity rarity;

    public ShopItems shopItem;
    private SpriteRenderer sr;

    //Purchased Item Type
    private StatBoost statBoost;
    private Weapon weapon;
    private Armour armour;
    private NewMove newmove;
    private MoveUpgrade moveUpgrade;
    private ImmediateEffect immediateEffect;

    public CharacterManager player;
    //public ShopManager shopManager;

    public void Awake()
    {
        rarity = shopItem.rarity;
        sr = this.GetComponent<SpriteRenderer>();
       // Debug.Log(rarity);

    }
    void Start()
    {
        sr.sprite = shopItem.Icon;

        //STAT BOOST
        if(shopItem is StatBoost)
        {
           statBoost = (StatBoost)shopItem;
            itemsType = itemType.StatBoost;
        }
        //WEAPON
        else if(shopItem is Weapon)
        {
            weapon = (Weapon)shopItem;
            itemsType = itemType.Weapon;
        }
        //ARMOUR
        else if (shopItem is Armour)
        {
            armour = (Armour)shopItem;
            itemsType = itemType.Armour;
        }
        //NEW MOVE
        else if (shopItem is NewMove)
        {
            newmove = (NewMove)shopItem;
            itemsType = itemType.NewMove;
        }
        //MOVE UPGRADE
        else if (shopItem is MoveUpgrade)
        {
            moveUpgrade = (MoveUpgrade)shopItem;
            itemsType = itemType.MoveUpgrade;
        }
        //IMMEDIATE EFFECT
        else if (shopItem is ImmediateEffect)
        {
            immediateEffect = (ImmediateEffect)shopItem;
            itemsType = itemType.Immediate;
        }

        description = shopItem.ItemDescription;
        itemName = shopItem.ItemName;
        price = shopItem.price;


        player = CharacterManager.instance;
    }


    public void OnBuy()
    {

        switch (itemsType)
        {
            case itemType.StatBoost:
                player.Attack += statBoost.AttackBoost;
                player.Defence += statBoost.DefenseBoost;
                player.maxHealth += statBoost.HealthBoost;
                player.currentHealth += statBoost.HealthBoost; //Courteous, might remove but this heals player by HealthBoost if they purchase it
                player.Speed += statBoost.SpeedBoost;
                break;

            case itemType.Weapon:
                player.weaponEquip(weapon);
                break;

            case itemType.Armour:

                break;

            case itemType.NewMove:

                break;

            case itemType.MoveUpgrade:

                break;

            case itemType.Immediate:

                break;
        }

        
    }
}
