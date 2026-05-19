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
    private HashSet<Player> AvailableForPlayers { get; } = new();

    public void Register()
    {
        if (EyassaPlugin.Instance == null || !EyassaPlugin.Instance.IsLoaded)
        {
            Log.Error("Trying to register an node before Eyassa is loaded");
            return;
        }
        RegisterOptions();
        OptionsManager.Nodes.Add(this);
    }

    public void RegisterOptions()
    {
        foreach (var option in Options)
        {
            option.Register();
        }
    }

    public bool IsCurrentlyVisible(Player player)
    {
        return AvailableForPlayers.Contains(player);
    }

    public List<IOption> GetVisibleOptions(Player player)
    {
        List<IOption> options = [];
        foreach (var option in Options)
        {
            if (option.IsCurrentlyVisible(player))
                options.Add(option);
        }

        return options;
    }
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



    public bool CheckSendRequired(Player? player)
    {
        if (player == null)
            return false;


        var didSeeBefore = AvailableForPlayers.Contains(player);
        bool isVisible;
        try
        {
            isVisible = IsVisibleToPlayer(player);
        }
        catch (Exception e)
        {
            Log.Error(e);
            return false;
        }
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
        foreach (var option in Options)
        {
            if (!option.CheckForUpdate(player))
                continue;

            optionUpdate = true;
            update = true;
        }
           
        
        return optionUpdate || update;
    }
}
