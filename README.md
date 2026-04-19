# **VR Molecular Lab - Technical Assessment**

A high-performance, data-driven VR chemistry simulation built with **Unity 6 LTS** and the **XR Interaction Toolkit 3.x**. This project demonstrates a scalable architecture for complex spatial interactions, chemical logic, and optimized XR performance.

---

##  Key Technical Features

### **1. Data-Driven Chemistry Engine**
* **Flyweight Pattern:** Utilized `ScriptableObjects` (`AtomData`, `MoleculeData`) to define chemical properties and recipes. This ensures minimal memory overhead and allows for expansion (adding new molecules) without touching the codebase.
* **State-Locked Bonding:** Implemented a robust "TryBond" logic with race-condition protection to ensure atoms only react when intentionally manipulated or combined in the synthesis flask.

### **2. Optimized VR Interactions**
* **Physics Hygiene:** Custom `SpawnZoneSensor` ensures the spawning area is cleared before instantiation, preventing physics instability and "explosions" common in XR development.
* **Dynamic World-Space UI:** Optimized canvases using the `Tracked Device Graphic Raycaster` for high-precision laser pointer interaction.
* **Visual Affordance:** Integrated **DOTween** for procedural feedback (scaling, punching, and "juice") to provide clear sensory cues during grabbing and successful synthesis.

### **3. Senior-Level Code Quality**
* **Decoupled Architecture:** Strictly followed the **Singleton pattern** for global managers while maintaining modular, component-based logic for physical objects to avoid monolithic scripts.
* **Memory Management:** Implemented explicit event unsubscription and tween cleanup in `OnDestroy` to prevent memory leaks on mobile VR hardware (Meta Quest).

---

## **🎮 Controls & Locomotion**

| Action | Input (Left Controller) | Input (Right Controller) |
| :--- | :--- | :--- |
| **Movement** | **Joystick (Continuous)** | — |
| **Turning** | — | **Joystick (Snap/Continuous)** |
| **Grab Atom/Flask** | **Grip Button** | **Grip Button** |
| **Break Molecule** | **Trigger Button** (While Holding) | **Trigger Button** (While Holding) |
| **UI Interaction** | **Trigger** (Hover + Click) | **Trigger** (Hover + Click) |
| **Toggle Laser** | **Primary Button (X)** | **Primary Button (A)** |

---

## **📂 Project Organization**
* **`_ChemistryLab/Data`**: Centralized registry for all chemical definitions.
* **`_ChemistryLab/Prefabs`**: Optimized XR Interactables with custom physics damping.
* **`_ChemistryLab/Scripts`**: Clean C# scripts organized with `#region` blocks for readability.

---

## Development Environment
* **Engine:** Unity 6 LTS (6000.3.13f1)
* **XR Framework:** XR Interaction Toolkit 3.x
* **Target Hardware:** Meta Quest (Android)
* **Essential Plugins:** DOTween, TextMeshPro
