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









## 	__Accelerometer.cs:__

```

#region File Description
//-----------------------------------------------------------------------------
// Accelerometer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using System;
#endregion

namespace Platformer2D
{
    /// <summary>
    /// A static encapsulation of accelerometer input to provide games with a polling-based
    /// accelerometer system.
    /// </summary>
    public static class Accelerometer
    {
        // we want to prevent the Accelerometer from being initialized twice.
        private static bool isInitialized = false;

        // whether or not the accelerometer is active
        private static bool isActive = false;

        /// <summary>
        /// Initializes the Accelerometer for the current game. This method can only be called once per game.
        /// </summary>
        public static void Initialize()
        {
            // make sure we don't initialize the Accelerometer twice
            if (isInitialized)
            {
                throw new InvalidOperationException("Initialize can only be called once");
            }

            // remember that we are initialized
            isInitialized = true;
        }

        /// <summary>
        /// Gets the current state of the accelerometer.
        /// </summary>
        /// <returns>A new AccelerometerState with the current state of the accelerometer.</returns>
        public static AccelerometerState GetState()
        {
            // make sure we've initialized the Accelerometer before we try to get the state
            if (!isInitialized)
            {
                throw new InvalidOperationException("You must Initialize before you can call GetState");
            }

            // create a new value for our state
            Vector3 stateValue = new Vector3();

            return new AccelerometerState(stateValue, isActive);
        }
    }

    /// <summary>
    /// An encapsulation of the accelerometer's current state.
    /// </summary>
    public struct AccelerometerState
    {
        /// <summary>
        /// Gets the accelerometer's current value in G-force.
        /// </summary>
        public Vector3 Acceleration { get; private set; }

        /// <summary>
        /// Gets whether or not the accelerometer is active and running.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Initializes a new AccelerometerState.
        /// </summary>
        /// <param name="acceleration">The current acceleration (in G-force) of the accelerometer.</param>
        /// <param name="isActive">Whether or not the accelerometer is active.</param>
        public AccelerometerState(Vector3 acceleration, bool isActive)
            : this()
        {
            Acceleration = acceleration;
            IsActive = isActive;
        }

        /// <summary>
        /// Returns a string containing the values of the Acceleration and IsActive properties.
        /// </summary>
        /// <returns>A new string describing the state.</returns>
        public override string ToString()
        {
            return string.Format("Acceleration: {0}, IsActive: {1}", Acceleration, IsActive);
        }
    }
}
