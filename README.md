# 🎮 Crazy Poppers  
*A Chain Reaction Puzzle Game Built in Unity*

---

## 📌 Overview  
**Crazy Poppers** is a chain-reaction based puzzle game inspired by *Crazy Popper* mechanics.  
Your goal is to clear the board by smartly using a limited number of taps.  
Each tapped popper triggers state changes or explosions that launch projectiles in four directions, creating satisfying chain reactions.  

---

## 🧩 Gameplay Mechanics  

| Popper Color | Hits Required | Next State | Final Action |
|--------------|---------------|------------|---------------|
| 🟪 Purple     | 1             | Explodes   | Launches projectiles |
| 🔵 Blue       | 2             | Purple     | Eventually explodes |
| 🟡 Yellow     | 3             | Blue       | Eventually explodes |

**Explosion Rules:**
- Exploded poppers launch **projectiles** in **4 directions** (Up, Down, Left, Right).
- Projectiles hit next poppers → apply hit logic.
- Projectiles stop only when:
  - They hit a popper, or
  - Exit the game board.

**Win Condition:**  
✔ All poppers are successfully removed from the board.

**Lose Condition:**  
❌ No taps remaining *after chain reaction ends* and poppers still exist.

---

## 🕹 Controls  
| Action | Input |
|--------|--------|
| Tap popper | Left Mouse Click / Touch |
| Restart level | Restart button |
| Next level | Next button (after win) |
| Open settings | Menu/Settings button |

---

## 📁 Project Structure

