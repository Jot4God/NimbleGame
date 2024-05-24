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

<p align="center">
 <img src="https://i.imgur.com/otOTTVR.png"  alt="NimbleGame" width=700>
</p>

<a name="procedimentos"></a>
# __Procedimentos do Jogo__

* __Objetivo:__
    - Conseguir ultrapassar todos os nivéis com menor tempo possível
* __Controlos:__
    - É possível utilizar o WASD/Arrow Keys e o "SPACE" para saltar
* __Score:__
    - Com mais coins e menos tempo maior será o score, de seguida compara com o teu amigo para definir um vencedor.
      
<a name="implementaçao"></a>
# __Implementação__

## __Estrutura de Pastas:__

* __Pong:__
    - __Content:__
      * Backgrounds
      * Levels
      * Overlays
      * Sounds
      * Sprites
      * Tiles
     
        
* __Code:__
    - [ Program.cs](#program)
    - [ PlatformGame.cs](#game)
    - [ Animation.cs](#animation)
    - [ AnimationPlayer.cs](#animationplayer)  
    - [ Circle.cs](#circle)
    - [ Enemy.cs](#enemy)
    - [ Gem.cs](#gem)
    - [ Layer.cs](#layer)
    - [ Level.cs](#level)
    - [ Player.cs](#player)
    - [ RectangleExtensions.cs](#rectangleextensions)
    - [ Tile.cs](#tile)
    
  
      
 __Code:__ É uma pasta com o código-fonte do jogo.

## __Melhorias:__

* __Adicionar sistema de ataque com os inimigos;__
* __Adicionar sistema que cria niveis novos automaticamente;__
* __Adicionar um menu ao jogo;__
* __Adicionar uma interface de som e uma leaderboard;__


<a name="conclusao"></a>
# __Conclusão:__
Em suma, o desenvolvimento deste jogo para além de nos mostrar uma parte desconhecida no processo criativo do jogo, por vezes em certos momentos pode ser stressante. Apesar disso, o desenvolvimento do NimbleGame foi um trabalho extremamente interessante e pertinente para o curso. Este projeto não apenas proporcionou uma oportunidade prática para aplicar conceitos de programação em C# e o uso do framework MonoGame, como também permitiu explorar áreas como design de sprites, efeitos sonoros e mecânicas de jogo. 

<a name="analise"></a>
# __Interpretação do Código-Fonte__

<a name="program"></a>
##  __Program.cs:__
O código Program.cs executa o jogo de plataforma através do nome NimbleGame. Além disso, inclui o namespace NimbleGame, este cria uma variável com o nome PlatformerGame. Por fim, é executada a função Run da variável do jogo para iniciar o loop principal e a execução do jogo.

<a name="game"></a>
##  __PlatformerGame.cs:__
Este código configura e gerencia um jogo de plataforma 2D, lidando com gráficos, lógica de jogo, entrada de usuário e desenho na tela. Ele carrega níveis, atualiza o estado do jogo e exibe informações relevantes ao jogador, proporcionando uma experiência de jogo completa.

<a name="animation"></a>
## 	__Animation.cs:__
Este código define uma classe Animation para representar uma textura animada no contexto de um jogo 2D utilizando a plataforma Microsoft XNA. A classe assume que cada frame da animação é quadrado e calcula o número de frames com base nessa suposição. A propriedade Texture armazena todas as frames da animação dispostas horizontalmente. A propriedade FrameTime especifica a duração de exibição de cada frame, enquanto IsLooping indica se a animação deve reiniciar ao alcançar o fim. A classe também inclui as propriedades FrameCount, FrameWidth e FrameHeight para obter o número de frames, a largura e a altura de cada frame, respectivamente. O construtor da classe inicializa estas propriedades recebendo como parâmetros a textura, o tempo de frame e o indicador de looping.


<a name="animationplayer"></a>
## 	__AnimationPlayer.cs:__
Este código define uma estrutura AnimationPlayer que controla a reprodução de uma animação no contexto de um jogo 2D utilizando a plataforma Microsoft XNA. A estrutura possui uma propriedade Animation que retorna a animação atualmente em reprodução e uma propriedade FrameIndex que indica o índice do frame atual.
A variável time guarda o tempo decorrido em segundos que o frame atual foi exibido. A propriedade Origin calcula a origem da textura no centro inferior de cada frame.
O método PlayAnimation(Animation animation) inicia ou continua a reprodução de uma animação, reiniciando a animação apenas se uma nova animação for passada. O método Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects) avança o tempo, atualiza o frame atual da animação com base no tempo decorrido e desenha o frame atual na posição especificada. Se a animação estiver configurada para loop, o índice do frame é reiniciado no final; caso contrário, é fixado no último frame.
Em resumo, a estrutura AnimationPlayer gerencia a reprodução de animações, lidando com a transição dos frames e desenhando o frame atual na tela, garantindo um controle eficiente da animação em jogos 2D

<a name="circle"></a>
## 	__Circle.cs:__
Este código define uma estrutura Circle que representa um círculo 2D no contexto de um jogo utilizando a plataforma Microsoft XNA. A estrutura possui duas propriedades públicas: Center, que representa a posição central do círculo, e Radius, que representa o raio do círculo.
O construtor Circle(Vector2 position, float radius) inicializa o círculo com a posição central e o raio fornecidos.
A estrutura também inclui o método Intersects(Rectangle rectangle), que determina se o círculo intersecciona com um retângulo dado. O método calcula se há uma sobreposição entre o círculo e o retângulo, retornando true se houver interseção e false caso contrário.
Em resumo, a estrutura Circle fornece uma representação simples de um círculo 2D, com funcionalidades para inicialização e verificação de interseção com retângulos.

<a name="enemy"></a>
## 	__Enemy.cs:__
A classe Enemy define um inimigo que se move de um lado para o outro numa plataforma, mudando de direção ao encontrar obstáculos ou ao chegar ao fim da plataforma. Esta classe gere a animação e a posição do inimigo, assegurando que ele se comporte de maneira adequada no contexto do jogo. O inimigo alterna entre as animações de corrida e inatividade dependendo do seu estado atual e da interação com o jogador e o ambiente do jogo.

<a name="gem"></a>
## 	__Gem.cs:__
O código define a classe Gem para representar um item valioso que o jogador pode recolher num jogo 2D desenvolvido com a plataforma Microsoft XNA. A gema tem uma textura, uma posição no mundo do jogo e um efeito sonoro associado à sua recolha. Ela pode ser desenhada na tela e tem um movimento de salto no ar para atrair a atenção do jogador. Quando recolhida, reproduz um som e concede pontos ao jogador. Essencialmente, a classe encapsula a lógica para criar e manipular gemas no jogo.

<a name="layer"></a>
##  __Layer.cs:__
A classe Layer é usada para criar e desenhar camadas de fundo que se deslocam (parallax scrolling) conforme a posição da câmera no jogo muda.

<a name="level"></a>
## 	__Level.cs:__
A classe Level representa um nível num jogo de plataforma 2D desenvolvido com a plataforma Microsoft XNA. Ela gerencia a estrutura física do nível, incluindo a disposição dos blocos de terreno, a localização do jogador, inimigos, gemas e a saída. Além disso, controla a lógica do jogo, como verificar colisões, atualizar o estado dos objetos no mundo, calcular a pontuação do jogador e verificar se o jogador alcançou a saída do nível. O nível também lida com o carregamento de conteúdo, como texturas e sons, e a renderização dos elementos na tela. É essencialmente o coração do jogo, onde todas as interações e eventos acontecem.

<a name="player"></a>
## 	__Player.cs:__
A classe Player gerencia todas as interações e comportamentos do personagem jogador em um jogo de plataforma 2D. Isso inclui controle de animações para ações como parar, correr, pular, celebrar e morrer. Além disso, ela controla a física do jogador, lidando com movimento horizontal e vertical, pulos, e detecção e resolução de colisões com o ambiente do jogo, como plataformas e obstáculos.
O jogador pode se mover horizontalmente usando controles analógicos, teclado ou gamepad, e pular usando botões específicos. A altura e duração do pulo são ajustadas para oferecer ao jogador mais controle sobre o salto.
A classe Player também gerencia o estado do jogador, incluindo se está vivo, se está no chão e sua posição atual. Além disso, ela carrega e reproduz sons correspondentes às ações do jogador, como pular, cair e ser morto, e desenha o jogador na tela com base em sua posição e animação atual. Em suma, é responsável por todas as interações relacionadas ao jogador no jogo de plataforma 2D


<a name="rectangleextensions"></a>
## 	__RectangleExtensions.cs:__
A classe RectangleExtensions oferece métodos úteis para trabalhar com retângulos em um ambiente de jogo 2D. O método GetIntersectionDepth calcula a profundidade da interseção entre dois retângulos, retornando um vetor que indica o quanto eles se sobrepõem em cada eixo. Isso permite determinar a direção correta para empurrar objetos a fim de resolver colisões. O método GetBottomCenter retorna a posição do centro da borda inferior do retângulo. Esses métodos são úteis para detecção e resolução de colisões entre objetos no jogo.

<a name="tile"></a>
## 	__Tile.cs:__
Este excerto de código define uma estrutura Tile e um TileCollision utilizados para representar os diferentes tipos de blocos. A estrutura Tile tem dois campos: Texture, que guarda a textura do bloco, e Collision, que indica o tipo de colisão do bloco (Passable, Impassable ou Platform). A estrutura também define as dimensões padrão de um bloco (Width e Height) e disponibiliza a constante Size para facilitar o acesso às dimensões. O enum TileCollision especifica os possíveis tipos de colisão que um bloco pode ter, como Passable (transponível), Impassable (impossível de transpor) e Platform (plataforma). Estes elementos são úteis para a construção e gestão do ambiente do jogo, permitindo a definição de diferentes comportamentos e interações com os blocos.

