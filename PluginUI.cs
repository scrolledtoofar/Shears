using Dalamud.Configuration;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Utility;
using ImGuiNET;
using Shears;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Text;
using Veda;

namespace Shears
{
    public class PluginUI
    {
        private readonly Plugin plugin;

        public PluginUI(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void Draw()
        {
            if (!Plugin.SettingsVisible)
            {
                return;
            }

            bool settingsVisible = Plugin.SettingsVisible;
            if (ImGui.Begin("Shears", ref settingsVisible, ImGuiWindowFlags.AlwaysAutoResize))
            {

                Race othersTargetRace = Plugin.PluginConfig.ChangeOthersTargetRace;

                bool onlyChangeHrothgars = Plugin.PluginConfig.OnlyChangeHrothgars;
                ImGui.Checkbox("Change Hrothgars", ref onlyChangeHrothgars);
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Only change hrothgar players"); }

                this.plugin.OnlyChangeHrothgars(onlyChangeHrothgars);

                if (ImGui.BeginCombo("Race", othersTargetRace.GetAttribute<Display>().Value))
                {
                    foreach (Race race in Enum.GetValues(typeof(Race)))
                    {
                        ImGui.PushID((byte)race);
                        if (ImGui.Selectable(race.GetAttribute<Display>().Value, race == othersTargetRace))
                        {
                            othersTargetRace = race;
                        }

                        if (race == othersTargetRace)
                        {
                            ImGui.SetItemDefaultFocus();
                        }

                        ImGui.PopID();
                    }
                    if (ImGui.IsItemHovered()) { ImGui.SetTooltip("The race to change all players to"); }
                    ImGui.EndCombo();
                }

                this.plugin.UpdateOtherRace(othersTargetRace);

                bool immersiveMode = Plugin.PluginConfig.ImmersiveMode;
                ImGui.Checkbox("Immersive Mode", ref immersiveMode);
                ImGui.Text("If Immersive Mode is enabled, \"Examine\" windows will also be modified.");
                

                this.plugin.UpdateImmersiveMode(immersiveMode);
                //}
                ImGui.End();
            }

            Plugin.SettingsVisible = settingsVisible;
            this.plugin.SaveConfig();
        }
    }
}