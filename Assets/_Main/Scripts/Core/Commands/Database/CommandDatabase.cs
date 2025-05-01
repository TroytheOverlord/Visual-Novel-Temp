using System.Collections.Generic;
using System;
using UnityEngine;

namespace COMMANDS
{
    public class CommandDatabase
    {
        // Dictionary to store command names and their associated delegates
        private Dictionary<string, Delegate> database = new Dictionary<string, Delegate>();

        // Method to check if a command exists in the database
        public bool HasCommand(string commandName) => database.ContainsKey(commandName);

        // Method to add a command to the database
        public void AddCommand(string commandName, Delegate command)
        {
            commandName = commandName.ToLower();

            // Check if the command already exists in the database
            if (!database.ContainsKey(commandName))
            {
                database.Add(commandName, command); // Corrected line (fixed semicolon)
            }
            else
            {
                Debug.LogError($"Command already exists in the database: '{commandName}'");
            }
        }

        // Method to retrieve a command from the database
        public Delegate GetCommand(string commandName)
        {
            commandName = commandName.ToLower();

            // Check if the command exists in the database
            if (!database.ContainsKey(commandName))
            {
                Debug.LogError($"Command '{commandName}' does not exist in the database");
                return null;
            }

            return database[commandName]; // Fixed the access to 'database'
        }
    }
}

