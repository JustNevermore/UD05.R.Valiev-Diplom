using System;
using ItemBehaviours;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "ItemConfig/Create New Item", order = 0)]
    
    public class ItemConfig : ScriptableObject
    {
        private int itemId;
        public int ItemId => itemId;

        [Header("Base")]
        [SerializeField] private ItemType itemType;
        [SerializeField] private bool isStackable;
        [SerializeField] private string itemName;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private int itemCost;
        [SerializeField, Range(1, 100)] private int rarity;
        [TextArea(5, 5)]
        [SerializeField] private string description;

        public ItemType Type => itemType;
        public bool IsStackable => isStackable;
        public string ItemName => itemName;
        public Sprite ItemIcon => itemIcon;
        public int ItemCost => itemCost;
        public int Rarity => rarity;
        public string Description => description;

        [Header("Armor")]
        [SerializeField] private float bonusHp;
        [SerializeField] private float bonusMp;
        [SerializeField] private float percentBonusDamage;
        [SerializeField] private float percentProtection;
        
        public float BonusHp => bonusHp;
        public float BonusMp => bonusMp;
        public float PercentBonusDamage => percentBonusDamage;
        public float PercentProtection => percentProtection;
        
        [Header("Weapon")]
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackMpCost;
        [SerializeField] private int spellChargeCount;
        [SerializeField] private float specialDamage;
        [SerializeField] private float specialMpCost;
        [SerializeField] private WeaponBehaviour moveSet;
        [SerializeField] private GameObject weaponView;

        public WeaponType WeapType => weaponType;
        public float AttackDamage => attackDamage;
        public float AttackMpCost => attackMpCost;
        public int SpellChargeCount => spellChargeCount;
        public float SpecialDamage => specialDamage;
        public float SpecialMpCost => specialMpCost;
        public WeaponBehaviour MoveSet => moveSet;
        public GameObject WeaponView => weaponView;
        
        [Header("Necklace")]
        [SerializeField] private NecklaceBehaviour defenceSkill;

        public NecklaceBehaviour DefenceSkill => defenceSkill;
        
        [Header("Ring")]
        [SerializeField] private Element elementOfDamage;
        [SerializeField] private float bonusDamage;
        [SerializeField] private bool hasDot;
        [SerializeField] private float dotDamage;
        [SerializeField] private bool hasSlow;

        public Element ElementOfDamage => elementOfDamage;
        public float BonusDamage => bonusDamage;
        public bool HasDot => hasDot;
        public float DotDamage => dotDamage;
        public bool HasSlow => hasSlow;

        [Header("Consumable")]
        [SerializeField] private ConsumableType typeOfConsumable;
        [SerializeField] private float restoreAmount;
        [SerializeField] private float restoreTime;
        
        public ConsumableType TypeOfConsumable => typeOfConsumable;
        public float RestoreAmount => restoreAmount;
        public float RestoreTime => restoreTime;

        public void SetId(int id)
        {
            itemId = id;
        }
    }
}