using System;
using System.Collections.Generic;
using Dunkel.Game.Components;
using Dunkel.Game.Entities;
using Dunkel.Game.Commands;
using Dunkel.Game.ComponentSystems;
using Dunkel.Game.Input;
using Dunkel.Game.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Dunkel.Game.Components.Attributes;

namespace Dunkel.Game.States
{
    public class GameState
    {
        private readonly ComponentSystemManager _componentSystemManager;
        private readonly CommandManager _commandManager;
        private readonly EntityFactory _entityFactory;
        private readonly Stack<Entity> _entities;
        private readonly List<ICommand> _queuedCommands;

        public GameState(ComponentSystemManager componentSystemManager, CommandManager commandManager, 
            EntityFactory entityFactory)
        {
            _componentSystemManager = componentSystemManager ?? throw new ArgumentNullException(nameof(componentSystemManager));
            _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            
            _entities = new Stack<Entity>();
            _queuedCommands = new List<ICommand>();
        }

        public void Input(InputManager im)
        {
            _queuedCommands.Clear();

            if (im.IsMouseLeftDown())
            {
                var mouse = im.GetMousePosition();
                var ship = _entityFactory.GetShip();
                var body = ship.GetComponent<BodyComponent>();
                var speed = ship.GetComponent<SpeedComponent>();
                body.SetDimension(50, 50);
                body.SetPosition(mouse.X, mouse.Y);
                speed.Velocity = new flint(5, 25);

                _entities.Push(ship);

                // TODO: add new build command to _commands
            }

            /*if (im.IsMouseRightDown())
            {
                if (_entities.TryPop(out var entity))
                {
                    entity.Die();
                }
            }*/
        }

        public void Update()
        {
            // TODO: send queued commands

            // TODO: handle each new command for this tick

            _componentSystemManager.Update();
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            _componentSystemManager.Draw(sb, delta);
        }
    }
}