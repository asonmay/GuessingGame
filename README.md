# Guessing Game

A console version of the two-game assignment: a **Guessing Game** (the computer picks a
secret number, you guess it) and a **Reverse Guessing Game** (you pick the number, the
computer guesses it and tries to catch you if your answers don't add up).

## Language and toolchain

- **Language:** C# 10
- **Target framework:** .NET 6.0 (`net6.0`)
- **Built with:** .NET SDK 8.0.303, which builds and runs `net6.0` projects; Visual Studio
  2022 created the original solution. Any .NET SDK 6.0 or newer will work.

## Build and run

From the folder containing `GuessingGame.sln`:

```
dotnet run --project GuessingGame
```

Or build first and run the executable:

```
dotnet build
GuessingGame\bin\Debug\net6.0\GuessingGame.exe
```

In Visual Studio: open `GuessingGame.sln` and press F5.

## Tools

I did not use any tools, just my brain.
