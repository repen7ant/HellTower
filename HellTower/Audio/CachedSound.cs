using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace HellTower.Audio
{
    public class CachedSound
    {
        public float[] AudioData { get; }
        public WaveFormat WaveFormat { get; }

        public CachedSound(string audioFileName)
        {
            using (var audioFileReader = new AudioFileReader(audioFileName))
            {
                var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    for (var i = 0; i < samplesRead; i++)
                        wholeFile.Add(readBuffer[i]);
                AudioData = wholeFile.ToArray();
                WaveFormat = audioFileReader.WaveFormat;
            }
        }
    }

    public class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound _cachedSound;
        private long _position;
        public float Volume { get; set; } = 1.0f;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            _cachedSound = cachedSound;
        }

        public WaveFormat WaveFormat => _cachedSound.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = _cachedSound.AudioData.Length - _position;
            var samplesToCopy = Math.Min(availableSamples, count);
            if (samplesToCopy <= 0)
                return 0;

            for (var i = 0; i < samplesToCopy; i++)
                buffer[offset + i] = _cachedSound.AudioData[_position + i] * Volume;

            _position += samplesToCopy;
            return (int)samplesToCopy;
        }
    }
}