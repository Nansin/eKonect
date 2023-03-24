using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SingletonComponent<UIController>
{
    [SerializeField] private GameObject panelNextWave;
    [SerializeField] private TMPro.TextMeshProUGUI textLevelGun;
    [SerializeField] private Button btnUpgradeGun;

    public GameObject PanelNextWave { get => panelNextWave; set => panelNextWave = value; }

    private void Start()
    {
        UpdateTextLevelGun();
    }

    private void OnEnable()
    {
        btnUpgradeGun.onClick.AddListener(OnClick_UpgradeGun);
    }
    private void OnDisable()
    {
        btnUpgradeGun.onClick.RemoveListener(OnClick_UpgradeGun);
    }

    private void OnClick_UpgradeGun()
    {
        Prefs.LevelGun++;
        Prefs.LevelGun = Prefs.LevelGun > ConfigController.Instance.GunDatabase.gunDatas.Count - 1 ? ConfigController.Instance.GunDatabase.gunDatas.Count - 1 : Prefs.LevelGun;
        UpdateTextLevelGun();
    }

    public void UpdateTextLevelGun()
    {
        textLevelGun.text = "" + (Prefs.LevelGun + 1);
    }
}
