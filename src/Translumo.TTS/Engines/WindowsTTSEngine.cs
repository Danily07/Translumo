using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Speech.Synthesis;

namespace Translumo.TTS.Engines;

public class WindowsTTSEngine : ITTSEngine
{
    private VoiceInfo _voiceInfo;
    private readonly SpeechSynthesizer _synthesizer;
    private readonly ReadOnlyDictionary<string, VoiceInfo> _voices;

    public WindowsTTSEngine(string languageCode)
    {
        _synthesizer = new SpeechSynthesizer();
        _synthesizer.SetOutputToDefaultAudioDevice();
        _synthesizer.Rate = 1;
        SpeechApiReflectionHelper.InjectOneCoreVoices(_synthesizer);
        _voices = _synthesizer.GetInstalledVoices(new CultureInfo(languageCode)).ToDictionary(x => x.VoiceInfo.Name, x => x.VoiceInfo).AsReadOnly();
        _voiceInfo = _voices.First().Value;
    }

    public void SpeechText(string text)
    {
        // https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/june/speech-text-to-speech-synthesis-in-net
        if (_voiceInfo == null)
        {
            return;
        }
        var builder = new PromptBuilder();
        builder.StartVoice(_voiceInfo);
        builder.AppendText(text);
        builder.EndVoice();
        _synthesizer.SpeakAsyncCancelAll();
        _synthesizer.SpeakAsync(builder);
    }

    public void Dispose()
    {
        _synthesizer.Dispose();
    }

    public string[] GetVoices() => _voices.Keys.ToArray();

    public void SetVoice(string voice) => _voiceInfo = _voices.First(x => x.Key.Equals(voice, StringComparison.OrdinalIgnoreCase)).Value;

    // by default SpeechSynthesizer show not all available voices
    // https://stackoverflow.com/a/71198211
    private static class SpeechApiReflectionHelper
    {
        private const string PROP_VOICE_SYNTHESIZER = "VoiceSynthesizer";
        private const string FIELD_INSTALLED_VOICES = "_installedVoices";

        private const string ONE_CORE_VOICES_REGISTRY = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices";

        private static readonly Type _objectTokenCategoryType = typeof(SpeechSynthesizer).Assembly
            .GetType("System.Speech.Internal.ObjectTokens.ObjectTokenCategory")!;

        private static readonly Type _voiceInfoType = typeof(SpeechSynthesizer).Assembly
            .GetType("System.Speech.Synthesis.VoiceInfo")!;

        private static readonly Type _installedVoiceType = typeof(SpeechSynthesizer).Assembly
            .GetType("System.Speech.Synthesis.InstalledVoice")!;


        public static void InjectOneCoreVoices(SpeechSynthesizer synthesizer)
        {
            var voiceSynthesizer = GetProperty(synthesizer, PROP_VOICE_SYNTHESIZER);
            if (voiceSynthesizer == null)
                throw new NotSupportedException($"Property not found: {PROP_VOICE_SYNTHESIZER}");

            var installedVoices = GetField(voiceSynthesizer, FIELD_INSTALLED_VOICES) as IList;
            if (installedVoices == null)
                throw new NotSupportedException($"Field not found or null: {FIELD_INSTALLED_VOICES}");

            if (_objectTokenCategoryType
                    .GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic)?
                    .Invoke(null, new object?[] { ONE_CORE_VOICES_REGISTRY }) is not IDisposable otc)
                throw new NotSupportedException($"Failed to call Create on {_objectTokenCategoryType} instance");

            using (otc)
            {
                if (_objectTokenCategoryType
                        .GetMethod("FindMatchingTokens", BindingFlags.Instance | BindingFlags.NonPublic)?
                        .Invoke(otc, new object?[] { null, null }) is not IList tokens)
                    throw new NotSupportedException($"Failed to list matching tokens");

                foreach (var token in tokens)
                {
                    if (token == null || GetProperty(token, "Attributes") == null)
                        continue;

                    var voiceInfo =
                        typeof(SpeechSynthesizer).Assembly
                            .CreateInstance(_voiceInfoType.FullName!, true,
                                BindingFlags.Instance | BindingFlags.NonPublic, null,
                                new object[] { token }, null, null);

                    if (voiceInfo == null)
                        throw new NotSupportedException($"Failed to instantiate {_voiceInfoType}");

                    var installedVoice =
                        typeof(SpeechSynthesizer).Assembly
                            .CreateInstance(_installedVoiceType.FullName!, true,
                                BindingFlags.Instance | BindingFlags.NonPublic, null,
                                new object[] { voiceSynthesizer, voiceInfo }, null, null);

                    if (installedVoice == null)
                        throw new NotSupportedException($"Failed to instantiate {_installedVoiceType}");

                    installedVoices.Add(installedVoice);
                }
            }
        }

        private static object? GetProperty(object target, string propName)
        {
            return target.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(target);
        }

        private static object? GetField(object target, string propName)
        {
            return target.GetType().GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(target);
        }
    }
}