# Game of Life (WPF)

Implementation of Conway's classic game “Life” in C# using the Windows Presentation Foundation (WPF).

## 🔍 Description

The game simulates the life of cells on a two-dimensional field. Each cell can be either alive or dead, and changes its state depending on the number of neighbors. This project demonstrates:

- Working with `WriteableBitmap` for fast graphical rendering
- A timer for periodically updating generations (`DispatcherTimer`)
- UI-interactive: change of parameters (resolution, density), start/stop of simulation
- Interactive editing of cells with the mouse (left/right button)

## 🧰 Technologies

- C#
- WPF (XAML)
- Extended WPF Toolkit (`xceed` - IntegerUpDown)
- WriteableBitmap API
- Canvas, Image, DispatcherTimer

## 🖼️ Interface

- Field for visualization (Canvas + Image)
- Parameters `Resolution` and `Density`
- Buttons **Start** and **Stop**
- Mode of drawing cells with the mouse

## 📸 Screenshot.

![image](https://github.com/user-attachments/assets/ddccbf11-1b2e-4ba9-8771-61fe85a996f7)


## 🚀 Starting.

1. Install [Visual Studio](https://visualstudio.microsoft.com/) from .NET Desktop Development
2. Clone the repository:
   ## bash
   git clone https://github.com/твій-нік/GameLifeWpf.git
