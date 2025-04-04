# 🃏 Memory Game

A fun and interactive **Memory Game** built with **WPF (Windows Presentation Foundation)** and **MVVM (Model-View-ViewModel)** architecture.

## ✨ Features

- 🖼️ Flip and match image-based cards
- ⏳ Timer to track game duration
- 🔄 Start a new game with different categories
- 📊 View player statistics (games played & wins)
- 💾 Save and load game progress
- 🎨 Multiple categories: **Drivers, Legends, Teams**
- ⚙️ Standard & custom grid sizes

## 🛠️ Technologies Used

- 🔹 **C#**
- 🔹 **WPF (Windows Presentation Foundation)**
- 🔹 **MVVM Architecture**
- 🔹 **XAML for UI**
- 🔹 **JSON Serialization for saving statistics**
- 🔹 **ObservableCollection for data binding**

## 🚀 Getting Started

### 📥 Prerequisites

Make sure you have the following installed:

- ✅ **.NET 6+**
- ✅ **Visual Studio 2022+**

### 🛠️ Setup

1. **Clone the repository**:
   ```sh
   git clone https://github.com/Oana-Sebastian/MemoryGame.git
   ```
2. **Navigate to the project folder**:
   ```sh
   cd MemoryGame
   ```
3. **Open the project in Visual Studio** and build the solution.
4. **Run the application** using `F5`.


## 📝 Usage

- Click on a card to reveal it.
- Match pairs of cards to clear the board.
- Track your remaining time at the top.
- Access game **categories** and **statistics** from the menu.
- Save/load your progress to continue later.

## 🛠️ Configuration

You can modify the default grid size by updating these values in `GameWindow.xaml.cs`:

```csharp
public GameWindow(User user, int rows = 4, int columns = 4)
```


