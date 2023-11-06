using Unity.Entities;
using Unity.Mathematics;

namespace chillestCapybara
{
    public struct GraveyardRandom : IComponentData
    {
        public Random value;
    }
}