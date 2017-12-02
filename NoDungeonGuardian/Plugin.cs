using System;
using OTAPI;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;

namespace NoDungeonGuardian
{
  [ApiVersion(2, 1)]
  public class Plugin : TerrariaPlugin
  {
    public override string Name => "NoDungeonGuardian";
    public override string Author => "Newy";
    public override string Description => "Stop dungeon guardians and dayprimes.";
    public override Version Version => typeof(Plugin).Assembly.GetName().Version;

    public Plugin(Main game) : base(game)
    {
    }

    public override void Initialize()
    {
      Hooks.Npc.PostUpdate += BlockDayPrimes;
    }

    protected override void Dispose(bool disposing)
    {
      // ReSharper disable once DelegateSubtraction
      Hooks.Npc.PostUpdate -= BlockDayPrimes;
      base.Dispose(disposing);
    }

    private static void BlockDayPrimes(NPC npc, int i)
    {
      if (npc == null || i > Main.npc.Length - 1 || i < 0 || !npc.active) return;

      if (npc.type != NPCID.DungeonGuardian &&
          npc.type != NPCID.SkeletronPrime &&
          npc.type != NPCID.SkeletronHead) return;

      if (npc.damage != 1000 && npc.damage != 9999) return;

      Main.npc[i].active = false;
      TSPlayer.All.SendData(PacketTypes.NpcUpdate, number: i);

      string caught = null;
      switch (npc.type)
      {
        case NPCID.DungeonGuardian:
          caught = "Dungeon Guardian";
          break;
        case NPCID.SkeletronHead:
          caught = "Day Skeletron";
          break;
        case NPCID.SkeletronPrime:
          caught = "Day Prime";
          break;
      }

      if (caught != null)
        TShock.Log.ConsoleInfo("Caught a wild {0}!", caught);
    }
  }
}