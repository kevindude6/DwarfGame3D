using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BradGame3D.Entities;
using System.Diagnostics;

namespace BradGame3D.Art
{

    public class ParticleManager
    {
        public SpriteSheet particleSheet;
        public enum ParticleType { CLOUD, BLOOD, DUST };
        public class Particle
        {
            public Vector3 pos;
            public Vector3 vel;
            public Vector3 accel;
            public float rot;
            public float rotVel;
            public ParticleType type;
            public BillboardVertex[] verts;
            public float width;
            public float height;
            public float life;
            public float maxLife;
        }
        public class Emitter
        {
            public Vector3 pos;
            public Vector3 velMin;
            public Vector3 velMax;
            public Vector3 accelMin;
            public Vector3 accelMax;
            public ParticleType type;
            public float widthMean;
            public float widthDev;
            public float heightMean;
            public float heightDev;
            public float life;
            public int particleEmitCount;
            public float emitSpeed;
            public float timeSinceLastParticle;
            public Random r;

            public Emitter(Vector3 p, Vector3 vmin, Vector3 vmax, ParticleType t, float wM, float wD, float hM, float hD, float l, int pCount, float eSpeed, Vector3 aMin, Vector3 aMax)
            {
                pos = p;
                velMin = vmin;
                velMax = vmax;
                accelMax = aMax;
                accelMin = aMin;
                type = t;
                widthMean = wM;
                widthDev = wD;
                heightMean = hM;
                heightDev = hD;
                life = l;
                particleEmitCount = pCount;
                emitSpeed = eSpeed;
                timeSinceLastParticle = emitSpeed;
                r = new Random();
            }
        }
        public List<Particle> particles = new List<Particle>();
        public List<Emitter> emitters = new List<Emitter>();

        public ParticleManager(GameScreen gameScreen)
        {
            SpriteSheet s = gameScreen.game.Content.Load<SpriteSheet>("SpriteData/" + "particles" + "DATA");
            s.setImage(gameScreen.game.Content.Load<Texture2D>("SpriteData/" + "particles"));
            particleSheet = s;
        }
        public void makeParticle(ParticleType t, Vector3 pos, Vector3 vel, Vector3 acceleration, float rotVel, float life, float width, float height)
        { 
            Particle p = new Particle();
            p.accel = acceleration;
            p.life = life;
            p.maxLife = life;
            p.pos = pos;
            p.vel = vel;
            p.type = t;
            p.rotVel = rotVel;
            p.width = width;
            p.height = height;
            p.verts = new BillboardVertex[6];
            p.verts[0] = new BillboardVertex(pos, particleSheet.getTexCoords((int)p.type,SpriteSheet.Corner.TL), new Vector2(-width / 2, height / 2));
            p.verts[1] = new BillboardVertex(pos, particleSheet.getTexCoords((int)p.type,SpriteSheet.Corner.TR), new Vector2(width / 2, height / 2));
            p.verts[2] = new BillboardVertex(pos, particleSheet.getTexCoords((int)p.type,SpriteSheet.Corner.BL), new Vector2(-width / 2, -height / 2));
            p.verts[3] = p.verts[2];
            p.verts[4] = p.verts[1];
            p.verts[5] = new BillboardVertex(pos, particleSheet.getTexCoords((int)p.type, SpriteSheet.Corner.BR), new Vector2(width / 2, -height / 2));
            particles.Add(p);
        }
        public void updateParticles(GameTime gameTime)
        {
            Emitter e;
            for (int a = emitters.Count() - 1; a >= 0; a--)
            {
                e = emitters[a];
                e.timeSinceLastParticle += gameTime.ElapsedGameTime.Milliseconds;
                while(e.timeSinceLastParticle >= e.emitSpeed)
                {
                    Vector3 vel = RandomFunctions.vecBetweenMinMax(e.r,e.velMin,e.velMax);
                    Vector3 accel = RandomFunctions.vecBetweenMinMax(e.r,e.accelMin,e.accelMin);
                    makeParticle(e.type, e.pos, vel, accel, 0, e.life, RandomFunctions.Normal(e.r, e.widthDev, e.widthMean), RandomFunctions.Normal(e.r, e.heightDev, e.heightMean));
                    e.timeSinceLastParticle -= e.emitSpeed;
                    e.particleEmitCount -= 1;
                    if (e.particleEmitCount <= 0)
                    {
                        emitters.RemoveAt(a);
                        break;
                    }
                }
                
            }
            Particle p;
            for (int i = 0; i < particles.Count(); i++ )
            {
                p = particles[i];
                p.vel = p.vel + p.accel * gameTime.ElapsedGameTime.Milliseconds/100f;
                p.pos = p.pos + p.vel * gameTime.ElapsedGameTime.Milliseconds / 100f;
                //Debug.WriteLine(p.pos);
                
                p.verts[0] = new BillboardVertex(p.pos, particleSheet.getTexCoords((int)p.type, SpriteSheet.Corner.TL), new Vector2(-p.width / 2, p.height / 2));
                p.verts[1] = new BillboardVertex(p.pos, particleSheet.getTexCoords((int)p.type, SpriteSheet.Corner.TR), new Vector2(p.width / 2, p.height / 2));
                p.verts[2] = new BillboardVertex(p.pos, particleSheet.getTexCoords((int)p.type, SpriteSheet.Corner.BL), new Vector2(-p.width / 2, -p.height / 2));
                p.verts[3] = p.verts[2];
                p.verts[4] = p.verts[1];
                p.verts[5] = new BillboardVertex(p.pos, particleSheet.getTexCoords((int)p.type, SpriteSheet.Corner.BR), new Vector2(p.width / 2, -p.height / 2));

                p.life -= gameTime.ElapsedGameTime.Milliseconds;
            }
            for (int i = particles.Count()-1; i >= 0; i--)
            {
                p = particles[i];
                if (p.life < 0)
                    particles.RemoveAt(i);
                    
            }
             
        }
        public void draw(GraphicsDeviceManager g, Effect e, EffectPass pass)
        {
            foreach (Particle p in particles)
            {
                e.Parameters["alphaValue"].SetValue(1 - (float) Math.Pow((p.maxLife-p.life)/p.maxLife,5));
                pass.Apply();
                g.GraphicsDevice.DrawUserPrimitives<BillboardVertex>(PrimitiveType.TriangleList, p.verts, 0, p.verts.Length / 3);
            }
        }
    }
}
