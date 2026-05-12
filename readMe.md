# Breakout Game

A simple Breakout-style game made in Unity using C#. The player controls a paddle, bounces a ball, breaks bricks, earns points, and tries to clear the board without losing all lives.

## Features

- Paddle movement using arrow keys
- Ball physics with custom bounce behavior
- Score tracking
- Lives system
- Win and lose scenes
- Normal bricks worth 100 points
- Strong bricks worth 250 points and require 2 hits
- Strong bricks change color after the first hit
- Brick break sound effect
- Paddle shrinks as score increases
- Color palette for normal bricks
## How to Run

- Clone the repo
- Open in Unity Hub
- Load the scene from Assets/Breakout/Breakout
- Press Play

## How to Play

- Use the Left Arrow key to move left
- Use the Right Arrow key to move right
- Keep the ball from falling off the bottom of the screen
- Break all the bricks to win
- If you lose all your lives, the game ends

## Brick Types

### Normal Bricks
- Break in 1 hit
- Worth 100 points
- Spawn in a random color from a preset color list

### Strong Bricks
- Break in 2 hits
- Worth 250 points
- Start as red
- Change to yellow after the first hit

## Scoring

- Normal brick: 100 points
- Strong brick: 250 points

## Paddle Difficulty System

The paddle gets smaller as the score increases.

Example:
- 0–999 points → normal size
- 1000+ points → smaller
- 2000+ points → smaller again
- 3000+ points → smallest allowed size

## Built With

- Unity
- C#
- TextMeshPro

## Scenes

- Main game scene
- Win scene
- Lose scene

## Future Improvements

- Start menu
- Multiple levels
- Power-ups
- High score system
- Better sound effects
- Particle effects when bricks break

## Author

Aprajita Srivastava

## Notes

This project was originally from https://github.com/OtspIII/SimpleGames/     
This project was created as a learning project to practice Unity, C#, game logic, collision handling, UI, and object management.