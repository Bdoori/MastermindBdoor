
# Mastermind Console Game

A console-based implementation of the classic Mastermind game in C#.  
Players must guess a 4-digit secret code (digits 0–8, no repeats) within a limited number of attempts.

This version handles all player inputs gracefully, with zero crash risk.

## Table of Contents

- [Getting Started](#getting-started)  
- [Assessment Requirements](#assessment-requirements)  
- [Enhancements & Details](#enhancements--details)  
- [Usage Examples](#usage-examples)  
- [Command-Line Options](#command-line-options)  
- [Gameplay Example](#gameplay-example)  
- [Project Structure](#project-structure)  
- [Stability Highlight](#stability-highlight)  
- [License](#license)  

## Getting Started

Clone your repo and run the game in a Windows console:

```bash
git clone https://github.com/YourUsername/MastermindBdoor.git
cd MastermindBdoor
dotnet build
dotnet run --project MastermindBdoor
```

When prompted, you can:
- Press Enter (empty input) to play with a random code (default 10 attempts).
- Type -c 1234 to set your own 4-digit code.
- Type -t 15 to change the number of attempts (1–20).
- Combine both (-c 5678 -t 12 or -t 12 -c 5678).
- Use -h or --help for full instructions.
- Press Ctrl+D at any time to exit cleanly.

---

### Assessment Requirements

- 9 colored pieces, code is 4 distinct digits 0–8.
- 10 attempts to guess the secret code.
- After each guess, displays:
  - Well-placed pieces count.
  - Misplaced pieces count.
- Win message: Congratz! You did it!
- Input from standard input, runs in Windows console.
- Handles Ctrl+D (EOF) as a normal exit.
- Supports -c [CODE] for custom code, -t [ATTEMPTS] for attempt limit.

### Enhancements & Details

These refinements go beyond the base requirements to deliver a polished, user-centric experience:

- Flags `-c` and `-t` can be passed in any order (e.g. `-c 1234 -t 12` or `-t 12 -c 1234`) for flexibility.  
- Color-coded console output:
  - Instructions in Yellow  
  - Help menus in Cyan  
  - Prompt in Dark Yellow  
  - Round headers in Blue  
  - Errors in Red  
  - Success in Green  
- Two tiers of help:
  - Full help at startup (`-h`, `--help`) with detailed usage  
  - Short help during gameplay (`-h`, `--help`) with quick commands  
- Input editing support:
  - Backspace to correct typos before pressing Enter   
- Strict validation rules:
  - Attempts (`-t`) must be between 1 and 20 (zero, negative, or >20 are rejected)  
  - Guesses must be exactly four unique digits (0–8); invalid guesses produce clear feedback  
  - Unexpected tokens or flags trigger friendly error messages  
- Comprehensive error handling and scenario testing:
  - Cover every edge case—no crashes, no surprises, just smooth gameplay.

### Usage Examples

```bash
# 1) Random code (press Enter at prompt)
dotnet run --project MastermindBdoor 

# 2) Custom code only
dotnet run --project MastermindBdoor -- -c 1234

# 3) Attempts only (1–20)
dotnet run --project MastermindBdoor -- -t 15

# 4) Both code and attempts (order doesn’t matter)
dotnet run --project MastermindBdoor -- -c 5678 -t 12
dotnet run --project MastermindBdoor -- -t 8 -c 9012
```

### Command-Line Options

- **[ENTER]**  
  Play with a random secret code and default attempts (10).  
- **-c `<code>`**  
  Set your own 4-digit secret code (0–8, unique).  
- **-t `<attempts>`**  
  Specify number of attempts (1–20).  
- **-h, --help**  
  Show usage instructions.  
- **Ctrl+D**  
  Exit the game immediately.

### Gameplay Example

```
Use -c to choose your secret code (4 unique digits from 0 to 8).
Use -t to set number of attempts (from 1 to 20).
Press Enter to play with a random code and default attempts.
Use -h or --help for instructions and help.
Press Ctrl+D to exit the game.

Enter your choice: -t 5 -c 2047

Will you find the secret code?
Please enter a valid guess:

Round 0:
> 1234
Well placed pieces: 0
Misplaced pieces: 2

Round 1:
> 5678
Well placed pieces: 0
Misplaced pieces: 1

Round 2:
> 0
Wrong input!

Round 2:
> 2047
Congratz! You did it!
```

### Project Structure

MastermindBdoor/
├── MastermindBdoor.csproj
├── Program.cs           # CLI parsing, setup & game logic
└── README.md            # Project documentation

### Stability Highlight

This project was designed with rigorous input validation and fail-safe mechanisms, 
ensuring the game responds reliably to any possible user behavior. 
No crashes, no surprises—just clean, solid gameplay.

### License

© 2025 Bdoor Alolayan. All rights reserved.