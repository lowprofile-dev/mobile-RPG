using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardList : MonoBehaviour
{
    [SerializeField] private GameObject _rewardObject;
    private List<GameObject> _rewardList;

    [SerializeField] Sprite goldSprite;
    [SerializeField] Sprite moneySprite;
    [SerializeField] Sprite jewelSprite;
    [SerializeField] Sprite swordSprite;
    [SerializeField] Sprite wandSprite;
    [SerializeField] Sprite daggerSprite;
    [SerializeField] Sprite bluntSprite;
    [SerializeField] Sprite staffSprite;
    [SerializeField] Sprite allXPSprite;
    [SerializeField] Sprite itemSprite;

    public void SetRewards(Quest quest)
    {
        if (_rewardList == null) _rewardList = new List<GameObject>();

        for (int i=0; i<_rewardList.Count; i++) ObjectPoolManager.Instance.ReturnObject(_rewardList[i]);
        _rewardList.Clear();

        for (int i = 0; i < quest.rewardList.Count; i++)
        {
            GameObject obj = ObjectPoolManager.Instance.GetObject(_rewardObject);
            obj.transform.parent = transform;

            switch (quest.rewardList[i])
            {
                case 0:
                    switch(quest.rewardIdList[i])
                    {
                        case 0: obj.GetComponent<Image>().sprite = goldSprite; break;
                        case 1: obj.GetComponent<Image>().sprite = moneySprite; break;
                        case 2: obj.GetComponent<Image>().sprite = jewelSprite; break;
                    }
                    break;

                case 1:
                    switch(quest.rewardIdList[i])
                    {
                        case 0: obj.GetComponent<Image>().sprite = swordSprite; break;
                        case 1: obj.GetComponent<Image>().sprite = wandSprite; break;
                        case 2: obj.GetComponent<Image>().sprite = daggerSprite; break;
                        case 3: obj.GetComponent<Image>().sprite = bluntSprite; break;
                        case 4: obj.GetComponent<Image>().sprite = staffSprite; break;
                        case 5: obj.GetComponent<Image>().sprite = allXPSprite; break;
                    }
                    break;

                case 2:
                    obj.GetComponent<Image>().sprite = itemSprite;
                    break;
            }

            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.rewardAmountList[i].ToString();
            _rewardList.Add(obj);
        }
    }
}
