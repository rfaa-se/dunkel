using System;
using Dunkel.Game.ComponentSystems.Draw;
using Dunkel.Game.ComponentSystems.Update;
using Dunkel.Game.Entities;
using Dunkel.Game.Commands;
using Dunkel.Game.Components;
using Dunkel.Game.ComponentSystems;
using Dunkel.Game.Input;
using Dunkel.Game.Options;
using Dunkel.Game.States;
using Dunkel.Game.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dunkel.Game
{
    public class Dunkel : Microsoft.Xna.Framework.Game
    {
        // MANAGERS
        private GraphicsDeviceManager _graphicsDeviceManager;
        private InputManager _inputManager;
        private GameState _gameState; // TODO: move me once a state manager is implemented

        // ENGINE
        private SpriteBatch _spriteBatch;
        private long _tickSize;
        private long _tickAccumulator;

        // OPTIONS
        private EngineOptions _options;

        // UTILITIES
        private FrameRater _frameRater;

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddOptions();
            
            services.AddSingleton<Microsoft.Xna.Framework.Game>(this);

            services.AddSingleton<GraphicsDeviceManager>();
            services.AddSingleton<InputManager>();
            services.AddSingleton<ComponentSystemManager>();
            services.AddSingleton<CommandManager>();

            services.AddSingleton<GameState>();
            
            services.AddSingleton<EntityFactory>();
            services.AddSingleton<ComponentFactory>();

            services.AddSingleton<IDrawComponentSystem, DrawRectangleSystem>();
            services.AddSingleton<IUpdateComponentSystem, MovementSystem>();
            services.AddSingleton<IUpdateComponentSystem, PreviousBodySystem>();

            services.AddSingleton<FrameRater>();

            return services;
        }

        public void ConfigureServices(IServiceProvider app)
        {
            _graphicsDeviceManager = app.GetRequiredService<GraphicsDeviceManager>();
            _inputManager = app.GetRequiredService<InputManager>();

            _options = app.GetRequiredService<IOptions<EngineOptions>>().Value;

            _gameState = app.GetRequiredService<GameState>();

            _frameRater = app.GetRequiredService<FrameRater>();
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            Window.Title = "Dunkel";
            IsMouseVisible = true;
            IsFixedTimeStep = false;

            _graphicsDeviceManager.PreferredBackBufferWidth = 1366;
            _graphicsDeviceManager.PreferredBackBufferHeight = 768;
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
            _graphicsDeviceManager.ApplyChanges();

            _tickSize = TimeSpan.TicksPerSecond / _options.TicksPerSecond;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            var delta = _tickAccumulator / (float)_tickSize;
            _gameState.Draw(_spriteBatch, delta);

            _spriteBatch.End();

            _frameRater.Update(gameTime.ElapsedGameTime.TotalSeconds);

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update(Keyboard.GetState(), Mouse.GetState());
            _gameState.Input(_inputManager);

            _tickAccumulator += gameTime.ElapsedGameTime.Ticks;
            while (_tickAccumulator > _tickSize)
            {
                UpdateFixed();

                _tickAccumulator -= _tickSize;
            }

            base.Update(gameTime);
        }

        private void UpdateFixed()
        {
            _gameState.Update();

            Window.Title = $"Dunkel | FPS:{_frameRater.Fps}";
        }
    }
}
