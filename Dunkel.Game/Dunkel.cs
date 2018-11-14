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
using System.Diagnostics;
using Dunkel.Game.Network.Packets;
using Dunkel.Game.Network;

namespace Dunkel.Game
{
    public class Dunkel : Microsoft.Xna.Framework.Game
    {
        // MANAGERS
        private GraphicsDeviceManager _graphicsDeviceManager;
        private InputManager _inputManager;
        private NetworkManager _networkManager;
        private GameState _gameState; // TODO: move me once a state manager is implemented

        // ENGINE
        private SpriteBatch _spriteBatch;
        private long _tickSize;
        private long _tickAccumulator;
        private TimeSpan _tickTime;
        private int _tickCounter;
        private int _ticksPerSecond;

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
            services.AddSingleton<NetworkManager>();
            services.AddSingleton<PacketManager>();

            services.AddSingleton<GameState>();
            
            services.AddSingleton<EntityFactory>();
            services.AddSingleton<ComponentFactory>();

            services.AddSingleton<IDrawComponentSystem, DrawRectangleSystem>();
            services.AddSingleton<IUpdateComponentSystem, MovementSystem>();
            services.AddSingleton<IUpdateComponentSystem, PreviousBodySystem>();

            services.AddSingleton<FrameRater>();
            services.AddSingleton<SelectionBox>();

            return services;
        }

        public void ConfigureServices(IServiceProvider app)
        {
            _graphicsDeviceManager = app.GetRequiredService<GraphicsDeviceManager>();
            _inputManager = app.GetRequiredService<InputManager>();
            _networkManager = app.GetRequiredService<NetworkManager>();

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
                _tickCounter++;
            }

            _tickTime += gameTime.ElapsedGameTime;
            if (_tickTime >= TimeSpan.FromSeconds(1))
            {
                _tickTime -= TimeSpan.FromSeconds(1);
                _ticksPerSecond = _tickCounter;
                _tickCounter = 0;
            }

            base.Update(gameTime);
        }

        private void UpdateFixed()
        {
            _networkManager.Update();
            _gameState.Update();

            Window.Title = $"Dunkel | TPS:{_ticksPerSecond} | FPS:{_frameRater.Fps}";
        }
    }
}
