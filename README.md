# Connect Four (or Four-In-A-Row)

The problem is to write the Connect Four game that can be played with two players on the same computer.
  
Here's a reminder of how Connect Four works:

* ![](http://upload.wikimedia.org/wikipedia/commons/a/ad/Connect_Four.gif)
* The board is 7 wide and 6 tall
* Players alternate dropping their discs into one of the columns
* A player wins when she has four of her discs in a row (vertically, horizontally, or diagonally)

Preferably, the UI is written in WPF. It doesn't have to be anything fancy (no animations needed, etc).

You can just have a grid with ellipses inside that you switch color, like so:

```
<Grid>
	<Grid.RowDefinitions>
		<RowDefinition Height="60"/>
		...
	</Grid.RowDefinitions>

	<Grid.ColumnDefinitions>
		<ColumnDefinition Column="60"/>
		...
	</Grid.ColumnDefinitions>
	
	<Ellipse Grid.Column="0" Grid.Row="0" Width="60" Height="60" Stroke="Black" Fill="White" .../>
	...
</Grid>
```

## Task
Assume this is real production code. As in, make it readable/maintainable/etc.
Use `git` best practices. 
Have some UnitTests where appropriate.

1. Allow two human users to play each other (using the same keyboard/mouse)
2. Auto-detect when a player has won and show some message
3. Let the users play again 

## Extensions
* Allow the users to enter their name
* Let the user pick which color disc she would like to use
* Make an AI player (doesn't have to be anything very elaborate!)

Take your time, there is no hard deadline. 
