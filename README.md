# VR Molecular Chemistry Lab 🧪

A high-performance, data-driven VR chemistry simulation built with **Unity 6 LTS** and the **XR Interaction Toolkit 3.x**. This project demonstrates a scalable architecture for complex spatial interactions, chemical logic, and optimized XR performance, developed as a technical assessment.

**Developer:** Muhammed Hashiq

---

## ⚙️ Key Technical Features

### 1. Data-Driven Chemistry Engine
* **Flyweight Pattern:** Utilized `ScriptableObjects` (`AtomData`, `MoleculeData`) to define chemical properties and recipes. This ensures minimal memory overhead and allows for expansion (adding new molecules) without touching the codebase.
* **State-Locked Bonding:** Implemented a robust "TryBond" logic with race-condition protection to ensure atoms only react when intentionally manipulated or combined in the synthesis flask.

### 2. Optimized VR Interactions
* **Physics Hygiene:** Custom `SpawnZoneSensor` ensures the spawning area is cleared before instantiation, preventing physics instability and "explosions" common in XR development.
* **Dynamic World-Space UI:** Optimized canvases using the `Tracked Device Graphic Raycaster` for high-precision laser pointer interaction.
* **Visual Affordance:** Integrated **DOTween** for procedural feedback (scaling, punching, and "juice") to provide clear sensory cues during grabbing and successful synthesis.

### 3. Code Quality
* **Decoupled Architecture:** Strictly followed the **Singleton pattern** for global managers while maintaining modular, component-based logic for physical objects to avoid monolithic scripts.
* **Memory Management:** Implemented explicit event unsubscription and tween cleanup in `OnDestroy` to prevent memory leaks on mobile VR hardware (Meta Quest).

---

## 🎮 Controls & Locomotion

This project is optimized for **Meta Quest** controllers. For testing via the **XR Interaction Simulator** on PC, equivalent keyboard shortcuts are provided:

| Action | VR Controller Input | PC Simulator Key |
| :--- | :--- | :--- |
| **Movement** | Left Joystick | I, J, K, L |
| **Turning** | Right Joystick | J, L |
| **Grab Atom** | Grip Button (Side) | G Key |
| **Toggle Laser Ray** | Primary Button (A / X) | Space Key |
| **Break Molecule** | Primary Button (A / X) (While Grabbing) | Space Key (While Grabbing) |
| **UI Interaction** | Grip Button | G Key |

---

## 📂 Project Organization
* `_ChemistryLab/Data`: Centralized registry for all chemical definitions.
* `_ChemistryLab/Prefabs`: Optimized XR Interactables with custom physics damping.
* `_ChemistryLab/Scripts`: Clean C# scripts organized with `#region` blocks for readability.

**Development Environment:**
* **Engine:** Unity 6 LTS (6000.3.13f1)
* **XR Framework:** XR Interaction Toolkit 3.x
* **Target Hardware:** Meta Quest (Android API 29+)
* **Essential Plugins:** DOTween, TextMeshPro

---

## 🤖 AI Tools & Integration

As part of the rapid development process for this assessment, several AI tools were leveraged to accelerate architecture planning, data structuring, and asset prototyping. 

* **Architecture & C# Logic (Claude & Gemini):**
    * Utilized AI assistants to help brainstorm and structure the decoupled C# architecture, specifically focusing on the implementation of the `MoleculeDatabase` using ScriptableObjects to ensure a clean, data-driven design.
    * Leveraged AI for rapid debugging and boilerplate code generation during the setup of the XR Interaction Toolkit event listeners and the "TryBond" logic.
* **Molecule Data Generation (Gemini):**
    * Used AI to rapidly compile and format the required chemical data (formulas, required elements, and bond types) for the minimum required molecules (H₂O, H₂, O₂, N₂, NH₃, CO₂, CH₄) to quickly populate the ScriptableObject database without manual data entry.
* **3D Asset Prototyping & Spatial Blocking (Meshy.ai):**
    * Integrated Meshy.ai into the early workflow to generate rapid 3D prototypes of atomic spheres and molecular structures. 
    * *Workflow Note:* Evaluated different generation models (v5 vs v6). While the free-tier topology required manual refinement for final VR performance, the AI-generated meshes were instrumental in the initial spatial blocking, scale testing, and visual affordance planning within the XR environment.
