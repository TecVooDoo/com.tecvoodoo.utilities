// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Utility methods for inserting and removing systems from Unity's PlayerLoop.
    /// Used internally by TimerBootstrapper.
    /// </summary>
    public static class PlayerLoopUtils
    {
        /// <summary>
        /// Removes a system from the player loop at the specified loop phase.
        /// </summary>
        /// <typeparam name="T">The PlayerLoop phase type (e.g. Update).</typeparam>
        /// <param name="loop">The root player loop system (passed by ref).</param>
        /// <param name="systemToRemove">The system to remove.</param>
        public static void RemoveSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToRemove)
        {
            if (loop.subSystemList == null) return;

            List<PlayerLoopSystem> subSystems = new List<PlayerLoopSystem>(loop.subSystemList);
            for (int i = 0; i < subSystems.Count; i++)
            {
                if (subSystems[i].type == systemToRemove.type
                    && subSystems[i].updateDelegate == systemToRemove.updateDelegate)
                {
                    subSystems.RemoveAt(i);
                    loop.subSystemList = subSystems.ToArray();
                    return;
                }
            }

            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
                RemoveSystem<T>(ref loop.subSystemList[i], systemToRemove);
            }
        }

        /// <summary>
        /// Inserts a system into the player loop at the specified phase and index.
        /// </summary>
        /// <typeparam name="T">The PlayerLoop phase type to insert into (e.g. Update).</typeparam>
        /// <param name="loop">The root player loop system (passed by ref).</param>
        /// <param name="systemToInsert">The system to insert.</param>
        /// <param name="index">The index within the phase's subsystem list.</param>
        /// <returns>True if the system was inserted successfully.</returns>
        public static bool InsertSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index)
        {
            if (loop.type == typeof(T))
            {
                List<PlayerLoopSystem> subSystems = new List<PlayerLoopSystem>();
                if (loop.subSystemList != null)
                {
                    subSystems.AddRange(loop.subSystemList);
                }
                subSystems.Insert(index, systemToInsert);
                loop.subSystemList = subSystems.ToArray();
                return true;
            }

            if (loop.subSystemList == null) return false;

            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
                if (InsertSystem<T>(ref loop.subSystemList[i], in systemToInsert, index))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Prints the full PlayerLoop hierarchy to the Unity console for debugging.
        /// </summary>
        /// <param name="loop">The player loop to print.</param>
        public static void PrintPlayerLoop(PlayerLoopSystem loop)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Unity Player Loop");

            if (loop.subSystemList != null)
            {
                for (int i = 0; i < loop.subSystemList.Length; i++)
                {
                    PrintSubsystem(loop.subSystemList[i], sb, 0);
                }
            }

            Debug.Log(sb.ToString());
        }

        static void PrintSubsystem(PlayerLoopSystem system, StringBuilder sb, int level)
        {
            sb.Append(' ', level * 2).AppendLine(system.type.ToString());

            if (system.subSystemList == null || system.subSystemList.Length == 0) return;

            for (int i = 0; i < system.subSystemList.Length; i++)
            {
                PrintSubsystem(system.subSystemList[i], sb, level + 1);
            }
        }
    }
}
