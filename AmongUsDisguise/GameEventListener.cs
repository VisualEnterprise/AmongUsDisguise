using Impostor.Api.Events;
using Impostor.Api.Events.Player;
using Impostor.Api.Innersloth;
using Impostor.Api.Net.Inner.Objects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace org.visualenterprise.AmongUsDisguise.Handlers {
    class GameEventListener : IEventListener {

        private readonly ILogger<Disguise> _logger;

        private Dictionary<string, bool> _game;

        public GameEventListener(ILogger<Disguise> logger) {
            _logger = logger;
            _game = new Dictionary<string, bool>();
        }

        [EventListener]
        public void OnGameCreated(IGameCreatedEvent e) {
            _game.Add(e.Game.Code, true);
        }

        [EventListener]
        public void OnGameStarted(IGameStartedEvent e) {
            _logger.LogInformation($"Game is starting.");
        }

        [EventListener]
        public void OnPlayerChat(IPlayerChatEvent e) {
            _logger.LogInformation($"{e.PlayerControl.PlayerInfo.PlayerName} said {e.Message}");
            if (e.Game.GameState == GameStates.NotStarted && e.Message.StartsWith("/")) {
                Task.Run(async () => await RunCommands(e).ConfigureAwait(false));
            }
        }

        private async Task RunCommands(IPlayerChatEvent e) {
            switch (e.Message.ToLowerInvariant()) {
                case "/disguise on":
                    if (e.ClientPlayer.IsHost) {
                        _game[e.Game.Code] = true;

                        await e.PlayerControl.SendChatAsync("The Disguise mod is now on!").ConfigureAwait(false);
                    } else {
                        await e.PlayerControl.SendChatAsync("You need to be host to change roles.").ConfigureAwait(false);
                    }
                    break;
                case "/disguise off":
                    if (e.ClientPlayer.IsHost) {
                        _game[e.Game.Code] = false;

                        await e.PlayerControl.SendChatAsync("The Disguise mod is now off!").ConfigureAwait(false);
                    } else {
                        await e.PlayerControl.SendChatAsync("You need to be host to change roles.").ConfigureAwait(false);
                    }
                    break;
                case "/disguise help":
                    await e.PlayerControl.SendChatAsync("When the special Disguise mod is on, the impostor takes the identity of their victim").ConfigureAwait(false);
                    await e.PlayerControl.SendChatAsync("The host can turn the Disguise mod on and off by typing '/disguise on' or '/disguise off'.").ConfigureAwait(false);
                    break;
            }
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

            if (_game[e.Game.Code]) {
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
        }

        [EventListener]
        public void OnGameEnd(IGameEndedEvent e) {
            _logger.LogInformation($"Game has ended.");
        }
    }
}
