using System.Collections.Generic;

namespace Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stats stat);
        IEnumerable<float> GetPercentageModifiers(Stats stat);
    }
}
