﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Routing
{
    /// <summary>
    /// A router that routes messages to a method conditionally.
    /// </summary>
    sealed public class ConditionalMethodRouter : IRouter
    {
        /// <summary>
        /// Create a conditional router targeting the specified method.
        /// </summary>
        /// <param name="condition">Condition that must be met for messages to be routed.</param>
        /// <param name="action">Action to run if the condition is met.</param>
        public ConditionalMethodRouter(Func<IMessage, bool> condition, Action<IMessage> action)
        {
            Action = action;
            Condition = condition;
        }

        /// <summary>
        /// Method to route messages to if the condition is met.
        /// </summary>
        public readonly Action<IMessage> Action;

        /// <summary>
        /// Condition to check messages against. Returns true if met.
        /// </summary>
        public readonly Func<IMessage, bool> Condition;

        /// <summary>
        /// If the condition is met, route the message and return true. Returns false otherwise.
        /// </summary>
        /// <param name="message">Message to route.</param>
        /// <returns>True if the message was handled.</returns>
        public bool Route(IMessage message)
        {
            if (Condition(message))
            {
                Action(message);
                return true;                
            }
            return false;
        }
    }
}