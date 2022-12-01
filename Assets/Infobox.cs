using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class Infobox : MonoBehaviour
{
    private GameObject currentlySelected;
    public Image enhImage;
    public TextMeshProUGUI enhTitle;
    public TextMeshProUGUI enhDescription;
    public TextMeshProUGUI enhCost;
    public GameObject infoObject;

    public List<Sprite> sprites;
    // Start is called before the first frame update
    void Start()
    {
        infoObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentlySelected = EventSystem.current.currentSelectedGameObject;
        EnhancementItem item = currentlySelected.GetComponent<EnhancementItem>();
        if (item != null)
        {
            infoObject.SetActive(true);
            switch (item.enhancementType)
            {
                case Enhancement.EnhancementType.Damage:
                    enhImage.sprite = sprites.ElementAt(1);
                    enhDescription.text = "Increases damage dealt by 4%";
                    enhTitle.text = "Damage";
                    break;
                case Enhancement.EnhancementType.Pickup:
                    enhImage.sprite = sprites.ElementAt(3);
                    enhDescription.text = "Increases pickup range by 3";
                    enhTitle.text = "Pickup";
                    break;
                case Enhancement.EnhancementType.Xp:
                    enhImage.sprite = sprites.ElementAt(7);
                    enhDescription.text = "Increases XP multiplier by 5%";
                    enhTitle.text = "XP";
                    break;
                case Enhancement.EnhancementType.Movement:
                    enhImage.sprite = sprites.ElementAt(6);
                    enhDescription.text = "Increases movement speed by 15%";
                    enhTitle.text = "Speed";
                    break;
                case Enhancement.EnhancementType.MaxHealth:
                    enhImage.sprite = sprites.ElementAt(2);
                    enhDescription.text = "Increases max health by 20";
                    enhTitle.text = "Health";
                    break;
                case Enhancement.EnhancementType.Recovery:
                    enhImage.sprite = sprites.ElementAt(5);
                    enhDescription.text = "Increases health recovery speed by 1x";
                    enhTitle.text = "Recovery";
                    break;
                case Enhancement.EnhancementType.Armour:
                    enhImage.sprite = sprites.ElementAt(0);
                    enhDescription.text = "Decreases damage taken by 3%";
                    enhTitle.text = "Armour";
                    break;
                case Enhancement.EnhancementType.Projectile:
                    enhImage.sprite = sprites.ElementAt(4);
                    enhDescription.text = "Increases projectile amount by 1";
                    enhTitle.text = "Projectile";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (item.currentLevel < item.toggleBoxes.Count)
            {
                enhCost.text = item.CalculateCost().ToString();
                if (item.CalculateCost() > GameManager.Instance.totalCoins)
                {
                    enhCost.color = new Color(0.3692f, 0.3692f, 0.3692f);
                }
                else
                {
                    enhCost.color = new Color(0.1843137f, 0.3647059f, 0.5686275f);
                }
            }
            else
            {
                enhCost.color = new Color(0.1843137f, 0.3647059f, 0.5686275f);
                enhCost.text = "MAX";
            }
        }
        else
        {
            infoObject.SetActive(false);
        }
    }
}
