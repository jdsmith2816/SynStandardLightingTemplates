using System;
using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using System.Drawing;

namespace SynStandardLightingTemplate
{
    public class Settings {
        public byte Color = 25;
    }
    internal class Program
    {
        public static Lazy<Settings>? LazySettings;
        public static Settings settings => LazySettings!.Value;
        public static Color ColorSet;
        public static CellLighting.Inherit cl = CellLighting.Inherit.AmbientColor | CellLighting.Inherit.DirectionalColor | CellLighting.Inherit.FogColor |
                    CellLighting.Inherit.FogNear | CellLighting.Inherit.FogFar | CellLighting.Inherit.DirectionalRotation | CellLighting.Inherit.DirectionalFade |
                    CellLighting.Inherit.ClipDistance | CellLighting.Inherit.FogPower | CellLighting.Inherit.FogMax | CellLighting.Inherit.LightFadeDistances;
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .SetAutogeneratedSettings<Settings>("Color Settings", "Colors.json", out LazySettings, false)
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "SynSLT.esp")
                .Run(args);
        }
        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            ColorSet = Color.FromArgb(settings.Color, settings.Color, settings.Color);
            foreach (var lt in state.LoadOrder.PriorityOrder.LightingTemplate().WinningContextOverrides())
            {
                Console.WriteLine($"Patching LIGHTING TEMPLATE {lt.Record.EditorID}");
                var nlt = lt.GetOrAddAsOverride(state.PatchMod);
                nlt.DirectionalRotationZ = 0;
                nlt.DirectionalRotationXY = 0;
                nlt.FogNear = 0;
                nlt.FogFar = 0;
                nlt.DirectionalFade = 0;
                nlt.FogPower = 1;
                nlt.FogMax = 1;
                nlt.FogClipDistance = 0;
                nlt.LightFadeEndDistance = 41000;
                nlt.LightFadeStartDistance = 30000;
                nlt.AmbientColor = ColorSet;
                nlt.DirectionalColor = ColorSet;
                nlt.FogNearColor = ColorSet;
                nlt.FogFarColor = ColorSet;
                nlt.AmbientColors.Scale = 0;
                nlt.AmbientColors.Specular = ColorSet;
                nlt.AmbientColors.DirectionalXMinus = ColorSet;
                nlt.AmbientColors.DirectionalXPlus = ColorSet;
                nlt.AmbientColors.DirectionalYMinus = ColorSet;
                nlt.AmbientColors.DirectionalYPlus = ColorSet;
                nlt.AmbientColors.DirectionalZMinus = ColorSet;
                nlt.AmbientColors.DirectionalZPlus = ColorSet;
                nlt.DirectionalAmbientColors = new();
                nlt.DirectionalAmbientColors.Scale = 0;
                nlt.DirectionalAmbientColors.Specular = ColorSet;
                nlt.DirectionalAmbientColors.DirectionalXMinus = ColorSet;
                nlt.DirectionalAmbientColors.DirectionalXPlus = ColorSet;
                nlt.DirectionalAmbientColors.DirectionalYMinus = ColorSet;
                nlt.DirectionalAmbientColors.DirectionalYPlus = ColorSet;
                nlt.DirectionalAmbientColors.DirectionalZMinus = ColorSet;
                nlt.DirectionalAmbientColors.DirectionalZPlus = ColorSet;
            }
            foreach (var cel in state.LoadOrder.PriorityOrder.Cell().WinningContextOverrides(state.LinkCache))
            {
                if (cel.Record != null && cel.Record.Lighting != null && (cel.Record.LightingTemplate.IsNull || cel.Record.Lighting.Inherits != cl))
                {
                    var nc = cel.GetOrAddAsOverride(state.PatchMod);
                    Console.WriteLine($"Patching CELL {nc.Name?.ToString() ?? nc.EditorID?.ToString()}");
                    if (nc.LightingTemplate.IsNull)
                    {
                        nc.LightingTemplate.SetTo(Skyrim.LightingTemplate.DefaultLightingTemplate);
                    }
                    if (nc.Lighting != null && nc.Lighting.Inherits != cl)
                    {
                        nc.Lighting.Inherits = cl;
                    }
                }
            };

            //Blackreach weather
            var nw = state.PatchMod.Weathers.GetOrAddAsOverride(Skyrim.Weather.BlackreachWeather.Resolve(state.LinkCache));
            nw.EffectLightingColor.Sunrise = ColorSet;
            nw.EffectLightingColor.Day = ColorSet;
            nw.EffectLightingColor.Sunset = ColorSet;
            nw.EffectLightingColor.Night = ColorSet;
            nw.FogFarColor.Sunrise = ColorSet;
            nw.FogFarColor.Day = ColorSet;
            nw.FogFarColor.Sunset = ColorSet;
            nw.FogFarColor.Night = ColorSet;
            nw.WaterMultiplierColor.Sunrise = ColorSet;
            nw.WaterMultiplierColor.Day = ColorSet;
            nw.WaterMultiplierColor.Sunset = ColorSet;
            nw.WaterMultiplierColor.Night = ColorSet;
            nw.FogDistanceDayFar = 0;
            nw.FogDistanceDayNear = 0;
            nw.FogDistanceNightFar = 0;
            nw.FogDistanceNightNear = 0;
            nw.FogDistanceDayPower = 0;
            nw.FogDistanceNightPower = 0;
            nw.FogDistanceNightMax = 0;
            nw.FogDistanceDayMax = 0;
            nw.ThunderLightningFrequency = new(0.0);
            nw.PrecipitationBeginFadeIn = new(0.0);
            nw.PrecipitationEndFadeOut = new(0.0);
            nw.Flags -= Weather.Flag.Snow;
            nw.DirectionalAmbientLightingColors = new();
            nw.DirectionalAmbientLightingColors.Sunrise.DirectionalXMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunrise.DirectionalXPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunrise.DirectionalYMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunrise.DirectionalYPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunrise.DirectionalZMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunrise.DirectionalZPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunrise.Specular = System.Drawing.Color.FromArgb(0, 0, 0);
            nw.DirectionalAmbientLightingColors.Sunrise.Scale = 0;
            nw.DirectionalAmbientLightingColors.Day.DirectionalXMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Day.DirectionalXPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Day.DirectionalYMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Day.DirectionalYPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Day.DirectionalZMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Day.DirectionalZPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Day.Specular = System.Drawing.Color.FromArgb(0, 0, 0);
            nw.DirectionalAmbientLightingColors.Day.Scale = 0;
            nw.DirectionalAmbientLightingColors.Sunset.DirectionalXMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunset.DirectionalXPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunset.DirectionalYMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunset.DirectionalYPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunset.DirectionalZMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunset.DirectionalZPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Sunset.Specular = System.Drawing.Color.FromArgb(0, 0, 0);
            nw.DirectionalAmbientLightingColors.Sunset.Scale = 0;
            nw.DirectionalAmbientLightingColors.Night.DirectionalXMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Night.DirectionalXPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Night.DirectionalYMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Night.DirectionalYPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Night.DirectionalZMinus = ColorSet;
            nw.DirectionalAmbientLightingColors.Night.DirectionalZPlus = ColorSet;
            nw.DirectionalAmbientLightingColors.Night.Specular = System.Drawing.Color.FromArgb(0, 0, 0);
            nw.DirectionalAmbientLightingColors.Night.Scale = 0;
        }
    }
}
