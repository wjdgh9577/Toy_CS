using CoreLibrary.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase
{
    void Start()
    {
        LogHandler.SetModule(new LogModule());
        Managers.Instance.SceneManager.CurrentScene = this;
        //Managers.Instance.UIManager.GetUI<UIGame>().Show();

        // TODO: GameManager�� GameRoomInfo�� �����Ͽ� �� ����
        var mapId = Managers.Instance.GameManager.MyGameRoomInfo.mapId;
        var prefabName = Managers.Instance.ResourceManager.MapInfos[mapId].prefabName;
        Managers.Instance.ResourceManager.Instantiate<Map>(Config.MAP_PREFAB_PATH, prefabName);
        Managers.Instance.ResourceManager.Instantiate<Player>(Config.ENTITY_PLAYER_PREFAB_PATH, "Player");
    }
}
