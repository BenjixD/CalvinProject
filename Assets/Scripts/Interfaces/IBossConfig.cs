using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IBossConfig
{
    IDictionary<AIBehaviour.State, IntentIndex[]> GetIntentConfig();
}

