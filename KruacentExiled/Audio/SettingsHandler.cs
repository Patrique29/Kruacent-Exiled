using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Settings.SettingsCategories;
using System;
using System.Collections.Generic;
using UserSettings.ServerSpecific;

namespace KruacentExiled.Audio
{
    internal class SettingsHandler: IUsingEvents
    {
        private int _idSound = 900;
        private int _idNoise = 901;
        private int _idMusic = 902;

        public const float DefaultValue = 50;

        private AudioHandler handler;
        public bool Debug => handler.Debug;
        public SettingsHandler(AudioHandler handler)
        {
            this.handler = handler;
            HeaderSetting header = new HeaderSetting(_idSound, "Sound");
            List<SettingBase> settings = new List<SettingBase>()
            {
                CreateSliderSetting(_idNoise,SoundType.Noise,header),
                CreateSliderSetting(_idMusic,SoundType.Music,header),
            };

            
            new SettingsCategory(header, 1005, settings);
        }


        public void SubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += SafeOnSettingValueReceived;
        }

        public void UnsubscribeEvents()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= SafeOnSettingValueReceived;
        }
        private void SafeOnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            //not catching the exception will desync & kick the player
            try
            {
                OnSettingValueReceived(Player.Get(hub), settingBase);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void OnSettingValueReceived(Player player, ServerSpecificSettingBase settingBase)
        {

            float volume = 0; 
            SoundType type = SoundType.Noise;
            bool flag = false;
            if (settingBase.SettingId == _idMusic)
            {
                volume = GetMusicVolume(player) / 25;
                type = SoundType.Music;
                flag = true;
            }

            if (settingBase.SettingId == _idNoise)
            {
                volume = GetNoiseVolume(player) / 25;
                type = SoundType.Noise;
                flag = true;

            }

            if (flag)
            {
                KELog.Debug($"change volume {type} to {volume}% ");

                MainPlugin.AudioHandler.ChangeVolume(player, volume, type);
            }
            

        }

        private SliderSetting CreateSliderSetting(int id, SoundType type, HeaderSetting header)
        {
            return new SliderSetting(id, type.ToString(), 0, 100, DefaultValue, header:header);
        }

        internal float GetNoiseVolume(Player player)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(player, _idNoise, out var setting)) return DefaultValue;
            return setting.SliderValue;
        }

        internal float GetMusicVolume(Player player)
        {
            if (!SettingBase.TryGetSetting<SliderSetting>(player, _idMusic, out var setting)) return DefaultValue;
            return setting.SliderValue;
        }


    }
}
