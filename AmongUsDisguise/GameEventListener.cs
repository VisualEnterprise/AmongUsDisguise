using Impostor.Api.Events;
using Impostor.Api.Events.Meeting;
using Impostor.Api.Events.Player;
using Impostor.Api.Net;
using Impostor.Api.Net.Inner.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cc.ts13.AmongUsDisguise.Handlers {
    class GameEventListener : IEventListener {

        private readonly ILogger<Disguise> _logger;
        private Dictionary<IInnerPlayerControl, IInnerPlayerControl> _kills;

        public GameEventListener(ILogger<Disguise> logger) {
            _logger = logger;
            _kills = new Dictionary<IInnerPlayerControl, IInnerPlayerControl>();
        }

        [EventListener]
        public void OnGameStarted(IGameStartedEvent e) {
            _logger.LogInformation($"Game is starting.");
        }

        [EventListener]
        public void OnPlayerMurder(IPlayerMurderEvent e) {
            _logger.LogInformation($"Player murdered");
            _kills.Add(e.Victim, e.PlayerControl);
            IInnerPlayerControl murderer = e.PlayerControl, victim = e.Victim;
            string mPlayerName = murderer.PlayerInfo.PlayerName, 
                vPlayerName = victim.PlayerInfo.PlayerName;
            byte mColorId = murderer.PlayerInfo.ColorId, 
                vColorId = victim.PlayerInfo.ColorId;
            uint mHatId = murderer.PlayerInfo.HatId, 
                vHatId = victim.PlayerInfo.HatId, 
                mPetId = murderer.PlayerInfo.PetId, 
                vPetId = victim.PlayerInfo.PetId, 
                mSkinId = murderer.PlayerInfo.SkinId, 
                vSkinId = victim.PlayerInfo.SkinId;
        }

        [EventListener]
        public void OnGameEnd(IGameEndedEvent e) {
            _logger.LogInformation($"Game has ended.");
        }

        [EventListener]
        public void OnPlayerChat(IPlayerChatEvent e) {
            _logger.LogInformation($"{e.PlayerControl.PlayerInfo.PlayerName} said {e.Message}");
        }

        [EventListener]
        public void OnPlayerSpawned(IPlayerSpawnedEvent e) {
        }
    }
}
