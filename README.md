# Trabalho Prático 02

#### NimbleGame Desenvolvido em MonoGame com C# 
  
##### Engenharia e Desenvolvimento de Jogos Digitais - Técnicas de Desenvolvimento de Videojogos 
###### Gabriel Rosas nº27943 / Ricardo Costa nº27927 / João Reis nº27917 

# __Indíce__
1. [__Introdução__](#Introdução)
2. [__Procedimentos do Jogo__](#procedimentos)
3. [__Implementação__](#Implementaçao)
4. [__Interpretação do Código-Fonte__](#analise)
5. [__Conclusão__](#conclusao)

# __Introdução__

NimbleGame é um jogo 2D desenvolvido em MonoGame com a linguagem C#. Nele, os jogadores enfrentam desafios estilo Mario, desde monstros a saltos precisos, enquanto buscam chegar ao final do jogo e coletar moedas para maximizar sua pontuação no menor tempo possivel. Com folhas de sprites de alta qualidade, efeitos sonoros envolventes e outros recursos que garantem uma experiência realista, NimbleGame promete diversão e adrenalina para os fãs de jogos de plataforma 2D.

<a name="procedimentos"></a>
# __Procedimentos do Jogo__

* __Objetivo:__
    - Passar as Fases todas com a maior pontuaçao possivel e no menor tempo possivel
* __Controlos:__
    - Utilizar as teclas WASD ou as setas do teclado
* __Score:__
    - Com mais coins e menos tempo maior será o score
      
<a name="implementaçao"></a>
# __Implementação__

* __Code:__
    - [ Animation.cs](#animation)
    - [ AnimationPlayer.cs](#animationplayer)  
    - [ Circle.cs](#circle)
    - [ Enemy.cs](#enemy)
    - [ Gem.cs](#gem)
    - [ Level.cs](#level)
    - [ Player.cs](#player)
    - [ RectangleExtensions.cs](#rectangleextensions)
    - [ Tile.cs](#tile)
    
  
      
 __Code:__ É uma pasta com o código-fonte do jogo.

<a name="analise"></a>
# __Interpretação do Código-Fonte__


<a name="animation"></a>
## 	__Animation.cs:__
Este código define uma classe Animation para representar uma textura animada no contexto de um jogo 2D utilizando a plataforma Microsoft XNA. A classe assume que cada frame da animação é quadrado e calcula o número de frames com base nessa suposição. A propriedade Texture armazena todas as frames da animação dispostas horizontalmente. A propriedade FrameTime especifica a duração de exibição de cada frame, enquanto IsLooping indica se a animação deve reiniciar ao alcançar o fim. A classe também inclui as propriedades FrameCount, FrameWidth e FrameHeight para obter o número de frames, a largura e a altura de cada frame, respectivamente. O construtor da classe inicializa estas propriedades recebendo como parâmetros a textura, o tempo de frame e o indicador de looping.

```
using System;
using Microsoft.Xna.Framework.Graphics;

namespace NimbleGame
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
Este código define uma estrutura AnimationPlayer que controla a reprodução de uma animação no contexto de um jogo 2D utilizando a plataforma Microsoft XNA. A estrutura possui uma propriedade Animation que retorna a animação atualmente em reprodução e uma propriedade FrameIndex que indica o índice do frame atual.
A variável time guarda o tempo decorrido em segundos que o frame atual foi exibido. A propriedade Origin calcula a origem da textura no centro inferior de cada frame.
O método PlayAnimation(Animation animation) inicia ou continua a reprodução de uma animação, reiniciando a animação apenas se uma nova animação for passada. O método Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects) avança o tempo, atualiza o frame atual da animação com base no tempo decorrido e desenha o frame atual na posição especificada. Se a animação estiver configurada para loop, o índice do frame é reiniciado no final; caso contrário, é fixado no último frame.
Em resumo, a estrutura AnimationPlayer gerencia a reprodução de animações, lidando com a transição dos frames e desenhando o frame atual na tela, garantindo um controle eficiente da animação em jogos 2D



```
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NimbleGame
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
Este código define uma estrutura Circle que representa um círculo 2D no contexto de um jogo utilizando a plataforma Microsoft XNA. A estrutura possui duas propriedades públicas: Center, que representa a posição central do círculo, e Radius, que representa o raio do círculo.
O construtor Circle(Vector2 position, float radius) inicializa o círculo com a posição central e o raio fornecidos.
A estrutura também inclui o método Intersects(Rectangle rectangle), que determina se o círculo intersecciona com um retângulo dado. O método calcula se há uma sobreposição entre o círculo e o retângulo, retornando true se houver interseção e false caso contrário.
Em resumo, a estrutura Circle fornece uma representação simples de um círculo 2D, com funcionalidades para inicialização e verificação de interseção com retângulos.

```
using System;
using Microsoft.Xna.Framework;

namespace NimbleGame
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
A classe Enemy define um inimigo que se move de um lado para o outro numa plataforma, mudando de direção ao encontrar obstáculos ou ao chegar ao fim da plataforma. Esta classe gere a animação e a posição do inimigo, assegurando que ele se comporte de maneira adequada no contexto do jogo. O inimigo alterna entre as animações de corrida e inatividade dependendo do seu estado atual e da interação com o jogador e o ambiente do jogo.

```
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NimbleGame
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
O código define a classe Gem para representar um item valioso que o jogador pode recolher num jogo 2D desenvolvido com a plataforma Microsoft XNA. A gema tem uma textura, uma posição no mundo do jogo e um efeito sonoro associado à sua recolha. Ela pode ser desenhada na tela e tem um movimento de salto no ar para atrair a atenção do jogador. Quando recolhida, reproduz um som e concede pontos ao jogador. Essencialmente, a classe encapsula a lógica para criar e manipular gemas no jogo.

```

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace NimbleGame
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
A classe Level representa um nível num jogo de plataforma 2D desenvolvido com a plataforma Microsoft XNA. Ela gerencia a estrutura física do nível, incluindo a disposição dos blocos de terreno, a localização do jogador, inimigos, gemas e a saída. Além disso, controla a lógica do jogo, como verificar colisões, atualizar o estado dos objetos no mundo, calcular a pontuação do jogador e verificar se o jogador alcançou a saída do nível. O nível também lida com o carregamento de conteúdo, como texturas e sons, e a renderização dos elementos na tela. É essencialmente o coração do jogo, onde todas as interações e eventos acontecem.

```

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace NimbleGame
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
A classe Player gerencia todas as interações e comportamentos do personagem jogador em um jogo de plataforma 2D. Isso inclui controle de animações para ações como parar, correr, pular, celebrar e morrer. Além disso, ela controla a física do jogador, lidando com movimento horizontal e vertical, pulos, e detecção e resolução de colisões com o ambiente do jogo, como plataformas e obstáculos.
O jogador pode se mover horizontalmente usando controles analógicos, teclado ou gamepad, e pular usando botões específicos. A altura e duração do pulo são ajustadas para oferecer ao jogador mais controle sobre o salto.
A classe Player também gerencia o estado do jogador, incluindo se está vivo, se está no chão e sua posição atual. Além disso, ela carrega e reproduz sons correspondentes às ações do jogador, como pular, cair e ser morto, e desenha o jogador na tela com base em sua posição e animação atual. Em suma, é responsável por todas as interações relacionadas ao jogador no jogo de plataforma 2D


```
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NimbleGame
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
A classe RectangleExtensions oferece métodos úteis para trabalhar com retângulos em um ambiente de jogo 2D. O método GetIntersectionDepth calcula a profundidade da interseção entre dois retângulos, retornando um vetor que indica o quanto eles se sobrepõem em cada eixo. Isso permite determinar a direção correta para empurrar objetos a fim de resolver colisões. O método GetBottomCenter retorna a posição do centro da borda inferior do retângulo. Esses métodos são úteis para detecção e resolução de colisões entre objetos no jogo.

```
using System;
using Microsoft.Xna.Framework;

namespace NimbleGame
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
Este excerto de código define uma estrutura Tile e um TileCollision utilizados para representar os diferentes tipos de blocos. A estrutura Tile tem dois campos: Texture, que guarda a textura do bloco, e Collision, que indica o tipo de colisão do bloco (Passable, Impassable ou Platform). A estrutura também define as dimensões padrão de um bloco (Width e Height) e disponibiliza a constante Size para facilitar o acesso às dimensões. O enum TileCollision especifica os possíveis tipos de colisão que um bloco pode ter, como Passable (transponível), Impassable (impossível de transpor) e Platform (plataforma). Estes elementos são úteis para a construção e gestão do ambiente do jogo, permitindo a definição de diferentes comportamentos e interações com os blocos.


```
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NimbleGame
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
<a name="conclusao"></a>
# __Conclusão:__
Em suma, o desenvolvimento deste jogo para além de nos mostrar uma parte desconhecida no processo criativo do jogo, por vezes em certos momentos pode ser stressante. Apesar disso, o desenvolvimento do NimbleGame foi um trabalho extremamente interessante e pertinente para o curso. Este projeto não apenas proporcionou uma oportunidade prática para aplicar conceitos de programação em C# e o uso do framework MonoGame, como também permitiu explorar áreas como design de sprites, efeitos sonoros e mecânicas de jogo. 
<p align="center">
 <img src=""  alt="PongPc" width=300>
</p>
