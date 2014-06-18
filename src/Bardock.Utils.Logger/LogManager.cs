﻿using System;
using System.Collections.Generic;
using System.Reflection;
namespace Bardock.Utils.Logger
{
    /// <summary>
    /// Manages the ILog instances
    /// </summary>
    public class LogManager
    {
        private Dictionary<Type, ILog> _logs = new Dictionary<Type, ILog>();


        private static LogManager _default = new LogManager();

        /// <summary>
        /// Get the default <see cref="LogManager" /> instance.
        /// </summary>
        public static LogManager Default { get { return _default; } }


        private ILogFactory _factory = null;

        /// <summary>
        /// Get or set ILogFactory implementation used to create the ILog instances.
        /// Using this property you can inject the desired ILogFactory.
        /// If there is defined the implementation in config, it will be used by default.
        /// Otherwise a <see cref="NullLogFactory"/> is used.
        /// </summary>
        public ILogFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    if (ConfigSection.Default == null || String.IsNullOrEmpty(ConfigSection.Default.LogFactory))
                    {
                        _factory = new NullLogFactory();
                    }
                    else
                    {
                        _factory = GetConfiguredLogFactory();
                    }
                }
                return _factory;
            }
            set 
            {
                _factory = value;
            }
        }

        private ILogFactory GetConfiguredLogFactory()
        {
            string[] assemblyAndTypeNames = ConfigSection.Default.LogFactory.Split(new char[] { ',' });
            var assembly = Assembly.Load(assemblyAndTypeNames[1].Trim());
            var type = assembly.GetType(assemblyAndTypeNames[0].Trim());
            return (ILogFactory)Activator.CreateInstance(type);
        }

        public ILog GetLog<T>()
        {
            return GetLog(typeof(T));
        }

        public ILog GetLog(object o)
        {
            return GetLog(o.GetType());
        }

        public ILog GetLog(Type t)
        {
            if ((!_logs.ContainsKey(t)))
            {
                _logs[t] = Factory.GetLog(t);
            }
            return _logs[t];
        }

    }
}