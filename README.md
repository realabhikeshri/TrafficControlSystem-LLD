# 🚦 Traffic Control System – C#  LLD

This repository contains a **Traffic Control System** implemented in C#
It models a multi-direction intersection with:

- Configurable **phases** (which directions are green together).
- Precise **signal timing** with green, yellow, and red durations.
- **Modes** like Normal and All-Red (emergency / maintenance).
- **Vehicle detection** for adaptive green times.
- Concurrency-safe control loop to avoid conflicting greens.

---

## 🎯 Problem

Design a software module that controls traffic lights at an intersection with **North–South** and **East–West** directions:

- Each direction has a traffic light with **Red, Yellow, Green** states.[web:37]
- Green / Yellow / Red durations are **configurable per phase**.[web:39]
- At any point, only **non-conflicting directions** may be green.
- The system must support:
  - Start / stop of the controller.
  - Switching **modes** (Normal, AllRed / Emergency).
  - Manual override of phases (for maintenance / police control).
  - Potential integration with **vehicle sensors**.

This project focuses on **core logic**, not GUI or hardware integration.

---

## 🧱 Design Overview

### Core Concepts

- **Direction**: `NorthSouth`, `EastWest`.
- **LightColor**: `Red`, `Yellow`, `Green`.
- **IntersectionMode**: `Normal`, `AllRed`, `FlashingYellow`.

### Entities

- `TrafficLight`
  - Maintains current `LightColor`, direction, and thread-safe transitions.
- `PhaseConfig`
  - Defines which directions are green together and their durations.
- `Intersection`
  - Holds lights for all directions and the list of `PhaseConfig`s.
- `VehicleDetectionSensor`
  - Provides boolean `IsVehicleWaiting` for adaptive control (simplified).

### Services

- `TimingPlanService`
  - Provides default timing plans and phase lists.
- `TrafficController` (implements `ITrafficController`)
  - **State machine** / **control loop**:
    - Iterates phases.
    - Changes lights to Green → Yellow → Red based on durations.
    - Supports `Start()`, `Stop()`, `SwitchMode()`, `ForcePhase()`.

The control loop runs on a background task and uses `CancellationToken` and locks for safe operations.

---

## 🔑 Patterns & Interview Talking Points

- **State-driven design** – Lights are a small state machine (Red–Green–Yellow).[web:37]
- **Strategy-ready** – `TimingPlanService` can be swapped for different strategies.
- **Thread-safety**
  - Central controller loop, with lock around updates.
  - Safe mode switching and manual overrides.
- **Extensibility**
  - Add more `Direction` values (e.g., turn lanes).
  - Add advanced modes (rush hour plans, pedestrian phases).
- **Error handling**
  - `InvalidPhaseException` for illegal phase configurations.


---

## Author

Abhishek Keshri
