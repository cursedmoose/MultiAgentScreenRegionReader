using CursedMoose.MASR.Logging;
using CursedMoose.MASR.OCR;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CursedMoose.MASR.ElevenLabs
{
    public record ElevenLabsConfig(
        string api_key,
        string model,
        bool remove_start_pattern,
        string start_pattern,
        VoiceSettings narrator_settings
        );

    public record VoiceSettings(
        string voice_name,
        string voice_id,
        float stability,
        float similarity,
        float style
    );

    public class ElevenLabs
    {
        public static readonly object TtsLock = new();

        readonly Logger log = new("ElevenLabs");
        readonly ElevenLabsConfig Config;
        public static readonly ElevenLabs Narrator = new(ReadConfigFile());

        public ElevenLabs(ElevenLabsConfig config)
        {
            Config = config;
        }

        public static ElevenLabsConfig ReadConfigFile()
        {
            var configText = File.ReadAllText("config/elevenlabs.config.json");
            var config = JsonSerializer.Deserialize<ElevenLabsConfig>(configText, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (config == null)
            {
                Console.WriteLine("No config file for elevenlabs was found. Creating a new one. Please be more careful with it.");
            }
            if (string.IsNullOrEmpty(config.api_key)) {
                Console.WriteLine("Please set an api_key in elevenlabs.config.json");
            }

            return config;
        }

        private void RunTtsStreamTask(string tts)
        {
            var program_arguments = string.Join(" ", "/C python lib/labs.py", Config.api_key, Config.narrator_settings.voice_id, Config.model);
            var tts_arguments = BuildStreamArgs(tts);

            if (tts_arguments.Trim().Length <= 0 ) {
                log.Error("No text found in selection.");
                return;
            }

            var all_arguments = string.Join(" ", program_arguments, tts_arguments);

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = all_arguments;
            process.StartInfo.CreateNoWindow = false;

            lock (TtsLock)
            {
                process.Start();
                process.WaitForExit();
            }
        }

        private string BuildStreamArgs(string inputString)
        {
            var cleanedInput = inputString;
            if (Config.remove_start_pattern)
            {
                cleanedInput = Regex.Replace(inputString, Config.start_pattern, "");
            }

            string[] sentences = Regex.Split(cleanedInput, @"(?<=[\.!\?])\s+");
            List<string> sentence_arguments = new List<string>();

            foreach (string sentence in sentences)
            {
                var cleanSentence = sentence
                    .Replace("\n", " ")
                    .Replace("\"", "\\\"")
                    .Replace("|", "I");
                sentence_arguments.Add(string.Join("", "\"", cleanSentence, "\""));
            }

            return string.Join(" ", sentence_arguments.ToArray());
        }

        public void StreamTts(string tts)
        {
            if (!string.IsNullOrEmpty(tts))
            {
                Task.Run(() => RunTtsStreamTask(tts));
            }
        }

        public async Task ReadImage(string filePath = "images/region.png")
        {
            var stopwatch = Stopwatch.StartNew();
            var text = await ImageTextReader.Instance.ReadText(filePath);
            stopwatch.Stop();
            log.Info($"Model time: {stopwatch.ElapsedMilliseconds}ms");
            StreamTts(text);
        }

        public async Task ReadImage(Bitmap bmp)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                var text = await ImageTextReader.Instance.ReadText(bmp);
                stopwatch.Stop();
                log.Info($"Model time: {stopwatch.ElapsedMilliseconds}ms");
                StreamTts(text);
            }
            catch (Exception ex)
            {
                log.Error($"Could not do TTS because of {ex.Source}: {ex.Message}");
            }
        }
    }
}
