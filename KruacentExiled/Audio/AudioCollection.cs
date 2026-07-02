using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Audio
{
    public class AudioCollection
    {


        private HashSet<AudioPlayer> noise;
        private HashSet<AudioPlayer> music;

        //private Dictionary<SoundType, HashSet<AudioPlayer>> audioPlayers;

        public AudioCollection()
        {
            

            noise = new HashSet<AudioPlayer>();
            music = new HashSet<AudioPlayer>();
        }


        public void AddPlayer(AudioPlayer player,SoundType type)
        {
            if(type == SoundType.Noise)
            {
                noise.Add(player);
            }

            if(type == SoundType.Music)
            {
                music.Add(player);
            }
        }

        public void ChangeVolume(float newVolume, SoundType type)
        {
            if(type == SoundType.Noise)
            {
                ChangeVolume(noise, newVolume);
            }

            if(type == SoundType.Music)
            {
                ChangeVolume(music, newVolume);
            }
        }


        private void ChangeVolume(HashSet<AudioPlayer> players, float newVolume)
        {
            foreach(AudioPlayer player in players)
            {
                foreach(AudioClipPlayback clip in player.ClipsById.Values)
                {
                    clip.Volume = newVolume;
                }
            }
        }

        public void DestroyAudioPlayer(AudioPlayer player)
        {
            foreach (AudioPlayer audio in noise.ToList())
            {
                if(audio == player)
                {
                    noise.Remove(player);
                }
            }

            foreach (AudioPlayer audio in music.ToList())
            {
                if (audio == player)
                {
                    music.Remove(player);
                }
            }
        }


    }
}
