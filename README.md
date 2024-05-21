# Trabalho Prático 02

#### NimbleGame Desenvolvido MonoGame com C# 
  
##### Engenharia e Desenvolvimento de Jogos Digitais - Técnicas de Desenvolvimento de Videojogos 
###### Gabriel Rosas nº27943 / Ricardo Costa nº27927 / João Reis 27917 

# __Indíce__
1. [__Introdução__](#Introdução)
2. [__Procedimentos do Jogo__](#procedimentos)
3. [__Implementação__](#Implementação)
4. [__Interpretação do Código-Fonte__](#analise)
5. [__Conclusão__](#Conclusão)

# __Introdução__

É composto por uma classe representa uma barra de jogo no Pong, que inclui métodos para mover a barra, controlar a velocidade da mesma e obter informações sobre a sua posição e textura. Através de uma breve análise, o primeiro bloco representa as declarações das variáveis que têm a função de guardar o efeito sonoro, a textura, a altura do mundo, a posição horizontal e vertical da barra e a posição vetical anterior, respetivamente. De seguida, estão presentes as constantes relativas à dimensão da barra, altura e largura e também o comprimento inicial, atual e máximo. No bloco de código seguinte, inicia-se a contrução da barra com os respetivos aspetos antes referidos. Por fim é adicionada a lógica para aumentar a velocidade da barra, para dar reset à velocidade, resets às posições iniciais, para mover para cima e para baixo, reset à textura, à posição anterior, ao centro da barra e a reprodução dos efeitos sonoros, respetivamente.





* __Code:__
    - [ Accelerometer.cs](#accelerometer)
    - [ Animation.cs](#animation)
    - [ AnimationPlayer.cs](#animationplayer)  
    - [ Circle.cs](#circle)
    - [ Enemy.cs](#enemy)
    - [ Gem.cs](#gem)
    - [ Level.cs](#level)
    - [ Player.cs](#player)
    - [ RectangleExtensions.cs](#rectangleextensions)
    - [ Tile.cs](#tile)
    - [ TouchCollectionExtensions.cs](#touchcollectionextensions)
  
      
 __Code:__ É uma pasta com o código-fonte do jogo.

<a name="analise"></a>
# __Interpretação do Código-Fonte__


<a name="accelerometer"></a>

## 	__Accelerometer.cs:__

```


namespace Platformer2D
{
    public static class Accelerometer
    {
        
        private static bool isInitialized = false;
        private static bool isActive = false;
        public static void Initialize()
        {
            if (isInitialized)
            {
                throw new InvalidOperationException("Initialize can only be called once");
            }
            isInitialized = true;
        }

        public static AccelerometerState GetState()
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("You must Initialize before you can call GetState");
            }

            Vector3 stateValue = new Vector3();

            return new AccelerometerState(stateValue, isActive);
        }
    }

    public struct AccelerometerState
    {
        public Vector3 Acceleration { get; private set; }

        public bool IsActive { get; private set; }
        public AccelerometerState(Vector3 acceleration, bool isActive)
            : this()
        {
            Acceleration = acceleration;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return string.Format("Acceleration: {0}, IsActive: {1}", Acceleration, IsActive);
        }
    }
}

```
<a name="animation"></a>
## 	__Animation.cs:__

```

using System;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D
{

    class Animation
    {
       
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

       
        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;

        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        public int FrameCount
        {
           
            get { return Texture.Width / FrameHeight; }
        }

        public int FrameWidth
        {
            get { return Texture.Height; }
        }

        public int FrameHeight
        {
            get { return Texture.Height; }
        }
    
        public Animation(Texture2D texture, float frameTime, bool isLooping)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
        }
    }
}
```

<a name="animationplayer"></a>
## 	__AnimationPlayer.cs:__


```
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D
{
    struct AnimationPlayer
    {
        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;

        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;

        private float time;

        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
        }

        public void PlayAnimation(Animation animation)
        {
            if (Animation == animation)
                return;

            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > Animation.FrameTime)
            {
                time -= Animation.FrameTime;

                if (Animation.IsLooping)
                {
                    frameIndex = (frameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                }
            }

            Rectangle source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);

            spriteBatch.Draw(Animation.Texture, position, source, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);
        }
    }
}

```
<a name="circle"></a>
## 	__Circle.cs:__

```
using System;
using Microsoft.Xna.Framework;

namespace Platformer2D
{
    struct Circle
    {
        public Vector2 Center;

        public float Radius;

        public Circle(Vector2 position, float radius)
        {
            Center = position;
            Radius = radius;
        }

        public bool Intersects(Rectangle rectangle)
        {
            Vector2 v = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(Center.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = Center - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }
    }
}

```
<a name="enemy"></a>
## 	__Enemy.cs:__

```
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D
{
    enum FaceDirection
    {
        Left = -1,
        Right = 1,
    }

    class Enemy
    {
        public Level Level
        {
            get { return level; }
        }
        Level level;

        public Vector2 Position
        {
            get { return position; }
        }
        Vector2 position;

        private Rectangle localBounds;

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        private Animation runAnimation;
        private Animation idleAnimation;
        private AnimationPlayer sprite;

        private FaceDirection direction = FaceDirection.Left;

        private float waitTime;

        private const float MaxWaitTime = 0.5f;

        private const float MoveSpeed = 64.0f;

        public Enemy(Level level, Vector2 position, string spriteSet)
        {
            this.level = level;
            this.position = position;

            LoadContent(spriteSet);
        }

        public void LoadContent(string spriteSet)
        {
            spriteSet = "Sprites/" + spriteSet + "/";
            runAnimation = new Animation(Level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true);
            idleAnimation = new Animation(Level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.15f, true);
            sprite.PlayAnimation(idleAnimation);

            // Calculate bounds within texture size.
            int width = (int)(idleAnimation.FrameWidth * 0.35);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.7);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }



        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float posX = Position.X + localBounds.Width / 2 * (int)direction;
            int tileX = (int)Math.Floor(posX / Tile.Width) - (int)direction;
            int tileY = (int)Math.Floor(Position.Y / Tile.Height);

            if (waitTime > 0)
            {
                waitTime = Math.Max(0.0f, waitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (waitTime <= 0.0f)
                {
                    // Then turn around.
                    direction = (FaceDirection)(-(int)direction);
                }
            }
            else
            {
                if (Level.GetCollision(tileX + (int)direction, tileY - 1) == TileCollision.Impassable ||
                    Level.GetCollision(tileX + (int)direction, tileY) == TileCollision.Passable)
                {
                    waitTime = MaxWaitTime;
                }
                else
                {
                    Vector2 velocity = new Vector2((int)direction * MoveSpeed * elapsed, 0.0f);
                    position = position + velocity;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Level.Player.IsAlive ||
                Level.ReachedExit ||
                Level.TimeRemaining == TimeSpan.Zero ||
                waitTime > 0)
            {
                sprite.PlayAnimation(idleAnimation);
            }
            else
            {
                sprite.PlayAnimation(runAnimation);
            }

            SpriteEffects flip = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }
    }
}


```
<a name="gem"></a>
## 	__Gem.cs:__

```

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Platformer2D
{
    class Gem
    {
        private Texture2D texture;
        private Vector2 origin;
        private SoundEffect collectedSound;

        public readonly int PointValue = 30;
        public readonly Color Color = Color.Yellow;

        private Vector2 basePosition;
        private float bounce;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        public Vector2 Position
        {
            get
            {
                return basePosition + new Vector2(0.0f, bounce);
            }
        }

        public Circle BoundingCircle
        {
            get
            {
                return new Circle(Position, Tile.Width / 3.0f);
            }
        }

        public Gem(Level level, Vector2 position)
        {
            this.level = level;
            this.basePosition = position;

            LoadContent();
        }

        public void LoadContent()
        {
            texture = Level.Content.Load<Texture2D>("Sprites/Gem");
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            collectedSound = Level.Content.Load<SoundEffect>("Sounds/GemCollected");
        }

        public void Update(GameTime gameTime)
        {
            const float BounceHeight = 0.18f;
            const float BounceRate = 3.0f;
            const float BounceSync = -0.75f;
       
            double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + Position.X * BounceSync;
            bounce = (float)Math.Sin(t) * BounceHeight * texture.Height;
        }

        public void OnCollected(Player collectedBy)
        {
            collectedSound.Play();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}

```
<a name="level"></a>
## 	__Level.cs:__

```

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Platformer2D
{
    class Level : IDisposable
    {
        private Tile[,] tiles;
        private Texture2D[] layers;
        private const int EntityLayer = 2;

        public Player Player
        {
            get { return player; }
        }
        Player player;

        private List<Gem> gems = new List<Gem>();
        private List<Enemy> enemies = new List<Enemy>();
      
        private Vector2 start;
        private Point exit = InvalidPosition;
        private static readonly Point InvalidPosition = new Point(-1, -1);
        private Random random = new Random(354668); // Arbitrary, but constant seed

        public int Score
        {
            get { return score; }
        }
        int score;

        public bool ReachedExit
        {
            get { return reachedExit; }
        }
        bool reachedExit;

        public TimeSpan TimeRemaining
        {
            get { return timeRemaining; }
        }
        TimeSpan timeRemaining;

        private const int PointsPerSecond = 5;
     
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        private SoundEffect exitReachedSound;

        #region Loading

        public Level(IServiceProvider serviceProvider, Stream fileStream, int levelIndex)
        {
            content = new ContentManager(serviceProvider, "Content");

            timeRemaining = TimeSpan.FromMinutes(2.0);

            LoadTiles(fileStream);

            layers = new Texture2D[3];
            for (int i = 0; i < layers.Length; ++i)
            {
                int segmentIndex = levelIndex;
                layers[i] = Content.Load<Texture2D>("Backgrounds/Layer" + i + "_" + segmentIndex);
            }
            exitReachedSound = Content.Load<SoundEffect>("Sounds/ExitReached");
        }

        private void LoadTiles(Stream fileStream)
        {
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            tiles = new Tile[width, lines.Count];

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
            }

            if (Player == null)
                throw new NotSupportedException("A level must have a starting point.");
            if (exit == InvalidPosition)
                throw new NotSupportedException("A level must have an exit.");

        }

        private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case '.':
                    return new Tile(null, TileCollision.Passable);

                case 'X':
                    return LoadExitTile(x, y);

                case 'G':
                    return LoadGemTile(x, y);

                case '-':
                    return LoadTile("Platform", TileCollision.Platform);

                case 'A':
                    return LoadEnemyTile(x, y, "MonsterA");
                case 'B':
                    return LoadEnemyTile(x, y, "MonsterB");
                case 'C':
                    return LoadEnemyTile(x, y, "MonsterC");
                case 'D':
                    return LoadEnemyTile(x, y, "MonsterD");

                case '~':
                    return LoadVarietyTile("BlockB", 2, TileCollision.Platform);

                case ':':
                    return LoadVarietyTile("BlockB", 2, TileCollision.Passable);

                case '1':
                    return LoadStartTile(x, y);

                case '#':
                    return LoadVarietyTile("BlockA", 7, TileCollision.Impassable);

                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

     
        private Tile LoadTile(string name, TileCollision collision)
        {
            return new Tile(Content.Load<Texture2D>("Tiles/" + name), collision);
        }

        private Tile LoadVarietyTile(string baseName, int variationCount, TileCollision collision)
        {
            int index = random.Next(variationCount);
            return LoadTile(baseName + index, collision);
        }


        private Tile LoadStartTile(int x, int y)
        {
            if (Player != null)
                throw new NotSupportedException("A level may only have one starting point.");

            start = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            player = new Player(this, start);

            return new Tile(null, TileCollision.Passable);
        }

        private Tile LoadExitTile(int x, int y)
        {
            if (exit != InvalidPosition)
                throw new NotSupportedException("A level may only have one exit.");

            exit = GetBounds(x, y).Center;

            return LoadTile("Exit", TileCollision.Passable);
        }

        private Tile LoadEnemyTile(int x, int y, string spriteSet)
        {
            Vector2 position = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            enemies.Add(new Enemy(this, position, spriteSet));

            return new Tile(null, TileCollision.Passable);
        }
        private Tile LoadGemTile(int x, int y)
        {
            Point position = GetBounds(x, y).Center;
            gems.Add(new Gem(this, new Vector2(position.X, position.Y)));

            return new Tile(null, TileCollision.Passable);
        }

        public void Dispose()
        {
            Content.Unload();
        }

        #endregion

        #region Bounds and collision

        public TileCollision GetCollision(int x, int y)
        {
            
            if (x < 0 || x >= Width)
                return TileCollision.Impassable;
            
            if (y < 0 || y >= Height)
                return TileCollision.Passable;

            return tiles[x, y].Collision;
        }

         
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
        }

     
        public int Width
        {
            get { return tiles.GetLength(0); }
        }

       
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        #endregion

        #region Update

        public void Update(
            GameTime gameTime, 
            KeyboardState keyboardState, 
            GamePadState gamePadState, 
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            
            if (!Player.IsAlive || TimeRemaining == TimeSpan.Zero)
            {
                
                Player.ApplyPhysics(gameTime);
            }
            else if (ReachedExit)
            {
              
                int seconds = (int)Math.Round(gameTime.ElapsedGameTime.TotalSeconds * 100.0f);
                seconds = Math.Min(seconds, (int)Math.Ceiling(TimeRemaining.TotalSeconds));
                timeRemaining -= TimeSpan.FromSeconds(seconds);
                score += seconds * PointsPerSecond;
            }
            else
            {
                timeRemaining -= gameTime.ElapsedGameTime;
                Player.Update(gameTime, keyboardState, gamePadState, accelState, orientation);
                UpdateGems(gameTime);

              
                if (Player.BoundingRectangle.Top >= Height * Tile.Height)
                    OnPlayerKilled(null);

                UpdateEnemies(gameTime);

                if (Player.IsAlive &&
                    Player.IsOnGround &&
                    Player.BoundingRectangle.Contains(exit))
                {
                    OnExitReached();
                }
            }

            if (timeRemaining < TimeSpan.Zero)
                timeRemaining = TimeSpan.Zero;
        }

        private void UpdateGems(GameTime gameTime)
        {
            for (int i = 0; i < gems.Count; ++i)
            {
                Gem gem = gems[i];

                gem.Update(gameTime);

                if (gem.BoundingCircle.Intersects(Player.BoundingRectangle))
                {
                    gems.RemoveAt(i--);
                    OnGemCollected(gem, Player);
                }
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);

                if (enemy.BoundingRectangle.Intersects(Player.BoundingRectangle))
                {
                    OnPlayerKilled(enemy);
                }
            }
        }

        private void OnGemCollected(Gem gem, Player collectedBy)
        {
            score += gem.PointValue;

            gem.OnCollected(collectedBy);
        }

        private void OnPlayerKilled(Enemy killedBy)
        {
            Player.OnKilled(killedBy);
        }

        private void OnExitReached()
        {
            Player.OnReachedExit();
            exitReachedSound.Play();
            reachedExit = true;
        }

        public void StartNewLife()
        {
            Player.Reset(start);
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= EntityLayer; ++i)
                spriteBatch.Draw(layers[i], Vector2.Zero, Color.White);

            DrawTiles(spriteBatch);

            foreach (Gem gem in gems)
                gem.Draw(gameTime, spriteBatch);

            Player.Draw(gameTime, spriteBatch);

            foreach (Enemy enemy in enemies)
                enemy.Draw(gameTime, spriteBatch);

            for (int i = EntityLayer + 1; i < layers.Length; ++i)
                spriteBatch.Draw(layers[i], Vector2.Zero, Color.White);
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // For each tile position
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // If there is a visible tile in that position
                    Texture2D texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        // Draw it in screen space.
                        Vector2 position = new Vector2(x, y) * Tile.Size;
                        spriteBatch.Draw(texture, position, Color.White);
                    }
                }
            }
        }

        #endregion
    }
}

```
<a name="player"></a>
## 	__Player.cs:__

```
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer2D
{
    class Player
    {
        // Animations
        private Animation idleAnimation;
        private Animation runAnimation;
        private Animation jumpAnimation;
        private Animation celebrateAnimation;
        private Animation dieAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private AnimationPlayer sprite;

        // Sounds
        private SoundEffect killedSound;
        private SoundEffect jumpSound;
        private SoundEffect fallSound;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        // Physics state
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

       
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

      
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f; 

       
        private const float MoveStickScale = 1.0f;
        private const float AccelerometerScale = 1.5f;
        private const Buttons JumpButton = Buttons.A;

        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

        private float movement;

        // Jumping state
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        private Rectangle localBounds;
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }


        public Player(Level level, Vector2 position)
        {
            this.level = level;

            LoadContent();

            Reset(position);
        }


        public void LoadContent()
        {
            // Load animated textures.
            idleAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Idle"), 0.1f, true);
            runAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Run"), 0.1f, true);
            jumpAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Jump"), 0.1f, false);
            celebrateAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Celebrate"), 0.1f, false);
            dieAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Die"), 0.1f, false);

            // Calculate bounds within texture size.            
            int width = (int)(idleAnimation.FrameWidth * 0.4);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.8);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);

            // Load sounds.            
            killedSound = Level.Content.Load<SoundEffect>("Sounds/PlayerKilled");
            jumpSound = Level.Content.Load<SoundEffect>("Sounds/PlayerJump");
            fallSound = Level.Content.Load<SoundEffect>("Sounds/PlayerFall");
        }


        public void Reset(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            isAlive = true;
            sprite.PlayAnimation(idleAnimation);
        }


        public void Update(
            GameTime gameTime, 
            KeyboardState keyboardState, 
            GamePadState gamePadState, 
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            GetInput(keyboardState, gamePadState, accelState, orientation);

            ApplyPhysics(gameTime);

            if (IsAlive && IsOnGround)
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0)
                {
                    sprite.PlayAnimation(runAnimation);
                }
                else
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }

            // Clear input.
            movement = 0.0f;
            isJumping = false;
        }

        private void GetInput(
            KeyboardState keyboardState, 
            GamePadState gamePadState,
            AccelerometerState accelState, 
            DisplayOrientation orientation)
        {
            movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            if (Math.Abs(accelState.Acceleration.Y) > 0.10f)
            {
                movement = MathHelper.Clamp(-accelState.Acceleration.Y * AccelerometerScale, -1f, 1f);

                if (orientation == DisplayOrientation.LandscapeRight)
                    movement = -movement;
            }

            if (gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
                movement = -1.0f;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadRight) ||
                     keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D))
            {
                movement = 1.0f;
            }

            isJumping =
                gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);
        }

        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);

            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;
         
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions();

            if (Position.X == previousPosition.X)
                velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        private float DoJump(float velocityY, GameTime gameTime)
        {
           
            if (isJumping)
            {
              
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    if (jumpTime == 0.0f)
                        jumpSound.Play();

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    sprite.PlayAnimation(jumpAnimation);
                }

                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        private void HandleCollisions()
        {

            Rectangle bounds = BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            isOnGround = false;

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    TileCollision collision = Level.GetCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {
                        Rectangle tileBounds = Level.GetBounds(x, y);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            if (absDepthY < absDepthX || collision == TileCollision.Platform)
                            {
                                if (previousBottom <= tileBounds.Top)
                                    isOnGround = true;

                                if (collision == TileCollision.Impassable || IsOnGround)
                                {
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);

                                    bounds = BoundingRectangle;
                                }
                            }
                            else if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                bounds = BoundingRectangle;
                            }
                        }
                    }
                }
            }

            previousBottom = bounds.Bottom;
        }

        public void OnKilled(Enemy killedBy)
        {
            isAlive = false;

            if (killedBy != null)
                killedSound.Play();
            else
                fallSound.Play();

            sprite.PlayAnimation(dieAnimation);
        }

        public void OnReachedExit()
        {
            sprite.PlayAnimation(celebrateAnimation);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X < 0)
                flip = SpriteEffects.None;

            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }
    }
}

```
<a name="rectangleextensions"></a>
## 	__RectangleExtensions.cs:__

```
using System;
using Microsoft.Xna.Framework;

namespace Platformer2D
{

    public static class RectangleExtensions
    {
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        public static Vector2 GetBottomCenter(this Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width / 2.0f, rect.Bottom);
        }
    }
}

```
<a name="tile"></a>
## 	__Tile.cs:__

```
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D
{
    enum TileCollision
    {
        Passable = 0,

        Impassable = 1,

        Platform = 2,
    }

    struct Tile
    {
        public Texture2D Texture;
        public TileCollision Collision;

        public const int Width = 40;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, TileCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }
}


```
<a name="touchcollectionextensions"></a>
## 	__TouchCollectionExtensions.cs:__

```

using Microsoft.Xna.Framework.Input.Touch;

namespace Platformer2D
{
    public static class TouchCollectionExtensions
    {
        public static bool AnyTouch(this TouchCollection touchState)
        {
            foreach (TouchLocation location in touchState)
            {
                if (location.State == TouchLocationState.Pressed || location.State == TouchLocationState.Moved)
                {
                    return true;
                }
            }
            return false;
        }
    }
}



