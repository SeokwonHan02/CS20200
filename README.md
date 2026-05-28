# Console-Based Nim Game

### Game Overview
The game starts with five piles of stones. Each pile contains a random number of stones between 5 and 14.

The human player always moves first. On each turn, the current player removes one or more stones from exactly one pile. The player who removes the final stone wins.
The human player may also type give up during their turn. If the player gives up, the game ends immediately and the AI wins.

---

### Prerequisite
Install the .NET 10 SDK.

You can check your .NET version with:

```bash
dotnet --version
```

### Run
From the project directory, run:

```bash
cd NimGame
dotnet run
```

### Build

```bash
dotnet build
```

---

### Opponent Selection
At the beginning of the game, the player chooses one of two AI opponents:

- Random AI : Chooses a non-empty pile and removes a random valid number of stones
- Optimal AI : Uses the mathematical Nim strategy  based on the XOR nim-sum

If the player enters an invalid option, the game asks again until a valid option is entered.

### Rules
- The game uses exactly five piles.
- Each pile starts with 5 to 14 stones.
- The human player moves first.
- On a turn, a player removes stones from exactly one pile.
- At least one stone must be removed.
- A player cannot remove more stones than the selected pile contains.
- The game ends when all piles are empty.
- The player who removes the last stone wins.
- The human player may type 'give up' to give up.

### Player Input
During the human player’s turn, enter a move in this format:

```
<pile> <stones>
```

For example:

```
3 2
```
This removes 2 stones from pile 3.

To give up the game, enter:

```
give up
```

Invalid inputs are rejected, and the player is asked to enter another move.

Examples of invalid moves:
```
6 2      # pile number must be between 1 and 5
2 0      # must remove at least one stone
3 20     # cannot remove more stones than the pile contains
hello    # input must be a valid move or "give up"
```

---

### Display Format

The game displays the current pile state before each turn.

Example:

```
Current piles:
Pile 1: ( 9) ● ● ● ● ●   ● ● ● ●  
Pile 2: ( 8) ● ● ● ● ●   ● ● ●  
Pile 3: (12) ● ● ● ● ●   ● ● ● ● ●   ● ●  
Pile 4: ( 9) ● ● ● ● ●   ● ● ● ●  
Pile 5: (14) ● ● ● ● ●   ● ● ● ● ●   ● ● ● ●  

Your move, ex) 3 2 or give up:
```

### Implementation Structure

The project is implemented in a single F# source file.

Main components:

| Function / Component | Responsibility |
|---|---|
| `GameAction` | Represents either a move or a give-up action |
| `showPiles` | Displays the current pile states using Unicode stone symbols |
| `isGameOver` | Checks whether all piles are empty |
| `applyMove` | Updates the selected pile after a move |
| `readMove` | Reads and validates player input |
| `randomAiMove` | Generates a random valid move |
| `optimalAiMove` | Generates an optimal Nim move using the XOR nim-sum strategy |
| `main` | Controls the overall game loop and turn progression |

---

### Project Files

Current repository structure:

```
CS20200/
├── README.md
└── NimGame/
    ├── NimGame.fsproj
    └── Program.fs
```

---

### LLM Usage

I used an LLM to get advice on improving the console UI formatting. In particular, I asked how to make the current pile display more readable and visually clean, including spacing between stones, grouping stones in sets of five, using Unicode symbols such as ●, and considering alternatives such as thin spaces.

However, the LLM did not directly provide a final UI design that clearly improved the visibility of the pile display. I had to manually test and adjust the formatting choices, including the spacing between symbols and the way stones are grouped, to make the console output easier to read.

The main limitation was that the LLM could suggest formatting ideas, but it could not reliably judge which format would be most readable in my actual terminal environment. Therefore, I manually tested and refined the final pile display.