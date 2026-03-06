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

## 📚 Credits

Project developed as a programming exercise focused on **software architecture, multiplayer networking, and testable gameplay systems**.

Author: [DannyLopezC](https://github.com/DannyLopezC)
