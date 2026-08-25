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

## Files

| File | What's in it |
| --- | --- |
| `Program.cs` | Greeting, main menu, and the "play again" loop shared by both games |
| `GameMode.cs` | The `GameMode` enum: the three things the main menu can be doing |
| `ConsoleInput.cs` | Everything both games share: reading numbers, the validated range questions, yes/no questions, formatting a guess list |
| `GuessingGameMode.cs` | Guessing Game, plus the bonus strategy analysis |
| `ReverseGuessingGameMode.cs` | Reverse Guessing Game, plus the guess-choosing algorithm and the end-of-game honesty checks |

The shared code is the part worth pointing at: both games ask for a range with the same
three validation rules, both ask yes/no questions, and both print a comma-separated list of
guesses, so those live in `ConsoleInput` and are called from both games. The "play again"
loop is also written once, in `Program.cs`, so neither game has to repeat it.

## Choices I made where the assignment left it open

- **Constant names.** The assignment names `MIN_ALLOWED_RANGE` and `MAX_ALLOWED_GUESSES`, so
  I used those names exactly even though C# convention would be `MinAllowedRange`. Values:
  minimum range width **10**, maximum guesses **10** in each game.
- **Bad range input restarts both questions.** If the pair is invalid (min above max, or
  narrower than 10) I ask for the minimum again rather than only the maximum, because a bad
  pair doesn't tell me which of the two numbers the user wants to change.
- **Out-of-range guesses don't cost an attempt.** In the Guessing Game, a guess outside the
  range is rejected and re-asked. It could never have been the answer, so charging an
  attempt for it would be unfair. Non-numeric input is treated the same way.
- **Guesses are stored in a fixed-size array**, as the assignment asks, sized to
  `MAX_ALLOWED_GUESSES` with a separate counter for how many slots are used.
- **The menu is a `GameMode` enum** with the values fixed to `1`, `2` and `3`, so the numbers
  the player types, the numbers printed in the menu, and the values compared in the code are
  all the same thing in one place. The player's answer is read as an `int` and cast to the
  enum, which is safe because the input helper has already restricted it to 1-3.
- **The menu takes 1/2/3**, and the reverse game's answers are 1/2/3 as specified. Yes/no
  questions accept `y`, `yes`, `n`, `no` in any capitalisation and re-ask on anything else.
- **Ctrl+Z / end of input exits cleanly.** `Console.ReadLine()` returns `null` when input
  closes; without handling that, every "ask until valid" loop would spin forever.
- **Play again repeats the same game** (including asking for a fresh range); saying no
  returns to the main menu, as specified.

## Notable bits of the logic

**How the computer guesses (reverse game).** The middle of the range that's still possible
is the best guess mathematically, because it halves what's left whichever answer comes back.
Always guessing the exact middle makes the computer completely predictable, so I nudge each
guess by up to a tenth of the remaining window. The window still shrinks by at least 40% per
guess, so the variation costs at most about one extra guess.

**Detecting dishonest answers (reverse game).** After each answer I keep a window `low..high`
of the numbers still consistent with everything I've been told.

- If the window narrows to exactly one number, that number *is* the answer — so the computer
  states it instead of asking. This is the assignment's "52 is too low, 54 is too high, so
  it's 53" case.
- If the window collapses to nothing (`low > high`), no number can satisfy all the answers,
  so someone wasn't playing fair, and the game says so.
- If the computer runs out of guesses, it asks what the number was and checks, in order:
  was it even inside the agreed range; was it a number the computer already guessed and was
  told was wrong; and (the bonus) did any answer point the wrong way — for example a guess of
  20 called "too high" when the number was 22. One wrong direction sends the search into the
  wrong half permanently, so the computer explains that it couldn't have won. If every answer
  checks out, it says so and blames the range being wider than 10 guesses can cover.

**Strategy analysis (guessing game bonus).** After the game I replay the player's guesses
and track the window they should have been able to deduce. For each guess I record whether
it was already ruled out by an earlier clue, and how far it was from the middle of the
remaining window as a fraction of the half-width (0.0 = dead centre, 1.0 = at the edge). If
the average is close to centre, that's a binary search; if the player won fast with guesses
that weren't near the middle, that's luck. Guess count is compared against
`ceil(log2(range size))`, the worst case for perfect play.

**Integer overflow at the edges.** A user can enter numbers up to `int.MaxValue`. Picking a
random number needs `maximum + 1` (because `Random.Next`'s upper bound is exclusive), the
range width needs `maximum - minimum`, and the midpoint needs `low + high` — all three
overflow 32-bit arithmetic at the extremes, so those calculations are done in `long` and
narrowed afterwards. Each game holds one `Random` in a `static readonly` field rather than
creating a new one per round, which is the usual way to avoid repeated seeds.

## Tools

> **TODO — rewrite this paragraph in your own words before submitting.** It has to describe
> what *you* actually did; the notes below are only a starting point.

I used Claude (Anthropic's AI assistant, via Claude Code) to draft the C# from the assignment
PDF, and the .NET CLI (`dotnet build` / `dotnet run`) to build and test. `<Say here what you
had to fix or change — e.g. anything you rewrote, renamed, simplified, or disagreed with
while reading it through.>` I tested each ending by scripting input into the program: a win
and a loss in both games, contradictory answers, answers that pointed the computer the wrong
way, a number outside the agreed range, and non-numeric input at every prompt.
