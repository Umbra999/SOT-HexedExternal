using Hexed.Extensions;
using Hexed.SDK.Athena;
using Hexed.SDK.Engine;
using Hexed.Wrappers;

namespace Hexed.Modules
{
    internal class AutoFish
    {
        private static KeyValuePair<int, int> LastKeyDirection = new(0x53, 0);
        private static bool wasFishing = false;

        public static void Update()
        {
            if (!ConfigHandler.AutoFish) return;

            AFishingRod FishingRod = GetLocalRod();
            if (FishingRod == null) return;

            if (FishingRod.ServerState == SDK.Offsets.EnumOffsets.EFishingRodServerState.NotBeingUsed)
            {
                if (wasFishing)
                {
                    wasFishing = false;
                    GeneralHelper.SendKeyUp(0x01);
                    GeneralHelper.SendKeyUp(LastKeyDirection.Key);
                }
                return;
            }

            wasFishing = true;

            if (FishingRod.ServerState == SDK.Offsets.EnumOffsets.EFishingRodServerState.ReelingInAComedyItem)
            {
                GeneralHelper.SendKeyDown(0x01);
                return;
            }

            switch (FishingRod.BattlingState)
            {
                case SDK.Offsets.EnumOffsets.EFishingRodBattlingState.Battling_Tired:
                    GeneralHelper.SendKeyDown(0x01);
                    GeneralHelper.SendKeyUp(LastKeyDirection.Key);
                    break;

                case SDK.Offsets.EnumOffsets.EFishingRodBattlingState.Battling_NotTiring:
                    GeneralHelper.SendKeyUp(0x01);
                    RecalculateKey();
                    break;

                case SDK.Offsets.EnumOffsets.EFishingRodBattlingState.Battling_Tiring:
                    GeneralHelper.SendKeyUp(0x01);
                    break;

                case SDK.Offsets.EnumOffsets.EFishingRodBattlingState.NotBattling:
                    GeneralHelper.SendKeyUp(0x01);
                    GeneralHelper.SendKeyUp(LastKeyDirection.Key);
                    break;
            }
        }

        private static AFishingRod GetLocalRod()
        {
            AAthenaPlayerCharacter Pirate = GameHelper.GetLocalPlayerCharacter();
            if (Pirate == null) return null;

            AActor WieldedItem = Pirate.WieldedItemComponent.CurrentlyWieldedItem;
            if (WieldedItem == null) return null;

            if (WieldedItem.ClassName.ToLower().Contains("rod")) return new AFishingRod(WieldedItem.Address);

            return null;
        }

        private static void RecalculateKey()
        {
            switch (LastKeyDirection.Key)
            {
                case 0x53:
                    if (LastKeyDirection.Value < Environment.TickCount - 500)
                    {
                        GeneralHelper.SendKeyUp(LastKeyDirection.Key);
                        LastKeyDirection = new(0x41, Environment.TickCount);
                    }
                    break;

                case 0x41:
                    if (LastKeyDirection.Value < Environment.TickCount - 500)
                    {
                        GeneralHelper.SendKeyUp(LastKeyDirection.Key);
                        LastKeyDirection = new(0x44, Environment.TickCount);
                    }
                    break;

                case 0x44:
                    if (LastKeyDirection.Value < Environment.TickCount - 500)
                    {
                        GeneralHelper.SendKeyUp(LastKeyDirection.Key);
                        LastKeyDirection = new(0x53, Environment.TickCount);
                    }
                    break;
            }

            GeneralHelper.SendKeyDown(LastKeyDirection.Key);
        }
    }
}
