using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public enum GeneratorAlghorithm
    {
        HillGenerator,
        PerlinNoiseGenerator,
        TruePerlinNoiseGenerator, //MOCK
        RandomGenerator,
        RectangleGenerator,
        VoronoiGenerator, //MOCK
        DiamondSquareGenerator, //MOCK
        RandomWalkGenerator
    }
}
