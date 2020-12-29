using Impostor.Api.Events.Managers;
using Impostor.Api.Plugins;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cc.ts13.AmongUsDisguise {

    [ImpostorPlugin(
        package: "cc.ts13.AmongUsDisguise",
        name: "AmongUsDisguise",
        author: "Siebs",
        version: "1.0.0")]
    public class Disguise : PluginBase {

        private readonly ILogger<Disguise> _logger;
        private readonly IEventManager _eventManager;
        private IDisposable _unregister;

        public Disguise(ILogger<Disguise> logger, IEventManager eventManager) {
            _logger = logger;
            _eventManager = eventManager;
        }

        public override ValueTask EnableAsync() {
            _logger.LogInformation("Detective is being enabled");
            _unregister = _eventManager.RegisterListener(new GameEventListener(_logger));
            return default;
        }

        public override ValueTask DisableAsync() {
            _logger.LogInformation("Detective is being disabled");
            _unregister.Dispose();
            return default;
        }

    }
}
