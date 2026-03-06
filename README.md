# 🃏 Soul of Steel – Multiplayer Card Game (Unity + Photon)

This project is a multiplayer card game developed in **Unity using C#**, implementing **Photon Networking (PUN)** for online multiplayer functionality.

The main goal of the project is to demonstrate **clean architecture, SOLID principles, and testable systems** applied to game development while building a functional multiplayer card game.

---

## 🎯 Project Objective

Develop a multiplayer card game while focusing on:

- Clean and maintainable code architecture.
- Implementation of **SOLID design principles**.
- Separation of responsibilities using **View–Controller architecture**.
- Multiplayer synchronization using **Photon Networking**.
- **Unit testing of gameplay logic** to ensure reliability.

The project emphasizes modular design, allowing gameplay systems, networking, and presentation layers to remain **decoupled, scalable, and testable**.

---

## 📌 Features

- Online multiplayer matches using **Photon PUN**.
- Turn-based card gameplay system.
- Modular architecture following **SOLID principles**.
- **View–Controller pattern** separating logic from presentation.
- Player matchmaking and room system.
- Network synchronization between players.
- **Unit tests for gameplay systems**.

---

## 🖼️ Preview

![Gameplay preview](https://github.com/DannyLopezC/SoulOfSteel/blob/main/preview.png)

---

## 🔧 Requirements

This project requires:

- **Unity 2021 or newer**
- **Photon PUN (Photon Unity Networking)**
- **C#**

Photon PUN is used to handle:

- Player connections
- Multiplayer rooms
- Game object synchronization
- Networking events

---

## ▶️ How to run the project

1. Clone this repository:

```
git clone https://github.com/DannyLopezC/SoulOfSteel.git
cd SoulOfSteel
```

2. Open the project in **Unity**.

3. Configure your **Photon App ID** in the Photon settings.

4. Open the main scene.

5. Press **Play** in the Unity Editor.

---

## 🧪 Unit Testing

The project includes **unit tests for gameplay systems** to validate core mechanics independently from the Unity scene.

Testing focuses on:

- Card logic
- Turn system behavior
- Gameplay rule validation

Tests are implemented using **Unity Test Framework** to ensure that game logic remains stable as the project evolves.

---

## 🎮 Controls

Interact with cards → **Mouse click**  
Play card → **Click on a card in your hand**  
Navigate UI → **Mouse**

_(Controls may vary depending on the current game state.)_

---

## 🧠 Concepts Used

- Object-Oriented Programming
- SOLID Design Principles
- View–Controller architecture
- Multiplayer networking
- Photon matchmaking and room system
- Client-server synchronization
- Turn-based gameplay systems
- Unit testing in Unity

---

## 🏗 Architecture

The project follows a modular architecture designed to keep gameplay logic, networking, and presentation clearly separated. The goal is to make the system **scalable, testable, and easy to maintain**.

### View–Controller Separation

The project uses a **View–Controller architecture** to separate presentation from game logic.

**View**

- Responsible for rendering cards, UI elements, and visual feedback.
- Contains no gameplay rules.
- Communicates user actions to controllers.

**Controller**

- Handles player input and interaction with the game.
- Coordinates gameplay systems.
- Acts as a bridge between the View layer and the core gameplay logic.

This separation allows gameplay systems to remain **independent from the Unity UI layer**, making them easier to test and modify.

---

### Gameplay Systems

Core gameplay mechanics are implemented as **independent systems**, each responsible for a specific aspect of the game logic.

Examples include:

- Card behavior and card effects
- Turn management
- Game state validation
- Player interaction logic

These systems follow **SOLID principles**, ensuring that responsibilities remain clearly defined and components remain loosely coupled.

---

### Networking Layer

Multiplayer functionality is implemented using **Photon PUN**, which manages:

- Player connections
- Room creation and matchmaking
- Network synchronization between clients

The networking layer is designed so that **gameplay systems remain independent from Photon**, allowing the game logic to be tested locally without requiring an active multiplayer session.

Controllers handle communication between gameplay systems and Photon events.

---

### Testable Gameplay Logic

A key design goal of the project is **testability**.

Core gameplay systems are written so they can be executed **outside the Unity scene environment**, allowing them to be validated using **unit tests**.

This approach allows:

- Validation of card mechanics
- Verification of turn system behavior
- Testing of gameplay rules

Using the **Unity Test Framework**, gameplay logic can be tested independently from rendering or networking.

---

### Design Principles

The architecture of the project emphasizes:

- **Single Responsibility Principle** – Each system handles a single concern.
- **Dependency Inversion** – High-level gameplay systems do not depend on specific implementations.
- **Loose Coupling** – Systems communicate through controlled interfaces.
- **Testability** – Gameplay logic can be validated without running the full game.

This architecture makes the project easier to extend with additional gameplay mechanics, networking features, or UI improvements.

---

## 📚 Credits

Project developed as a programming exercise focused on **software architecture, multiplayer networking, and testable gameplay systems**.

Author: [DannyLopezC](https://github.com/DannyLopezC)
