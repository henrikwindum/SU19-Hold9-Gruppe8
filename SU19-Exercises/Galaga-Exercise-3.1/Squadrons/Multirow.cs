using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3._1.GalagaEntities;

namespace Galaga_Exercise_3._1.Squadrons {
    public class Multirow : ISquadron {
        private int maxRow;

        public Multirow(int rows) {
            if (rows > 5) {
                throw new Exception("Method Multirow: There can be a maximum of 5 rows");
            }

            Enemies = new EntityContainer<Enemy>();
            maxRow = rows;
            MaxEnemies = Enemies.CountEntities();
        }

        public int MaxEnemies { get; }
        public EntityContainer<Enemy> Enemies { get; set; }

        void ISquadron.CreateEnemies(List<Image> enemyStrides) {
            for (var i = 1; i < maxRow; i++) {
                for (var j = 1; j < 9; j++) {
                    Enemies.AddDynamicEntity(
                        new Enemy(new DynamicShape(new Vec2F(0.1f * j, 1.0f - 0.1f * i),
                                new Vec2F(0.1f, 0.1f)),
                            new ImageStride(80, enemyStrides)));
                }
            }
        }
    }
}