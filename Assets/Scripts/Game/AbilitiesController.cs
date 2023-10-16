using UnityEngine;
using UnityEngine.UI;

public class AbilitiesController : MonoBehaviour
{
    [SerializeField]
    private GameObject itemSlot1;
    [SerializeField]
    private GameObject itemSlot2;
    [SerializeField]
    private GameObject itemSlot3;
    public void OnButtonHoverEnter()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnButtonHoverEnter();
    }

    public void OnButtonHoverExit()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnButtonHoverExit();
    }

    public void UseItemSlot1()
    {
        Debug.Log("UseItemSlot1");
        PlayerPrefs.SetInt("ItemSlot1", 0);
        UseItem(itemSlot1, UIManager.Instance.items[0]);

        //ScoreManager.Instance.IsScoreIncreaseByItem = true;

    }
    public void UseItemSlot2()
    {
        Debug.Log("UseItemSlot2");
        PlayerPrefs.SetInt("ItemSlot2", 0);
        UseItem(itemSlot2, UIManager.Instance.items[1]);
    }
    public void UseItemSlot3()
    {
        Debug.Log("UseItemSlot3");
        PlayerPrefs.SetInt("ItemSlot3", 0);
        UseItem(itemSlot3, UIManager.Instance.items[2]);

    }
    public void UseSkillSlot()
    {
        Debug.Log("UseSkillSlot");
    }

    public void UseItem(GameObject itemSlot, ItemID id)
    {
        // 아이템 사용 부분
        ItemType type = DataTableManager.GetTable<ItemTable>().GetItemInfo(id).type;
        float value = DataTableManager.GetTable<ItemTable>().GetItemInfo(id).value;
        float duration = DataTableManager.GetTable<ItemTable>().GetItemInfo(id).duration;
        int price = DataTableManager.GetTable<ItemTable>().GetItemInfo(id).price;


        if (id != ItemID.None)
        {
            switch (type)
            {
                case ItemType.Score:
                    ScoreManager.Instance.IsScoreIncreaseByItem = true;
                    ScoreManager.Instance.itemValue = value;
                    UIManager.Instance.scoreItemDuration = duration;
                    break;
                case ItemType.Timer:
                    UIManager.Instance.isStopTimer = true;
                    UIManager.Instance.stopDuration = duration;
                    break;
                case ItemType.Bomb:

                    if (value == 0)
                    {
                        BlockManager blockManager = BlockManager.Instance;
                        foreach (Block obs in blockManager.obsList)
                        {
                            if (obs != null)
                            {
                                blockManager.RemoveBlock(obs);
                                PlayBombObsEffect(obs);
                            }
                        }
                        blockManager.obsList.Clear();
                    }
                    else
                    {
                        int count = Mathf.Min((int)value, BlockManager.Instance.obsList.Count);

                        for (int i = 0; i < count; ++i)
                        {
                            Block obs = BlockManager.Instance.obsList[i];
                            if (obs != null)
                            {
                                BlockManager.Instance.RemoveBlock(obs);
                                PlayBombObsEffect(obs);
                            }
                        }
                        BlockManager.Instance.obsList.RemoveRange(0, count);
                    }
                    break;
            }
        }
        Image slotImage = itemSlot.transform.GetChild(1).GetComponent<Image>();
        slotImage.sprite = null;
        Color imageColor = slotImage.color;
        imageColor.a = 0f;
        slotImage.color = imageColor;
    }

    public void PlayBombObsEffect(Block obs)
    {
        ParticleSystem effect = Instantiate(obs.mergeEffect2, obs.transform.position, Quaternion.identity);
        effect.gameObject.SetActive(true);
        effect.Play();
    }
}
