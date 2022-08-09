using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ilang;

public class Shop : MonoBehaviour
{

    public enum ShopItemType
    {
        Shoe,
        Claw,
        Bomb,
        Time
    }

    [System.Serializable]
    public class ShopItem
    {
        public Sprite sprite;
        public int cost;
        public string description;
        public ShopItemType type;
    }

    [SerializeField]
    Image _shopUI;
    [SerializeField]
    Image _leftArrow;
    [SerializeField]
    Image _rightArrow;
    [SerializeField]
    SpriteRenderer _slot;

    [SerializeField]
    TextProChange _description;
    [SerializeField]
    TextProChange _gold;

    [SerializeField]
    ShopItem[] _shoes = new ShopItem[3];
    [SerializeField]
    ShopItem[] _claws = new ShopItem[3];
    [SerializeField]
    ShopItem _bomb;
    [SerializeField]
    ShopItem _worm;

    [SerializeField]
    AudioClip _shopOpen;
    [SerializeField]
    AudioClip _shopClose;
    [SerializeField]
    AudioClip _pageTurn;
    [SerializeField]
    AudioClip _buyItem;

    int selectIndex = 0;
    List<ShopItem> _items;
    Animator _anim;

    void Start() {
        ResetShop();
        ShowUI();
        _shopUI.gameObject.SetActive(false);
        _anim = GetComponent<Animator>();
    }

    void ResetShop() {
        _items = new List<ShopItem>();
        if (Game.Instance.shoeLevel != 3) {
            _items.Add(_shoes[Game.Instance.shoeLevel]);
        }
        if (Game.Instance.digLevel != 3) {
            _items.Add(_claws[Game.Instance.digLevel]);
        }
        _items.Add(_bomb);
        if (!Game.Instance.worm)
            _items.Add(_worm);
    }

    void ShowUI() {
        selectIndex = Mathf.Clamp(selectIndex, 0, _items.Count - 1);
        _slot.sprite = _items[selectIndex].sprite;
        //_slot.SetNativeSize();
        _description.ChangeText(_items[selectIndex].description);
        _gold.ChangeText(_items[selectIndex].cost.ToString());

        _leftArrow.enabled = _rightArrow.enabled = true;
        if (selectIndex == 0)
            _leftArrow.enabled = false;
        if (selectIndex == _items.Count - 1)
            _rightArrow.enabled = false;
    }

    void BuyItem() {
        var item = _items[selectIndex];

        if (Game.Instance.goldInt.Value >= item.cost) {
            SoundMgr.Instance.PlaySFX(_buyItem);
            Game.Instance.goldInt.AddValue(-item.cost);
            switch (item.type) {
            case ShopItemType.Shoe:
                Game.Instance.shoeLevel++;
                break;
            case ShopItemType.Claw:
                Game.Instance.digLevel++;
                if (Game.Instance.digLevel == 3) {
                    Game.Instance.stoneInt.SetValue(50);
                }
                break;
            case ShopItemType.Bomb:
                Game.Instance.bombInt.AddValue(1);
                break;
            case ShopItemType.Time:
                Game.Instance.worm = true;
                break;
            }
            ResetShop();
        }
    }


    void Update() {
        if (_shopUI.gameObject.activeSelf && _items.Count > 0) {
            ShowUI();
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (_leftArrow.enabled) {
                    selectIndex--;
                    SoundMgr.Instance.PlaySFX(_pageTurn);
                }
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (_rightArrow.enabled) {
                    selectIndex++;
                    SoundMgr.Instance.PlaySFX(_pageTurn);
                }
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                BuyItem();
            } else if (Input.GetKeyDown(KeyCode.Escape)) {
                ExitShop();
                SoundMgr.Instance.PlaySFX(_shopClose);
            }
        } else
            ExitShop();
    }
    public void EnterShop() {
        if (_items.Count > 0) {
            if (Game.Instance.State == Game.eState.Overworld) {
                SoundMgr.Instance.PlaySFX(_shopOpen);
                Game.Instance.ShopMode(true);
                _shopUI.gameObject.SetActive(true);
                TimeMgr.Instance.PauseGame();
            }
        }
    }
    public void ExitShop() {
        Game.Instance.ShopMode(false);
        _shopUI.gameObject.SetActive(false);
        TimeMgr.Instance.UnpauseGame();
    }

    public void Disable() {
        _shopUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void TryCloseShop() {
        if (_anim && gameObject.activeSelf) {
            _anim.Play("shop_close");
        }
    }
}
