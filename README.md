# CrossZeroAPI

This API is made for creating a tic-tac-toe game regardless of the specific UI implementation.

Supports configurable game field size and customizable player selection and marking.

## Game processor

The absract `TTCGameProcessor` class realizes a game loop.
The game can be configured through the **constructor**

`TTCGameProcessor(int gameFieldSize, IPlayer player1, IPlayer player2, Marks player1Mark)`

or public **properties**:
```c#
IPlayer Player1 { get; set; }
IPlayer Player2 { get; set; }
int GameFieldSize { get; set; }
Marks Player1Mark { get; set; }
```
To start the game loop use the `Play()` method or it's async variant - `PlayAsync()`.

To display UI, a derived class should implement the following methods:
- `RenderGameField(ReadOnlyTable gameField)`

Is called after every game turn.
- `RenderLastFieldAndResult(ReadOnlyTable gameField, EndResult result)`

Is called after end of the game.

## Custom game
If there is need to create a custom game loop, use the `TTCGame` class. It realizes players' turn order.
Use the `MakePlayerTurn()` method to make the current player's turn.

**Properties**:
- `CurrentMark { get; }`

The mark of the current player.
- `IsEnd { get; }`

Indicates whether game is over.
- `GameField { get; }`

Readonly array representing the game field.

## Player
Player should implement the `IPlayer` interface. It does not specify the nature of the player so the game can have different computer/human player variations.
The computer player(AI) though should implement the `IAI` interface that inherits the `IPlayer` interface. 

The API has the `AI` class that realizes a computer player.
