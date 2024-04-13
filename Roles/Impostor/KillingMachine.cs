﻿using AmongUs.GameOptions;
using static TOHE.Options;

namespace TOHE.Roles.Impostor;

internal class KillingMachine : RoleBase
{
    //===========================SETUP================================\\
    private const int Id = 23800;

    private static readonly HashSet<byte> PlayerIds = [];
    public static bool HasEnabled => PlayerIds.Any();
    public override bool IsEnable => HasEnabled;
    public override CustomRoles ThisRoleBase => CustomRoles.Impostor;
    public override Custom_RoleType ThisRoleType => Custom_RoleType.ImpostorKilling;
    //==================================================================\\

    private static OptionItem MNKillCooldown;

    public override void SetupCustomOption()
    {
        SetupRoleOptions(Id, TabGroup.ImpostorRoles, CustomRoles.KillingMachine);
        MNKillCooldown = FloatOptionItem.Create(Id + 5, "KillCooldown", new(2.5f, 180f, 2.5f), 10f, TabGroup.ImpostorRoles, false)
            .SetParent(CustomRoleSpawnChances[CustomRoles.KillingMachine])
            .SetValueFormat(OptionFormat.Seconds);
    }

    public override void Init()
    {
        PlayerIds.Clear();
    }
    public override void Add(byte playerId)
    {
        PlayerIds.Add(playerId);
    }

    public override bool CanUseImpostorVentButton(PlayerControl pc) => false;
    public override bool CanUseSabotage(PlayerControl pc) => false;
    public override void SetKillCooldown(byte id) => Main.AllPlayerKillCooldown[id] = MNKillCooldown.GetFloat();

    public override void ApplyGameOptions(IGameOptions opt, byte playerId)
    {
        opt.SetFloat(FloatOptionNames.ImpostorLightMod, 0.2f);
    }

    public override bool ForcedCheckMurderAsKiller(PlayerControl killer, PlayerControl target)
    {
        killer.RpcMurderPlayer(target);
        killer.ResetKillCooldown();
        return false;
    }
}
