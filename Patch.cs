using Elements.Core;
using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;
using System;

namespace Resolutions
{
    public class Patch : ResoniteMod
    {
        public override string Name => "Resolutions";
        public override string Author => "LeCloutPanda";
        public override string Version => "1.0.0";

        public static ModConfiguration config;
        [AutoRegisterConfigKey] private static ModConfigurationKey<int2> RESOLUTION = new ModConfigurationKey<int2>("Photo Resolution", "", () => new int2(1920, 1080));
        [AutoRegisterConfigKey] private static ModConfigurationKey<int2> TiMER_RESOLUTION = new ModConfigurationKey<int2>("Timer Photo Resolution", "", () => new int2(2560, 1440));
        [AutoRegisterConfigKey] private static ModConfigurationKey<float> MIN_FOV = new ModConfigurationKey<float>("Min Fov", "", () => 20f);
        [AutoRegisterConfigKey] private static ModConfigurationKey<float> MAX_FOV = new ModConfigurationKey<float>("Max Fov", "", () => 90f);
        [AutoRegisterConfigKey] private static ModConfigurationKey<bool> DEBUG_GESTURE = new ModConfigurationKey<bool>("Enable Debug Gesture", "", () => false);
        [AutoRegisterConfigKey] private static ModConfigurationKey<float> MIN_DISTANCE = new ModConfigurationKey<float>("Min Distance", "", () => 0.1f);
        [AutoRegisterConfigKey] private static ModConfigurationKey<float> MAX_DISTANCE = new ModConfigurationKey<float>("Max Distance", "", () => 0.5f);

        public override void OnEngineInit()
        {
            config = GetConfiguration();

            Harmony harmony = new Harmony($"dev.{Author}.{Name}");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(PhotoCaptureManager), "GetCaptureCameraPosition")]
        static class HeheMakeCustomResPhotos
        {
            [HarmonyPostfix]
            static void ChangeResolution(PhotoCaptureManager __instance)
            {
                if (__instance.Slot.ActiveUserRoot.ActiveUser != __instance.LocalUser) return;
                __instance.NormalResolution.Value = config.GetValue(RESOLUTION);
                __instance.TimerResolution.Value = config.GetValue(TiMER_RESOLUTION);
                __instance.MinFOV.Value = config.GetValue(MIN_FOV);
                __instance.MaxFOV.Value = config.GetValue(MAX_FOV);
                __instance.DebugGesture.Value = config.GetValue(DEBUG_GESTURE);
                __instance.MinDistance.Value = config.GetValue(MIN_DISTANCE);
                __instance.MaxDistance.Value = config.GetValue(MAX_DISTANCE);

            }
        }
    }
}