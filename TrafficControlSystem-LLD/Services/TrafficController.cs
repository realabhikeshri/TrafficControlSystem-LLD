using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficControlSystem_LLD.Enums;
using TrafficControlSystem_LLD.Exceptions;
using TrafficControlSystem_LLD.Models;

namespace TrafficControlSystem_LLD.Services
{
    public class TrafficController : ITrafficController
    {
        private readonly Intersection _intersection;
        private readonly List<PhaseConfig> _phases;
        private readonly object _lock = new();

        private CancellationTokenSource? _cts;
        private Task? _controlLoopTask;
        private IntersectionMode _mode = IntersectionMode.Normal;
        private int _currentPhaseIndex = 0;

        private TrafficController(Intersection intersection)
        {
            _intersection = intersection;
            _phases = intersection.Phases.ToList();

            ValidatePhases(_phases);
        }

        public static TrafficController CreateDefault()
        {
            var timingPlan = new TimingPlanService();
            var phases = timingPlan.GetDefaultPhases();
            var intersection = new Intersection(phases);
            return new TrafficController(intersection);
        }

        public void Start()
        {
            lock (_lock)
            {
                if (_controlLoopTask != null && !_controlLoopTask.IsCompleted) return;

                _cts = new CancellationTokenSource();
                _controlLoopTask = Task.Run(() => RunControlLoop(_cts.Token));
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                _cts?.Cancel();
            }

            _controlLoopTask?.Wait();
            _intersection.SetAll(LightColor.Red);
        }

        public void SwitchMode(IntersectionMode mode)
        {
            lock (_lock)
            {
                _mode = mode;
                if (mode == IntersectionMode.AllRed)
                {
                    _intersection.SetAll(LightColor.Red);
                }
            }
            Console.WriteLine($"[Controller] Mode switched to {mode}");
        }

        public IntersectionMode GetCurrentMode()
        {
            lock (_lock)
            {
                return _mode;
            }
        }

        private async Task RunControlLoop(CancellationToken token)
        {
            Console.WriteLine("[Controller] Control loop started");

            while (!token.IsCancellationRequested)
            {
                IntersectionMode mode;
                lock (_lock)
                {
                    mode = _mode;
                }

                if (mode == IntersectionMode.AllRed)
                {
                    _intersection.SetAll(LightColor.Red);
                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                    continue;
                }

                if (mode == IntersectionMode.FlashingYellow)
                {
                    // Simple flashing yellow on all directions
                    _intersection.SetAll(LightColor.Yellow);
                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                    _intersection.SetAll(LightColor.Red);
                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                    continue;
                }

                // Normal mode: follow phases
                var phase = _phases[_currentPhaseIndex];
                await ExecutePhase(phase, token);

                _currentPhaseIndex = (_currentPhaseIndex + 1) % _phases.Count;
            }

            Console.WriteLine("[Controller] Control loop exiting");
        }

        private async Task ExecutePhase(PhaseConfig phase, CancellationToken token)
        {
            Console.WriteLine($"[Controller] Executing phase {phase.PhaseNumber}");

            // 1. Set all to Red
            _intersection.SetAll(LightColor.Red);

            // 2. Set green for phase directions
            foreach (var dir in phase.GreenDirections)
            {
                _intersection.GetLight(dir).SetColor(LightColor.Green);
            }

            LogLights("GREEN");

            await Task.Delay(phase.GreenDuration, token);

            // 3. Switch to yellow for phase directions
            foreach (var dir in phase.GreenDirections)
            {
                _intersection.GetLight(dir).SetColor(LightColor.Yellow);
            }

            LogLights("YELLOW");
            await Task.Delay(phase.YellowDuration, token);

            // 4. After yellow, back to red
            foreach (var dir in phase.GreenDirections)
            {
                _intersection.GetLight(dir).SetColor(LightColor.Red);
            }

            LogLights("RED");
        }

        private void LogLights(string label)
        {
            var snapshot = _intersection.GetAllLights()
                .Select(l => l.ToString());
            Console.WriteLine($"[{label}] " + string.Join(" | ", snapshot));
        }

        private static void ValidatePhases(List<PhaseConfig> phases)
        {
            if (phases.Count == 0)
            {
                throw new InvalidPhaseException("At least one phase is required.");
            }

            // Simple validation: no two opposite directions green simultaneously
            foreach (var phase in phases)
            {
                var set = phase.GreenDirections.ToHashSet();

                if (set.Contains(Direction.NorthSouth) && set.Contains(Direction.EastWest))
                {
                    throw new InvalidPhaseException("Conflicting directions in same phase.");
                }
            }
        }
    }
}
