using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;

namespace HellTower.Audio
{
    public class SoundManager : IDisposable
    {
        private IWavePlayer _musicPlayer;
        private AudioFileReader _musicReader;
        private float _musicVolume = 1.0f;

        private readonly Dictionary<string, CachedSound> _effects = new Dictionary<string, CachedSound>();
        private float _effectsVolume = 1.0f;

        private readonly string _audioPath;

        private readonly MixingSampleProvider _sfxMixer;
        private readonly IWavePlayer _sfxPlayer;

        public SoundManager(string audioFolderPath)
        {
            _audioPath = audioFolderPath;
            _sfxMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
            {
                ReadFully = true
            };
            _sfxPlayer = new WaveOutEvent();
            _sfxPlayer.Init(_sfxMixer);
            _sfxPlayer.Play();
        }

        public void PlayMusic(string filename, bool loop = true)
        {
            StopMusic();
            var path = Path.Combine(_audioPath, filename);
            if (!File.Exists(path)) 
                return;
            _musicReader = new AudioFileReader(path) { Volume = _musicVolume };
            _musicPlayer = new WaveOutEvent();
            _musicPlayer.Init(_musicReader);
            _musicPlayer.PlaybackStopped += (s, e) =>
            {
                if (loop)
                {
                    _musicReader.Position = 0;
                    _musicPlayer.Play();
                }
            };
            _musicPlayer.Play();
        }

        public void PauseMusic() => _musicPlayer?.Pause();

        public void ResumeMusic() => _musicPlayer?.Play();

        public void StopMusic()
        {
            _musicPlayer?.Stop();
            _musicPlayer?.Dispose();
            _musicReader?.Dispose();
            _musicPlayer = null;
            _musicReader = null;
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Math.Max(0, Math.Min(1, volume));
            if (_musicReader != null)
                _musicReader.Volume = _musicVolume;
        }

        public void PlayEffect(string filename)
        {
            var path = Path.Combine(_audioPath, filename);
            if (!File.Exists(path)) return;

            if (!_effects.ContainsKey(filename))
                _effects[filename] = new CachedSound(path);

            ISampleProvider provider = new CachedSoundSampleProvider(_effects[filename]) { Volume = _effectsVolume };

            if (provider.WaveFormat.SampleRate != _sfxMixer.WaveFormat.SampleRate)
                provider = new WdlResamplingSampleProvider(provider, _sfxMixer.WaveFormat.SampleRate);

            if (provider.WaveFormat.Channels != _sfxMixer.WaveFormat.Channels)
            {
                if (_sfxMixer.WaveFormat.Channels == 2 && provider.WaveFormat.Channels == 1)
                    provider = new MonoToStereoSampleProvider(provider);
                else if (_sfxMixer.WaveFormat.Channels == 1 && provider.WaveFormat.Channels == 2)
                    provider = new StereoToMonoSampleProvider(provider);
            }
            _sfxMixer.AddMixerInput(provider);
        }

        public void SetEffectsVolume(float volume) => _effectsVolume = Math.Max(0, Math.Min(1, volume));

        public void Dispose()
        {
            StopMusic();
            _effects.Clear();
            _sfxPlayer?.Dispose();
        }
    }
}