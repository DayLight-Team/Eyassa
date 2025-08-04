# Eyassa

**Eyassa** is a plugin for [EXILED](https://github.com/ExMod-Team/EXILED) that provides **server-specific, per-player settings** for SCP: Secret Laboratory servers.  
It allows devs to handle SSS on a per player basis
---

## 🔧 Features


- Built-in support for:
  - Buttons (`ButtonOption`)
  - Two Button Options (`TwoButtonOption`)
  - Sliders (`SliderOption`)
  - Dropdowns  (`DropdownOption`)
  - Input fields (`InputTextOption`, `TextAreaOption`)
- Easy to extend with your own options and logic
---

## 📦 Installation

1. Download the latest release from [GitHub Releases](https://github.com/your-name/Eyassa/releases).
2. Place the `.dll` into your server's `Plugins/` folder.
3. Start the server once to generate default configuration files.
4. Create and register your own nodes to expose settings in-game.

---

## 🧪 Example Option: `ButtonOption`

```csharp
using Exiled.API.Features;
using Eyassa.Models.Options;

namespace Eyassa.Test.Options;

public class TestButton : ButtonOption
{
    public override string GetLabel(Player player) => $"Hello {player.Nickname}";

    public override string GetButtonText(Player player) => $"Heal {player.MaxHealth - player.Health} HP";

    public override string GetHint(Player player) => "Heals you to full health";

    public override float GetHoldTime(Player player) => (player.MaxHealth - player.Health) / 10;

    public override void OnValueChanged(Player? player)
    {
        player?.Heal(player.MaxHealth - player.Health);
    }
}
