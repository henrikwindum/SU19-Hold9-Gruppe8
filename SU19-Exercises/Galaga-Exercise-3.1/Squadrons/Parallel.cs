using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3._1.GalagaEntities;

namespace Galaga_Exercise_3._1.Squadrons {
    public class Parallel : ISquadron {
        private int rows;

        public Parallel(int maxRows) {
            if (maxRows > 5 || maxRows < 2) {
                throw new Exception("Method Parallel: Input must be 2 <= maxrows <= 5");
            }

            Enemies = new EntityContainer<Enemy>();
            MaxEnemies = Enemies.CountEntities();
            rows = maxRows;
        }

        public EntityContainer<Enemy> Enemies { get; set; }
        public int MaxEnemies { get; }

        void ISquadron.CreateEnemies(List<Image> enemyStrides) {
            for (var i = 1; i <= rows; i++) {
                for (var j = 0; j < rows * 1.3; j++) {
                    Enemies.AddDynamicEntity(
                        new Enemy(
                            new DynamicShape(
                                new Vec2F(0.3f - 0.1f * i / 2 + 0.1f * j,
                                    0.9f - 0.1f * rows + 0.1f * i),
                                new Vec2F(0.1f, 0.1f)),
                            new ImageStride(80, enemyStrides)));
                }
            }
        }
    }
}