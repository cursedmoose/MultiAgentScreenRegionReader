# MultiAgentScreenRegionReader
 Allows a user to select a screen region to be read by a TTS Agent

### Warning: this will cost you actual real hard earned dollars to run because ElevenLabs is a business.

# Prerequisites
1. You need an ElevenLabs Account: https://elevenlabs.io/sign-up
2. ElevenLabs will give you an API Key. Do not share this.

# Installation
You have to do this manually because I'm a doofus and I don't know how to do it for you yet.
1. Install [.Net Runtime 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Install Python dependencies in an Administrator `cmd.exe` window
- Do you have Python? `python --version`
  - No? -> Install [Python 3.12](https://www.python.org/downloads/release/python-3120/)
  - Yes? Continue on, friend.
- Install [elevenlabs module](https://github.com/elevenlabs/elevenlabs-python)
  - `pip install elevenlabs`

3. Install MPV https://mpv.io/
- `choco install mpv`
  - or download the installer

# Setup
 To run:
 - Add your ElevenLabs API Key into `config/elevenlabs.config.json`
  - You can also configure your narrator voice here.
 - Run MultiAgentScreenRegionReader.exe

 Controls:
 - Alt+Ctrl+Space will begin selecting a screen region. This will be saved until you close the program.
 - Alt+Space will take a screenshot of the selected region, run Tesseract over it, feed the output to ElevenLabs, and play a TTS audio stream using MPV.

## Contributing
I don't know how to do this either but the repo is at https://github.com/cursedmoose/MultiAgentScreenRegionReader

I wrote it in C# because I think it's fun and easy to write.

If you like this thing, you can pay me back by sending me a nice message.

## Special Thanks
- My friends who watched me develop [Hellbot](https://github.com/cursedmoose/HellBot), the progenitor to this software
- CohhCarnage for the need, the idea, and the moral support
