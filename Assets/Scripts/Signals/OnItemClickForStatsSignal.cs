using UnityEngine;

namespace Signals
{
    public struct OnItemClickForStatsSignal
    {
        public Vector2 Coord;

        public OnItemClickForStatsSignal(Vector2 coord)
        {
            Coord = coord;
        }
    }
}