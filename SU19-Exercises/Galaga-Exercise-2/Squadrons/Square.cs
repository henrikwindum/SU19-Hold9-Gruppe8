using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2.Squadrons {
    public class Square : ISquadron {
        private int yrows;

        public Square(int maxrows) {
            if (maxrows > 5) {
                throw new Exception("Enemies must be on top half of the screen");
            }

            Enemies = new EntityContainer<Enemy>();
            MaxEnemies = Enemies.CountEntities();
            yrows = maxrows;
        }

        public EntityContainer<Enemy> Enemies { get; set; }
        public int MaxEnemies { get; }

        void ISquadron.CreateEnemies(List<Image> enemyStrides) {
            for (var i = 1; i <= yrows; i++) {
                for (var j = 0; j < yrows * 1.3; j++) {
                    Enemies.AddDynamicEntity(
                        new Enemy(
                            new DynamicShape(
                                new Vec2F(0.3f - 0.1f * i / 2 + 0.1f * j,
                                    0.9f - 0.1f * yrows + 0.1f * i),
                                new Vec2F(0.1f, 0.1f)),
                            new ImageStride(80, enemyStrides)));
                }
            }
        }
    }
}