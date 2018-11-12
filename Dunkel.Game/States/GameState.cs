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
using Microsoft.Xna.Framework;

namespace Dunkel.Game.States
{
    public class GameState
    {
        private readonly ComponentSystemManager _componentSystemManager;
        private readonly CommandManager _commandManager;
        private readonly SelectionBox _selectionBox;
        private readonly EntityFactory _entityFactory;
        private readonly List<Entity> _entities;
        private readonly Queue<ICommand> _queuedCommands;
        private readonly Queue<Point> _queuedEntites;

        public GameState(ComponentSystemManager componentSystemManager, CommandManager commandManager, 
            SelectionBox selectionBox, EntityFactory entityFactory)
        {
            _componentSystemManager = componentSystemManager ?? throw new ArgumentNullException(nameof(componentSystemManager));
            _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            _selectionBox = selectionBox ?? throw new ArgumentNullException(nameof(selectionBox));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            
            _entities = new List<Entity>();
            _queuedCommands = new Queue<ICommand>();
            _queuedEntites = new Queue<Point>();
        }

        public void Input(InputManager im)
        {
            _selectionBox.Input(im);

            if (im.IsMouseLeftDown())
            {
                _queuedEntites.Enqueue(im.GetMousePosition());
            }
        }

        public void Update()
        {
            while (_queuedEntites.TryDequeue(out var mouse))
            {/*
                var ship = _entityFactory.GetShip();
                var body = ship.GetComponent<BodyComponent>();
                var speed = ship.GetComponent<SpeedComponent>();
                body.SetDimension(50, 50);
                body.SetPosition(mouse.X, mouse.Y);
                speed.Velocity = new flint(5, 25);

                _entities.Add(ship);*/
            }

            while (_queuedCommands.TryDequeue(out var command))
            {
                // TODO: send queued commands
            }

            // TODO: handle each new command for this tick

            _componentSystemManager.Update();
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            _componentSystemManager.Draw(sb, delta);

            _selectionBox.Draw(sb, delta);
        }
    }
}