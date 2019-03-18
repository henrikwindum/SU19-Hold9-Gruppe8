using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2.Squadrons {
    public class Multirow : ISquadron {
        private int maxrow;

        public Multirow(int yrows) {
            if (yrows > 5) {
                throw new Exception("Method Multirow: There can be a maximum of 5 rows");
            }

            Enemies = new EntityContainer<Enemy>();
            maxrow = yrows;
            MaxEnemies = Enemies.CountEntities();
        }

        public int MaxEnemies { get; }
        public EntityContainer<Enemy> Enemies { get; set; }

        void ISquadron.CreateEnemies(List<Image> enemyStrides) {
            for (var i = 1; i < maxrow; i++) {
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