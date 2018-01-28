using System.Collections.Generic;
using GameObjects;

namespace Generators
{
    public interface IGenerator
    {
        IGameObject Generate();
    }
}
