using InventorySystem;
using TMPro;
using UnityEngine;

namespace Ui.InventorySecondaryUi
{
    public class ItemStatsWindow : MonoBehaviour
    {
        private readonly string fireElement = "Огонь";
        private readonly string iceElement = "Лёд";
        private readonly string acidElement = "Кислота";
        private readonly string typeWeapon = "Оружие";
        private readonly string typeNecklace = "Ожерелье";
        private readonly string typeRing = "Кольцо";
        private readonly string typeArmor = "Броня";
        private readonly string typeConsumable = "Расходник";

        [SerializeField] private TextMeshProUGUI fieldName;
        [SerializeField] private TextMeshProUGUI fieldType;
        [SerializeField] private TextMeshProUGUI fieldCost;
        [SerializeField] private TextMeshProUGUI fieldDescription;
        [SerializeField] private TextMeshProUGUI fieldBonusHp;
        [SerializeField] private TextMeshProUGUI fieldBonusMp;
        [SerializeField] private TextMeshProUGUI fieldPercentBonusDamage;
        [SerializeField] private TextMeshProUGUI fieldPercentProtection;
        [SerializeField] private TextMeshProUGUI fieldAttackDamage;
        [SerializeField] private TextMeshProUGUI fieldAttackMpCost;
        [SerializeField] private TextMeshProUGUI fieldSpellChargeCount;
        [SerializeField] private TextMeshProUGUI fieldSpecialDamage;
        [SerializeField] private TextMeshProUGUI fieldSpecialMpCost;
        [SerializeField] private TextMeshProUGUI fieldElementOfDamage;
        [SerializeField] private TextMeshProUGUI fieldBonusDamage;
        [SerializeField] private TextMeshProUGUI fieldDotDamage;
        [SerializeField] private TextMeshProUGUI fieldRestoreAmount;
        [SerializeField] private TextMeshProUGUI fieldRestoreTime;

        private void Awake()
        {
            foreach (var field in GetComponentsInChildren<StatFieldMarker>())
            {
                field.gameObject.SetActive(false);
            }
        }

        public void DisplayItemStats(ItemConfig item)
        {
            fieldName.gameObject.SetActive(true);
            fieldName.text = item.ItemName;
            fieldType.gameObject.SetActive(true);

            switch (item.Type)
            {
                case ItemType.None:
                    fieldType.gameObject.SetActive(false);
                    break;
                case ItemType.Weapon:
                    fieldType.text = typeWeapon;
                    break;
                case ItemType.Necklace:
                    fieldType.text = typeNecklace;
                    break;
                case ItemType.Ring:
                    fieldType.text = typeRing;
                    break;
                case ItemType.Armor:
                    fieldType.text = typeArmor;
                    break;
                case ItemType.Consumable:
                    fieldType.text = typeConsumable;
                    break;
            }
            
            fieldCost.gameObject.SetActive(true);
            fieldCost.text = item.ItemCost.ToString();
            fieldDescription.gameObject.SetActive(true);
            fieldDescription.text = item.Description;

            CompareToZero(item.BonusHp, fieldBonusHp);

            CompareToZero(item.BonusMp, fieldBonusMp);
            
            CompareToZero(item.PercentBonusDamage, fieldPercentBonusDamage);
            
            CompareToZero(item.PercentProtection, fieldPercentProtection);

            CompareToZero(item.AttackDamage, fieldAttackDamage);
            
            CompareToZero(item.AttackMpCost, fieldAttackMpCost);
            
            CompareToZero(item.SpellChargeCount, fieldSpellChargeCount);
            
            CompareToZero(item.SpecialDamage, fieldSpecialDamage);
            
            CompareToZero(item.SpecialMpCost, fieldSpecialMpCost);
            
            switch (item.ElementOfDamage)
            {
                case Element.None:
                    fieldElementOfDamage.gameObject.SetActive(false);
                    break;
                case Element.Fire:
                    fieldElementOfDamage.gameObject.SetActive(true);
                    fieldElementOfDamage.text = fireElement;
                    break;
                case Element.Ice:
                    fieldElementOfDamage.gameObject.SetActive(true);
                    fieldElementOfDamage.text = iceElement;
                    break;
                case Element.Acid:
                    fieldElementOfDamage.gameObject.SetActive(true);
                    fieldElementOfDamage.text = acidElement;
                    break;
                default:
                    fieldElementOfDamage.gameObject.SetActive(false);
                    break;
            }
            
            CompareToZero(item.BonusDamage, fieldBonusDamage);
            
            CompareToZero(item.DotDamage, fieldDotDamage);
            
            CompareToZero(item.RestoreAmount, fieldRestoreAmount);
            
            CompareToZero(item.RestoreTime, fieldRestoreTime);
        }

        private void CompareToZero(float target, TextMeshProUGUI field)
        {
            if (target == 0)
            {
                field.gameObject.SetActive(false);
            }
            else
            {
                field.gameObject.SetActive(true);
                field.text = $"{target}";
            }
        }
    }
}