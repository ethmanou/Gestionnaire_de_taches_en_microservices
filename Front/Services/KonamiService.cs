// KonamiService.cs

using System;
using System.Collections.Generic;
using Front.Entities;

public class KonamiService : IKonamiCodeHandler
{
    public readonly List<string> konamiCode = new List<string>
    {
        "ArrowUp",
        "ArrowUp"
    };

    public int konamiCodeIndex = 0;

    public event Action KonamiCodeCompleted;

    public void HandleKeyPress(string key)
    {
        Console.WriteLine($"Key pressed: {key}");
        if (key == konamiCode[konamiCodeIndex])
        {
            konamiCodeIndex++;

            if (konamiCodeIndex == konamiCode.Count)
            {
                KonamiCodeCompleted?.Invoke();
                konamiCodeIndex = 0;
            }
        }
        else
        {
            konamiCodeIndex = 0;
        }
    }
    
}
