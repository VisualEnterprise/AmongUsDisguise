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

namespace org.visualenterprise.AmongUsDisguise.Handlers {
    class GameEventListener : IEventListener {

        private readonly ILogger<Disguise> _logger;

        public GameEventListener(ILogger<Disguise> logger) {
            _logger = logger;
        }

        [EventListener]
        public void OnGameStarted(IGameStartedEvent e) {
            _logger.LogInformation($"Game is starting.");
        }

        [EventListener]
        public async void OnPlayerMurder(IPlayerMurderEvent e) {
            _logger.LogInformation($"Player murdered");
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

            await victim.SetNameAsync(mPlayerName);
            await murderer.SetNameAsync(vPlayerName);
            await victim.SetColorAsync(mColorId);
            await murderer.SetColorAsync(vColorId);
            await victim.SetHatAsync(mHatId);
            await murderer.SetHatAsync(vHatId);
            await victim.SetPetAsync(mPetId);
            await murderer.SetPetAsync(vPetId);
            await victim.SetSkinAsync(mSkinId);
            await murderer.SetSkinAsync(vSkinId);
        }

        [EventListener]
        public void OnGameEnd(IGameEndedEvent e) {
            _logger.LogInformation($"Game has ended.");
        }

        [EventListener]
        public void OnPlayerChat(IPlayerChatEvent e) {
            _logger.LogInformation($"{e.PlayerControl.PlayerInfo.PlayerName} said {e.Message}");
        }
    }
}
