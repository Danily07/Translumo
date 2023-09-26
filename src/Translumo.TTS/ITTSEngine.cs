using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translumo.TTS;

public interface ITTSEngine
{
    void SpeechText(string text);
}
