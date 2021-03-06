﻿using System.Runtime.InteropServices;

namespace FerretLib.SFML
{
    /// <summary>
    /// TODO: move to appropriate FerretLib
    /// </summary>
    internal class HighPerformanceTimer
    {
        [DllImport("Kernel32.dll")]
        protected static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        protected static extern bool QueryPerformanceFrequency(out long lpFrequency);
        
        private readonly long POLL_INTERVAL; // Number of 'ticks' per second
        protected readonly double POLL_MULTIPLIER; // Multiply by this to convert ticks to seconds

        public HighPerformanceTimer()
        {                       
            QueryPerformanceFrequency(out POLL_INTERVAL);
            POLL_MULTIPLIER = 1d / POLL_INTERVAL;               
        }

        public double GetTicks()
        {
            long ticks;
            QueryPerformanceCounter(out ticks);
            return ticks * POLL_MULTIPLIER;
        }
    }
}
