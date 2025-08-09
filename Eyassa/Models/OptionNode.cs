using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Eyassa.Interfaces;
using Eyassa.Managers;
using LabApi.Features.Console;
using MEC;

namespace Eyassa.Models;

public abstract class OptionNode
{
    public abstract List<IOption> Options { get; }
    public virtual int Priority { get; set; } = 0;
    public virtual bool IsVisibleToPlayer(Player? player) => true;
    private List<Player> AvailableForPlayers { get; } = new();

    public void Register()
    {
        UpdateOptions();
        OptionsManager.Nodes.Add(this);
    }

    public void UpdateOptions()
    {
        foreach (var option in Options)
        {
            option.Register();
        }
    }

    public List<IOption> GetVisibleOptions(Player? player) => Options.Where(x => x.IsCurrentlyVisible(player)).ToList();
    public void UpdateNode(Player? player)
    {
        if(player == null)
            return;
        foreach (var option in Options)
        {
            try
            {
                option.UpdateOption(player);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    public bool CheckForUpdateRequirement(Player? player)
    {
        if (player == null)
            return false;


        var didSeeBefore = AvailableForPlayers.Contains(player);
        var isVisible = IsVisibleToPlayer(player);
        var update = false;
        switch (isVisible)
        {
            case true when !didSeeBefore:
                AvailableForPlayers.Add(player);
                update = true;
                break;
            case false when didSeeBefore:
                AvailableForPlayers.Remove(player);
                update = true;
                break;
        }
        
        var optionUpdate = false;

        
        foreach (var unused in Options.Where(option => option.CheckForUpdate(player)))
        {
            optionUpdate = true;
            update = true;
        }
           
        
        return optionUpdate || update;
    }
}