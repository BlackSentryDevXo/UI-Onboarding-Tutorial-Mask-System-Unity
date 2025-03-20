
https://github.com/user-attachments/assets/01dd8ff6-89cd-4f7b-9308-72f4a0b2855b

https://github.com/user-attachments/assets/ae2f385d-9c79-4ba4-ad23-88d624018de0
# 🎮 Unity UI Tutorial System

A **fully customizable, dynamic UI tutorial system** for Unity that helps guide players by highlighting buttons, displaying tooltips, and managing tutorial sequences. This system is designed to be **modular**, **scalable**, and **easy to integrate** into any game project.

   Uploading Screen Recording 2025-03-20 at 7.38.10 PM.mp4…

## ✨ Features
- **Dynamic Tutorial Sequences** – Easily create and manage multiple tutorial sequences.
- **Auto-Generated Enums** – Automatically generate `ButtonID` and `SequenceID` enums from ScriptableObjects.
- **Button Highlighting** – Focuses on buttons and makes them stand out during tutorials.
- **Customizable Tooltips** – Position tooltips near buttons dynamically.
- **Event-Based System** – Hooks into Unity UI buttons with event-driven interactions.
- **Editor Integration** – Simple tools to manage tutorials from the Unity Editor.

---

## 📥 Installation & Setup
### 1️⃣ Clone the Repository
```bash
git clone https://github.com/BlackSentryDevXo/UI-Obboarding-Tutorial-Mask-System-Unity.git
```

### 2️⃣ Import into Unity
- Open your Unity project.
- Drag and drop the `TutorialSystem` folder into your `Assets` directory.

### 3️⃣ Add the `UITutorialManager` to Your Scene
1. Create an empty GameObject and name it **`UITutorialManager`**.
2. Attach the **`UITutorialManager.cs`** script.
3. Assign:
   - **Overlay Panel** (to dim the screen during tutorials).
   - **Tooltip Prefab** (for showing instructions).
   - <img width="964" alt="Screenshot 2025-03-20 at 7 56 01 PM" src="https://github.com/user-attachments/assets/e9e8ad5b-2aa4-467d-bbfc-a241493716ee" />
  
### 4️⃣ Define Your Buttons
1. Create a `ScriptableObject` for buttons:
   - **Go to** `Assets > Create > UI > Buttons DB`
   - Name it `ButtonsDatabase`.
   - Add the button names.
   - Click `Generate Enum` (or use `Tools > ButtonID Enum Generator`).
   <img width="624" alt="Screenshot 2025-03-20 at 7 57 39 PM" src="https://github.com/user-attachments/assets/9f281ad2-16fe-477f-aca3-d93646a2cda4" />
   <img width="513" alt="Screenshot 2025-03-20 at 7 56 44 PM" src="https://github.com/user-attachments/assets/5c7a7573-9449-41b5-a8c6-cb1d7677e020" />

2. **IMPORTANT!!** Add UIButton.cs to every button you want highlighted
3. **IMPORTANT!!** For every button you want highlighted, Assign `ButtonID` to each UI button in the Inspector.


### 5️⃣ Create Tutorial Sequences
1. **Go to** `Assets > Create > UI > SequenceID DB`.
2. Add a new `SequenceDatabase`.
3. List the tutorial sequence names.
4. Click `Generate Enum`.
5. Assign tutorial sequences in `UITutorialManager`.
   <img width="624" alt="Screenshot 2025-03-20 at 7 57 39 PM" src="https://github.com/user-attachments/assets/423380f4-3fd5-436e-b828-b482f1dfe5d0" />
   <img width="510" alt="Screenshot 2025-03-20 at 7 56 57 PM" src="https://github.com/user-attachments/assets/78338707-71cf-4a11-9a38-65fa76a886f5" />

Your Tutorial manager should look something like this when everything is well setup
   <img width="496" alt="Screenshot 2025-03-20 at 7 53 50 PM" src="https://github.com/user-attachments/assets/8a479f78-052b-40d1-89d6-5c0d0e98705a" />

### 6️⃣ Install `NaughtyAttributes` (Optional)
To take advantage of enhanced Inspector UI flexibility, install the **NaughtyAttributes** package:
- **Download from GitHub**: [NaughtyAttributes Repository](https://github.com/dbrizov/NaughtyAttributes)
- **Import into Unity** via Package Manager.
- This allows for better Inspector usability in the project.

---

## 📌 How to Use
### 🎯 Starting a Tutorial
Call the `StartTutorial` function with a `SequenceID`:
```csharp
UITutorialManager tutorialManager = FindObjectOfType<UITutorialManager>();
tutorialManager.StartTutorial(SequenceID.seq_shop);
```

### 🎯 Highlight a Button with a Tooltip
```csharp
tutorialManager.FocusOnButton(ButtonID.shop, "Click here to open the shop!");
```

### 🎯 Ending a Tutorial
```csharp
tutorialManager.EndTutorial();
```
---

## 🛠️ Customization
### 🖊️ Modify Tooltip Appearance
- Edit the **TooltipUI Prefab** to change text styles, colors, or animations.
- Modify `TooltipUI.cs` to change how tooltips position themselves.

### ⚡ Add Custom Logic to Steps
Each step can execute custom logic using the `OnStepCompleted` event:
```csharp
var step = new UITutorialStep { 
    buttonID = ButtonID.shop,
    message = "Click here to open the shop!",
    OnStepCompleted = () => Debug.Log("Shop button clicked!")
};
```
### Here are some visuals of how a well setup project should look like in action
   <img width="289" alt="Screenshot 2025-03-20 at 7 54 55 PM" src="https://github.com/user-attachments/assets/84fc3a92-5012-417c-8d7d-a1e7a8981b15" />
   <img width="289" alt="Screenshot 2025-03-20 at 7 55 22 PM" src="https://github.com/user-attachments/assets/b5fa6760-a802-4a99-8d14-24a88390085b" />


### 🎛️ Adjust Button Highlight Effects
Modify `UIButton.cs` to change how buttons are visually highlighted during a tutorial.

---

## 🤝 Contributing
We welcome contributions! Feel free to **fork** the project, submit **pull requests**, or **report issues**.

### ✅ How to Contribute
1. **Fork the repository**.
2. **Create a new branch** (`feature/tutorial-improvement`).
3. **Make your changes & commit**.
4. **Push the branch & open a PR**.

---

## 📅 Future Improvements
- **👀 UI Animation System** – Add smooth transitions for highlighting buttons.
- **📖 Localization Support** – Multi-language support for tutorials.
- **📊 Analytics Integration** – Track tutorial completion rates.

---

## 📜 License
This project is licensed under the **MIT License**. Feel free to use and modify it!

---

## 👏 Credits
Developed by **[Your Name]**. Inspired by various UI tutorials in the gaming industry.

---

## ⭐ Support the Project
If you find this useful, **give it a ⭐ on GitHub**! 🚀

