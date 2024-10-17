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

        // TODO: GameManager의 GameRoomInfo를 참조하여 씬 구성
        var mapId = Managers.Instance.GameManager.MyGameRoomInfo.mapId;
        var players = Managers.Instance.GameManager.MyGameRoomInfo.players;
        var prefabName = Managers.Instance.ResourceManager.MapInfos[mapId].prefabName;
        
        Managers.Instance.ResourceManager.Instantiate<Map>(Config.MAP_PREFAB_PATH, prefabName);

        foreach (var p in players)
        {
            var player = Managers.Instance.ResourceManager.Instantiate<Player>(Config.ENTITY_PLAYER_PREFAB_PATH, "Player");
            player.PlayerInfo = p;
            p.Player = player;
            player.SetPosition(p.Position);
        }
    }
}
