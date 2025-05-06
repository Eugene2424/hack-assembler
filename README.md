# Hack Assembler (Project 6 – Nand2Tetris)

**Assembler for the Hack computer, implemented in C#**

This repository contains my implementation of Project 6 from the [Nand2Tetris](https://www.nand2tetris.org/) course. The assembler translates Hack assembly language (`.asm` files) into binary machine code (`.hack` files) compatible with the Hack computer platform.

## 📁 Project Structure

```
hack-assembler/
├── HackAssembler/           # C# source code for the assembler
├── HackAssembler.sln        # Visual Studio solution file
├── .gitignore
└── README.md                # This file
```

## 🚀 Getting Started

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Building the Assembler

1. Clone the repository:

   ```bash
   git clone https://github.com/Eugene2424/hack-assembler.git
   cd hack-assembler
   ```

2. Navigate to the project directory:

   ```bash
   cd HackAssembler
   ```

3. Build the project:

   ```bash
   dotnet build
   ```

### Running the Assembler

After building, you can run the assembler with a Hack assembly file:

```bash
dotnet run --path/to/YourProgram.asm
```

This will generate a corresponding `YourProgram.hack` file in the same directory.

## 🧠 Features

* **Two-Pass Assembly Process**: Handles labels and variables by first resolving all symbols before translation.
* **Symbol Table Management**: Supports predefined symbols, labels, and user-defined variables.
* **Instruction Parsing**: Differentiates between A-instructions and C-instructions for accurate binary translation.

## ✅ Completed Tasks

* ✅ Parsing and translating A-instructions (e.g., `@value`)
* ✅ Parsing and translating C-instructions (e.g., `D=M+1;JGT`)
* ✅ Handling labels (e.g., `(LOOP)`) and updating symbol table
* ✅ Managing variables and allocating memory addresses starting from 16
* ✅ Generating correct `.hack` binary files compatible with the Hack platform

## 📘 Notes

This project is part of my personal journey through the Nand2Tetris course and serves as a portfolio piece. While it's fully functional, I'm not actively maintaining it or reviewing external contributions. Feel free to explore and fork for your own learning purposes.
