using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga_Exercise_2.Squadrons {
    public interface ISquadron {
        EntityContainer<Enemy> Enemies { get; set; }
        int MaxEnemies { get; }

        void CreateEnemies(List<Image> enemyStrides);
    }
}