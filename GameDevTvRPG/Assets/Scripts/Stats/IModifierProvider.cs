using System.Collections.Generic;

namespace Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stats stat);
    }
}
