using System.Collections.Generic;
using GameObjects;

namespace Generators
{
    public interface IGenerator
    {
        IGameObject Generate(float offsetX = 0, float offsetY = 0);
    }
}
