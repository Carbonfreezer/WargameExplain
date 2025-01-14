# Explain Wargame



## Description
The presented program is a Framework for writing explainable AIs for Hex and Counter Wargames. It's a collaborative effort, with Fabian Fell contributing the Othello representation, and Christoph Lürig writing the rest.
It currently supports several sample games:
* Take that Hill has a one-person hex wargame with several rule changes.
* Othello as a two Person Zero sum strategy game
* A simple betting game to test the functionality of random game Elements.
* Coop game: This is a simple non-zero-sum game (strictly semi-cooperative) to test if the AI finds the Nash Equilibrium.
* Tic Tac Toe: It is a straightforward game that tests AI functionality.

The Project is structured as a direct application and a Client-Server pair. This pair is used to parallelize games for analysis and training. Initially, we implemented this in C# with TPL. However, we encountered scaling issues due to the garbage collector.   